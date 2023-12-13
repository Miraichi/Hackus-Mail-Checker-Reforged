using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Overlays
{
	// Token: 0x0200005A RID: 90
	public partial class StatisticsDetailsPage : Page
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00007BCE File Offset: 0x00005DCE
		public StatisticsDetailsPage()
		{
			base.DataContext = MainViewModel.Instance;
			this.InitializeComponent();
		}
	}
}
