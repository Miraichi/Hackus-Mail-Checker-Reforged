using System;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Services.Background
{
	// Token: 0x02000085 RID: 133
	internal static class BackgroundRestSaver
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00006D77 File Offset: 0x00004F77
		public static StatisticsManager StatisticsManager
		{
			get
			{
				return StatisticsManager.Instance;
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001ABE0 File Offset: 0x00018DE0
		public static void Start()
		{
			if (CheckerSettings.Instance.SaveRest)
			{
				int saveRestDelay = CheckerSettings.Instance.SaveRestDelay;
				BackgroundRestSaver._timer = new Timer(new TimerCallback(BackgroundRestSaver.SaveRest), null, saveRestDelay * 60000, saveRestDelay * 60000);
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000973B File Offset: 0x0000793B
		public static void Stop()
		{
			Timer timer = BackgroundRestSaver._timer;
			if (timer != null)
			{
				timer.Change(-1, -1);
			}
			Timer timer2 = BackgroundRestSaver._timer;
			if (timer2 == null)
			{
				return;
			}
			timer2.method_0();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000975F File Offset: 0x0000795F
		private static void SaveRest(object obj)
		{
			FileManager.SaveRest();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00009684 File Offset: 0x00007884
		void method_0()
		{
			base.Dispose();
		}

		// Token: 0x04000298 RID: 664
		private static Timer _timer;
	}
}
