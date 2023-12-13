using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Views
{
	// Token: 0x020001D4 RID: 468
	public partial class SchedulerWindow : Window
	{
		// Token: 0x06000DB7 RID: 3511 RVA: 0x0000DFB9 File Offset: 0x0000C1B9
		public SchedulerWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00006A55 File Offset: 0x00004C55
		private void OnHide(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0000DFC7 File Offset: 0x0000C1C7
		private void OnClose(object sender, RoutedEventArgs e)
		{
			SchedulerController.Instance.Close();
		}
	}
}
