using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001AB RID: 427
	public partial class AuthenticationPage : Page
	{
		// Token: 0x06000C84 RID: 3204 RVA: 0x0000D7C3 File Offset: 0x0000B9C3
		public AuthenticationPage(object dataContext)
		{
			this.InitializeComponent();
			base.DataContext = dataContext;
		}
	}
}
