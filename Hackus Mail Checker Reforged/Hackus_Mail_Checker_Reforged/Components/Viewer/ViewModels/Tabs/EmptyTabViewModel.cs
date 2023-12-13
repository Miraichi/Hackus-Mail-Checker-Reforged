using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using MailBee.ImapMail;
using MailBee.Pop3Mail;
using MailBee.Proxy;
using MailBee.Security;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs
{
	// Token: 0x02000169 RID: 361
	internal class EmptyTabViewModel : BindableObject, IDisposable
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x0000C5E9 File Offset: 0x0000A7E9
		public EmptyTabViewModel()
		{
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0003C228 File Offset: 0x0003A428
		public EmptyTabViewModel(MailboxResult mailboxResult)
		{
			this.Login = mailboxResult.Address;
			this.Password = mailboxResult.Password;
			this.LoginCommand.Execute(null);
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0000C607 File Offset: 0x0000A807
		public ViewerSettings ViewerSettings
		{
			get
			{
				return ViewerSettings.Instance;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0000C60E File Offset: 0x0000A80E
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0003C278 File Offset: 0x0003A478
		public string Login
		{
			get
			{
				return this._login;
			}
			set
			{
				if (value.Contains(':'))
				{
					int num = value.IndexOf(':');
					this._login = value.Substring(0, num);
					this.Password = value.Substring(num + 1, value.Length - num - 1);
					if (this.Password != string.Empty)
					{
						this.LoginCommand.Execute(null);
					}
				}
				else if (value.Contains(';'))
				{
					int num2 = value.IndexOf(';');
					this._login = value.Substring(0, num2);
					this.Password = value.Substring(num2 + 1, value.Length - num2 - 1);
					if (this.Password != string.Empty)
					{
						this.LoginCommand.Execute(null);
					}
				}
				else
				{
					this._login = value;
				}
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1156029122));
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0000C616 File Offset: 0x0000A816
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0000C61E File Offset: 0x0000A81E
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1928335903));
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0000C637 File Offset: 0x0000A837
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x0000C63F File Offset: 0x0000A83F
		public string ErrorMessage
		{
			get
			{
				return this._errorMessage;
			}
			set
			{
				this._errorMessage = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-435032899));
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0000C658 File Offset: 0x0000A858
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0000C660 File Offset: 0x0000A860
		public bool IsBusy
		{
			get
			{
				return this._isBusy;
			}
			set
			{
				this._isBusy = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1014052106));
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0003C350 File Offset: 0x0003A550
		public RelayCommand LoginCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._loginCommand) == null)
				{
					result = (this._loginCommand = new RelayCommand(delegate(object obj)
					{
						EmptyTabViewModel.<<get_LoginCommand>b__23_0>d <<get_LoginCommand>b__23_0>d;
						<<get_LoginCommand>b__23_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_LoginCommand>b__23_0>d.<>4__this = this;
						<<get_LoginCommand>b__23_0>d.<>1__state = -1;
						<<get_LoginCommand>b__23_0>d.<>t__builder.Start<EmptyTabViewModel.<<get_LoginCommand>b__23_0>d>(ref <<get_LoginCommand>b__23_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0003C384 File Offset: 0x0003A584
		public RelayCommand OpenToolsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openToolsCommand) == null)
				{
					result = (this._openToolsCommand = new RelayCommand(delegate(object obj)
					{
						ViewerController.Instance.OpenToolsTab();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0003C3CC File Offset: 0x0003A5CC
		private void SetupImap(Imap imap, Server server)
		{
			if (server.Socket == SocketType.SSL)
			{
				imap.SslMode = SslStartupMode.OnConnect;
			}
			else
			{
				imap.SslMode = SslStartupMode.Manual;
			}
			imap.Timeout = ViewerSettings.Instance.Timeout * 1000;
			if (this.ViewerSettings.UseProxy)
			{
				if (this.ViewerSettings.ProxyTakeType != ProxyTakeType.Random)
				{
					if (this.ViewerSettings.IsValidProxy())
					{
						imap.Proxy.Name = this.ViewerSettings.Host;
						imap.Proxy.Port = this.ViewerSettings.Port;
						if (this.ViewerSettings.UseAuthentication)
						{
							imap.Proxy.AccountName = this.ViewerSettings.Username;
							imap.Proxy.Password = this.ViewerSettings.Password;
						}
						switch (this.ViewerSettings.ProxyType)
						{
						case ProxyType.HTTP:
							imap.Proxy.Protocol = ProxyProtocol.Http;
							return;
						case ProxyType.SOCKS4:
							imap.Proxy.Protocol = ProxyProtocol.Socks4;
							return;
						case ProxyType.SOCKS5:
							imap.Proxy.Protocol = ProxyProtocol.Socks5;
							break;
						default:
							return;
						}
					}
				}
				else if (ProxyManager.Instance.Any())
				{
					ProxyClient proxy = ProxyManager.Instance.GetProxy();
					imap.Proxy.Name = proxy.Host;
					imap.Proxy.Port = proxy.Port;
					if (!string.IsNullOrEmpty(proxy.Username))
					{
						imap.Proxy.AccountName = proxy.Username;
						imap.Proxy.Password = proxy.Password;
					}
					switch (ProxySettings.Instance.ProxyType)
					{
					case ProxyType.HTTP:
						imap.Proxy.Protocol = ProxyProtocol.Http;
						return;
					case ProxyType.SOCKS4:
						imap.Proxy.Protocol = ProxyProtocol.Socks4;
						return;
					case ProxyType.SOCKS5:
						imap.Proxy.Protocol = ProxyProtocol.Socks5;
						return;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0003C598 File Offset: 0x0003A798
		private void SetupPop3(Pop3 pop3, Server server)
		{
			if (server.Socket == SocketType.SSL)
			{
				pop3.SslMode = SslStartupMode.OnConnect;
			}
			else
			{
				pop3.SslMode = SslStartupMode.Manual;
			}
			pop3.Timeout = ViewerSettings.Instance.Timeout * 1000;
			if (this.ViewerSettings.UseProxy)
			{
				if (this.ViewerSettings.ProxyTakeType != ProxyTakeType.Random)
				{
					if (this.ViewerSettings.IsValidProxy())
					{
						pop3.Proxy.Name = this.ViewerSettings.Host;
						pop3.Proxy.Port = this.ViewerSettings.Port;
						if (this.ViewerSettings.UseAuthentication)
						{
							pop3.Proxy.AccountName = this.ViewerSettings.Username;
							pop3.Proxy.Password = this.ViewerSettings.Password;
						}
						switch (this.ViewerSettings.ProxyType)
						{
						case ProxyType.HTTP:
							pop3.Proxy.Protocol = ProxyProtocol.Http;
							return;
						case ProxyType.SOCKS4:
							pop3.Proxy.Protocol = ProxyProtocol.Socks4;
							return;
						case ProxyType.SOCKS5:
							pop3.Proxy.Protocol = ProxyProtocol.Socks5;
							break;
						default:
							return;
						}
					}
				}
				else if (ProxyManager.Instance.Any())
				{
					ProxyClient proxy = ProxyManager.Instance.GetProxy();
					pop3.Proxy.Name = proxy.Host;
					pop3.Proxy.Port = proxy.Port;
					if (!string.IsNullOrEmpty(proxy.Username))
					{
						pop3.Proxy.AccountName = proxy.Username;
						pop3.Proxy.Password = proxy.Password;
					}
					switch (ProxySettings.Instance.ProxyType)
					{
					case ProxyType.HTTP:
						pop3.Proxy.Protocol = ProxyProtocol.Http;
						return;
					case ProxyType.SOCKS4:
						pop3.Proxy.Protocol = ProxyProtocol.Socks4;
						return;
					case ProxyType.SOCKS5:
						pop3.Proxy.Protocol = ProxyProtocol.Socks5;
						return;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0000C679 File Offset: 0x0000A879
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				this._isDisposed = true;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000559 RID: 1369
		private bool _isDisposed;

		// Token: 0x0400055A RID: 1370
		private string _login = "";

		// Token: 0x0400055B RID: 1371
		private string _password = "";

		// Token: 0x0400055C RID: 1372
		private string _errorMessage;

		// Token: 0x0400055D RID: 1373
		private bool _isBusy;

		// Token: 0x0400055E RID: 1374
		private RelayCommand _loginCommand;

		// Token: 0x0400055F RID: 1375
		private RelayCommand _openToolsCommand;
	}
}
