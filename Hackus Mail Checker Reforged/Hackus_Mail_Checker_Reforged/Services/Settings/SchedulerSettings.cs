using System;
using System.Collections.ObjectModel;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Settings
{
	// Token: 0x02000065 RID: 101
	public class SchedulerSettings : BindableObject
	{
		// Token: 0x06000325 RID: 805 RVA: 0x00008308 File Offset: 0x00006508
		private SchedulerSettings()
		{
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00008338 File Offset: 0x00006538
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000834E File Offset: 0x0000654E
		public static SchedulerSettings Instance
		{
			get
			{
				SchedulerSettings result;
				if ((result = SchedulerSettings._instance) == null)
				{
					result = (SchedulerSettings._instance = new SchedulerSettings());
				}
				return result;
			}
			set
			{
				SchedulerSettings._instance = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00008356 File Offset: 0x00006556
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0000835E File Offset: 0x0000655E
		public ObservableCollection<Request> Requests
		{
			get
			{
				return this._requests;
			}
			set
			{
				this._requests = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1589259968));
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00008377 File Offset: 0x00006577
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000837F File Offset: 0x0000657F
		public int MaxThreads
		{
			get
			{
				return this._maxThreads;
			}
			set
			{
				this._maxThreads = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-83667061));
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00008398 File Offset: 0x00006598
		// (set) Token: 0x0600032D RID: 813 RVA: 0x000083A0 File Offset: 0x000065A0
		public int Delay
		{
			get
			{
				return this._delay;
			}
			set
			{
				this._delay = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-2109388035));
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600032E RID: 814 RVA: 0x000083B9 File Offset: 0x000065B9
		// (set) Token: 0x0600032F RID: 815 RVA: 0x000083C1 File Offset: 0x000065C1
		public bool UseProxy
		{
			get
			{
				return this._useProxy;
			}
			set
			{
				this._useProxy = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-2007371735));
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000330 RID: 816 RVA: 0x000083DA File Offset: 0x000065DA
		// (set) Token: 0x06000331 RID: 817 RVA: 0x000083E2 File Offset: 0x000065E2
		public bool EnableNotifications
		{
			get
			{
				return this._enableNotifications;
			}
			set
			{
				this._enableNotifications = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(350261376));
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000332 RID: 818 RVA: 0x000083FB File Offset: 0x000065FB
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00008403 File Offset: 0x00006603
		public bool DownloadMails
		{
			get
			{
				return this._downloadMails;
			}
			set
			{
				this._downloadMails = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1422395917));
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000841C File Offset: 0x0000661C
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00008424 File Offset: 0x00006624
		public bool DeleteMails
		{
			get
			{
				return this._deleteMails;
			}
			set
			{
				this._deleteMails = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(658446298));
			}
		}

		// Token: 0x040001CF RID: 463
		private static SchedulerSettings _instance;

		// Token: 0x040001D0 RID: 464
		private ObservableCollection<Request> _requests = new ObservableCollection<Request>();

		// Token: 0x040001D1 RID: 465
		private int _maxThreads = 50;

		// Token: 0x040001D2 RID: 466
		private int _delay = 3;

		// Token: 0x040001D3 RID: 467
		private bool _useProxy;

		// Token: 0x040001D4 RID: 468
		private bool _enableNotifications = true;

		// Token: 0x040001D5 RID: 469
		private bool _downloadMails = true;

		// Token: 0x040001D6 RID: 470
		private bool _deleteMails;
	}
}
