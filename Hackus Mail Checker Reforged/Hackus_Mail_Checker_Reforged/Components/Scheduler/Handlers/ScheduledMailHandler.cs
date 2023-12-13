using System;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Handlers
{
	// Token: 0x020001E2 RID: 482
	public class ScheduledMailHandler
	{
		// Token: 0x06000E30 RID: 3632 RVA: 0x0000E3CF File Offset: 0x0000C5CF
		public ScheduledMailHandler(ScheduledMail mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0004774C File Offset: 0x0004594C
		public OperationResult Handle()
		{
			if (this._mailbox != null && this._server != null)
			{
				if (this._server.Protocol == ProtocolType.IMAP)
				{
					this._mailHandler = new ScheduledImapHandler(this._mailbox, this._server);
				}
				else if (this._server.Protocol == ProtocolType.POP3)
				{
					this._mailHandler = new ScheduledPop3Handler(this._mailbox, this._server);
				}
				return this.Check();
			}
			return OperationResult.Error;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000477C0 File Offset: 0x000459C0
		private OperationResult Check()
		{
			OperationResult operationResult = OperationResult.Ok;
			for (int i = 0; i < 10; i++)
			{
				if (SchedulerSettings.Instance.UseProxy)
				{
					OperationResult operationResult2 = this.CreateConnection();
					if (operationResult2 != OperationResult.HostNotFound)
					{
						if (operationResult2 != OperationResult.Error)
						{
							goto IL_26;
						}
					}
					return operationResult2;
				}
				IL_26:
				ExceptionType exceptionType;
				operationResult = this._mailHandler.Login(out exceptionType);
				if (operationResult == OperationResult.Ok)
				{
					this._mailHandler.SearchMessages();
					this._mailHandler.Dispose();
					return OperationResult.Ok;
				}
				if (operationResult != OperationResult.Error)
				{
					if (operationResult == OperationResult.ServerDisabled)
					{
						operationResult = OperationResult.Blocked;
					}
					if (operationResult == OperationResult.HostNotFound)
					{
						operationResult = OperationResult.Error;
					}
					this._mailHandler.Dispose();
					return operationResult;
				}
				this._mailHandler.Dispose();
			}
			return operationResult;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0004785C File Offset: 0x00045A5C
		private OperationResult CreateConnection()
		{
			OperationResult operationResult;
			do
			{
				ProxyClient proxy = ProxyManager.Instance.GetProxy();
				operationResult = this._mailHandler.Connect(proxy);
			}
			while (operationResult == OperationResult.Error);
			return operationResult;
		}

		// Token: 0x04000793 RID: 1939
		private IMailHandler _mailHandler;

		// Token: 0x04000794 RID: 1940
		private ScheduledMail _mailbox;

		// Token: 0x04000795 RID: 1941
		private Server _server;
	}
}
