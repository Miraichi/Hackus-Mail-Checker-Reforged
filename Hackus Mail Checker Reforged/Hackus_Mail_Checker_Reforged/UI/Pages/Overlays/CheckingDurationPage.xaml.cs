using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Overlays
{
	// Token: 0x02000052 RID: 82
	public partial class CheckingDurationPage : Page
	{
		// Token: 0x06000287 RID: 647 RVA: 0x00007927 File Offset: 0x00005B27
		public CheckingDurationPage(TimeSpan duration)
		{
			this.InitializeComponent();
			base.DataContext = new DurationMessageViewModel(duration);
		}
	}
}
