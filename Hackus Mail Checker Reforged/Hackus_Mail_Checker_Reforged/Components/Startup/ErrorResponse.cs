using System;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001B9 RID: 441
	internal class ErrorResponse
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0000D94D File Offset: 0x0000BB4D
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x0000D955 File Offset: 0x0000BB55
		public string Error { get; set; }

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0000D95E File Offset: 0x0000BB5E
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.Error);
		}
	}
}
