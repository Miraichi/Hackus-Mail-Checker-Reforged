using System;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Handlers;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler
{
	// Token: 0x020001CE RID: 462
	public static class ScheduledWorker
	{
		// Token: 0x06000D8F RID: 3471 RVA: 0x00045AFC File Offset: 0x00043CFC
		public static void Run()
		{
			for (;;)
			{
				try
				{
					ScheduledMail scheduledMail;
					if (!Scheduler.Instance.WaitingMails.TryDequeue(out scheduledMail))
					{
						Thread.Sleep(500);
						continue;
					}
					scheduledMail.Status = MailStatus.Processing;
					Server server = ConfigurationManager.Instance.Configuration.FindInDatabase(scheduledMail.GetDomain());
					if (server == null)
					{
						scheduledMail.Status = MailStatus.Error;
					}
					else
					{
						OperationResult operationResult = new ScheduledMailHandler(scheduledMail, server).Handle();
						scheduledMail.LastExecuted = new DateTime?(DateTime.Now);
						if (operationResult == OperationResult.Ok)
						{
							scheduledMail.Status = MailStatus.Stopped;
						}
						else if (operationResult != OperationResult.Bad && operationResult != OperationResult.Blocked)
						{
							scheduledMail.Status = MailStatus.Error;
						}
						else
						{
							scheduledMail.Status = MailStatus.Invalid;
						}
					}
					continue;
				}
				catch (ThreadAbortException)
				{
				}
				catch
				{
					continue;
				}
				break;
			}
		}
	}
}
