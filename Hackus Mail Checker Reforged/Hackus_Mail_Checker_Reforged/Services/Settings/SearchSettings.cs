using System;
using System.Collections.ObjectModel;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Settings
{
	// Token: 0x02000066 RID: 102
	internal class SearchSettings : BindableObject
	{
		// Token: 0x06000336 RID: 822 RVA: 0x000179C0 File Offset: 0x00015BC0
		private SearchSettings()
		{
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000843D File Offset: 0x0000663D
		// (set) Token: 0x06000338 RID: 824 RVA: 0x00008453 File Offset: 0x00006653
		public static SearchSettings Instance
		{
			get
			{
				SearchSettings result;
				if ((result = SearchSettings._instance) == null)
				{
					result = (SearchSettings._instance = new SearchSettings());
				}
				return result;
			}
			set
			{
				SearchSettings._instance = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000845B File Offset: 0x0000665B
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00008463 File Offset: 0x00006663
		public ObservableCollection<Request> Requests
		{
			get
			{
				return this._requests;
			}
			set
			{
				this._requests = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1897553056));
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000847C File Offset: 0x0000667C
		// (set) Token: 0x0600033C RID: 828 RVA: 0x00008484 File Offset: 0x00006684
		public ObservableCollection<RequestGroup> RequestGroups
		{
			get
			{
				return this._requestGroups;
			}
			set
			{
				this._requestGroups = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(963700017));
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000849D File Offset: 0x0000669D
		// (set) Token: 0x0600033E RID: 830 RVA: 0x000084A5 File Offset: 0x000066A5
		public ObservableCollection<Folder> Folders
		{
			get
			{
				return this._folders;
			}
			set
			{
				this._folders = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-533819503));
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000084BE File Offset: 0x000066BE
		// (set) Token: 0x06000340 RID: 832 RVA: 0x000084C6 File Offset: 0x000066C6
		public ObservableCollection<string> AttachmentFilters
		{
			get
			{
				return this._attachmentFilters;
			}
			set
			{
				this._attachmentFilters = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1795958182));
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000341 RID: 833 RVA: 0x000084DF File Offset: 0x000066DF
		// (set) Token: 0x06000342 RID: 834 RVA: 0x000084E7 File Offset: 0x000066E7
		public bool Search
		{
			get
			{
				return this._search;
			}
			set
			{
				this._search = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(2016064082));
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00008500 File Offset: 0x00006700
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00008508 File Offset: 0x00006708
		public bool UseSearchLimit
		{
			get
			{
				return this._useSearchLimit;
			}
			set
			{
				this._useSearchLimit = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(723166958));
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00008521 File Offset: 0x00006721
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00008529 File Offset: 0x00006729
		public int SearchLimit
		{
			get
			{
				return this._searchLimit;
			}
			set
			{
				this._searchLimit = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(846201352));
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00008542 File Offset: 0x00006742
		// (set) Token: 0x06000348 RID: 840 RVA: 0x0000854A File Offset: 0x0000674A
		public bool CheckDate
		{
			get
			{
				return this._checkDate;
			}
			set
			{
				this._checkDate = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(466298620));
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00008563 File Offset: 0x00006763
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000856B File Offset: 0x0000676B
		public DateTime? DateFrom
		{
			get
			{
				return this._dateFrom;
			}
			set
			{
				this._dateFrom = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-827578586));
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00008584 File Offset: 0x00006784
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000858C File Offset: 0x0000678C
		public DateTime? DateTo
		{
			get
			{
				return this._dateTo;
			}
			set
			{
				this._dateTo = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-739661742));
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000085A5 File Offset: 0x000067A5
		// (set) Token: 0x0600034E RID: 846 RVA: 0x000085AD File Offset: 0x000067AD
		public bool DownloadLetters
		{
			get
			{
				return this._downloadLetters;
			}
			set
			{
				this._downloadLetters = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(201690757));
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600034F RID: 847 RVA: 0x000085C6 File Offset: 0x000067C6
		// (set) Token: 0x06000350 RID: 848 RVA: 0x000085CE File Offset: 0x000067CE
		public int DownloadLettersLimit
		{
			get
			{
				return this._downloadLettersLimit;
			}
			set
			{
				this._downloadLettersLimit = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(208362237));
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000351 RID: 849 RVA: 0x000085E7 File Offset: 0x000067E7
		// (set) Token: 0x06000352 RID: 850 RVA: 0x000085EF File Offset: 0x000067EF
		public bool DownloadIntoSingleFile
		{
			get
			{
				return this._downloadIntoSingleFile;
			}
			set
			{
				this._downloadIntoSingleFile = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1687262369));
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00008608 File Offset: 0x00006808
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00008610 File Offset: 0x00006810
		public bool DeleteWhenDownloaded
		{
			get
			{
				return this._deleteWhenDownloaded;
			}
			set
			{
				this._deleteWhenDownloaded = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(650882830));
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00008629 File Offset: 0x00006829
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00008631 File Offset: 0x00006831
		public DownloadMode DownloadMode
		{
			get
			{
				return this._downloadMode;
			}
			set
			{
				this._downloadMode = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1733965450));
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000864A File Offset: 0x0000684A
		// (set) Token: 0x06000358 RID: 856 RVA: 0x00008652 File Offset: 0x00006852
		public FoldersMode FoldersMode
		{
			get
			{
				return this._foldersMode;
			}
			set
			{
				this._foldersMode = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1905602616));
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000866B File Offset: 0x0000686B
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00008673 File Offset: 0x00006873
		public bool SearchAttachments
		{
			get
			{
				return this._searchAttachments;
			}
			set
			{
				this._searchAttachments = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1125509948));
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000868C File Offset: 0x0000688C
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00008694 File Offset: 0x00006894
		public SearchAttachmentsMode SearchAttachmentsMode
		{
			get
			{
				return this._searchAttachmentsMode;
			}
			set
			{
				this._searchAttachmentsMode = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-942015058));
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600035D RID: 861 RVA: 0x000086AD File Offset: 0x000068AD
		// (set) Token: 0x0600035E RID: 862 RVA: 0x000086B5 File Offset: 0x000068B5
		public bool ParseContacts
		{
			get
			{
				return this._parseContacts;
			}
			set
			{
				this._parseContacts = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1886959755));
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600035F RID: 863 RVA: 0x000086CE File Offset: 0x000068CE
		// (set) Token: 0x06000360 RID: 864 RVA: 0x000086D6 File Offset: 0x000068D6
		public bool UseAttachmentFilters
		{
			get
			{
				return this._useAttachmentFilters;
			}
			set
			{
				this._useAttachmentFilters = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-355499576));
			}
		}

		// Token: 0x040001D7 RID: 471
		private static SearchSettings _instance;

		// Token: 0x040001D8 RID: 472
		private ObservableCollection<Request> _requests = new ObservableCollection<Request>();

		// Token: 0x040001D9 RID: 473
		private ObservableCollection<RequestGroup> _requestGroups = new ObservableCollection<RequestGroup>();

		// Token: 0x040001DA RID: 474
		private ObservableCollection<Folder> _folders = new ObservableCollection<Folder>
		{
			new Folder(<Module>.smethod_4<string>(-521243277))
		};

		// Token: 0x040001DB RID: 475
		private ObservableCollection<string> _attachmentFilters = new ObservableCollection<string>();

		// Token: 0x040001DC RID: 476
		private bool _search;

		// Token: 0x040001DD RID: 477
		private bool _useSearchLimit;

		// Token: 0x040001DE RID: 478
		private int _searchLimit = 5;

		// Token: 0x040001DF RID: 479
		private bool _checkDate;

		// Token: 0x040001E0 RID: 480
		private DateTime? _dateFrom;

		// Token: 0x040001E1 RID: 481
		private DateTime? _dateTo;

		// Token: 0x040001E2 RID: 482
		private bool _downloadLetters;

		// Token: 0x040001E3 RID: 483
		private int _downloadLettersLimit = 5;

		// Token: 0x040001E4 RID: 484
		private bool _downloadIntoSingleFile;

		// Token: 0x040001E5 RID: 485
		private bool _deleteWhenDownloaded;

		// Token: 0x040001E6 RID: 486
		private DownloadMode _downloadMode;

		// Token: 0x040001E7 RID: 487
		private FoldersMode _foldersMode;

		// Token: 0x040001E8 RID: 488
		private bool _searchAttachments;

		// Token: 0x040001E9 RID: 489
		private SearchAttachmentsMode _searchAttachmentsMode = SearchAttachmentsMode.InDownloaded;

		// Token: 0x040001EA RID: 490
		private bool _parseContacts;

		// Token: 0x040001EB RID: 491
		private bool _useAttachmentFilters;
	}
}
