using System;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000141 RID: 321
	internal class Settings
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x0003AFD8 File Offset: 0x000391D8
		public Settings(int version)
		{
			this.Version = version;
			this.CheckerSettings = CheckerSettings.Instance;
			this.SearchSettings = SearchSettings.Instance;
			this.ProxySettings = ProxySettings.Instance;
			this.ViewerSettings = ViewerSettings.Instance;
			this.WebSettings = WebSettings.Instance;
			this.SchedulerSettings = SchedulerSettings.Instance;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x0000C166 File Offset: 0x0000A366
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x0000C16E File Offset: 0x0000A36E
		public int Version { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0000C177 File Offset: 0x0000A377
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x0000C17F File Offset: 0x0000A37F
		public CheckerSettings CheckerSettings { get; set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0000C188 File Offset: 0x0000A388
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x0000C190 File Offset: 0x0000A390
		public SearchSettings SearchSettings { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0000C199 File Offset: 0x0000A399
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x0000C1A1 File Offset: 0x0000A3A1
		public ProxySettings ProxySettings { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0000C1AA File Offset: 0x0000A3AA
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x0000C1B2 File Offset: 0x0000A3B2
		public ViewerSettings ViewerSettings { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0000C1BB File Offset: 0x0000A3BB
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0000C1C3 File Offset: 0x0000A3C3
		public WebSettings WebSettings { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0000C1CC File Offset: 0x0000A3CC
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0000C1D4 File Offset: 0x0000A3D4
		public SchedulerSettings SchedulerSettings { get; set; }
	}
}
