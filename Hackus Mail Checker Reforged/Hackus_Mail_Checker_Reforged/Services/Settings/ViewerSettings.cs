using System;
using Hackus_Mail_Checker_Reforged.Components.Viewer;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Settings
{
	// Token: 0x02000067 RID: 103
	internal class ViewerSettings : BindableObject
	{
		// Token: 0x06000361 RID: 865 RVA: 0x000086EF File Offset: 0x000068EF
		private ViewerSettings()
		{
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00008715 File Offset: 0x00006915
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0000872B File Offset: 0x0000692B
		public static ViewerSettings Instance
		{
			get
			{
				ViewerSettings result;
				if ((result = ViewerSettings._instance) == null)
				{
					result = (ViewerSettings._instance = new ViewerSettings());
				}
				return result;
			}
			set
			{
				ViewerSettings._instance = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00008733 File Offset: 0x00006933
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0000873B File Offset: 0x0000693B
		public int PaginationLimit
		{
			get
			{
				return this._paginationLimit;
			}
			set
			{
				this._paginationLimit = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1864154205));
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00008754 File Offset: 0x00006954
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000875C File Offset: 0x0000695C
		public int ReconnectLimit
		{
			get
			{
				return this._reconnectLimit;
			}
			set
			{
				this._reconnectLimit = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1542538789));
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00008775 File Offset: 0x00006975
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000877D File Offset: 0x0000697D
		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				this._timeout = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1373932328));
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00008796 File Offset: 0x00006996
		// (set) Token: 0x0600036B RID: 875 RVA: 0x0000879E File Offset: 0x0000699E
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000087B7 File Offset: 0x000069B7
		// (set) Token: 0x0600036D RID: 877 RVA: 0x000087BF File Offset: 0x000069BF
		public ProxyTakeType ProxyTakeType
		{
			get
			{
				return this._proxyTakeType;
			}
			set
			{
				this._proxyTakeType = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(2086494713));
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000087D8 File Offset: 0x000069D8
		// (set) Token: 0x0600036F RID: 879 RVA: 0x000087E0 File Offset: 0x000069E0
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000087F9 File Offset: 0x000069F9
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00008801 File Offset: 0x00006A01
		public string Host
		{
			get
			{
				return this._host;
			}
			set
			{
				this._host = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-202505271));
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000881A File Offset: 0x00006A1A
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00008822 File Offset: 0x00006A22
		public int Port
		{
			get
			{
				return this._port;
			}
			set
			{
				this._port = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1569683094));
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000883B File Offset: 0x00006A3B
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00008843 File Offset: 0x00006A43
		public bool UseAuthentication
		{
			get
			{
				return this._useAuthentication;
			}
			set
			{
				this._useAuthentication = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-367905283));
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000885C File Offset: 0x00006A5C
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00008864 File Offset: 0x00006A64
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-29271243));
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000887D File Offset: 0x00006A7D
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00008885 File Offset: 0x00006A85
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000889E File Offset: 0x00006A9E
		// (set) Token: 0x0600037B RID: 891 RVA: 0x000088A6 File Offset: 0x00006AA6
		public TranslationLanguage TranslationFromLanguage
		{
			get
			{
				return this._translationFromLanguage;
			}
			set
			{
				this._translationFromLanguage = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1342678004));
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600037C RID: 892 RVA: 0x000088BF File Offset: 0x00006ABF
		// (set) Token: 0x0600037D RID: 893 RVA: 0x000088C7 File Offset: 0x00006AC7
		public TranslationLanguage TranslationToLanguage
		{
			get
			{
				return this._translationToLanguage;
			}
			set
			{
				this._translationToLanguage = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1020231785));
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000088E0 File Offset: 0x00006AE0
		public bool IsValidProxy()
		{
			return !string.IsNullOrWhiteSpace(this.Host) && this.Port >= 1 && (!this.UseAuthentication || (!string.IsNullOrWhiteSpace(this.Username) && !string.IsNullOrWhiteSpace(this.Password)));
		}

		// Token: 0x040001EC RID: 492
		private static ViewerSettings _instance;

		// Token: 0x040001ED RID: 493
		private int _paginationLimit = 50;

		// Token: 0x040001EE RID: 494
		private int _reconnectLimit = 2;

		// Token: 0x040001EF RID: 495
		private int _timeout = 10;

		// Token: 0x040001F0 RID: 496
		private bool _useProxy;

		// Token: 0x040001F1 RID: 497
		private ProxyTakeType _proxyTakeType;

		// Token: 0x040001F2 RID: 498
		private ProxyType _proxyType;

		// Token: 0x040001F3 RID: 499
		private string _host;

		// Token: 0x040001F4 RID: 500
		private int _port;

		// Token: 0x040001F5 RID: 501
		private bool _useAuthentication;

		// Token: 0x040001F6 RID: 502
		private string _username;

		// Token: 0x040001F7 RID: 503
		private string _password;

		// Token: 0x040001F8 RID: 504
		private TranslationLanguage _translationFromLanguage;

		// Token: 0x040001F9 RID: 505
		private TranslationLanguage _translationToLanguage = TranslationLanguage.Russian;
	}
}
