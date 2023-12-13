using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using HandyControl.Controls;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Popups
{
	// Token: 0x02000048 RID: 72
	public partial class AddRequestsPopup : UserControl, IDisposable, ISingleOpen
	{
		// Token: 0x06000239 RID: 569 RVA: 0x000075D0 File Offset: 0x000057D0
		public AddRequestsPopup()
		{
			this.InitializeComponent();
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600023A RID: 570 RVA: 0x000167FC File Offset: 0x000149FC
		// (remove) Token: 0x0600023B RID: 571 RVA: 0x00016838 File Offset: 0x00014A38
		public event EventHandler Canceled;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600023C RID: 572 RVA: 0x000075DE File Offset: 0x000057DE
		public bool CanDispose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000075E1 File Offset: 0x000057E1
		public void Dispose()
		{
			System.Windows.Window.GetWindow(this).Close();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000075EE File Offset: 0x000057EE
		private void Close(object sender, RoutedEventArgs e)
		{
			EventHandler canceled = this.Canceled;
			if (canceled == null)
			{
				return;
			}
			canceled(this, EventArgs.Empty);
		}
	}
}
