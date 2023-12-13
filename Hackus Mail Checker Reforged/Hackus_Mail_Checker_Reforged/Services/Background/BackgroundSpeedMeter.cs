using System;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using HandyControl.Tools;

namespace Hackus_Mail_Checker_Reforged.Services.Background
{
	// Token: 0x02000086 RID: 134
	internal static class BackgroundSpeedMeter
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x00009766 File Offset: 0x00007966
		public static void Start()
		{
			BackgroundSpeedMeter._timer = new Timer(new TimerCallback(BackgroundSpeedMeter.CheckSpeed), null, 60000, 60000);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00009789 File Offset: 0x00007989
		public static void Stop()
		{
			Timer timer = BackgroundSpeedMeter._timer;
			if (timer != null)
			{
				timer.Change(-1, -1);
			}
			Timer timer2 = BackgroundSpeedMeter._timer;
			if (timer2 == null)
			{
				return;
			}
			timer2.method_0();
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001AC2C File Offset: 0x00018E2C
		private static void CheckSpeed(object obj)
		{
			StatisticsManager.Instance.Speed = StatisticsManager.Instance.CheckedStrings - StatisticsManager.Instance.LastCheckedStrings;
			StatisticsManager.Instance.LastCheckedStrings = StatisticsManager.Instance.CheckedStrings;
			if (StatisticsManager.Instance.Speed > StatisticsManager.Instance.MaxSpeed)
			{
				StatisticsManager.Instance.MaxSpeed = StatisticsManager.Instance.Speed;
			}
			if (StatisticsManager.Instance.LastCheckedStrings > 0 && StatisticsManager.Instance.Speed > 0)
			{
				TimeSpan timeSpan = TimeSpan.FromMinutes((double)((MailManager.Instance.Count() - StatisticsManager.Instance.LastCheckedStrings) / StatisticsManager.Instance.Speed));
				string text = ResourceHelper.GetResource<string>(<Module>.smethod_4<string>(1818310996)) + <Module>.smethod_5<string>(-1400629406);
				if (timeSpan.Days > 0)
				{
					text += string.Format(<Module>.smethod_3<string>(-454507766), timeSpan.Days);
				}
				if (timeSpan.Hours > 0)
				{
					text += string.Format(<Module>.smethod_4<string>(-2114808361), timeSpan.Hours);
				}
				if (timeSpan.Minutes > 0)
				{
					text += string.Format(<Module>.smethod_5<string>(-120642929), timeSpan.Minutes);
				}
				if (timeSpan.Seconds > 0)
				{
					text += string.Format(<Module>.smethod_2<string>(-1585619158), timeSpan.Seconds);
				}
				StatisticsManager.Instance.EstimatedCompletionTime = text;
				return;
			}
			StatisticsManager.Instance.EstimatedCompletionTime = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(-640823942)) + <Module>.smethod_2<string>(-649890199);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00009684 File Offset: 0x00007884
		void method_0()
		{
			base.Dispose();
		}

		// Token: 0x04000299 RID: 665
		private static Timer _timer;
	}
}
