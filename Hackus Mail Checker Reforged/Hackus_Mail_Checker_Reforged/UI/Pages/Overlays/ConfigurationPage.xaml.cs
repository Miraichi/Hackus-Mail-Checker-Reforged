using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Overlays
{
	// Token: 0x02000053 RID: 83
	public partial class ConfigurationPage : Page
	{
		// Token: 0x0600028A RID: 650 RVA: 0x0000794A File Offset: 0x00005B4A
		public ConfigurationPage()
		{
			this.InitializeComponent();
			base.DataContext = new ConfigurationViewModel();
		}
	}
}
