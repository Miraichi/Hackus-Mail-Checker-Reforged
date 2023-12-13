using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using HandyControl.Controls;
using MailBee.Pop3Mail;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Views.Tabs
{
	// Token: 0x02000164 RID: 356
	public partial class Pop3Tab : UserControl, IDisposable, IStyleConnector
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x0000C442 File Offset: 0x0000A642
		public Pop3Tab()
		{
			this.InitializeComponent();
			this.TranslateFromComboBox.SelectedIndex = (int)ViewerSettings.Instance.TranslationFromLanguage;
			this.TranslateToComboBox.SelectedIndex = ViewerSettings.Instance.TranslationToLanguage - TranslationLanguage.English;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0000C47C File Offset: 0x0000A67C
		public Pop3Tab(Mailbox mailbox, Server server, Pop3 client) : this()
		{
			base.DataContext = new Pop3TabViewModel(mailbox, server, client);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0000C492 File Offset: 0x0000A692
		private void OnResultDataGridDoubleClick(object sender, MouseButtonEventArgs e)
		{
			RelayCommand openMessageCommand = ((Pop3TabViewModel)base.DataContext).OpenMessageCommand;
			DataGridRow dataGridRow = sender as DataGridRow;
			openMessageCommand.Execute((dataGridRow != null) ? dataGridRow.Item : null);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0003BE94 File Offset: 0x0003A094
		private void OnResultListBoxDoubleClick(object sender, MouseButtonEventArgs e)
		{
			RelayCommand openMessageCommand = ((Pop3TabViewModel)base.DataContext).OpenMessageCommand;
			ListBoxItem listBoxItem = sender as ListBoxItem;
			openMessageCommand.Execute((listBoxItem != null) ? listBoxItem.method_0() : null);
			ListBoxItem listBoxItem2 = sender as ListBoxItem;
			this.ListBoxResults.SelectedItem = listBoxItem2.DataContext;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00006655 File Offset: 0x00004855
		private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
		{
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0003BEE0 File Offset: 0x0003A0E0
		public void HideScriptErrors(WebBrowser wb, bool Hide)
		{
			FieldInfo field = typeof(WebBrowser).GetField(<Module>.smethod_4<string>(-1580297471), BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				return;
			}
			object value = field.GetValue(wb);
			if (value != null)
			{
				value.GetType().InvokeMember(<Module>.smethod_6<string>(-943578156), BindingFlags.SetProperty, null, value, new object[]
				{
					Hide
				});
				return;
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00006655 File Offset: 0x00004855
		private void WB_Navigated(object sender, NavigationEventArgs e)
		{
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0003BF4C File Offset: 0x0003A14C
		private void CopyFromHeader(object sender, RoutedEventArgs e)
		{
			try
			{
				this.CopyPopup.IsOpen = true;
			}
			catch
			{
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0000C4BB File Offset: 0x0000A6BB
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				((Pop3TabViewModel)base.DataContext).Dispose();
				this._isDisposed = true;
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0003C0A8 File Offset: 0x0003A2A8
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			EventSetter eventSetter;
			if (connectionId == 4)
			{
				eventSetter = new EventSetter();
				eventSetter.Event = Control.MouseDoubleClickEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.OnResultDataGridDoubleClick);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			if (connectionId != 6)
			{
				return;
			}
			eventSetter = new EventSetter();
			eventSetter.Event = UIElement.PreviewMouseLeftButtonDownEvent;
			eventSetter.Handler = new MouseButtonEventHandler(this.OnResultListBoxDoubleClick);
			((Style)target).Setters.Add(eventSetter);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0000C43A File Offset: 0x0000A63A
		object method_0()
		{
			return base.Content;
		}

		// Token: 0x04000543 RID: 1347
		private bool _isDisposed;
	}
}
