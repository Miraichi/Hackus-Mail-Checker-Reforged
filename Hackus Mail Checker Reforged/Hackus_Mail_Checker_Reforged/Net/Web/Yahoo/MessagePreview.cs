using System;
using System.Collections.Generic;
using Hackus_Mail_Checker_Reforged.Helpers;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Yahoo
{
	// Token: 0x020000C2 RID: 194
	internal class MessagePreview
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00009E36 File Offset: 0x00008036
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00009E3E File Offset: 0x0000803E
		public string Mid { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00009E47 File Offset: 0x00008047
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x00009E4F File Offset: 0x0000804F
		public string Subject { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00009E58 File Offset: 0x00008058
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x00009E60 File Offset: 0x00008060
		public List<PreviewFrom> FromList { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00009E69 File Offset: 0x00008069
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x00009E71 File Offset: 0x00008071
		public List<AttachmentInfo> Attachments { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00009E7A File Offset: 0x0000807A
		// (set) Token: 0x0600062E RID: 1582 RVA: 0x00009E82 File Offset: 0x00008082
		public string CreationDate { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00009E8B File Offset: 0x0000808B
		[JsonIgnore]
		public DateTime DateTime
		{
			get
			{
				return DateHelpers.UnixTimeStampToDateInMilliseconds(long.Parse(this.CreationDate));
			}
		}
	}
}
