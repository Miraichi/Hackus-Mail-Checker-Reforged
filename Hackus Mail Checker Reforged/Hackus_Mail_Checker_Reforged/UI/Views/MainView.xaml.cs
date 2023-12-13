using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.Windows.Threading;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.UI.Models;
using Hackus_Mail_Checker_Reforged.UI.Pages.Settings;
using Hackus_Mail_Checker_Reforged.UI.ViewModels;
using HandyControl.Controls;

namespace Hackus_Mail_Checker_Reforged.UI.Views
{
	// Token: 0x02000028 RID: 40
	public partial class MainView : System.Windows.Window, IStyleConnector
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00012248 File Offset: 0x00010448
		public MainView()
		{
			this.InitializeComponent();
			PagesManager.Instance.RegisterFrame(this.MainSettingsFrame, FrameType.MainSettings);
			PagesManager.Instance.RegisterFrame(this.MainOverlayFrame, FrameType.MainOverlay);
			base.DataContext = MainViewModel.Instance;
			this._statsUpdater = new DispatcherTimer(DispatcherPriority.Render);
			this._statsUpdater.Interval = TimeSpan.FromMilliseconds(100.0);
			this._statsUpdater.Tick += this.UpdateStatistics;
			this._statsUpdater.Start();
			base.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000069A6 File Offset: 0x00004BA6
		private void OnScreenshot(object sender, MouseButtonEventArgs e)
		{
			(this.ScreenshotIconElement.TryFindResource(<Module>.smethod_5<string>(364964925)) as Storyboard).Begin(this.ScreenshotIconElement);
			this.GetPngImage(this.ResultStatisticsBorder, 1.0);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000122E4 File Offset: 0x000104E4
		public void GetPngImage(UIElement source, double scale)
		{
			double height = source.RenderSize.Height;
			double width = source.RenderSize.Width;
			double num = height * scale;
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(width * scale), (int)num, 96.0, 96.0, PixelFormats.Pbgra32);
			VisualBrush brush = new VisualBrush(source);
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			using (drawingContext)
			{
				drawingContext.PushTransform(new ScaleTransform(scale, scale));
				drawingContext.DrawRectangle(brush, null, new Rect(new Point(0.0, 0.0), new Point(width, height)));
			}
			renderTargetBitmap.Render(drawingVisual);
			Clipboard.SetImage(renderTargetBitmap);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000069E2 File Offset: 0x00004BE2
		private void GlobalSettings_Selected(object sender, RoutedEventArgs e)
		{
			PagesManager.Instance.OpenCachedPage(typeof(GlobalSettingsPage), FrameType.MainSettings);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000123BC File Offset: 0x000105BC
		private void GlobalSearchSettings_Selected(object sender, RoutedEventArgs e)
		{
			SideMenuItem sideMenuItem = e.OriginalSource as SideMenuItem;
			if ((string)sideMenuItem.Header == <Module>.smethod_2<string>(1675724900))
			{
				PagesManager.Instance.OpenCachedPage(typeof(SearchSettingsPage), FrameType.MainSettings);
				return;
			}
			if ((string)sideMenuItem.Header == <Module>.smethod_5<string>(897861962))
			{
				PagesManager.Instance.OpenCachedPage(typeof(RequestsSettingsPage), FrameType.MainSettings);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000069F9 File Offset: 0x00004BF9
		private void SearchSettings_Selected(object sender, RoutedEventArgs e)
		{
			PagesManager.Instance.OpenCachedPage(typeof(SearchSettingsPage), FrameType.MainSettings);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006A10 File Offset: 0x00004C10
		private void RequestsSettings_Selected(object sender, RoutedEventArgs e)
		{
			PagesManager.Instance.OpenCachedPage(typeof(RequestsSettingsPage), FrameType.MainSettings);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006A27 File Offset: 0x00004C27
		private void ConfigurationSettings_Selected(object sender, RoutedEventArgs e)
		{
			PagesManager.Instance.OpenCachedPage(typeof(ConfigurationSettingsPage), FrameType.MainSettings);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006A3E File Offset: 0x00004C3E
		private void WebSettings_Selected(object sender, RoutedEventArgs e)
		{
			PagesManager.Instance.OpenCachedPage(typeof(ImapSettingsPage), FrameType.MainSettings);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006A55 File Offset: 0x00004C55
		private void OnHide(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006A5E File Offset: 0x00004C5E
		private void HideInTray(object sender, RoutedEventArgs e)
		{
			base.WindowState = WindowState.Minimized;
			base.ShowInTaskbar = false;
			this.NotifyIconElement.Visibility = Visibility.Visible;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006A7A File Offset: 0x00004C7A
		private void ShowFromTray(object sender, RoutedEventArgs e)
		{
			base.ShowInTaskbar = true;
			this.NotifyIconElement.Visibility = Visibility.Collapsed;
			base.WindowState = WindowState.Normal;
			base.Activate();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00012438 File Offset: 0x00010638
		private void DataGridTextColumn_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				Clipboard.SetText(((TextBlock)((DataGridCell)sender).Content).Text);
				this.CopyPopup.IsOpen = true;
			}
			catch
			{
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00012480 File Offset: 0x00010680
		private void UpdateStatistics(object sender, EventArgs e)
		{
			this.GoodMailsCount.Text = StatisticsManager.Instance.GoodMailsCount.ToString();
			this.FoundMailsCount.Text = StatisticsManager.Instance.FoundMailsCount.ToString();
			this.BadMailsCount.Text = StatisticsManager.Instance.BadMailsCount.ToString();
			this.BlockedMailsCount.Text = StatisticsManager.Instance.BlockedMailsCount.ToString();
			this.MultipasswordMailsCount.Text = StatisticsManager.Instance.MultipasswordMailsCount.ToString();
			this.ErrorMailsCount.Text = StatisticsManager.Instance.ErrorMailsCount.ToString();
			this.NoHostMailsCount.Text = StatisticsManager.Instance.NoHostMailsCount.ToString();
			this.CaptchaMailsCount.Text = StatisticsManager.Instance.CaptchaMailsCount.ToString();
			if (ThreadsManager.Instance.State != CheckerState.Running)
			{
				if (ThreadsManager.Instance.State != CheckerState.Paused)
				{
					base.TaskbarItemInfo.ProgressValue = 0.0;
					return;
				}
			}
			base.TaskbarItemInfo.ProgressValue = (double)StatisticsManager.Instance.CheckedStrings / (double)StatisticsManager.Instance.LoadedStrings;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006655 File Offset: 0x00004855
		private void OnShowDetails(object sender, MouseButtonEventArgs e)
		{
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000128BC File Offset: 0x00010ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 22:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.DataGridTextColumn_MouseDown);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			case 23:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.DataGridTextColumn_MouseDown);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			case 24:
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.DataGridTextColumn_MouseDown);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x04000075 RID: 117
		private DispatcherTimer _statsUpdater;
	}
}
