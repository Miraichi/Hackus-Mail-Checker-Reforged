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
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using HandyControl.Tools;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Handlers
{
	// Token: 0x020001E3 RID: 483
	public class ScheduledImapHandler : IMailHandler
	{
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00047888 File Offset: 0x00045A88
		public ScheduledImapHandler(ScheduledMail mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
			if (mailbox.Context == null)
			{
				mailbox.Context = new ImapContext();
			}
			this._context = (ImapContext)mailbox.Context;
			this._folder = Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder.Parse(<Module>.smethod_3<string>(-762368918));
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000478E4 File Offset: 0x00045AE4
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

		// Token: 0x06000E37 RID: 3639 RVA: 0x000479B8 File Offset: 0x00045BB8
		public OperationResult Login(out ExceptionType exceptionType)
		{
			exceptionType = ExceptionType.Undefined;
			this._imapClient = new ImapClient();
			int num = CheckerSettings.Instance.Timeout * 1000;
			this._imapClient.ConnectTimeout = num;
			this._imapClient.ReadWriteTimeout = num;
			OperationResult result;
			try
			{
				this._imapClient.Connect(this._server.Hostname, this._server.Port, this._server.Socket == Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL, this._tcpClient);
				this._imapClient.Authenticate(this._mailbox.Address, this._mailbox.Password);
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

		// Token: 0x06000E38 RID: 3640 RVA: 0x00047AB0 File Offset: 0x00045CB0
		public OperationResult SelectFolder(Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder folder)
		{
			for (int i = 0; i < 2; i++)
			{
				try
				{
					this._imapClient.SelectFolder(folder);
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

		// Token: 0x06000E39 RID: 3641 RVA: 0x00047B14 File Offset: 0x00045D14
		public void SearchMessages()
		{
			List<Request> list = new List<Request>();
			OperationResult operationResult = OperationResult.Retry;
			int num = 0;
			while (operationResult == OperationResult.Retry)
			{
				operationResult = this.ProcessSearch(list);
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
			this._context.IsInitialized = true;
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
								Message = ScheduledImapHandler.<>c__DisplayClass12_0.smethod_0(<Module>.smethod_2<string>(73804730), finded, request),
								Time = DateTime.Now
							});
						});
					}
				}
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00047C08 File Offset: 0x00045E08
		private OperationResult ProcessSearch(List<Request> checkedRequests)
		{
			Request request4 = null;
			try
			{
				if (this._folder.MessageCount == 0)
				{
					return OperationResult.Ok;
				}
				try
				{
					this._imapClient.SelectFolder(this._folder);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (MailException ex)
				{
					if (ex.Message != null)
					{
						string text = ex.Message.ToLower();
						if (text.Contains(<Module>.smethod_3<string>(-704876428)) || text.Contains(<Module>.smethod_4<string>(-1527405962)))
						{
							return OperationResult.Ok;
						}
					}
					throw;
				}
				using (IEnumerator<Request> enumerator = SchedulerSettings.Instance.Requests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Request request = enumerator.Current;
						try
						{
							try
							{
								Request request2 = checkedRequests.FirstOrDefault((Request r) => ScheduledImapHandler.<>c__DisplayClass13_0.smethod_0(r.Sender, request.Sender) && ScheduledImapHandler.<>c__DisplayClass13_0.smethod_0(r.Body, request.Body) && ScheduledImapHandler.<>c__DisplayClass13_0.smethod_0(r.Subject, request.Subject));
								if (request2 == null)
								{
									request2 = request.Clone();
									request2.FindedUids = new HashSet<Uid>();
									request2.SavedUids = new HashSet<Uid>();
									request2.CheckedFolders = new HashSet<string>();
									checkedRequests.Add(request2);
								}
								else if (request2.IsChecked || request2.CheckedFolders.Any((string u) => u == this._folder.Name) || (request2.SavedUids != null && request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit))
								{
									continue;
								}
								request4 = request2;
								Uid[] array = request2.FindedUids.Any((Uid u) => u.Folder.Name == this._folder.Name) ? (from u in request2.FindedUids
								where u.Folder.Name == this._folder.Name
								select u).ToArray<Uid>() : this._imapClient.Search(this.BuildSearchCondition(request2));
								if (!this._context.IsInitialized)
								{
									this._context.CheckedUids.AddRange(array);
								}
								else if (array.Length != 0)
								{
									array = array.Except(this._context.CheckedUids).ToArray<Uid>();
									if (array.Length != 0)
									{
										this._context.CheckedUids.AddRange(array);
										request2.FindedUids.UnionWith(array);
										if (request2.Sender != null && request2.Body == null && request2.Subject == null)
										{
											if (SchedulerSettings.Instance.DownloadMails)
											{
												Uid[] array2 = array;
												for (int i = 0; i < array2.Length; i++)
												{
													Uid uid = array2[i];
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage;
														try
														{
															mailMessage = ((request2.Errors == 0) ? this._imapClient.FetchFrom(uid) : this._imapClient.FetchMessage(uid, true, true));
														}
														catch (ThreadAbortException)
														{
															throw;
														}
														catch (Exception ex2)
														{
															if (ex2.Message == null || !ex2.Message.Contains(<Module>.smethod_5<string>(1891727559)))
															{
																throw;
															}
															goto IL_393;
														}
														if (mailMessage != null && !string.IsNullOrEmpty(mailMessage.From) && mailMessage.From.ContainsIgnoreCase(request2.Sender))
														{
															try
															{
																mailMessage = this._imapClient.FetchMessage(uid, false, true);
															}
															catch (ThreadAbortException)
															{
																throw;
															}
															catch (Exception ex3)
															{
																if (ex3.Message == null || !ex3.Message.Contains(<Module>.smethod_6<string>(2021793209)))
																{
																	throw;
																}
																goto IL_393;
															}
															if (mailMessage != null && mailMessage.AlternateViews.Count != 0)
															{
																request2.SavedUids.Add(uid);
																SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage);
															}
														}
													}
													IL_393:;
												}
											}
										}
										else if (request2.Sender == null && request2.Body == null && request2.Subject != null)
										{
											if (SchedulerSettings.Instance.DownloadMails)
											{
												Uid[] array2 = array;
												for (int i = 0; i < array2.Length; i++)
												{
													Uid uid = array2[i];
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage2;
														try
														{
															mailMessage2 = ((request2.Errors == 0) ? this._imapClient.FetchSubject(uid) : this._imapClient.FetchMessage(uid, true, true));
														}
														catch (Exception ex4)
														{
															if (ex4.Message == null || !ex4.Message.Contains(<Module>.smethod_4<string>(-401647309)))
															{
																throw;
															}
															goto IL_52C;
														}
														if (mailMessage2 != null && !string.IsNullOrEmpty(mailMessage2.Subject) && mailMessage2.Subject.ContainsIgnoreCase(request2.Subject))
														{
															try
															{
																mailMessage2 = this._imapClient.FetchMessage(uid, false, true);
															}
															catch (Exception ex5)
															{
																if (ex5.Message == null || !ex5.Message.Contains(<Module>.smethod_4<string>(-401647309)))
																{
																	throw;
																}
																goto IL_52C;
															}
															if (mailMessage2 != null && mailMessage2.AlternateViews.Count != 0)
															{
																request2.SavedUids.Add(uid);
																SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage2);
															}
														}
													}
													IL_52C:;
												}
											}
										}
										else if (request2.Sender == null && request2.Body != null && request2.Subject == null && SchedulerSettings.Instance.DownloadMails)
										{
											Uid[] array2 = array;
											for (int i = 0; i < array2.Length; i++)
											{
												Uid uid = array2[i];
												if (!request2.SavedUids.Any((Uid u) => u == uid))
												{
													Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage3;
													try
													{
														mailMessage3 = this._imapClient.FetchMessage(uid, false, true);
													}
													catch (Exception ex6)
													{
														if (ex6.Message == null || !ex6.Message.Contains(<Module>.smethod_2<string>(802576006)))
														{
															throw;
														}
														goto IL_653;
													}
													if (mailMessage3 != null && !string.IsNullOrEmpty(mailMessage3.Body) && mailMessage3.Body.ContainsIgnoreCase(request2.Body) && mailMessage3.AlternateViews.Count != 0)
													{
														request2.SavedUids.Add(uid);
														SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage3);
													}
												}
												IL_653:;
											}
										}
										if (request2.Sender != null && request2.Body == null && request2.Subject != null)
										{
											if (SchedulerSettings.Instance.DownloadMails)
											{
												Uid[] array2 = array;
												for (int i = 0; i < array2.Length; i++)
												{
													Uid uid = array2[i];
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage4;
														try
														{
															mailMessage4 = this._imapClient.FetchMessage(uid, true, true);
														}
														catch (Exception ex7)
														{
															if (ex7.Message == null || !ex7.Message.Contains(<Module>.smethod_2<string>(802576006)))
															{
																throw;
															}
															goto IL_7FE;
														}
														if (mailMessage4 != null && !string.IsNullOrEmpty(mailMessage4.From) && mailMessage4.From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(mailMessage4.Subject) && mailMessage4.Subject.ContainsIgnoreCase(request2.Subject))
														{
															try
															{
																mailMessage4 = this._imapClient.FetchMessage(uid, false, true);
															}
															catch (Exception ex8)
															{
																if (ex8.Message == null || !ex8.Message.Contains(<Module>.smethod_2<string>(802576006)))
																{
																	throw;
																}
																goto IL_7FE;
															}
															if (mailMessage4 != null && mailMessage4.AlternateViews.Count != 0)
															{
																request2.SavedUids.Add(uid);
																SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage4);
															}
														}
													}
													IL_7FE:;
												}
											}
										}
										else if (request2.Sender != null && request2.Body != null && request2.Subject == null)
										{
											if (SchedulerSettings.Instance.DownloadMails)
											{
												Uid[] array2 = array;
												for (int i = 0; i < array2.Length; i++)
												{
													Uid uid = array2[i];
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage5;
														try
														{
															mailMessage5 = ((request2.Errors == 0) ? this._imapClient.FetchFrom(uid) : this._imapClient.FetchMessage(uid, true, true));
														}
														catch (Exception ex9)
														{
															if (ex9.Message == null || !ex9.Message.Contains(<Module>.smethod_5<string>(1891727559)))
															{
																throw;
															}
															goto IL_9BA;
														}
														if (mailMessage5 != null && !string.IsNullOrEmpty(mailMessage5.From) && mailMessage5.From.ContainsIgnoreCase(request2.Sender))
														{
															try
															{
																mailMessage5 = this._imapClient.FetchMessage(uid, false, true);
															}
															catch (Exception ex10)
															{
																if (ex10.Message == null || !ex10.Message.Contains(<Module>.smethod_4<string>(-401647309)))
																{
																	throw;
																}
																goto IL_9BA;
															}
															if (mailMessage5 != null && !string.IsNullOrEmpty(mailMessage5.Body) && mailMessage5.Body.ContainsIgnoreCase(request2.Body) && mailMessage5.AlternateViews.Count != 0)
															{
																request2.SavedUids.Add(uid);
																SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage5);
															}
														}
													}
													IL_9BA:;
												}
											}
										}
										else if (request2.Sender == null && request2.Body != null && request2.Subject != null)
										{
											if (SchedulerSettings.Instance.DownloadMails)
											{
												Uid[] array2 = array;
												for (int i = 0; i < array2.Length; i++)
												{
													Uid uid = array2[i];
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage6;
														try
														{
															mailMessage6 = ((request2.Errors == 0) ? this._imapClient.FetchSubject(uid) : this._imapClient.FetchMessage(uid, true, true));
														}
														catch (Exception ex11)
														{
															if (ex11.Message == null || !ex11.Message.Contains(<Module>.smethod_3<string>(207274666)))
															{
																throw;
															}
															goto IL_B76;
														}
														if (mailMessage6 != null && !string.IsNullOrEmpty(mailMessage6.Subject) && mailMessage6.Subject.ContainsIgnoreCase(request2.Subject))
														{
															try
															{
																mailMessage6 = this._imapClient.FetchMessage(uid, false, true);
															}
															catch (Exception ex12)
															{
																if (ex12.Message == null || !ex12.Message.Contains(<Module>.smethod_5<string>(1891727559)))
																{
																	throw;
																}
																goto IL_B76;
															}
															if (mailMessage6 != null && !string.IsNullOrEmpty(mailMessage6.Body) && mailMessage6.Body.ContainsIgnoreCase(request2.Body) && mailMessage6.AlternateViews.Count != 0)
															{
																request2.SavedUids.Add(uid);
																SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage6);
															}
														}
													}
													IL_B76:;
												}
											}
										}
										else if (request2.Sender != null && request2.Body != null && request2.Subject != null && SchedulerSettings.Instance.DownloadMails)
										{
											Uid[] array2 = array;
											for (int i = 0; i < array2.Length; i++)
											{
												Uid uid = array2[i];
												if (!request2.SavedUids.Any((Uid u) => u == uid))
												{
													Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage mailMessage7;
													try
													{
														mailMessage7 = this._imapClient.FetchMessage(uid, true, true);
													}
													catch (Exception ex13)
													{
														if (ex13.Message == null || !ex13.Message.Contains(<Module>.smethod_3<string>(207274666)))
														{
															throw;
														}
														goto IL_D3E;
													}
													if (mailMessage7 != null && !string.IsNullOrEmpty(mailMessage7.From) && mailMessage7.From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(mailMessage7.Subject) && mailMessage7.Subject.ContainsIgnoreCase(request2.Subject))
													{
														try
														{
															mailMessage7 = this._imapClient.FetchMessage(uid, false, true);
														}
														catch (Exception ex14)
														{
															if (ex14.Message == null || !ex14.Message.Contains(<Module>.smethod_5<string>(1891727559)))
															{
																throw;
															}
															goto IL_D3E;
														}
														if (mailMessage7 != null && !string.IsNullOrEmpty(mailMessage7.Body) && mailMessage7.Body.ContainsIgnoreCase(request2.Body) && mailMessage7.AlternateViews.Count != 0)
														{
															request2.SavedUids.Add(uid);
															SchedulerFileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage7);
														}
													}
												}
												IL_D3E:;
											}
										}
									}
								}
							}
							finally
							{
								request4.IsChecked = true;
								request4.CheckedFolders.Add(this._folder.Name);
							}
						}
						catch (ThreadAbortException)
						{
							throw;
						}
						catch (Exception ex15)
						{
							if ((ex15 is IOException || ex15 is SocketException || ex15 is TimeoutException || ex15 is ArgumentNullException || ex15 is NullReferenceException) && request4 != null)
							{
								if (request4.Errors <= 2)
								{
									request4.Errors++;
									request4.IsChecked = false;
									return OperationResult.Retry;
								}
								request4.IsChecked = true;
								return OperationResult.Retry;
							}
						}
					}
				}
				if (SchedulerSettings.Instance.DeleteMails && this._context.IsInitialized)
				{
					try
					{
						HashSet<Uid> hashSet = new HashSet<Uid>();
						foreach (Request request3 in checkedRequests)
						{
							foreach (Uid item in (SchedulerSettings.Instance.DownloadMails ? request3.SavedUids : request3.FindedUids))
							{
								hashSet.Add(item);
							}
						}
						if (hashSet.Any<Uid>())
						{
							this._imapClient.DeleteMessages(hashSet.ToArray<Uid>());
						}
					}
					catch
					{
					}
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

		// Token: 0x06000E3B RID: 3643 RVA: 0x00048D58 File Offset: 0x00046F58
		private Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition BuildSearchCondition(Request request)
		{
			Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition searchCondition = new Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition();
			if (request.Sender != null)
			{
				searchCondition = Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.From(request.Sender);
				if (request.Subject != null)
				{
					searchCondition &= Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.Subject(request.Subject);
				}
				if (request.Body != null)
				{
					searchCondition &= Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.Body(request.Body);
				}
			}
			else if (request.Subject != null)
			{
				searchCondition = Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.Subject(request.Subject);
				if (request.Sender != null)
				{
					searchCondition &= Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.From(request.Sender);
				}
				if (request.Body != null)
				{
					searchCondition &= Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.Body(request.Body);
				}
			}
			else if (request.Body != null)
			{
				searchCondition = Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.Body(request.Body);
				if (request.Sender != null)
				{
					searchCondition &= Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.From(request.Sender);
				}
				if (request.Subject != null)
				{
					searchCondition &= Hackus_Mail_Checker_Reforged.Net.Mail.SearchCondition.Subject(request.Subject);
				}
			}
			return searchCondition;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00048E50 File Offset: 0x00047050
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
					this._imapClient.Dispose();
					return false;
				}
				this._imapClient.Dispose();
			}
			return false;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00048EC8 File Offset: 0x000470C8
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

		// Token: 0x06000E3E RID: 3646 RVA: 0x0000E3E5 File Offset: 0x0000C5E5
		public void Dispose()
		{
			this._imapClient.Dispose();
		}

		// Token: 0x04000796 RID: 1942
		private TcpClient _tcpClient;

		// Token: 0x04000797 RID: 1943
		private ScheduledMail _mailbox;

		// Token: 0x04000798 RID: 1944
		private Server _server;

		// Token: 0x04000799 RID: 1945
		private ImapClient _imapClient;

		// Token: 0x0400079A RID: 1946
		private ImapContext _context;

		// Token: 0x0400079B RID: 1947
		private Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder _folder;
	}
}
