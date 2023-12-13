using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001B7 RID: 439
	public partial class LoadingPage : Page
	{
		// Token: 0x06000CBA RID: 3258 RVA: 0x0000D8EE File Offset: 0x0000BAEE
		public LoadingPage(object dataContext)
		{
			this.InitializeComponent();
			base.DataContext = dataContext;
		}
	}
}
