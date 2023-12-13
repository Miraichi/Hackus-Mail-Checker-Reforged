using System;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001BB RID: 443
	internal class LoginResponse
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0000D98F File Offset: 0x0000BB8F
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0000D997 File Offset: 0x0000BB97
		public string Token { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		public string Key { get; set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0000D9B1 File Offset: 0x0000BBB1
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x0000D9B9 File Offset: 0x0000BBB9
		public string DatabasePath { get; set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0000D9C2 File Offset: 0x0000BBC2
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0000D9CA File Offset: 0x0000BBCA
		public string DatabaseVersion { get; set; }

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0000D9D3 File Offset: 0x0000BBD3
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.Token) && !string.IsNullOrEmpty(this.Key) && !string.IsNullOrEmpty(this.DatabasePath) && !string.IsNullOrEmpty(this.DatabaseVersion);
		}
	}
}
