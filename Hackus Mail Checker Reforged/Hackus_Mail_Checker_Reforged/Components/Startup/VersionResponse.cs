using System;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001BC RID: 444
	internal class VersionResponse
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0000DA0C File Offset: 0x0000BC0C
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x0000DA14 File Offset: 0x0000BC14
		public bool IsLastVersion { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0000DA1D File Offset: 0x0000BC1D
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x0000DA25 File Offset: 0x0000BC25
		public string LastVersion { get; set; }
	}
}
