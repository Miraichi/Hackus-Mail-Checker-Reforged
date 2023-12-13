using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Views
{
	// Token: 0x02000029 RID: 41
	public partial class ShortMainView : Window
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00006AA7 File Offset: 0x00004CA7
		public ShortMainView()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}
	}
}
