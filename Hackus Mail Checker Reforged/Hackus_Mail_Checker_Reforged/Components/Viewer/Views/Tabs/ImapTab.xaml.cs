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
using MailBee.ImapMail;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Views.Tabs
{
	// Token: 0x02000163 RID: 355
	public partial class ImapTab : UserControl, IDisposable, IStyleConnector
	{
		// Token: 0x06000A45 RID: 2629 RVA: 0x0000C37E File Offset: 0x0000A57E
		public ImapTab()
		{
			this.InitializeComponent();
			this.TranslateFromComboBox.SelectedIndex = (int)ViewerSettings.Instance.TranslationFromLanguage;
			this.TranslateToComboBox.SelectedIndex = ViewerSettings.Instance.TranslationToLanguage - TranslationLanguage.English;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0000C3B8 File Offset: 0x0000A5B8
		public ImapTab(Mailbox mailbox, Server server, Imap client) : this()
		{
			base.DataContext = new ImapTabViewModel(mailbox, server, client);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0000C3CE File Offset: 0x0000A5CE
		private void DeleteMessages(object sender, RoutedEventArgs e)
		{
			((ImapTabViewModel)base.DataContext).DeleteMessagesCommand.Execute(this.ResultDataGrid.SelectedItems);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
		private void OnResultDataGridDoubleClick(object sender, MouseButtonEventArgs e)
		{
			RelayCommand openMessageCommand = ((ImapTabViewModel)base.DataContext).OpenMessageCommand;
			DataGridRow dataGridRow = sender as DataGridRow;
			openMessageCommand.Execute((dataGridRow != null) ? dataGridRow.Item : null);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0003BBB0 File Offset: 0x00039DB0
		private void OnResultListBoxDoubleClick(object sender, MouseButtonEventArgs e)
		{
			RelayCommand openMessageCommand = ((ImapTabViewModel)base.DataContext).OpenMessageCommand;
			ListBoxItem listBoxItem = sender as ListBoxItem;
			openMessageCommand.Execute((listBoxItem != null) ? listBoxItem.method_0() : null);
			ListBoxItem listBoxItem2 = sender as ListBoxItem;
			this.ListBoxResults.SelectedItem = listBoxItem2.DataContext;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00006655 File Offset: 0x00004855
		private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
		{
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003BBFC File Offset: 0x00039DFC
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
				value.GetType().InvokeMember(<Module>.smethod_4<string>(-529548164), BindingFlags.SetProperty, null, value, new object[]
				{
					Hide
				});
				return;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00006655 File Offset: 0x00004855
		private void WB_Navigated(object sender, NavigationEventArgs e)
		{
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0003BC68 File Offset: 0x00039E68
		private void CopyFromHeader(object sender, RoutedEventArgs e)
		{
			try
			{
				Clipboard.SetText(this.FromHeader.Text);
				this.CopyPopup.IsOpen = true;
			}
			catch
			{
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0000C419 File Offset: 0x0000A619
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				((ImapTabViewModel)base.DataContext).Dispose();
				this._isDisposed = true;
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0003BE14 File Offset: 0x0003A014
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			EventSetter eventSetter;
			if (connectionId == 6)
			{
				eventSetter = new EventSetter();
				eventSetter.Event = UIElement.PreviewMouseLeftButtonDownEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.OnResultListBoxDoubleClick);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			if (connectionId != 9)
			{
				return;
			}
			eventSetter = new EventSetter();
			eventSetter.Event = Control.MouseDoubleClickEvent;
			eventSetter.Handler = new MouseButtonEventHandler(this.OnResultDataGridDoubleClick);
			((Style)target).Setters.Add(eventSetter);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0000C43A File Offset: 0x0000A63A
		object method_0()
		{
			return base.Content;
		}

		// Token: 0x04000537 RID: 1335
		private bool _isDisposed;
	}
}
