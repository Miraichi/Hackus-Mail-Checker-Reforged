using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Pages.Overlays;
using HandyControl.Controls;
using HandyControl.Data;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x02000072 RID: 114
	internal class PagesManager
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00008EDE File Offset: 0x000070DE
		public static PagesManager Instance
		{
			get
			{
				PagesManager result;
				if ((result = PagesManager._instance) == null)
				{
					result = (PagesManager._instance = new PagesManager());
				}
				return result;
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00008EF4 File Offset: 0x000070F4
		public void RegisterFrame(Frame frame, FrameType frameType)
		{
			switch (frameType)
			{
			case FrameType.MainSettings:
				this._mainSettingsFrame = frame;
				return;
			case FrameType.MainOverlay:
				this._mainOverlay = frame;
				return;
			case FrameType.Startup:
				this._startupFrame = frame;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00019390 File Offset: 0x00017590
		public void OpenPage(Page page, FrameType frameType)
		{
			Application.Current.Dispatcher.Invoke(delegate()
			{
				switch (frameType)
				{
				case FrameType.MainSettings:
					PagesManager.<>c__DisplayClass8_0.smethod_0(this._mainSettingsFrame, page);
					return;
				case FrameType.MainOverlay:
					PagesManager.<>c__DisplayClass8_0.smethod_0(this._mainOverlay, page);
					return;
				case FrameType.Startup:
					PagesManager.<>c__DisplayClass8_0.smethod_0(this._startupFrame, page);
					return;
				default:
					return;
				}
			});
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000193D4 File Offset: 0x000175D4
		public void OpenCachedPage(Type pageType, FrameType frameType)
		{
			Func<Page, bool> <>9__1;
			Application.Current.Dispatcher.Invoke(delegate()
			{
				IEnumerable<Page> cachedPages = this._cachedPages;
				Func<Page, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((Page p) => PagesManager.<>c__DisplayClass9_0.smethod_2(PagesManager.<>c__DisplayClass9_0.smethod_1(p), pageType)));
				}
				Page page = cachedPages.FirstOrDefault(predicate);
				if (page == null)
				{
					page = (Page)PagesManager.<>c__DisplayClass9_0.smethod_0(pageType);
					this._cachedPages.Add(page);
				}
				this.OpenPage(page, frameType);
			});
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00008F20 File Offset: 0x00007120
		public void OpenErrorMessagePage(string message, Action afterClose = null)
		{
			if (afterClose == null)
			{
				this._mainOverlay.Navigate(new ErrorMessagePage(message));
				return;
			}
			this._mainOverlay.Navigate(new ErrorMessagePage(message, afterClose));
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00008F4B File Offset: 0x0000714B
		public void GoBackPage(FrameType frameType)
		{
			if (frameType != FrameType.MainSettings)
			{
				if (frameType != FrameType.MainOverlay)
				{
					return;
				}
				if (this._mainOverlay.CanGoBack)
				{
					this._mainOverlay.GoBack();
				}
			}
			else if (this._mainSettingsFrame.CanGoBack)
			{
				this._mainSettingsFrame.GoBack();
				return;
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00008F88 File Offset: 0x00007188
		public void ClearFrame(FrameType frameType)
		{
			if (frameType == FrameType.MainSettings)
			{
				this._mainSettingsFrame.NavigationService.RemoveBackEntry();
				return;
			}
			if (frameType == FrameType.MainOverlay)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					this._mainOverlay.NavigationService.RemoveBackEntry();
					this._mainOverlay.Content = null;
					this._mainOverlay.NavigationService.Content = null;
				});
				return;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00019418 File Offset: 0x00017618
		public void OpenSuccessMessageBox(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = <Module>.smethod_3<string>(-1430198211),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_6<string>(-1964559648),
				IconKey = <Module>.smethod_2<string>(729149099),
				StyleKey = <Module>.smethod_5<string>(375297047),
				ConfirmContent = <Module>.smethod_3<string>(-1992084495)
			});
		}

		// Token: 0x04000235 RID: 565
		private static PagesManager _instance;

		// Token: 0x04000236 RID: 566
		private Frame _mainSettingsFrame;

		// Token: 0x04000237 RID: 567
		private Frame _mainOverlay;

		// Token: 0x04000238 RID: 568
		private Frame _startupFrame;

		// Token: 0x04000239 RID: 569
		private List<Page> _cachedPages = new List<Page>();
	}
}
