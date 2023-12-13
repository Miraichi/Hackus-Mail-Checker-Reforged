using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs;
using Hackus_Mail_Checker_Reforged.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Views.Tabs
{
	// Token: 0x02000162 RID: 354
	public partial class EmptyTab : UserControl, IDisposable
	{
		// Token: 0x06000A40 RID: 2624 RVA: 0x0000C31A File Offset: 0x0000A51A
		public EmptyTab()
		{
			this.InitializeComponent();
			base.DataContext = new EmptyTabViewModel();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0000C333 File Offset: 0x0000A533
		public EmptyTab(MailboxResult mailboxResult)
		{
			this.InitializeComponent();
			base.DataContext = new EmptyTabViewModel(mailboxResult);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000C34D File Offset: 0x0000A54D
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				((EmptyTabViewModel)base.DataContext).Dispose();
				base.DataContext = null;
				this._isDisposed = true;
			}
		}

		// Token: 0x04000535 RID: 1333
		private bool _isDisposed;
	}
}
