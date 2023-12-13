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
	// Token: 0x02000050 RID: 80
	public class RebruteDomainsFilterOverlayPage : Page, IComponentConnector
	{
		// Token: 0x06000281 RID: 641 RVA: 0x000078DD File Offset: 0x00005ADD
		public RebruteDomainsFilterOverlayPage()
		{
			this.InitializeComponent();
			base.DataContext = MainViewModel.Instance;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00017268 File Offset: 0x00015468
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri(<Module>.smethod_4<string>(1182913144), UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000078F6 File Offset: 0x00005AF6
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.SettingsPathElement = (Path)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x0400017D RID: 381
		internal Path SettingsPathElement;

		// Token: 0x0400017E RID: 382
		private bool _contentLoaded;
	}
}
