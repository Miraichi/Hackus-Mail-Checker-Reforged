using System;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001B8 RID: 440
	internal class EncryptionKeyResponse
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0000D90C File Offset: 0x0000BB0C
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x0000D914 File Offset: 0x0000BB14
		public string Guid { get; set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0000D91D File Offset: 0x0000BB1D
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0000D925 File Offset: 0x0000BB25
		public string Key { get; set; }

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0000D92E File Offset: 0x0000BB2E
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.Key) && !string.IsNullOrEmpty(this.Guid);
		}
	}
}
