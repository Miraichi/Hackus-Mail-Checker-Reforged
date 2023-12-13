using System;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.MailRu
{
	// Token: 0x020000D9 RID: 217
	public class AttachmentInfo
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0000A10A File Offset: 0x0000830A
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x0000A112 File Offset: 0x00008312
		[JsonProperty("name")]
		public string Name { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0000A11B File Offset: 0x0000831B
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0000A123 File Offset: 0x00008323
		[JsonProperty("content_type")]
		public string ContentType { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0000A12C File Offset: 0x0000832C
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0000A134 File Offset: 0x00008334
		[JsonProperty("href")]
		public Href Href { get; set; }
	}
}
