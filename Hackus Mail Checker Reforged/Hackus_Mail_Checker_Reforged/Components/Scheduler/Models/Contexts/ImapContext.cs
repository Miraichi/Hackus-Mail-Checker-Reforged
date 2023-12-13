using System;
using System.Collections.Generic;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Models.Contexts
{
	// Token: 0x020001DF RID: 479
	public class ImapContext : MailContext
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0000E381 File Offset: 0x0000C581
		// (set) Token: 0x06000E28 RID: 3624 RVA: 0x0000E389 File Offset: 0x0000C589
		public List<Uid> CheckedUids { get; set; } = new List<Uid>();
	}
}
