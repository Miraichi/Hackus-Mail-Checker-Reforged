using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels;
using Hackus_Mail_Checker_Reforged.Components.Viewer.Views;
using Hackus_Mail_Checker_Reforged.Components.Viewer.Views.Tabs;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.UI.Models;
using HandyControl.Controls;
using HandyControl.Tools;
using MailBee.ImapMail;
using MailBee.Pop3Mail;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer
{
	// Token: 0x02000160 RID: 352
	internal class ViewerController : BindableObject
	{
		// Token: 0x06000A2D RID: 2605 RVA: 0x00006C91 File Offset: 0x00004E91
		private ViewerController()
		{
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0000C2A7 File Offset: 0x0000A4A7
		public static ViewerController Instance
		{
			get
			{
				ViewerController result;
				if ((result = ViewerController._instance) == null)
				{
					result = (ViewerController._instance = new ViewerController());
				}
				return result;
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0003B508 File Offset: 0x00039708
		public void Show(bool openNewTab = true)
		{
			if (this.IsOpen())
			{
				this._viewerWindow.WindowState = WindowState.Normal;
				this._viewerWindow.Activate();
				return;
			}
			this._viewerWindow = new ViewerMainWindow();
			this._viewerViewModel = new ViewerMainViewModel();
			this._viewerWindow.DataContext = this._viewerViewModel;
			this._viewerWindow.Show();
			if (openNewTab)
			{
				this.OpenNewTab();
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0003B574 File Offset: 0x00039774
		public void ShowTools()
		{
			if (this.IsOpen())
			{
				this._viewerWindow.WindowState = WindowState.Normal;
				this._viewerWindow.Activate();
				this.OpenToolsTab();
				return;
			}
			this._viewerWindow = new ViewerMainWindow();
			this._viewerViewModel = new ViewerMainViewModel();
			this._viewerWindow.DataContext = this._viewerViewModel;
			this._viewerWindow.Show();
			this.OpenToolsTab();
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0003B5E0 File Offset: 0x000397E0
		public void OpenNewTab()
		{
			HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
			{
				Header = ResourceHelper.GetResource<string>(<Module>.smethod_2<string>(-339664398)),
				Content = new EmptyTab()
			};
			tabItem.SetValue(IconElement.GeometryProperty, ResourceHelper.GetResource<Geometry>(<Module>.smethod_2<string>(908840931)));
			tabItem.SetValue(IconElement.HeightProperty, 12.0);
			tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(<Module>.smethod_3<string>(1277012806)));
			this._viewerViewModel.Tabs.Add(tabItem);
			tabItem.IsSelected = true;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0003B680 File Offset: 0x00039880
		public void OpenNewTab(MailboxResult mailboxResult)
		{
			HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
			{
				Header = ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(-1894181922)),
				Content = new EmptyTab(mailboxResult)
			};
			tabItem.SetValue(IconElement.GeometryProperty, ResourceHelper.GetResource<Geometry>(<Module>.smethod_3<string>(-168112987)));
			tabItem.SetValue(IconElement.HeightProperty, 12.0);
			tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(<Module>.smethod_2<string>(-1305284150)));
			this._viewerViewModel.Tabs.Add(tabItem);
			tabItem.IsSelected = true;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0003B720 File Offset: 0x00039920
		public void OpenSettingsTab()
		{
			HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
			{
				Header = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(290654426)),
				Content = new SettingsTab()
			};
			tabItem.SetValue(IconElement.GeometryProperty, ResourceHelper.GetResource<Geometry>(<Module>.smethod_6<string>(245296456)));
			tabItem.SetValue(IconElement.HeightProperty, 12.0);
			tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(<Module>.smethod_3<string>(1277012806)));
			this._viewerViewModel.Tabs.Add(tabItem);
			tabItem.IsSelected = true;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0003B7C0 File Offset: 0x000399C0
		public void OpenImapTab(Mailbox mailbox, Server server, Imap client)
		{
			HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
			{
				Header = (mailbox.Address ?? ""),
				Content = new ImapTab(mailbox, server, client)
			};
			tabItem.SetValue(IconElement.GeometryProperty, ResourceHelper.GetResource<Geometry>(<Module>.smethod_3<string>(-408646046)));
			tabItem.SetValue(IconElement.HeightProperty, 12.0);
			tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(<Module>.smethod_4<string>(-1057326977)));
			this._viewerViewModel.Tabs.Add(tabItem);
			tabItem.IsSelected = true;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0003B860 File Offset: 0x00039A60
		public void OpenPop3Tab(Mailbox mailbox, Server server, Pop3 client)
		{
			HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
			{
				Header = (mailbox.Address ?? ""),
				Content = new Pop3Tab(mailbox, server, client)
			};
			tabItem.SetValue(IconElement.GeometryProperty, ResourceHelper.GetResource<Geometry>(<Module>.smethod_4<string>(-1149297045)));
			tabItem.SetValue(IconElement.HeightProperty, 12.0);
			tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(<Module>.smethod_6<string>(95822237)));
			this._viewerViewModel.Tabs.Add(tabItem);
			tabItem.IsSelected = true;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0003B900 File Offset: 0x00039B00
		public void OpenToolsTab()
		{
			HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
			{
				Header = ResourceHelper.GetResource<string>(<Module>.smethod_6<string>(393040890)),
				Content = new ToolsTab()
			};
			tabItem.SetValue(IconElement.GeometryProperty, ResourceHelper.GetResource<Geometry>(<Module>.smethod_6<string>(1875674585)));
			tabItem.SetValue(IconElement.HeightProperty, 12.0);
			tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(<Module>.smethod_2<string>(-1305284150)));
			this._viewerViewModel.Tabs.Add(tabItem);
			tabItem.IsSelected = true;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0003B9A0 File Offset: 0x00039BA0
		public void CloseTab(object tabViewModel)
		{
			HandyControl.Controls.TabItem tabItem = null;
			foreach (HandyControl.Controls.TabItem tabItem2 in this._viewerViewModel.Tabs)
			{
				UserControl userControl = tabItem2.Content as UserControl;
				if (((userControl != null) ? userControl.DataContext : null).Equals(tabViewModel))
				{
					tabItem = tabItem2;
					break;
				}
			}
			if (tabItem != null)
			{
				this._viewerViewModel.Tabs.Remove(tabItem);
				IDisposable disposable = tabItem.Content as IDisposable;
				if (disposable == null)
				{
					return;
				}
				disposable.Dispose();
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0003BA3C File Offset: 0x00039C3C
		public void Close()
		{
			foreach (HandyControl.Controls.TabItem tabItem in new List<HandyControl.Controls.TabItem>(this._viewerViewModel.Tabs))
			{
				UserControl userControl = tabItem.Content as UserControl;
				this.CloseTab((userControl != null) ? userControl.DataContext : null);
			}
			this._viewerWindow.Close();
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0000C2BD File Offset: 0x0000A4BD
		private bool IsOpen()
		{
			return Application.Current.Windows.OfType<ViewerMainWindow>().Any<ViewerMainWindow>();
		}

		// Token: 0x04000530 RID: 1328
		private ViewerMainWindow _viewerWindow;

		// Token: 0x04000531 RID: 1329
		private ViewerMainViewModel _viewerViewModel;

		// Token: 0x04000532 RID: 1330
		private static ViewerController _instance;
	}
}
