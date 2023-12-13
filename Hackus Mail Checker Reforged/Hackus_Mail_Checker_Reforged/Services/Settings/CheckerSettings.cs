using System;
using System.Collections.ObjectModel;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Settings
{
	// Token: 0x02000063 RID: 99
	internal class CheckerSettings : BindableObject
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0001792C File Offset: 0x00015B2C
		private CheckerSettings()
		{
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00007DA4 File Offset: 0x00005FA4
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00007DBA File Offset: 0x00005FBA
		public static CheckerSettings Instance
		{
			get
			{
				CheckerSettings result;
				if ((result = CheckerSettings._instance) == null)
				{
					result = (CheckerSettings._instance = new CheckerSettings());
				}
				return result;
			}
			set
			{
				CheckerSettings._instance = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00007DC2 File Offset: 0x00005FC2
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00007DCA File Offset: 0x00005FCA
		public ObservableCollection<DomainFilter> IgnoreDomainFilters
		{
			get
			{
				return this._ignoreDomainFilters;
			}
			set
			{
				this._ignoreDomainFilters = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(326775884));
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00007DE3 File Offset: 0x00005FE3
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00007DEB File Offset: 0x00005FEB
		public ObservableCollection<DomainFilter> RebruteDomainFilters
		{
			get
			{
				return this._rebruteDomainFilters;
			}
			set
			{
				this._rebruteDomainFilters = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(359990851));
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00007E04 File Offset: 0x00006004
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00007E0C File Offset: 0x0000600C
		public int Threads
		{
			get
			{
				return this._threads;
			}
			set
			{
				this._threads = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(2142878159));
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00007E25 File Offset: 0x00006025
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00007E2D File Offset: 0x0000602D
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00007E46 File Offset: 0x00006046
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00007E4E File Offset: 0x0000604E
		public int Rebrute
		{
			get
			{
				return this._rebrute;
			}
			set
			{
				this._rebrute = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1438381101));
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00007E67 File Offset: 0x00006067
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00007E6F File Offset: 0x0000606F
		public bool UseIgnoreDomainFilters
		{
			get
			{
				return this._useIgnoreDomainFilters;
			}
			set
			{
				this._useIgnoreDomainFilters = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1523039093));
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00007E88 File Offset: 0x00006088
		// (set) Token: 0x060002DF RID: 735 RVA: 0x00007E90 File Offset: 0x00006090
		public bool UseRebruteDomainFilters
		{
			get
			{
				return this._useRebruteDomainFilters;
			}
			set
			{
				this._useRebruteDomainFilters = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(974931413));
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00007EA9 File Offset: 0x000060A9
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00007EB1 File Offset: 0x000060B1
		public bool ProcessMultipassword
		{
			get
			{
				return this._processMultipassword;
			}
			set
			{
				this._processMultipassword = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(759722163));
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00007ECA File Offset: 0x000060CA
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00007ED2 File Offset: 0x000060D2
		public bool SaveRest
		{
			get
			{
				return this._saveRest;
			}
			set
			{
				this._saveRest = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(2144607944));
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00007EEB File Offset: 0x000060EB
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00007EF3 File Offset: 0x000060F3
		public int SaveRestDelay
		{
			get
			{
				return this._saveRestDelay;
			}
			set
			{
				this._saveRestDelay = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1642961672));
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00007F0C File Offset: 0x0000610C
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00007F14 File Offset: 0x00006114
		public bool StartOnLoadedBase
		{
			get
			{
				return this._startOnLoadedBase;
			}
			set
			{
				this._startOnLoadedBase = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1144446592));
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00007F2D File Offset: 0x0000612D
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00007F35 File Offset: 0x00006135
		public bool RebruteImapWithPop3
		{
			get
			{
				return this._rebruteImapWithPop3;
			}
			set
			{
				this._rebruteImapWithPop3 = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1260433178));
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00007F4E File Offset: 0x0000614E
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00007F56 File Offset: 0x00006156
		public bool UsePop3Limit
		{
			get
			{
				return this._usePop3Limit;
			}
			set
			{
				this._usePop3Limit = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(920115474));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00007F6F File Offset: 0x0000616F
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00007F77 File Offset: 0x00006177
		public int Pop3Limit
		{
			get
			{
				return this._pop3Limit;
			}
			set
			{
				this._pop3Limit = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(6535536));
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00007F90 File Offset: 0x00006190
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00007F98 File Offset: 0x00006198
		public bool RebruteBlocked
		{
			get
			{
				return this._rebruteBlocked;
			}
			set
			{
				this._rebruteBlocked = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-567704454));
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00007FB1 File Offset: 0x000061B1
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00007FB9 File Offset: 0x000061B9
		public bool CheckFolderAccess
		{
			get
			{
				return this._checkFolderAccess;
			}
			set
			{
				this._checkFolderAccess = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1104900779));
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00007FD2 File Offset: 0x000061D2
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x00007FDA File Offset: 0x000061DA
		public AllowedProtocols AllowedProtocols
		{
			get
			{
				return this._allowedProtocols;
			}
			set
			{
				this._allowedProtocols = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(880952412));
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00007FF3 File Offset: 0x000061F3
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00007FFB File Offset: 0x000061FB
		public bool SearchServer
		{
			get
			{
				return this._searchServer;
			}
			set
			{
				this._searchServer = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-158171324));
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00008014 File Offset: 0x00006214
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000801C File Offset: 0x0000621C
		public bool UseProxyToSearchServer
		{
			get
			{
				return this._useProxyToSearchServer;
			}
			set
			{
				this._useProxyToSearchServer = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1764191921));
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00008035 File Offset: 0x00006235
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000803D File Offset: 0x0000623D
		public bool SearchServerByMX
		{
			get
			{
				return this._searchServerByMX;
			}
			set
			{
				this._searchServerByMX = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1991674155));
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00008056 File Offset: 0x00006256
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000805E File Offset: 0x0000625E
		public bool GuessSubdomainByDomain
		{
			get
			{
				return this._guessSubdomainByDomain;
			}
			set
			{
				this._guessSubdomainByDomain = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(688040986));
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00008077 File Offset: 0x00006277
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000807F File Offset: 0x0000627F
		public bool GuessSubdomainByMX
		{
			get
			{
				return this._guessSubdomainByMX;
			}
			set
			{
				this._guessSubdomainByMX = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1744369711));
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00008098 File Offset: 0x00006298
		// (set) Token: 0x060002FF RID: 767 RVA: 0x000080A0 File Offset: 0x000062A0
		public bool SearchServerByAutoConfig
		{
			get
			{
				return this._searchServerByAutoConfig;
			}
			set
			{
				this._searchServerByAutoConfig = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1955911162));
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000300 RID: 768 RVA: 0x000080B9 File Offset: 0x000062B9
		// (set) Token: 0x06000301 RID: 769 RVA: 0x000080C1 File Offset: 0x000062C1
		public bool SearchServerByAutoDiscover
		{
			get
			{
				return this._searchServerByAutoDiscover;
			}
			set
			{
				this._searchServerByAutoDiscover = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-968403880));
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000302 RID: 770 RVA: 0x000080DA File Offset: 0x000062DA
		// (set) Token: 0x06000303 RID: 771 RVA: 0x000080E2 File Offset: 0x000062E2
		public ExtendedProtocolType GuessProtocolType
		{
			get
			{
				return this._guessProtocolType;
			}
			set
			{
				this._guessProtocolType = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1971278138));
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000304 RID: 772 RVA: 0x000080FB File Offset: 0x000062FB
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00008103 File Offset: 0x00006303
		public ExtendedSocketType GuessSocketType
		{
			get
			{
				return this._guessSocketType;
			}
			set
			{
				this._guessSocketType = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-3081102));
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000811C File Offset: 0x0000631C
		// (set) Token: 0x06000307 RID: 775 RVA: 0x00008124 File Offset: 0x00006324
		public ConfigurationProviderType ConfigurationProviderType
		{
			get
			{
				return this._configurationProviderType;
			}
			set
			{
				this._configurationProviderType = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1668770971));
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000813D File Offset: 0x0000633D
		// (set) Token: 0x06000309 RID: 777 RVA: 0x00008145 File Offset: 0x00006345
		public string ConfigurationDatabasePath
		{
			get
			{
				return this._configurationDatabasePath;
			}
			set
			{
				this._configurationDatabasePath = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-81179824));
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000815E File Offset: 0x0000635E
		// (set) Token: 0x0600030B RID: 779 RVA: 0x00008166 File Offset: 0x00006366
		public bool NotifyOnCompleted
		{
			get
			{
				return this._notifyOnCompleted;
			}
			set
			{
				this._notifyOnCompleted = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-825848801));
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000817F File Offset: 0x0000637F
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00008187 File Offset: 0x00006387
		public int ConnectionExceptionRebrute
		{
			get
			{
				return this._connectionExceptionRebrute;
			}
			set
			{
				this._connectionExceptionRebrute = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-2047781818));
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600030E RID: 782 RVA: 0x000081A0 File Offset: 0x000063A0
		// (set) Token: 0x0600030F RID: 783 RVA: 0x000081A8 File Offset: 0x000063A8
		public int TimeoutExceptionRebrute
		{
			get
			{
				return this._timeoutExceptionRebrute;
			}
			set
			{
				this._timeoutExceptionRebrute = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1305184267));
			}
		}

		// Token: 0x040001A5 RID: 421
		private static CheckerSettings _instance;

		// Token: 0x040001A6 RID: 422
		private ObservableCollection<DomainFilter> _ignoreDomainFilters = new ObservableCollection<DomainFilter>();

		// Token: 0x040001A7 RID: 423
		private ObservableCollection<DomainFilter> _rebruteDomainFilters = new ObservableCollection<DomainFilter>();

		// Token: 0x040001A8 RID: 424
		private int _threads = 200;

		// Token: 0x040001A9 RID: 425
		private int _timeout = 15;

		// Token: 0x040001AA RID: 426
		private int _rebrute = 2;

		// Token: 0x040001AB RID: 427
		private bool _useIgnoreDomainFilters;

		// Token: 0x040001AC RID: 428
		private bool _useRebruteDomainFilters;

		// Token: 0x040001AD RID: 429
		private bool _processMultipassword = true;

		// Token: 0x040001AE RID: 430
		private bool _saveRest;

		// Token: 0x040001AF RID: 431
		private int _saveRestDelay = 5;

		// Token: 0x040001B0 RID: 432
		private bool _startOnLoadedBase;

		// Token: 0x040001B1 RID: 433
		private bool _rebruteImapWithPop3;

		// Token: 0x040001B2 RID: 434
		private bool _usePop3Limit = true;

		// Token: 0x040001B3 RID: 435
		private int _pop3Limit = 50;

		// Token: 0x040001B4 RID: 436
		private bool _rebruteBlocked;

		// Token: 0x040001B5 RID: 437
		private bool _checkFolderAccess;

		// Token: 0x040001B6 RID: 438
		private AllowedProtocols _allowedProtocols = AllowedProtocols.Both;

		// Token: 0x040001B7 RID: 439
		private bool _searchServer = true;

		// Token: 0x040001B8 RID: 440
		private bool _useProxyToSearchServer;

		// Token: 0x040001B9 RID: 441
		private bool _searchServerByMX = true;

		// Token: 0x040001BA RID: 442
		private bool _guessSubdomainByDomain = true;

		// Token: 0x040001BB RID: 443
		private bool _guessSubdomainByMX;

		// Token: 0x040001BC RID: 444
		private bool _searchServerByAutoConfig;

		// Token: 0x040001BD RID: 445
		private bool _searchServerByAutoDiscover;

		// Token: 0x040001BE RID: 446
		private ExtendedProtocolType _guessProtocolType;

		// Token: 0x040001BF RID: 447
		private ExtendedSocketType _guessSocketType;

		// Token: 0x040001C0 RID: 448
		private ConfigurationProviderType _configurationProviderType;

		// Token: 0x040001C1 RID: 449
		private string _configurationDatabasePath;

		// Token: 0x040001C2 RID: 450
		private bool _notifyOnCompleted = true;

		// Token: 0x040001C3 RID: 451
		private int _connectionExceptionRebrute = 2;

		// Token: 0x040001C4 RID: 452
		private int _timeoutExceptionRebrute = 1;
	}
}
