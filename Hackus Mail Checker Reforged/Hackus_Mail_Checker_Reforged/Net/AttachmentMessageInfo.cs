using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x0200009F RID: 159
	public class AttachmentMessageInfo
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00009A56 File Offset: 0x00007C56
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00009A5E File Offset: 0x00007C5E
		public string Uid { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00009A67 File Offset: 0x00007C67
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00009A6F File Offset: 0x00007C6F
		public List<string> Filenames { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00009A78 File Offset: 0x00007C78
		public bool HasAttachments
		{
			get
			{
				return this.Filenames != null && this.Filenames.Any<string>();
			}
		}
	}
}
