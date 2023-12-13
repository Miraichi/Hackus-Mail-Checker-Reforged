using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Settings
{
	// Token: 0x02000064 RID: 100
	internal class ProxySettings : BindableObject
	{
		// Token: 0x06000310 RID: 784 RVA: 0x00006C91 File Offset: 0x00004E91
		private ProxySettings()
		{
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000081C1 File Offset: 0x000063C1
		// (set) Token: 0x06000312 RID: 786 RVA: 0x000081D7 File Offset: 0x000063D7
		public static ProxySettings Instance
		{
			get
			{
				ProxySettings result;
				if ((result = ProxySettings._instance) == null)
				{
					result = (ProxySettings._instance = new ProxySettings());
				}
				return result;
			}
			set
			{
				ProxySettings._instance = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000081DF File Offset: 0x000063DF
		// (set) Token: 0x06000314 RID: 788 RVA: 0x000081E7 File Offset: 0x000063E7
		public bool UseProxy
		{
			get
			{
				return this._useProxy;
			}
			set
			{
				this._useProxy = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-233141280));
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00008200 File Offset: 0x00006400
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00008208 File Offset: 0x00006408
		public ProxyType ProxyType
		{
			get
			{
				return this._proxyType;
			}
			set
			{
				this._proxyType = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(193318320));
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00008221 File Offset: 0x00006421
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00008229 File Offset: 0x00006429
		public bool UseAuthentication
		{
			get
			{
				return this._useAuthentication;
			}
			set
			{
				this._useAuthentication = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1184879036));
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00008242 File Offset: 0x00006442
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000824A File Offset: 0x0000644A
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00008263 File Offset: 0x00006463
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000826B File Offset: 0x0000646B
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1433986953));
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00008284 File Offset: 0x00006484
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000828C File Offset: 0x0000648C
		public bool UseAutoUpdate
		{
			get
			{
				return this._useAutoUpdate;
			}
			set
			{
				this._useAutoUpdate = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(829457829));
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000082A5 File Offset: 0x000064A5
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000082AD File Offset: 0x000064AD
		public int UpdateDelay
		{
			get
			{
				return this._UpdateDelay;
			}
			set
			{
				this._UpdateDelay = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1131525718));
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000082C6 File Offset: 0x000064C6
		// (set) Token: 0x06000322 RID: 802 RVA: 0x000082CE File Offset: 0x000064CE
		public string WebLinks
		{
			get
			{
				return this._webLinks;
			}
			set
			{
				this._webLinks = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-843189084));
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000323 RID: 803 RVA: 0x000082E7 File Offset: 0x000064E7
		// (set) Token: 0x06000324 RID: 804 RVA: 0x000082EF File Offset: 0x000064EF
		public bool UseWebSources
		{
			get
			{
				return this._useWebSources;
			}
			set
			{
				this._useWebSources = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(345565894));
			}
		}

		// Token: 0x040001C5 RID: 453
		private static ProxySettings _instance;

		// Token: 0x040001C6 RID: 454
		private bool _useProxy;

		// Token: 0x040001C7 RID: 455
		private ProxyType _proxyType;

		// Token: 0x040001C8 RID: 456
		private bool _useAuthentication;

		// Token: 0x040001C9 RID: 457
		private string _login;

		// Token: 0x040001CA RID: 458
		private string _password;

		// Token: 0x040001CB RID: 459
		private bool _useAutoUpdate;

		// Token: 0x040001CC RID: 460
		private int _UpdateDelay;

		// Token: 0x040001CD RID: 461
		private string _webLinks;

		// Token: 0x040001CE RID: 462
		private bool _useWebSources;
	}
}
