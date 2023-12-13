using System;
using System.Diagnostics;
using System.Windows;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.UI.Pages.Overlays;

namespace Hackus_Mail_Checker_Reforged.Services.Background
{
	// Token: 0x02000087 RID: 135
	internal static class BackgroundStopwatch
	{
		// Token: 0x060004BE RID: 1214 RVA: 0x000097AD File Offset: 0x000079AD
		public static void Start()
		{
			BackgroundStopwatch._stopwatch.Restart();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000097B9 File Offset: 0x000079B9
		public static void Resume()
		{
			if (BackgroundStopwatch._stopwatch != null && !BackgroundStopwatch._stopwatch.IsRunning)
			{
				BackgroundStopwatch._stopwatch.Start();
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000097D8 File Offset: 0x000079D8
		public static void Stop()
		{
			if (BackgroundStopwatch._stopwatch != null && BackgroundStopwatch._stopwatch.IsRunning)
			{
				BackgroundStopwatch._stopwatch.Stop();
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x000097F7 File Offset: 0x000079F7
		public static void Show()
		{
			if (BackgroundStopwatch._stopwatch != null)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					PagesManager.Instance.OpenPage(new CheckingDurationPage(BackgroundStopwatch.<>c.smethod_0(BackgroundStopwatch._stopwatch)), FrameType.MainOverlay);
				});
			}
		}

		// Token: 0x0400029A RID: 666
		private static Stopwatch _stopwatch = new Stopwatch();
	}
}
