using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Hackus_Mail_Checker_Reforged.Components.Tools.Views
{
	// Token: 0x020001A8 RID: 424
	public partial class ShufflePage : Page
	{
		// Token: 0x06000C7B RID: 3195 RVA: 0x0000D769 File Offset: 0x0000B969
		public ShufflePage(object dataContext)
		{
			base.DataContext = dataContext;
			this.InitializeComponent();
		}
	}
}
