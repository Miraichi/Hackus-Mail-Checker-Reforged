using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001CA RID: 458
	public partial class UpdateAskPage : Page
	{
		// Token: 0x06000D74 RID: 3444 RVA: 0x0000DD30 File Offset: 0x0000BF30
		public UpdateAskPage(object dataContext)
		{
			this.InitializeComponent();
			base.DataContext = dataContext;
		}
	}
}
