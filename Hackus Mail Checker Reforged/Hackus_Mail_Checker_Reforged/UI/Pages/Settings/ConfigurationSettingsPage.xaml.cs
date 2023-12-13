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
	// Token: 0x02000042 RID: 66
	public partial class ConfigurationSettingsPage : Page
	{
		// Token: 0x06000225 RID: 549 RVA: 0x000074F9 File Offset: 0x000056F9
		public ConfigurationSettingsPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}
	}
}
