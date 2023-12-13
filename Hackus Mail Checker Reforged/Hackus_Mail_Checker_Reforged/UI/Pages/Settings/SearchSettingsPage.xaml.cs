using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Settings
{
	// Token: 0x02000046 RID: 70
	public partial class SearchSettingsPage : Page
	{
		// Token: 0x06000233 RID: 563 RVA: 0x00007584 File Offset: 0x00005784
		public SearchSettingsPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}
	}
}
