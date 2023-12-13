using System;
using System.Collections.Specialized;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x02000118 RID: 280
	public class MimePart
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0000B311 File Offset: 0x00009511
		// (set) Token: 0x060008AC RID: 2220 RVA: 0x0000B319 File Offset: 0x00009519
		public NameValueCollection Headers { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0000B322 File Offset: 0x00009522
		// (set) Token: 0x060008AE RID: 2222 RVA: 0x0000B32A File Offset: 0x0000952A
		public string Body { get; set; }
	}
}
