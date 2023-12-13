using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001CD RID: 461
	public partial class WebLoadingPage : Page
	{
		// Token: 0x06000D8C RID: 3468 RVA: 0x0000DE1B File Offset: 0x0000C01B
		public WebLoadingPage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
