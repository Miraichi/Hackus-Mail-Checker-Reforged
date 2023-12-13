using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Tools.Views
{
	// Token: 0x020001A5 RID: 421
	public partial class DistinctPage : Page
	{
		// Token: 0x06000C72 RID: 3186 RVA: 0x0000D6FE File Offset: 0x0000B8FE
		public DistinctPage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
