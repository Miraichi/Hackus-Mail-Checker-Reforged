using System;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Yahoo
{
	// Token: 0x020000C1 RID: 193
	public class AttachmentInfo
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00009DF2 File Offset: 0x00007FF2
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00009DFA File Offset: 0x00007FFA
		[JsonProperty("filename")]
		public string Name { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00009E03 File Offset: 0x00008003
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00009E0B File Offset: 0x0000800B
		[JsonProperty("downloadUrl")]
		public string Url { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00009E14 File Offset: 0x00008014
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00009E1C File Offset: 0x0000801C
		[JsonProperty("name")]
		public string SearchName { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00009E25 File Offset: 0x00008025
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x00009E2D File Offset: 0x0000802D
		[JsonProperty("downloadLink")]
		public string SearchUrl { get; set; }
	}
}
