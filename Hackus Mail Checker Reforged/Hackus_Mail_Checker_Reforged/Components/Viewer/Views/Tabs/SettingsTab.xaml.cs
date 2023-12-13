using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Views.Tabs
{
	// Token: 0x02000165 RID: 357
	public partial class SettingsTab : UserControl, IDisposable
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		public SettingsTab()
		{
			this.InitializeComponent();
			base.DataContext = new SettingsTabViewModel();
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0000C4F5 File Offset: 0x0000A6F5
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				((SettingsTabViewModel)base.DataContext).Dispose();
				this._isDisposed = true;
			}
		}

		// Token: 0x0400054D RID: 1357
		private bool _isDisposed;
	}
}
