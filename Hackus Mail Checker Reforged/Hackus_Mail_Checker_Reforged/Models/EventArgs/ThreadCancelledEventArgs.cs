using System;
using System.Threading;

namespace Hackus_Mail_Checker_Reforged.Models.EventArgs
{
	// Token: 0x02000143 RID: 323
	internal class ThreadCancelledEventArgs : EventArgs
	{
		// Token: 0x06000A0B RID: 2571 RVA: 0x0000C1DD File Offset: 0x0000A3DD
		public ThreadCancelledEventArgs()
		{
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0000C1E5 File Offset: 0x0000A3E5
		public ThreadCancelledEventArgs(Thread thread)
		{
			this.Thread = thread;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0000C1F4 File Offset: 0x0000A3F4
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public Thread Thread { get; set; }
	}
}
