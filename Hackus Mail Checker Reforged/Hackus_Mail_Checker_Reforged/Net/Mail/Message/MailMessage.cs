using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x02000113 RID: 275
	public class MailMessage : IDisposable
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0000B0E3 File Offset: 0x000092E3
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x0000B0EB File Offset: 0x000092EB
		public NameValueCollection Headers { get; internal set; } = new NameValueCollection();

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0000B0F4 File Offset: 0x000092F4
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0000B0FC File Offset: 0x000092FC
		public HashSet<Attachment> AlternateViews { get; set; } = new HashSet<Attachment>();

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0000B105 File Offset: 0x00009305
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x0000B10D File Offset: 0x0000930D
		public HashSet<Attachment> AdditionalFiles { get; set; } = new HashSet<Attachment>();

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0000B116 File Offset: 0x00009316
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0000B11E File Offset: 0x0000931E
		public string Subject { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0000B127 File Offset: 0x00009327
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x0000B12F File Offset: 0x0000932F
		public string From { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0000B138 File Offset: 0x00009338
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0000B140 File Offset: 0x00009340
		public Folder Folder { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0000B149 File Offset: 0x00009349
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x0000B151 File Offset: 0x00009351
		public DateTime Date { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0000B15A File Offset: 0x0000935A
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x0000B162 File Offset: 0x00009362
		public string RawDate { get; set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0000B16B File Offset: 0x0000936B
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x0000B173 File Offset: 0x00009373
		public Uid Uid { get; set; }

		// Token: 0x0600087D RID: 2173 RVA: 0x0000B17C File Offset: 0x0000937C
		public void Dispose()
		{
			this.Headers.Clear();
			this.Headers = null;
			this.AlternateViews.Clear();
			this.AlternateViews = null;
			this.AdditionalFiles.Clear();
			this.AdditionalFiles = null;
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00033B90 File Offset: 0x00031D90
		public string Body
		{
			get
			{
				Attachment attachment = new Attachment();
				int count = this.AlternateViews.Count;
				if (count == 0)
				{
					return null;
				}
				if (count == 1)
				{
					return (string)this.AlternateViews.First<Attachment>().Body;
				}
				if (SearchSettings.Instance.DownloadMode == DownloadMode.Plain)
				{
					if (this.AlternateViews.Any((Attachment v) => MailMessage.<>c.smethod_0(v.ContentType, <Module>.smethod_5<string>(93151217))))
					{
						attachment = this.AlternateViews.FirstOrDefault((Attachment v) => MailMessage.<>c.smethod_0(v.ContentType, <Module>.smethod_5<string>(93151217)));
					}
					else
					{
						attachment = this.AlternateViews.FirstOrDefault((Attachment v) => MailMessage.<>c.smethod_0(v.ContentType, <Module>.smethod_4<string>(1173908513)));
					}
				}
				else if (this.AlternateViews.Any((Attachment v) => MailMessage.<>c.smethod_0(v.ContentType, <Module>.smethod_2<string>(642151244))))
				{
					attachment = this.AlternateViews.FirstOrDefault((Attachment v) => MailMessage.<>c.smethod_0(v.ContentType, <Module>.smethod_5<string>(473053949)));
				}
				else
				{
					attachment = this.AlternateViews.FirstOrDefault((Attachment v) => MailMessage.<>c.smethod_0(v.ContentType, <Module>.smethod_6<string>(955020623)));
				}
				if (!string.IsNullOrEmpty((string)attachment.Body))
				{
					return (string)attachment.Body;
				}
				return string.Empty;
			}
		}
	}
}
