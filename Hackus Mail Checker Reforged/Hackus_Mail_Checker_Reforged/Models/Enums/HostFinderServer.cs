using System;

namespace Hackus_Mail_Checker_Reforged.Models.Enums
{
	// Token: 0x0200014D RID: 333
	internal class HostFinderServer
	{
		// Token: 0x06000A0F RID: 2575 RVA: 0x0000619C File Offset: 0x0000439C
		public HostFinderServer()
		{
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0000C205 File Offset: 0x0000A405
		public HostFinderServer(HostFinderStatus status)
		{
			this.Status = status;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0000C214 File Offset: 0x0000A414
		public HostFinderServer(Server server, HostFinderStatus status)
		{
			this.Server = server;
			this.Status = status;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0000C22A File Offset: 0x0000A42A
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0000C232 File Offset: 0x0000A432
		public Server Server { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0000C23B File Offset: 0x0000A43B
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0000C243 File Offset: 0x0000A443
		public HostFinderStatus Status { get; set; }
	}
}
