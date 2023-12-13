using System;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Yandex
{
	// Token: 0x020000C0 RID: 192
	public class AttachmentInfo
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x00009DBF File Offset: 0x00007FBF
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00009DC7 File Offset: 0x00007FC7
		[JsonProperty("name")]
		public string Name { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00009DD0 File Offset: 0x00007FD0
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00009DD8 File Offset: 0x00007FD8
		[JsonProperty("hid")]
		public string Hid { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00009DE1 File Offset: 0x00007FE1
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x00009DE9 File Offset: 0x00007FE9
		public decimal Ids { get; set; }
	}
}
