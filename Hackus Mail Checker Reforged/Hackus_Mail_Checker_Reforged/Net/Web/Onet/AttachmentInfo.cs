using System;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Onet
{
	// Token: 0x020000D6 RID: 214
	public class AttachmentInfo
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0000A093 File Offset: 0x00008293
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0000A09B File Offset: 0x0000829B
		[JsonProperty("content-url")]
		public string Url { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0000A0A4 File Offset: 0x000082A4
		// (set) Token: 0x06000681 RID: 1665 RVA: 0x0000A0AC File Offset: 0x000082AC
		[JsonProperty("filename")]
		public string Name { get; set; }
	}
}
