using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.UI.Views;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Notifications
{
	// Token: 0x0200005C RID: 92
	public partial class CompleteNotification : UserControl
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x00007C09 File Offset: 0x00005E09
		public CompleteNotification()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00017764 File Offset: 0x00015964
		private void Show(object sender, RoutedEventArgs e)
		{
			foreach (object obj in Application.Current.Windows)
			{
				MainView mainView = obj as MainView;
				if (mainView != null)
				{
					mainView.ShowInTaskbar = true;
					mainView.NotifyIconElement.Visibility = Visibility.Collapsed;
					mainView.WindowState = WindowState.Normal;
					mainView.Activate();
					base.Visibility = Visibility.Collapsed;
				}
			}
		}
	}
}
