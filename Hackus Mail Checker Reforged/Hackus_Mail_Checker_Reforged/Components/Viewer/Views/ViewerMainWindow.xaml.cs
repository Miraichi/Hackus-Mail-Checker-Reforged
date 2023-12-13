using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using HandyControl.Controls;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Views
{
	// Token: 0x02000161 RID: 353
	public partial class ViewerMainWindow : System.Windows.Window
	{
		// Token: 0x06000A3A RID: 2618 RVA: 0x0000C2D3 File Offset: 0x0000A4D3
		public ViewerMainWindow()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00006A55 File Offset: 0x00004C55
		private void OnHide(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0000C2E1 File Offset: 0x0000A4E1
		private void OnClose(object sender, RoutedEventArgs e)
		{
			ViewerController.Instance.Close();
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0000C2ED File Offset: 0x0000A4ED
		private void Tabs_Closed(object sender, EventArgs e)
		{
			RoutedEventArgs routedEventArgs = e as RoutedEventArgs;
			IDisposable disposable = (((routedEventArgs != null) ? routedEventArgs.OriginalSource : null) as System.Windows.Controls.TabItem).Content as IDisposable;
			if (disposable == null)
			{
				return;
			}
			disposable.Dispose();
		}
	}
}
