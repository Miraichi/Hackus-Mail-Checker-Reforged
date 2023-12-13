using System;
using System.Windows.Media;
using Hackus_Mail_Checker_Reforged.Components.Scheduler;
using Notification.Wpf;
using Notification.Wpf.Constants;

namespace Hackus_Mail_Checker_Reforged.Services.Shared
{
	// Token: 0x02000069 RID: 105
	public static class Notifier
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x00008C72 File Offset: 0x00006E72
		static Notifier()
		{
			NotificationConstants.MessageSize = 14.0;
			NotificationConstants.TitleSize = 14.0;
			NotificationConstants.NotificationsOverlayWindowMaxCount = 5U;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00017A2C File Offset: 0x00015C2C
		public static void ShowSchedulerNotification(string title, string message)
		{
			NotificationContent notificationContent = new NotificationContent();
			notificationContent.Title = title;
			notificationContent.Message = message;
			notificationContent.Type = 1;
			notificationContent.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(<Module>.smethod_4<string>(2118262795));
			notificationContent.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(<Module>.smethod_4<string>(-2089542752));
			Action action = new Action(SchedulerController.Instance.Show);
			NotificationManager notificationManager = Notifier._notificationManager;
			object obj = notificationContent;
			string text = "";
			Action action2 = action;
			notificationManager.Show(obj, text, new TimeSpan?(TimeSpan.FromSeconds(3.0)), action2, null, true, false);
		}

		// Token: 0x04000213 RID: 531
		private static readonly NotificationManager _notificationManager = new NotificationManager(null);
	}
}
