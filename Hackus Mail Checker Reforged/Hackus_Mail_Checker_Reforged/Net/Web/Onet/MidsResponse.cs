using System;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Onet
{
	// Token: 0x020000D8 RID: 216
	internal class MidsResponse
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0000A0D7 File Offset: 0x000082D7
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x0000A0DF File Offset: 0x000082DF
		public long Mid { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0000A0E8 File Offset: 0x000082E8
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0000A0F0 File Offset: 0x000082F0
		public string From { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0000A0F9 File Offset: 0x000082F9
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0000A101 File Offset: 0x00008301
		public string Subject { get; set; }
	}
}
