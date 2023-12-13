using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000135 RID: 309
	public class Proxy : BindableObject
	{
		// Token: 0x060009A2 RID: 2466 RVA: 0x00006C91 File Offset: 0x00004E91
		public Proxy()
		{
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0000BD13 File Offset: 0x00009F13
		public Proxy(string host, int port)
		{
			this.Host = host;
			this.Port = port;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0000BD29 File Offset: 0x00009F29
		public Proxy(string host, int port, string username, string password) : this(host, port)
		{
			this.UseAuthentication = true;
			this.Username = username;
			this.Password = password;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0000BD49 File Offset: 0x00009F49
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0000BD51 File Offset: 0x00009F51
		public string Host
		{
			get
			{
				return this._host;
			}
			set
			{
				this._host = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-127630368));
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0000BD6A File Offset: 0x00009F6A
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0000BD72 File Offset: 0x00009F72
		public int Port
		{
			get
			{
				return this._port;
			}
			set
			{
				this._port = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1500373833));
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0000BD8B File Offset: 0x00009F8B
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0000BD93 File Offset: 0x00009F93
		public bool UseAuthentication
		{
			get
			{
				return this._useAuthentication;
			}
			set
			{
				this._useAuthentication = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(20832748));
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0000BDAC File Offset: 0x00009FAC
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-188245252));
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0000BDCD File Offset: 0x00009FCD
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0000BDD5 File Offset: 0x00009FD5
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-2024528016));
			}
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0003A8B4 File Offset: 0x00038AB4
		public override bool Equals(object obj)
		{
			Proxy proxy = obj as Proxy;
			if (proxy != null && this.Host == proxy.Host)
			{
				if (this.Port == proxy.Port)
				{
					if (this.Username == proxy.Username && this.Password == proxy.Password)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0003A918 File Offset: 0x00038B18
		public override int GetHashCode()
		{
			return this.Host.GetHashCode() ^ this.Port.GetHashCode();
		}

		// Token: 0x040004A0 RID: 1184
		private string _host;

		// Token: 0x040004A1 RID: 1185
		private int _port;

		// Token: 0x040004A2 RID: 1186
		private bool _useAuthentication;

		// Token: 0x040004A3 RID: 1187
		private string _username;

		// Token: 0x040004A4 RID: 1188
		private string _password;
	}
}
