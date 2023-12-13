using System;
using System.Collections.Generic;
using Hackus_Mail_Checker_Reforged.Helpers;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Yahoo
{
	// Token: 0x020000C7 RID: 199
	internal class Headers
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00009F03 File Offset: 0x00008103
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x00009F0B File Offset: 0x0000810B
		public List<From> From { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00009F14 File Offset: 0x00008114
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x00009F1C File Offset: 0x0000811C
		public string Date { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00009F25 File Offset: 0x00008125
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x00009F2D File Offset: 0x0000812D
		public string Subject { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00009F36 File Offset: 0x00008136
		[JsonIgnore]
		public DateTime DateTime
		{
			get
			{
				return DateHelpers.UnixTimeStampToDate(long.Parse(this.Date));
			}
		}
	}
}
