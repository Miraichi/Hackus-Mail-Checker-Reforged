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
	// Token: 0x02000059 RID: 89
	public partial class RequestsManagerPage : Page
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x00007BAC File Offset: 0x00005DAC
		public RequestsManagerPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}
	}
}
