using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Tools.Views
{
	// Token: 0x020001AA RID: 426
	public partial class SortPage : Page
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x0000D7A5 File Offset: 0x0000B9A5
		public SortPage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
