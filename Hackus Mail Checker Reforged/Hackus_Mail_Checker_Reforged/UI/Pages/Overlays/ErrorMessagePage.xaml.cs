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
	// Token: 0x02000056 RID: 86
	public partial class ErrorMessagePage : Page
	{
		// Token: 0x06000293 RID: 659 RVA: 0x000079C9 File Offset: 0x00005BC9
		public ErrorMessagePage()
		{
			this.InitializeComponent();
			base.DataContext = new ErrorMessageViewModel();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000079E2 File Offset: 0x00005BE2
		public ErrorMessagePage(string message)
		{
			this.InitializeComponent();
			base.DataContext = new ErrorMessageViewModel(message);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000079FC File Offset: 0x00005BFC
		public ErrorMessagePage(string message, Action actionAfterClose)
		{
			this.InitializeComponent();
			base.DataContext = new ErrorMessageViewModel(message, actionAfterClose);
		}
	}
}
