using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.UI.Pages.Popups.ViewModels;
using HandyControl.Controls;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Popups
{
	// Token: 0x02000049 RID: 73
	public partial class SearchGroups : UserControl, IDisposable, ISingleOpen
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0000762B File Offset: 0x0000582B
		public SearchGroups()
		{
			this.InitializeComponent();
			base.DataContext = new SearchGroupsViewModel();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000242 RID: 578 RVA: 0x000168A8 File Offset: 0x00014AA8
		// (remove) Token: 0x06000243 RID: 579 RVA: 0x000168E4 File Offset: 0x00014AE4
		public event EventHandler Canceled;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000075DE File Offset: 0x000057DE
		public bool CanDispose
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000075E1 File Offset: 0x000057E1
		public void Dispose()
		{
			System.Windows.Window.GetWindow(this).Close();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00007644 File Offset: 0x00005844
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
