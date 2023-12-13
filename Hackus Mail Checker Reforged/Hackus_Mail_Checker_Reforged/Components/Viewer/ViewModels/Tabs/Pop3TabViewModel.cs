using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hackus_Mail_Checker_Reforged.Components.Viewer.Models;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using HandyControl.Data;
using HandyControl.Tools;
using MailBee.Mime;
using MailBee.Pop3Mail;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs
{
	// Token: 0x02000180 RID: 384
	internal class Pop3TabViewModel : BindableObject, IDisposable
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
		public Pop3TabViewModel(Mailbox mailbox, Server server, Pop3 client)
		{
			this.Mailbox = mailbox;
			this.Server = server;
			this.Pop3 = client;
			this.Limit = ViewerSettings.Instance.PaginationLimit;
			this.ReconnectLimit = ViewerSettings.Instance.ReconnectLimit;
			this.Messages = new ObservableCollection<MailMessage>();
			this.Attachments = new ObservableCollection<Hackus_Mail_Checker_Reforged.Components.Viewer.Models.Attachment>();
			this.Pop3.MessageDownloaded += this.OnMessageDownloaded;
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0000CD64 File Offset: 0x0000AF64
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0000CD6C File Offset: 0x0000AF6C
		public ObservableCollection<MailMessage> Messages
		{
			get
			{
				return this._messages;
			}
			set
			{
				this._messages = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1364324253));
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0000CD85 File Offset: 0x0000AF85
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0000CD8D File Offset: 0x0000AF8D
		public ObservableCollection<Hackus_Mail_Checker_Reforged.Components.Viewer.Models.Attachment> Attachments
		{
			get
			{
				return this._attachments;
			}
			set
			{
				this._attachments = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-223475279));
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0000CDA6 File Offset: 0x0000AFA6
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x0000CDAE File Offset: 0x0000AFAE
		public ObservableCollection<MailMessage> SelectedMessages
		{
			get
			{
				return this._selectedMessages;
			}
			set
			{
				this._selectedMessages = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1261770006));
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0000CDC7 File Offset: 0x0000AFC7
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0000CDCF File Offset: 0x0000AFCF
		public Mailbox Mailbox
		{
			get
			{
				return this._mailbox;
			}
			set
			{
				this._mailbox = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(992838585));
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		public Server Server
		{
			get
			{
				return this._server;
			}
			set
			{
				this._server = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(413798310));
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0000CE09 File Offset: 0x0000B009
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0000CE11 File Offset: 0x0000B011
		public Pop3 Pop3
		{
			get
			{
				return this._pop3;
			}
			set
			{
				this._pop3 = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1284541984));
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0000CE2A File Offset: 0x0000B02A
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0000CE32 File Offset: 0x0000B032
		public int Limit
		{
			get
			{
				return this._limit;
			}
			set
			{
				this._limit = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-2104776208));
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0000CE4B File Offset: 0x0000B04B
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0000CE53 File Offset: 0x0000B053
		public int ReconnectLimit
		{
			get
			{
				return this._reconnectLimit;
			}
			set
			{
				this._reconnectLimit = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(367348827));
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0000CE6C File Offset: 0x0000B06C
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0000CE74 File Offset: 0x0000B074
		public int MaxPageCount
		{
			get
			{
				return this._maxPageCount;
			}
			set
			{
				this._maxPageCount = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1770412128));
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0000CE8D File Offset: 0x0000B08D
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0000CE95 File Offset: 0x0000B095
		public int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
			set
			{
				this._pageIndex = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1594299212));
				base.OnPropertyChanged(<Module>.smethod_6<string>(-333572785));
				base.OnPropertyChanged(<Module>.smethod_5<string>(734139158));
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0000CECE File Offset: 0x0000B0CE
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0000CED6 File Offset: 0x0000B0D6
		public bool IsPop3Busy
		{
			get
			{
				return this._isPop3Busy;
			}
			set
			{
				this._isPop3Busy = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1965364893));
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0000CEEF File Offset: 0x0000B0EF
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0000CEF7 File Offset: 0x0000B0F7
		public string Pop3OperationStatus
		{
			get
			{
				return this._pop3OperationStatus;
			}
			set
			{
				this._pop3OperationStatus = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1767716119));
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0000CF10 File Offset: 0x0000B110
		// (set) Token: 0x06000B85 RID: 2949 RVA: 0x0000CF18 File Offset: 0x0000B118
		public MailMessage Message
		{
			get
			{
				return this._message;
			}
			set
			{
				this._message = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1960491344));
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0000CF31 File Offset: 0x0000B131
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x0000CF39 File Offset: 0x0000B139
		public bool ShowMessageBody
		{
			get
			{
				return this._showMessageBody;
			}
			set
			{
				this._showMessageBody = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1561064195));
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0000CF52 File Offset: 0x0000B152
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x0000CF5A File Offset: 0x0000B15A
		public bool ShowAttachments
		{
			get
			{
				return this._showAttachments;
			}
			set
			{
				this._showAttachments = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1252153368));
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0000CF73 File Offset: 0x0000B173
		public bool CanMoveForward
		{
			get
			{
				return this.MaxPageCount > this.PageIndex;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0000CF83 File Offset: 0x0000B183
		public bool CanMoveBack
		{
			get
			{
				return this.PageIndex > 1;
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0003FC80 File Offset: 0x0003DE80
		private void OnMessageDownloaded(object sender, Pop3MessageDownloadedEventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(e.DownloadedMessage.From))
				{
					Func<MailMessage, bool> <>9__1;
					Application.Current.Dispatcher.Invoke(delegate()
					{
						IEnumerable<MailMessage> messages = this.Messages;
						Func<MailMessage, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((MailMessage m) => Pop3TabViewModel.<>c__DisplayClass69_0.smethod_1(m) == Pop3TabViewModel.<>c__DisplayClass69_0.smethod_1(Pop3TabViewModel.<>c__DisplayClass69_0.smethod_0(e))));
						}
						if (!messages.Any(predicate))
						{
							ObservableCollection<MailMessage> messages2 = this.Messages;
							if (messages2 == null)
							{
								return;
							}
							messages2.Add(Pop3TabViewModel.<>c__DisplayClass69_0.smethod_0(e));
						}
					});
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0003FCF0 File Offset: 0x0003DEF0
		private Task DownloadMailHeaders(int offset)
		{
			Pop3TabViewModel.<DownloadMailHeaders>d__70 <DownloadMailHeaders>d__;
			<DownloadMailHeaders>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadMailHeaders>d__.<>4__this = this;
			<DownloadMailHeaders>d__.offset = offset;
			<DownloadMailHeaders>d__.<>1__state = -1;
			<DownloadMailHeaders>d__.<>t__builder.Start<Pop3TabViewModel.<DownloadMailHeaders>d__70>(ref <DownloadMailHeaders>d__);
			return <DownloadMailHeaders>d__.<>t__builder.Task;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0003FD3C File Offset: 0x0003DF3C
		public RelayCommand UpdatePageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._updatePageCommand) == null)
				{
					result = (this._updatePageCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<<get_UpdatePageCommand>b__73_0>d <<get_UpdatePageCommand>b__73_0>d;
						<<get_UpdatePageCommand>b__73_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_UpdatePageCommand>b__73_0>d.<>4__this = this;
						<<get_UpdatePageCommand>b__73_0>d.obj = obj;
						<<get_UpdatePageCommand>b__73_0>d.<>1__state = -1;
						<<get_UpdatePageCommand>b__73_0>d.<>t__builder.Start<Pop3TabViewModel.<<get_UpdatePageCommand>b__73_0>d>(ref <<get_UpdatePageCommand>b__73_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0003FD70 File Offset: 0x0003DF70
		private Task<bool> EstablishConnection()
		{
			Pop3TabViewModel.<EstablishConnection>d__74 <EstablishConnection>d__;
			<EstablishConnection>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<EstablishConnection>d__.<>4__this = this;
			<EstablishConnection>d__.<>1__state = -1;
			<EstablishConnection>d__.<>t__builder.Start<Pop3TabViewModel.<EstablishConnection>d__74>(ref <EstablishConnection>d__);
			return <EstablishConnection>d__.<>t__builder.Task;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0003FDB4 File Offset: 0x0003DFB4
		public RelayCommand InitializeCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._initializeCommand) == null)
				{
					result = (this._initializeCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<<get_InitializeCommand>b__77_0>d <<get_InitializeCommand>b__77_0>d;
						<<get_InitializeCommand>b__77_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_InitializeCommand>b__77_0>d.<>4__this = this;
						<<get_InitializeCommand>b__77_0>d.<>1__state = -1;
						<<get_InitializeCommand>b__77_0>d.<>t__builder.Start<Pop3TabViewModel.<<get_InitializeCommand>b__77_0>d>(ref <<get_InitializeCommand>b__77_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B91 RID: 2961 RVA: 0x0003FDE8 File Offset: 0x0003DFE8
		public RelayCommand OpenMessagesListCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openMessagesListCommand) == null)
				{
					result = (this._openMessagesListCommand = new RelayCommand(delegate(object obj)
					{
						this.ShowMessageBody = false;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0003FE1C File Offset: 0x0003E01C
		public RelayCommand GoForwardCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._goForwardCommand) == null)
				{
					result = (this._goForwardCommand = new RelayCommand(delegate(object obj)
					{
						this.UpdatePageCommand.Execute(new FunctionEventArgs<int>(this.PageIndex + 1));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0003FE50 File Offset: 0x0003E050
		public RelayCommand GoBackCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._goBackCommand) == null)
				{
					result = (this._goBackCommand = new RelayCommand(delegate(object obj)
					{
						this.UpdatePageCommand.Execute(new FunctionEventArgs<int>(this.PageIndex - 1));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0003FE84 File Offset: 0x0003E084
		public RelayCommand OpenMessageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openMessageCommand) == null)
				{
					result = (this._openMessageCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<<get_OpenMessageCommand>b__89_0>d <<get_OpenMessageCommand>b__89_0>d;
						<<get_OpenMessageCommand>b__89_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_OpenMessageCommand>b__89_0>d.<>4__this = this;
						<<get_OpenMessageCommand>b__89_0>d.obj = obj;
						<<get_OpenMessageCommand>b__89_0>d.<>1__state = -1;
						<<get_OpenMessageCommand>b__89_0>d.<>t__builder.Start<Pop3TabViewModel.<<get_OpenMessageCommand>b__89_0>d>(ref <<get_OpenMessageCommand>b__89_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0003FEB8 File Offset: 0x0003E0B8
		public RelayCommand SwitchAttachmentsModeCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._switchAttachmentsModeCommand) == null)
				{
					result = (this._switchAttachmentsModeCommand = new RelayCommand(delegate(object obj)
					{
						if (!this.ShowAttachments)
						{
							this.Attachments.Clear();
							foreach (object obj2 in this.Message.Attachments)
							{
								MailBee.Mime.Attachment attachment = (MailBee.Mime.Attachment)obj2;
								this.Attachments.Add(new Hackus_Mail_Checker_Reforged.Components.Viewer.Models.Attachment(attachment));
							}
						}
						this.ShowAttachments = !this.ShowAttachments;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0003FEEC File Offset: 0x0003E0EC
		public RelayCommand DownloadAttachmentCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._downloadAttachmentCommand) == null)
				{
					result = (this._downloadAttachmentCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<>c.<<get_DownloadAttachmentCommand>b__95_0>d <<get_DownloadAttachmentCommand>b__95_0>d;
						<<get_DownloadAttachmentCommand>b__95_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_DownloadAttachmentCommand>b__95_0>d.obj = obj;
						<<get_DownloadAttachmentCommand>b__95_0>d.<>1__state = -1;
						<<get_DownloadAttachmentCommand>b__95_0>d.<>t__builder.Start<Pop3TabViewModel.<>c.<<get_DownloadAttachmentCommand>b__95_0>d>(ref <<get_DownloadAttachmentCommand>b__95_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0003FF34 File Offset: 0x0003E134
		public RelayCommand DownloadAttachmentByPathCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._downloadAttachmentByPathCommand) == null)
				{
					result = (this._downloadAttachmentByPathCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<>c.<<get_DownloadAttachmentByPathCommand>b__98_0>d <<get_DownloadAttachmentByPathCommand>b__98_0>d;
						<<get_DownloadAttachmentByPathCommand>b__98_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_DownloadAttachmentByPathCommand>b__98_0>d.obj = obj;
						<<get_DownloadAttachmentByPathCommand>b__98_0>d.<>1__state = -1;
						<<get_DownloadAttachmentByPathCommand>b__98_0>d.<>t__builder.Start<Pop3TabViewModel.<>c.<<get_DownloadAttachmentByPathCommand>b__98_0>d>(ref <<get_DownloadAttachmentByPathCommand>b__98_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0003FF7C File Offset: 0x0003E17C
		public RelayCommand DisconnectCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._disconnectCommand) == null)
				{
					result = (this._disconnectCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<<get_DisconnectCommand>b__101_0>d <<get_DisconnectCommand>b__101_0>d;
						<<get_DisconnectCommand>b__101_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_DisconnectCommand>b__101_0>d.<>4__this = this;
						<<get_DisconnectCommand>b__101_0>d.<>1__state = -1;
						<<get_DisconnectCommand>b__101_0>d.<>t__builder.Start<Pop3TabViewModel.<<get_DisconnectCommand>b__101_0>d>(ref <<get_DisconnectCommand>b__101_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0003FFB0 File Offset: 0x0003E1B0
		public RelayCommand ChangeTranslationFromLanguageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._changeTranslationFromLanguageCommand) == null)
				{
					result = (this._changeTranslationFromLanguageCommand = new RelayCommand(delegate(object obj)
					{
						ComboBox comboBox = (obj as SelectionChangedEventArgs).Source as ComboBox;
						ComboBoxItem comboBoxItem = ((comboBox != null) ? comboBox.SelectedItem : null) as ComboBoxItem;
						string value2 = ((comboBoxItem != null) ? comboBoxItem.Tag : null) as string;
						TranslationLanguage translationFromLanguage = (TranslationLanguage)Enum.Parse(typeof(TranslationLanguage), value2);
						this._translationFromLanguage = translationFromLanguage;
						ViewerSettings.Instance.TranslationFromLanguage = translationFromLanguage;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0003FFE4 File Offset: 0x0003E1E4
		public RelayCommand ChangeTranslationToLanguageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._changeTranslationToLanguageCommand) == null)
				{
					result = (this._changeTranslationToLanguageCommand = new RelayCommand(delegate(object obj)
					{
						ComboBox comboBox = (obj as SelectionChangedEventArgs).Source as ComboBox;
						ComboBoxItem comboBoxItem = ((comboBox != null) ? comboBox.SelectedItem : null) as ComboBoxItem;
						string value2 = ((comboBoxItem != null) ? comboBoxItem.Tag : null) as string;
						TranslationLanguage translationToLanguage = (TranslationLanguage)Enum.Parse(typeof(TranslationLanguage), value2);
						this._translationToLanguage = translationToLanguage;
						ViewerSettings.Instance.TranslationToLanguage = translationToLanguage;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00040018 File Offset: 0x0003E218
		public RelayCommand TranslateCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._translateCommand) == null)
				{
					result = (this._translateCommand = new RelayCommand(delegate(object obj)
					{
						Pop3TabViewModel.<<get_TranslateCommand>b__110_0>d <<get_TranslateCommand>b__110_0>d;
						<<get_TranslateCommand>b__110_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_TranslateCommand>b__110_0>d.<>4__this = this;
						<<get_TranslateCommand>b__110_0>d.<>1__state = -1;
						<<get_TranslateCommand>b__110_0>d.<>t__builder.Start<Pop3TabViewModel.<<get_TranslateCommand>b__110_0>d>(ref <<get_TranslateCommand>b__110_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0000CF8E File Offset: 0x0000B18E
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				this.DisconnectCommand.Execute(null);
				this._isDisposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00006BF6 File Offset: 0x00004DF6
		public string Resource(string key)
		{
			return ResourceHelper.GetResource<string>(key);
		}

		// Token: 0x04000600 RID: 1536
		private bool _isInitialized;

		// Token: 0x04000601 RID: 1537
		private bool _isDisposed;

		// Token: 0x04000602 RID: 1538
		private TranslationLanguage _translationFromLanguage;

		// Token: 0x04000603 RID: 1539
		private TranslationLanguage _translationToLanguage;

		// Token: 0x04000604 RID: 1540
		private ObservableCollection<MailMessage> _messages;

		// Token: 0x04000605 RID: 1541
		private ObservableCollection<Hackus_Mail_Checker_Reforged.Components.Viewer.Models.Attachment> _attachments;

		// Token: 0x04000606 RID: 1542
		private ObservableCollection<MailMessage> _selectedMessages;

		// Token: 0x04000607 RID: 1543
		private Mailbox _mailbox;

		// Token: 0x04000608 RID: 1544
		private Server _server;

		// Token: 0x04000609 RID: 1545
		private Pop3 _pop3;

		// Token: 0x0400060A RID: 1546
		private int _limit = 50;

		// Token: 0x0400060B RID: 1547
		private int _reconnectLimit = 1;

		// Token: 0x0400060C RID: 1548
		private int _maxPageCount = 1;

		// Token: 0x0400060D RID: 1549
		private int _pageIndex;

		// Token: 0x0400060E RID: 1550
		private bool _isPop3Busy;

		// Token: 0x0400060F RID: 1551
		private string _pop3OperationStatus;

		// Token: 0x04000610 RID: 1552
		private MailMessage _message;

		// Token: 0x04000611 RID: 1553
		private bool _showMessageBody;

		// Token: 0x04000612 RID: 1554
		private bool _showAttachments;

		// Token: 0x04000613 RID: 1555
		private RelayCommand _updatePageCommand;

		// Token: 0x04000614 RID: 1556
		private RelayCommand _initializeCommand;

		// Token: 0x04000615 RID: 1557
		private RelayCommand _openMessagesListCommand;

		// Token: 0x04000616 RID: 1558
		private RelayCommand _goForwardCommand;

		// Token: 0x04000617 RID: 1559
		private RelayCommand _goBackCommand;

		// Token: 0x04000618 RID: 1560
		private RelayCommand _openMessageCommand;

		// Token: 0x04000619 RID: 1561
		private RelayCommand _switchAttachmentsModeCommand;

		// Token: 0x0400061A RID: 1562
		private RelayCommand _downloadAttachmentCommand;

		// Token: 0x0400061B RID: 1563
		private RelayCommand _downloadAttachmentByPathCommand;

		// Token: 0x0400061C RID: 1564
		private RelayCommand _disconnectCommand;

		// Token: 0x0400061D RID: 1565
		private RelayCommand _changeTranslationFromLanguageCommand;

		// Token: 0x0400061E RID: 1566
		private RelayCommand _changeTranslationToLanguageCommand;

		// Token: 0x0400061F RID: 1567
		private RelayCommand _translateCommand;
	}
}
