using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Tools.Views
{
	// Token: 0x020001A7 RID: 423
	public partial class NormalizePage : Page
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x0000D74B File Offset: 0x0000B94B
		public NormalizePage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
