using System;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Rambler
{
	// Token: 0x020000CD RID: 205
	public class AttachmentInfo
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0000A010 File Offset: 0x00008210
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0000A018 File Offset: 0x00008218
		[JsonProperty("name")]
		public string Name { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0000A021 File Offset: 0x00008221
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0000A029 File Offset: 0x00008229
		[JsonProperty("download_url")]
		public string Url { get; set; }
	}
}
