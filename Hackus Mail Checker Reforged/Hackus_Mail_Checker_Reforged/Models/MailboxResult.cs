using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000134 RID: 308
	public class MailboxResult : BindableObject
	{
		// Token: 0x06000993 RID: 2451 RVA: 0x00006C91 File Offset: 0x00004E91
		public MailboxResult()
		{
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0000BC08 File Offset: 0x00009E08
		public MailboxResult(Mailbox mailbox)
		{
			this.Address = mailbox.Address;
			this.Password = mailbox.Password;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0000BC28 File Offset: 0x00009E28
		public MailboxResult(string address, string password)
		{
			this.Address = address;
			this.Password = password;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0000BC3E File Offset: 0x00009E3E
		public MailboxResult(Mailbox mailbox, string request, int count) : this(mailbox)
		{
			this.Request = request;
			this.Count = count;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0000BC55 File Offset: 0x00009E55
		public MailboxResult(string address, string password, string request, int count) : this(address, password)
		{
			this.Request = request;
			this.Count = count;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0000BC6E File Offset: 0x00009E6E
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0000BC76 File Offset: 0x00009E76
		public string Address
		{
			get
			{
				return this._address;
			}
			set
			{
				this._address = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-731926710));
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0000BC8F File Offset: 0x00009E8F
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0000BC97 File Offset: 0x00009E97
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		public string Request
		{
			get
			{
				return this._request;
			}
			set
			{
				this._request = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1134872088));
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0000BCD1 File Offset: 0x00009ED1
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0000BCD9 File Offset: 0x00009ED9
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(530817781));
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0000BCF2 File Offset: 0x00009EF2
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0000BCFA File Offset: 0x00009EFA
		public Proxy Proxy
		{
			get
			{
				return this._proxy;
			}
			set
			{
				this._proxy = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(617979530));
			}
		}

		// Token: 0x0400049B RID: 1179
		private string _address;

		// Token: 0x0400049C RID: 1180
		private string _password;

		// Token: 0x0400049D RID: 1181
		private string _request;

		// Token: 0x0400049E RID: 1182
		private int _count;

		// Token: 0x0400049F RID: 1183
		private Proxy _proxy;
	}
}
