using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Settings
{
	// Token: 0x02000068 RID: 104
	internal class WebSettings : BindableObject
	{
		// Token: 0x0600037F RID: 895 RVA: 0x0000891F File Offset: 0x00006B1F
		private WebSettings()
		{
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000893C File Offset: 0x00006B3C
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00008952 File Offset: 0x00006B52
		public static WebSettings Instance
		{
			get
			{
				WebSettings result;
				if ((result = WebSettings._instance) == null)
				{
					result = (WebSettings._instance = new WebSettings());
				}
				return result;
			}
			set
			{
				WebSettings._instance = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000895A File Offset: 0x00006B5A
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00008962 File Offset: 0x00006B62
		public bool RebruteCaptcha
		{
			get
			{
				return this._rebruteCaptcha;
			}
			set
			{
				this._rebruteCaptcha = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(944641913));
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000897B File Offset: 0x00006B7B
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00008983 File Offset: 0x00006B83
		public int RebruteCaptchaLimit
		{
			get
			{
				return this._rebruteCaptchaLimit;
			}
			set
			{
				this._rebruteCaptchaLimit = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(821201803));
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0000899C File Offset: 0x00006B9C
		// (set) Token: 0x06000387 RID: 903 RVA: 0x000089A4 File Offset: 0x00006BA4
		public bool CheckWeb
		{
			get
			{
				return this._checkWeb;
			}
			set
			{
				this._checkWeb = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-21627623));
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000388 RID: 904 RVA: 0x000089BD File Offset: 0x00006BBD
		// (set) Token: 0x06000389 RID: 905 RVA: 0x000089C5 File Offset: 0x00006BC5
		public bool CheckMailRu
		{
			get
			{
				return this._checkMailRu;
			}
			set
			{
				this._checkMailRu = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-2075117795));
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600038A RID: 906 RVA: 0x000089DE File Offset: 0x00006BDE
		// (set) Token: 0x0600038B RID: 907 RVA: 0x000089E6 File Offset: 0x00006BE6
		public MailRuMethod MailRuMethod
		{
			get
			{
				return this._mailRuMethod;
			}
			set
			{
				this._mailRuMethod = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1969531692));
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000089FF File Offset: 0x00006BFF
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00008A07 File Offset: 0x00006C07
		public bool CheckYandex
		{
			get
			{
				return this._checkYandex;
			}
			set
			{
				this._checkYandex = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-660761832));
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00008A20 File Offset: 0x00006C20
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00008A28 File Offset: 0x00006C28
		public bool CheckRambler
		{
			get
			{
				return this._checkRambler;
			}
			set
			{
				this._checkRambler = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(18782460));
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00008A41 File Offset: 0x00006C41
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00008A49 File Offset: 0x00006C49
		public bool CheckGmx
		{
			get
			{
				return this._checkGmx;
			}
			set
			{
				this._checkGmx = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1626381584));
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00008A62 File Offset: 0x00006C62
		// (set) Token: 0x06000393 RID: 915 RVA: 0x00008A6A File Offset: 0x00006C6A
		public bool CheckOnet
		{
			get
			{
				return this._checkOnet;
			}
			set
			{
				this._checkOnet = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-377876255));
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00008A83 File Offset: 0x00006C83
		// (set) Token: 0x06000395 RID: 917 RVA: 0x00008A8B File Offset: 0x00006C8B
		public bool CheckProton
		{
			get
			{
				return this._checkProton;
			}
			set
			{
				this._checkProton = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(288268644));
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00008AA4 File Offset: 0x00006CA4
		// (set) Token: 0x06000397 RID: 919 RVA: 0x00008AAC File Offset: 0x00006CAC
		public bool CheckYahoo
		{
			get
			{
				return this._checkYahoo;
			}
			set
			{
				this._checkYahoo = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-91634088));
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00008AC5 File Offset: 0x00006CC5
		// (set) Token: 0x06000399 RID: 921 RVA: 0x00008ACD File Offset: 0x00006CCD
		public bool CheckAol
		{
			get
			{
				return this._checkAol;
			}
			set
			{
				this._checkAol = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-471536820));
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00008AE6 File Offset: 0x00006CE6
		// (set) Token: 0x0600039B RID: 923 RVA: 0x00008AEE File Offset: 0x00006CEE
		public bool CheckApple
		{
			get
			{
				return this._checkApple;
			}
			set
			{
				this._checkApple = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-341499453));
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00008B07 File Offset: 0x00006D07
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00008B0F File Offset: 0x00006D0F
		public bool CheckInteria
		{
			get
			{
				return this._checkInteria;
			}
			set
			{
				this._checkInteria = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1130763513));
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00008B28 File Offset: 0x00006D28
		// (set) Token: 0x0600039F RID: 927 RVA: 0x00008B30 File Offset: 0x00006D30
		public bool CheckSeznam
		{
			get
			{
				return this._checkSeznam;
			}
			set
			{
				this._checkSeznam = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(99602626));
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00008B49 File Offset: 0x00006D49
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x00008B51 File Offset: 0x00006D51
		public bool SolveCaptcha
		{
			get
			{
				return this._solveCaptcha;
			}
			set
			{
				this._solveCaptcha = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(599423315));
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00008B6A File Offset: 0x00006D6A
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x00008B72 File Offset: 0x00006D72
		public CaptchaSolvationService CaptchaSolvationService
		{
			get
			{
				return this._captchaSolvationService;
			}
			set
			{
				this._captchaSolvationService = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1088926777));
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00008B8B File Offset: 0x00006D8B
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00008B93 File Offset: 0x00006D93
		public string CaptchaSolvationKey
		{
			get
			{
				return this._captchaSolvationKey;
			}
			set
			{
				this._captchaSolvationKey = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-43307037));
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00008BAC File Offset: 0x00006DAC
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00008BB4 File Offset: 0x00006DB4
		public bool ParseMailRuContacts
		{
			get
			{
				return this._parseMailRuContacts;
			}
			set
			{
				this._parseMailRuContacts = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-428661202));
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00008BCD File Offset: 0x00006DCD
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00008BD5 File Offset: 0x00006DD5
		public bool DeleteMailRuWarningMessages
		{
			get
			{
				return this._deleteMailRuWarningMessages;
			}
			set
			{
				this._deleteMailRuWarningMessages = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-209229593));
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00008BEE File Offset: 0x00006DEE
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00008BF6 File Offset: 0x00006DF6
		public bool DeleteYandexWarningMessages
		{
			get
			{
				return this._deleteYandexWarningMessages;
			}
			set
			{
				this._deleteYandexWarningMessages = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-197339210));
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00008C0F File Offset: 0x00006E0F
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00008C17 File Offset: 0x00006E17
		public bool EnableYandexImapAccess
		{
			get
			{
				return this._enableYandexImapAccess;
			}
			set
			{
				this._enableYandexImapAccess = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(883573352));
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00008C30 File Offset: 0x00006E30
		// (set) Token: 0x060003AF RID: 943 RVA: 0x00008C38 File Offset: 0x00006E38
		public bool EnableGmxImapAccess
		{
			get
			{
				return this._enableGmxImapAccess;
			}
			set
			{
				this._enableGmxImapAccess = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(772710004));
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00008C51 File Offset: 0x00006E51
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x00008C59 File Offset: 0x00006E59
		public bool DeleteYahooWarningMessages
		{
			get
			{
				return this._deleteYahooWarningMessages;
			}
			set
			{
				this._deleteYahooWarningMessages = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-160270058));
			}
		}

		// Token: 0x040001FA RID: 506
		private static WebSettings _instance;

		// Token: 0x040001FB RID: 507
		private bool _rebruteCaptcha = true;

		// Token: 0x040001FC RID: 508
		private int _rebruteCaptchaLimit = 5;

		// Token: 0x040001FD RID: 509
		private bool _checkWeb = true;

		// Token: 0x040001FE RID: 510
		private bool _checkMailRu;

		// Token: 0x040001FF RID: 511
		private MailRuMethod _mailRuMethod;

		// Token: 0x04000200 RID: 512
		private bool _checkYandex;

		// Token: 0x04000201 RID: 513
		private bool _checkRambler;

		// Token: 0x04000202 RID: 514
		private bool _checkGmx;

		// Token: 0x04000203 RID: 515
		private bool _checkOnet;

		// Token: 0x04000204 RID: 516
		private bool _checkProton;

		// Token: 0x04000205 RID: 517
		private bool _checkYahoo;

		// Token: 0x04000206 RID: 518
		private bool _checkAol;

		// Token: 0x04000207 RID: 519
		private bool _checkApple;

		// Token: 0x04000208 RID: 520
		private bool _checkInteria;

		// Token: 0x04000209 RID: 521
		private bool _checkSeznam;

		// Token: 0x0400020A RID: 522
		private bool _solveCaptcha;

		// Token: 0x0400020B RID: 523
		private CaptchaSolvationService _captchaSolvationService;

		// Token: 0x0400020C RID: 524
		private string _captchaSolvationKey;

		// Token: 0x0400020D RID: 525
		private bool _parseMailRuContacts;

		// Token: 0x0400020E RID: 526
		private bool _deleteMailRuWarningMessages;

		// Token: 0x0400020F RID: 527
		private bool _deleteYandexWarningMessages;

		// Token: 0x04000210 RID: 528
		private bool _enableYandexImapAccess;

		// Token: 0x04000211 RID: 529
		private bool _enableGmxImapAccess;

		// Token: 0x04000212 RID: 530
		private bool _deleteYahooWarningMessages;
	}
}
