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
	// Token: 0x02000166 RID: 358
	public partial class ToolsTab : UserControl, IDisposable
	{
		// Token: 0x06000A66 RID: 2662 RVA: 0x0000C530 File Offset: 0x0000A730
		public ToolsTab()
		{
			this.InitializeComponent();
			base.DataContext = new ToolsTabViewModel(this.ContentFrame);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0000C54F File Offset: 0x0000A74F
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				((ToolsTabViewModel)base.DataContext).Dispose();
				this._isDisposed = true;
			}
		}

		// Token: 0x04000550 RID: 1360
		private bool _isDisposed;
	}
}
