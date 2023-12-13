using System;
using System.Text.RegularExpressions;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000140 RID: 320
	public class Server : BindableObject
	{
		// Token: 0x060009EB RID: 2539 RVA: 0x0000C076 File Offset: 0x0000A276
		public Server()
		{
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0000C089 File Offset: 0x0000A289
		public Server(string domain, string hostname, int port, ProtocolType protocol, SocketType socket)
		{
			this.Domain = domain;
			this.Hostname = hostname;
			this.Port = port;
			this.Protocol = protocol;
			this.Socket = socket;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0000C0C1 File Offset: 0x0000A2C1
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x0000C0C9 File Offset: 0x0000A2C9
		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				this._domain = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1749812650));
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0000C0E2 File Offset: 0x0000A2E2
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x0000C0EA File Offset: 0x0000A2EA
		public string Hostname
		{
			get
			{
				return this._hostname;
			}
			set
			{
				this._hostname = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(90370938));
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0000C103 File Offset: 0x0000A303
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x0000C10B File Offset: 0x0000A30B
		public int Port
		{
			get
			{
				return this._port;
			}
			set
			{
				this._port = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1623621146));
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0000C124 File Offset: 0x0000A324
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0000C12C File Offset: 0x0000A32C
		public ProtocolType Protocol
		{
			get
			{
				return this._protocol;
			}
			set
			{
				this._protocol = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-74506143));
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0000C145 File Offset: 0x0000A345
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x0000C14D File Offset: 0x0000A34D
		public SocketType Socket
		{
			get
			{
				return this._socket;
			}
			set
			{
				this._socket = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1534346053));
			}
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0003ADD8 File Offset: 0x00038FD8
		public static Server GetFromString(string line)
		{
			Match match = Regex.Match(line, <Module>.smethod_2<string>(1556211310));
			if (!match.Success)
			{
				return null;
			}
			int port;
			if (int.TryParse(match.Groups[4].Value, out port))
			{
				return new Server
				{
					Domain = match.Groups[1].Value,
					Hostname = match.Groups[2].Value,
					Port = port,
					Protocol = ((match.Groups[3].Value == <Module>.smethod_2<string>(1123723022)) ? ProtocolType.IMAP : ProtocolType.POP3),
					Socket = ((match.Groups[5].Value == <Module>.smethod_2<string>(816196536)) ? SocketType.SSL : SocketType.Plain)
				};
			}
			return null;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003AEB4 File Offset: 0x000390B4
		public Server Clone()
		{
			return new Server
			{
				Domain = this.Domain,
				Hostname = this.Hostname,
				Port = this.Port,
				Protocol = this.Protocol,
				Socket = this.Socket
			};
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0003AF04 File Offset: 0x00039104
		public override bool Equals(object obj)
		{
			Server server = obj as Server;
			return server != null && !(this.Domain != server.Domain) && !(this.Hostname != server.Hostname) && this.Port == server.Port && this.Protocol == server.Protocol && this.Socket == server.Socket;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003AF78 File Offset: 0x00039178
		public override int GetHashCode()
		{
			return this.Domain.GetHashCode() ^ this.Hostname.GetHashCode() ^ this.Port.GetHashCode() ^ this.Protocol.GetHashCode() ^ this.Socket.GetHashCode();
		}

		// Token: 0x040004C0 RID: 1216
		private string _domain;

		// Token: 0x040004C1 RID: 1217
		private string _hostname;

		// Token: 0x040004C2 RID: 1218
		private int _port = 993;

		// Token: 0x040004C3 RID: 1219
		private ProtocolType _protocol;

		// Token: 0x040004C4 RID: 1220
		private SocketType _socket;
	}
}
