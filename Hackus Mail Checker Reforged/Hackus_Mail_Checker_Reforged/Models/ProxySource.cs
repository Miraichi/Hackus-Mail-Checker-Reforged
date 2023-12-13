using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000136 RID: 310
	internal class ProxySource
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x0000BDEE File Offset: 0x00009FEE
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0000BDF6 File Offset: 0x00009FF6
		public ProxySourceType Type { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0000BDFF File Offset: 0x00009FFF
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0000BE07 File Offset: 0x0000A007
		public string Path { get; set; }
	}
}
