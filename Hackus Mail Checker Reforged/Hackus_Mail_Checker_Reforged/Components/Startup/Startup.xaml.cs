using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using HandyControl.Controls;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001BD RID: 445
	public partial class Startup : System.Windows.Window
	{
		// Token: 0x06000CDA RID: 3290 RVA: 0x00043C2C File Offset: 0x00041E2C
		public Startup()
		{
			this.InitializeComponent();
			StartupViewModel startupViewModel = new StartupViewModel();
			startupViewModel.CloseStartupWindow += this.DataContext_CloseStartupWindow;
			startupViewModel.NextState += this.DataContext_NextState;
			base.DataContext = startupViewModel;
			PagesManager.Instance.RegisterFrame(this.StartupFrame, FrameType.Startup);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0000DA2E File Offset: 0x0000BC2E
		private void DataContext_CloseStartupWindow(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0000DA36 File Offset: 0x0000BC36
		private void DataContext_NextState(object sender, EventArgs e)
		{
			this.step.Next();
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00043C88 File Offset: 0x00041E88
		private void OnDrag(object sender, MouseButtonEventArgs e)
		{
			try
			{
				base.DragMove();
			}
			catch
			{
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0000DA2E File Offset: 0x0000BC2E
		private void OnClose(object sender, RoutedEventArgs e)
		{
			base.Close();
		}
	}
}
