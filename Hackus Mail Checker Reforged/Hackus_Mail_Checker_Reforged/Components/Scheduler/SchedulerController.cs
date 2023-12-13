using System;
using System.Linq;
using System.Windows;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.ViewModels;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Views;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler
{
	// Token: 0x020001D1 RID: 465
	public class SchedulerController
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0000DF0D File Offset: 0x0000C10D
		public static SchedulerController Instance
		{
			get
			{
				SchedulerController result;
				if ((result = SchedulerController._instance) == null)
				{
					result = (SchedulerController._instance = new SchedulerController());
				}
				return result;
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0000619C File Offset: 0x0000439C
		private SchedulerController()
		{
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x000460C0 File Offset: 0x000442C0
		public void Show()
		{
			if (this.IsOpen())
			{
				this._schedulerWindow.WindowState = WindowState.Normal;
				this._schedulerWindow.Activate();
				return;
			}
			if (this._schedulerViewModel == null)
			{
				this._schedulerViewModel = new SchedulerViewModel();
			}
			this._schedulerWindow = new SchedulerWindow();
			this._schedulerWindow.DataContext = this._schedulerViewModel;
			this._schedulerWindow.Show();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0000DF23 File Offset: 0x0000C123
		public void Close()
		{
			this._schedulerWindow.Close();
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0000DF30 File Offset: 0x0000C130
		private bool IsOpen()
		{
			return Application.Current.Windows.OfType<SchedulerWindow>().Any<SchedulerWindow>();
		}

		// Token: 0x0400074F RID: 1871
		private SchedulerWindow _schedulerWindow;

		// Token: 0x04000750 RID: 1872
		private SchedulerViewModel _schedulerViewModel;

		// Token: 0x04000751 RID: 1873
		private static SchedulerController _instance;
	}
}
