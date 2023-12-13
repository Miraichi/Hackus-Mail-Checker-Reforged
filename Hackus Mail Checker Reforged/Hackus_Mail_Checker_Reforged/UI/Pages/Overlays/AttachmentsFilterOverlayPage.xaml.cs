using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Overlays
{
	// Token: 0x02000054 RID: 84
	public partial class AttachmentsFilterOverlayPage : Page
	{
		// Token: 0x0600028D RID: 653 RVA: 0x00007963 File Offset: 0x00005B63
		public AttachmentsFilterOverlayPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}
	}
}
