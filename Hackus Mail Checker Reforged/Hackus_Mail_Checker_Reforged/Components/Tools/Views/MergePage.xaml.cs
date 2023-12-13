using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Tools.Views
{
	// Token: 0x020001A6 RID: 422
	public partial class MergePage : Page
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x0000D71C File Offset: 0x0000B91C
		public MergePage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
