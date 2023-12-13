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
	// Token: 0x02000043 RID: 67
	public partial class GlobalSettingsPage : Page
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000753D File Offset: 0x0000573D
		public GlobalSettingsPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}
	}
}
