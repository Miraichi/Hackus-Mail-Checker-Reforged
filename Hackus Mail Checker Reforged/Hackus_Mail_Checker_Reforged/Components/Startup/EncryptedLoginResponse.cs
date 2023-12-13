using System;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001BA RID: 442
	internal class EncryptedLoginResponse
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0000D96E File Offset: 0x0000BB6E
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0000D976 File Offset: 0x0000BB76
		public string Response { get; set; }

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0000D97F File Offset: 0x0000BB7F
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.Response);
		}
	}
}
