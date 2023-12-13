using System;
using System.Collections.ObjectModel;
using Hackus_Mail_Checker_Reforged.UI.Models;
using HandyControl.Controls;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels
{
	// Token: 0x02000167 RID: 359
	internal class ViewerMainViewModel : BindableObject
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0000C58A File Offset: 0x0000A78A
		public ViewerController Controller
		{
			get
			{
				return ViewerController.Instance;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0000C591 File Offset: 0x0000A791
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0000C599 File Offset: 0x0000A799
		public ObservableCollection<TabItem> Tabs
		{
			get
			{
				return this._tabs;
			}
			set
			{
				this._tabs = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1743498216));
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0003C198 File Offset: 0x0003A398
		public RelayCommand OpenNewTabCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openNewTabCommand) == null)
				{
					result = (this._openNewTabCommand = new RelayCommand(delegate(object obj)
					{
						ViewerController.Instance.OpenNewTab();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0003C1E0 File Offset: 0x0003A3E0
		public RelayCommand OpenViewerSettingsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openViewerSettingsCommand) == null)
				{
					result = (this._openViewerSettingsCommand = new RelayCommand(delegate(object obj)
					{
						ViewerController.Instance.OpenSettingsTab();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x04000553 RID: 1363
		private ObservableCollection<TabItem> _tabs = new ObservableCollection<TabItem>();

		// Token: 0x04000554 RID: 1364
		private RelayCommand _openNewTabCommand;

		// Token: 0x04000555 RID: 1365
		private RelayCommand _openViewerSettingsCommand;
	}
}
