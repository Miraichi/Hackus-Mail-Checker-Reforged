using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Tools.Views
{
	// Token: 0x020001A9 RID: 425
	public partial class SortDomainsPage : Page
	{
		// Token: 0x06000C7E RID: 3198 RVA: 0x0000D787 File Offset: 0x0000B987
		public SortDomainsPage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
