using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x02000071 RID: 113
	internal class MailManager : BindableObject
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00008E35 File Offset: 0x00007035
		public static MailManager Instance
		{
			get
			{
				MailManager result;
				if ((result = MailManager._instance) == null)
				{
					result = (MailManager._instance = new MailManager());
				}
				return result;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000190CC File Offset: 0x000172CC
		private MailManager()
		{
			this.MailboxResults = new ObservableCollection<MailboxResult>();
			BindingOperations.EnableCollectionSynchronization(this.MailboxResults, this._locker);
			this.FilteredMailboxResults.Filter = ((object o) => this.FilterResults(o as MailboxResult));
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00008E4B File Offset: 0x0000704B
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00008E53 File Offset: 0x00007053
		public string SearchQuery
		{
			get
			{
				return this._searchQuery;
			}
			set
			{
				this._searchQuery = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(422498998));
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00008E6C File Offset: 0x0000706C
		public ICollectionView FilteredMailboxResults
		{
			get
			{
				return CollectionViewSource.GetDefaultView(this.MailboxResults);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00008E79 File Offset: 0x00007079
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x00008E81 File Offset: 0x00007081
		public ObservableCollection<MailboxResult> MailboxResults
		{
			get
			{
				return this._mailboxResults;
			}
			set
			{
				this._mailboxResults = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1419643292));
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00019154 File Offset: 0x00017354
		public Mailbox GetMailbox()
		{
			Mailbox result = null;
			while (!this._mailboxQueue.TryDequeue(out result))
			{
				if (this._mailboxQueue.IsEmpty)
				{
					return null;
				}
				Thread.Sleep(20);
			}
			return result;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001918C File Offset: 0x0001738C
		public void AddResult(MailboxResult result)
		{
			object locker = this._locker;
			lock (locker)
			{
				this.MailboxResults.Add(result);
			}
			if (result.Request != null)
			{
				StatisticsManager.Instance.AddRequestValue(new RequestResult(result.Request, result.Count));
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000191F8 File Offset: 0x000173F8
		public void ClearResults()
		{
			object locker = this._locker;
			lock (locker)
			{
				this.MailboxResults.Clear();
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00019240 File Offset: 0x00017440
		public void UploadBase(List<Mailbox> mailboxes)
		{
			if (mailboxes != null && mailboxes.Any<Mailbox>())
			{
				object locker = this._locker;
				lock (locker)
				{
					this._mailboxStorage = mailboxes;
					StatisticsManager.Instance.LoadedStrings = this._mailboxStorage.Count;
				}
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000192A4 File Offset: 0x000174A4
		public void LoadQueue()
		{
			object locker = this._locker;
			lock (locker)
			{
				this._mailboxQueue = new ConcurrentQueue<Mailbox>(this._mailboxStorage);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00008E9A File Offset: 0x0000709A
		public int Count()
		{
			return this._mailboxStorage.Count<Mailbox>();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00008EA7 File Offset: 0x000070A7
		public bool Any()
		{
			return this._mailboxStorage.Any<Mailbox>();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000192F0 File Offset: 0x000174F0
		public void AddToQueue(Mailbox mailbox)
		{
			object locker = this._locker;
			lock (locker)
			{
				this._mailboxQueue.Enqueue(mailbox);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00008EB4 File Offset: 0x000070B4
		public List<Mailbox> QueueToList()
		{
			if (this._mailboxQueue.Any<Mailbox>())
			{
				return new List<Mailbox>(this._mailboxQueue);
			}
			return null;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00019338 File Offset: 0x00017538
		private bool FilterResults(MailboxResult mailboxResult)
		{
			return mailboxResult == null || mailboxResult.Address.ContainsIgnoreCase(this.SearchQuery) || mailboxResult.Password.ContainsIgnoreCase(this.SearchQuery) || (!string.IsNullOrEmpty(mailboxResult.Request) && mailboxResult.Request.ContainsIgnoreCase(this.SearchQuery));
		}

		// Token: 0x0400022D RID: 557
		private static MailManager _instance;

		// Token: 0x0400022E RID: 558
		private object _locker = new object();

		// Token: 0x0400022F RID: 559
		private List<Mailbox> _mailboxStorage = new List<Mailbox>();

		// Token: 0x04000230 RID: 560
		private ConcurrentQueue<Mailbox> _mailboxQueue = new ConcurrentQueue<Mailbox>();

		// Token: 0x04000231 RID: 561
		public HashSet<string> SavedLogins = new HashSet<string>();

		// Token: 0x04000232 RID: 562
		public HashSet<string> SavedContacts = new HashSet<string>();

		// Token: 0x04000233 RID: 563
		private string _searchQuery = "";

		// Token: 0x04000234 RID: 564
		private ObservableCollection<MailboxResult> _mailboxResults;
	}
}
