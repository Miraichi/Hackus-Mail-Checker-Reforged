using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Overlays
{
	// Token: 0x02000057 RID: 87
	public partial class ProxyLoaderPage : Page
	{
		// Token: 0x06000298 RID: 664 RVA: 0x00007A20 File Offset: 0x00005C20
		public ProxyLoaderPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00007A39 File Offset: 0x00005C39
		private void WebBorder_MouseEnter(object sender, MouseEventArgs e)
		{
			(this.GlobeIconElement.TryFindResource(<Module>.smethod_2<string>(-1776009086)) as Storyboard).Begin(this.GlobeIconElement);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00007A60 File Offset: 0x00005C60
		private void WebBorder_MouseLeave(object sender, MouseEventArgs e)
		{
			(this.GlobeIconElement.TryFindResource(<Module>.smethod_3<string>(1286980681)) as Storyboard).Begin(this.GlobeIconElement);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00007A87 File Offset: 0x00005C87
		private void FolderBorder_MouseEnter(object sender, MouseEventArgs e)
		{
			(this.FolderIconElement.TryFindResource(<Module>.smethod_5<string>(1611172877)) as Storyboard).Begin(this.FolderIconElement);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00007AAE File Offset: 0x00005CAE
		private void FolderBorder_MouseLeave(object sender, MouseEventArgs e)
		{
			(this.FolderIconElement.TryFindResource(<Module>.smethod_2<string>(-1210237932)) as Storyboard).Begin(this.FolderIconElement);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00007AD5 File Offset: 0x00005CD5
		private void WebCloseBorder_MouseEnter(object sender, MouseEventArgs e)
		{
			(this.WebBorderElement.TryFindResource(<Module>.smethod_5<string>(91561949)) as Storyboard).Begin(this.WebBorderElement);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00007AFC File Offset: 0x00005CFC
		private void WebCloseBorder_MouseLeave(object sender, MouseEventArgs e)
		{
			(this.WebBorderElement.TryFindResource(<Module>.smethod_4<string>(74464009)) as Storyboard).Begin(this.WebBorderElement);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00007B23 File Offset: 0x00005D23
		private void WebCloseBorder_MouseDown(object sender, EventArgs e)
		{
			MainViewModel.Instance.ProxyLoaderViewModel.LoadFromWeb = true;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00007B35 File Offset: 0x00005D35
		private void FileCloseBorder_MouseEnter(object sender, MouseEventArgs e)
		{
			(this.FileBorderElement.TryFindResource(<Module>.smethod_2<string>(1254133036)) as Storyboard).Begin(this.FileBorderElement);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00007B5C File Offset: 0x00005D5C
		private void FileCloseBorder_MouseLeave(object sender, MouseEventArgs e)
		{
			(this.FileBorderElement.TryFindResource(<Module>.smethod_6<string>(-688483077)) as Storyboard).Begin(this.FileBorderElement);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00007B83 File Offset: 0x00005D83
		private void FileCloseBorder_MouseDown(object sender, EventArgs e)
		{
			MainViewModel.Instance.ProxyLoaderViewModel.LoadFromFile = true;
		}
	}
}
