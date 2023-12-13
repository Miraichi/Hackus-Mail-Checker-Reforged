using System;
using System.Collections.Generic;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Onet
{
	// Token: 0x020000D7 RID: 215
	internal class SearchResponse
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0000A0B5 File Offset: 0x000082B5
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0000A0BD File Offset: 0x000082BD
		public int Total_count { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0000A0C6 File Offset: 0x000082C6
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0000A0CE File Offset: 0x000082CE
		public List<MidsResponse> Mails { get; set; }
	}
}
