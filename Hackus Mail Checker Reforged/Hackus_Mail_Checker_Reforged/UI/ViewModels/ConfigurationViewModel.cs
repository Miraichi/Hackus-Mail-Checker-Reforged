using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.UI.Models;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;

namespace Hackus_Mail_Checker_Reforged.UI.ViewModels
{
	// Token: 0x0200002A RID: 42
	internal class ConfigurationViewModel : BindableObject
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006AE7 File Offset: 0x00004CE7
		public ConfigurationManager Configuration
		{
			get
			{
				return ConfigurationManager.Instance;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006AEE File Offset: 0x00004CEE
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00006AF6 File Offset: 0x00004CF6
		public ObservableCollection<Server> FilteredServers
		{
			get
			{
				return this._filteredServers;
			}
			set
			{
				this._filteredServers = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(261985572));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006B0F File Offset: 0x00004D0F
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00006B17 File Offset: 0x00004D17
		public Server EditServer
		{
			get
			{
				return this._editServer;
			}
			set
			{
				this._editServer = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1339674714));
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00006B30 File Offset: 0x00004D30
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00006B38 File Offset: 0x00004D38
		public Server SelectedServer
		{
			get
			{
				return this._selectedServer;
			}
			set
			{
				this._selectedServer = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(2088188867));
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00006B51 File Offset: 0x00004D51
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00006B59 File Offset: 0x00004D59
		public string SearchQuery
		{
			get
			{
				return this._searchQuery;
			}
			set
			{
				this._searchQuery = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-456435205));
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00006B72 File Offset: 0x00004D72
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00006B7A File Offset: 0x00004D7A
		public SearchServerType SearchType
		{
			get
			{
				return this._searchType;
			}
			set
			{
				this._searchType = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1018321489));
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006B93 File Offset: 0x00004D93
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00006B9B File Offset: 0x00004D9B
		public string Login
		{
			get
			{
				return this._login;
			}
			set
			{
				this._login = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1156029122));
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006BB4 File Offset: 0x00004DB4
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00006BBC File Offset: 0x00004DBC
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1500479806));
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006BD5 File Offset: 0x00004DD5
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00006BDD File Offset: 0x00004DDD
		public bool IsLoading
		{
			get
			{
				return this._isLoading;
			}
			set
			{
				this._isLoading = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1577805830));
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000129B8 File Offset: 0x00010BB8
		public RelayCommand SearchCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._searchCommand) == null)
				{
					result = (this._searchCommand = new RelayCommand(delegate(object obj)
					{
						this.FilteredServers.Clear();
						if (string.IsNullOrWhiteSpace(this.SearchQuery))
						{
							return;
						}
						foreach (Server item in ConfigurationManager.Instance.Configuration.FindAll(this.SearchQuery, this.SearchType))
						{
							this.FilteredServers.Add(item);
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000129EC File Offset: 0x00010BEC
		public RelayCommand CopyCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._copyCommand) == null)
				{
					result = (this._copyCommand = new RelayCommand(delegate(object obj)
					{
						Server server = obj as Server;
						if (server != null)
						{
							this.EditServer = new Server();
							this.EditServer.Domain = server.Domain;
							this.EditServer.Hostname = server.Hostname;
							this.EditServer.Port = server.Port;
							this.EditServer.Protocol = server.Protocol;
							this.EditServer.Socket = server.Socket;
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00012A20 File Offset: 0x00010C20
		public RelayCommand AddCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addCommand) == null)
				{
					result = (this._addCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.EditServer.Domain) || string.IsNullOrWhiteSpace(this.EditServer.Hostname))
						{
							this.SendSimpleErrorMessage(this.Resource(<Module>.smethod_5<string>(-1524216613)));
							return;
						}
						if (ConfigurationManager.Instance.Configuration.FindInDatabase(this.EditServer.Domain.ToLower()) != null)
						{
							this.SendSimpleErrorMessage(this.Resource(<Module>.smethod_3<string>(-1458977524)));
							return;
						}
						ConfigurationManager.Instance.Configuration.Add(this.EditServer);
						this.SearchCommand.Execute(null);
						this.EditServer = new Server();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00012A54 File Offset: 0x00010C54
		public RelayCommand EditCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._editCommand) == null)
				{
					result = (this._editCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.EditServer.Domain) || string.IsNullOrWhiteSpace(this.EditServer.Hostname))
						{
							this.SendSimpleErrorMessage(this.Resource(<Module>.smethod_4<string>(-1858131433)));
							return;
						}
						Server server = ConfigurationManager.Instance.Configuration.FindInDatabase(this.EditServer.Domain.ToLower());
						if (server != null)
						{
							ConfigurationManager.Instance.Configuration.Update(server, this.EditServer);
							this.SearchCommand.Execute(null);
							this.EditServer = new Server();
							return;
						}
						this.SendSimpleErrorMessage(this.Resource(<Module>.smethod_3<string>(-294794873)));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00012A88 File Offset: 0x00010C88
		public RelayCommand ResetCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._resetCommand) == null)
				{
					result = (this._resetCommand = new RelayCommand(delegate(object obj)
					{
						this.EditServer = new Server();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00012ABC File Offset: 0x00010CBC
		public RelayCommand RemoveCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeCommand) == null)
				{
					result = (this._removeCommand = new RelayCommand(delegate(object obj)
					{
						Server server = obj as Server;
						if (server != null)
						{
							ConfigurationManager.Instance.Configuration.Remove(server);
							this.SearchCommand.Execute(null);
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00012AF0 File Offset: 0x00010CF0
		public RelayCommand ConnectCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._connectCommand) == null)
				{
					result = (this._connectCommand = new RelayCommand(delegate(object obj)
					{
						ConfigurationViewModel.<<get_ConnectCommand>b__55_0>d <<get_ConnectCommand>b__55_0>d;
						<<get_ConnectCommand>b__55_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_ConnectCommand>b__55_0>d.<>4__this = this;
						<<get_ConnectCommand>b__55_0>d.<>1__state = -1;
						<<get_ConnectCommand>b__55_0>d.<>t__builder.Start<ConfigurationViewModel.<<get_ConnectCommand>b__55_0>d>(ref <<get_ConnectCommand>b__55_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00012B24 File Offset: 0x00010D24
		public RelayCommand LoginCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._loginCommand) == null)
				{
					result = (this._loginCommand = new RelayCommand(delegate(object obj)
					{
						ConfigurationViewModel.<<get_LoginCommand>b__58_0>d <<get_LoginCommand>b__58_0>d;
						<<get_LoginCommand>b__58_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_LoginCommand>b__58_0>d.<>4__this = this;
						<<get_LoginCommand>b__58_0>d.<>1__state = -1;
						<<get_LoginCommand>b__58_0>d.<>t__builder.Start<ConfigurationViewModel.<<get_LoginCommand>b__58_0>d>(ref <<get_LoginCommand>b__58_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00012B58 File Offset: 0x00010D58
		public RelayCommand CloseOverlayCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._closeOverlayCommand) == null)
				{
					result = (this._closeOverlayCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.ClearFrame(FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00012BA0 File Offset: 0x00010DA0
		private Task<bool> ConnectAsync(Server server)
		{
			ConfigurationViewModel.<ConnectAsync>d__62 <ConnectAsync>d__;
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<ConnectAsync>d__.server = server;
			<ConnectAsync>d__.<>1__state = -1;
			<ConnectAsync>d__.<>t__builder.Start<ConfigurationViewModel.<ConnectAsync>d__62>(ref <ConnectAsync>d__);
			return <ConnectAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00012BE4 File Offset: 0x00010DE4
		private Task<Hackus_Mail_Checker_Reforged.Models.Enums.OperationResult> LoginAsync(Server server, string login, string password)
		{
			ConfigurationViewModel.<LoginAsync>d__63 <LoginAsync>d__;
			<LoginAsync>d__.<>t__builder = AsyncTaskMethodBuilder<Hackus_Mail_Checker_Reforged.Models.Enums.OperationResult>.Create();
			<LoginAsync>d__.server = server;
			<LoginAsync>d__.login = login;
			<LoginAsync>d__.password = password;
			<LoginAsync>d__.<>1__state = -1;
			<LoginAsync>d__.<>t__builder.Start<ConfigurationViewModel.<LoginAsync>d__63>(ref <LoginAsync>d__);
			return <LoginAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00012C38 File Offset: 0x00010E38
		private void SendSimpleErrorMessage(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(-416025122)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_6<string>(-1669070780),
				IconKey = <Module>.smethod_6<string>(1292737040),
				StyleKey = <Module>.smethod_2<string>(-1218410250),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_2<string>(862431965))
			});
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00012CB8 File Offset: 0x00010EB8
		private void SendSimpleSuccessMessage(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(1857950914)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_3<string>(-1499387607),
				IconKey = <Module>.smethod_5<string>(1098145450),
				StyleKey = <Module>.smethod_5<string>(375297047),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(-1867162284))
			});
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006BF6 File Offset: 0x00004DF6
		public string Resource(string key)
		{
			return ResourceHelper.GetResource<string>(key);
		}

		// Token: 0x0400008D RID: 141
		private ObservableCollection<Server> _filteredServers = new ObservableCollection<Server>();

		// Token: 0x0400008E RID: 142
		private Server _editServer = new Server();

		// Token: 0x0400008F RID: 143
		private Server _selectedServer;

		// Token: 0x04000090 RID: 144
		private string _searchQuery;

		// Token: 0x04000091 RID: 145
		private SearchServerType _searchType;

		// Token: 0x04000092 RID: 146
		private string _login;

		// Token: 0x04000093 RID: 147
		private string _password;

		// Token: 0x04000094 RID: 148
		private bool _isLoading;

		// Token: 0x04000095 RID: 149
		private RelayCommand _searchCommand;

		// Token: 0x04000096 RID: 150
		private RelayCommand _copyCommand;

		// Token: 0x04000097 RID: 151
		private RelayCommand _addCommand;

		// Token: 0x04000098 RID: 152
		private RelayCommand _editCommand;

		// Token: 0x04000099 RID: 153
		private RelayCommand _resetCommand;

		// Token: 0x0400009A RID: 154
		private RelayCommand _removeCommand;

		// Token: 0x0400009B RID: 155
		private RelayCommand _connectCommand;

		// Token: 0x0400009C RID: 156
		private RelayCommand _loginCommand;

		// Token: 0x0400009D RID: 157
		private RelayCommand _closeOverlayCommand;
	}
}
