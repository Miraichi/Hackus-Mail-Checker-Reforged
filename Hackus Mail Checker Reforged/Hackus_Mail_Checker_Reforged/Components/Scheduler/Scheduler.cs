using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.Services.Shared;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler
{
	// Token: 0x020001CF RID: 463
	public class Scheduler : BindableObject
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0000DE39 File Offset: 0x0000C039
		public static Scheduler Instance
		{
			get
			{
				Scheduler result;
				if ((result = Scheduler._instance) == null)
				{
					result = (Scheduler._instance = new Scheduler());
				}
				return result;
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0000DE4F File Offset: 0x0000C04F
		private Scheduler()
		{
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0000DE8E File Offset: 0x0000C08E
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x0000DE96 File Offset: 0x0000C096
		public ObservableCollection<ScheduledMail> Mails
		{
			get
			{
				return this._mails;
			}
			set
			{
				this._mails = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1514250622));
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0000DEAF File Offset: 0x0000C0AF
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0000DEB7 File Offset: 0x0000C0B7
		public ObservableCollection<Notification> Notifications
		{
			get
			{
				return this._notifications;
			}
			set
			{
				this._notifications = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(470929222));
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x0000DED8 File Offset: 0x0000C0D8
		public ConcurrentQueue<ScheduledMail> WaitingMails { get; set; } = new ConcurrentQueue<ScheduledMail>();

		// Token: 0x06000D98 RID: 3480 RVA: 0x00045BC0 File Offset: 0x00043DC0
		public void Start()
		{
			if (this._isActive)
			{
				return;
			}
			this._isActive = true;
			if (this._dispatcher == null)
			{
				this.RunDispatcher();
			}
			this._threads = new List<Thread>();
			for (int i = 0; i < SchedulerSettings.Instance.MaxThreads; i++)
			{
				Thread thread = new Thread(delegate()
				{
					ScheduledWorker.Run();
				});
				thread.IsBackground = true;
				thread.Start();
				this._threads.Add(thread);
			}
			this.Notifications.Clear();
			this.Notifications.CollectionChanged += this.Notifications_CollectionChanged;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00045C6C File Offset: 0x00043E6C
		private void Notifications_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			IList newItems = e.NewItems;
			if (newItems != null && newItems.Count > 0 && SchedulerSettings.Instance.EnableNotifications)
			{
				foreach (object obj in newItems)
				{
					Notification notification = (Notification)obj;
					Notifier.ShowSchedulerNotification(notification.Address, notification.Message);
				}
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00045CEC File Offset: 0x00043EEC
		public void Stop()
		{
			if (!this._isActive)
			{
				return;
			}
			DispatcherTimer dispatcher = this._dispatcher;
			if (dispatcher != null)
			{
				dispatcher.Stop();
			}
			this._dispatcher = null;
			this.WaitingMails = new ConcurrentQueue<ScheduledMail>();
			foreach (Thread thread in this._threads)
			{
				try
				{
					thread.Abort();
				}
				catch
				{
				}
			}
			object mailsLocker = this._mailsLocker;
			lock (mailsLocker)
			{
				foreach (ScheduledMail scheduledMail in this.Mails)
				{
					scheduledMail.LastExecuted = null;
					scheduledMail.Status = MailStatus.Stopped;
					scheduledMail.Context = null;
				}
			}
			this._isActive = false;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00045E04 File Offset: 0x00044004
		public void AddNotification(Notification notification)
		{
			object notificationsLocker = this._notificationsLocker;
			lock (notificationsLocker)
			{
				this.Notifications.Add(notification);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00045E4C File Offset: 0x0004404C
		public void RunDispatcher()
		{
			this._dispatcher = new DispatcherTimer(DispatcherPriority.Normal);
			this._dispatcher.Interval = TimeSpan.FromSeconds(1.0);
			this._dispatcher.Tick += this.ProcessAccounts;
			this._dispatcher.Start();
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00045EA4 File Offset: 0x000440A4
		public void RemoveAllMails()
		{
			object mailsLocker = this._mailsLocker;
			lock (mailsLocker)
			{
				foreach (ScheduledMail item in (from m in this.Mails
				where m.Status != MailStatus.Processing && m.Status != MailStatus.Waiting
				select m).ToList<ScheduledMail>())
				{
					this.Mails.Remove(item);
				}
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00045F50 File Offset: 0x00044150
		private void ProcessAccounts(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			if (this._dispatcher.Interval == TimeSpan.FromSeconds(1.0))
			{
				this._dispatcher.Stop();
				this._dispatcher.Interval = TimeSpan.FromSeconds(10.0);
				this._dispatcher.Start();
			}
			object mailsLocker = this._mailsLocker;
			lock (mailsLocker)
			{
				foreach (ScheduledMail scheduledMail in this.Mails)
				{
					if (scheduledMail.LastExecuted != null)
					{
						if ((now - scheduledMail.LastExecuted).Value.TotalMinutes > (double)SchedulerSettings.Instance.Delay && scheduledMail.Status == MailStatus.Stopped)
						{
							scheduledMail.Status = MailStatus.Waiting;
							this.WaitingMails.Enqueue(scheduledMail);
						}
					}
					else if (scheduledMail.Status == MailStatus.Stopped)
					{
						scheduledMail.Status = MailStatus.Waiting;
						this.WaitingMails.Enqueue(scheduledMail);
					}
				}
			}
		}

		// Token: 0x04000743 RID: 1859
		private static Scheduler _instance;

		// Token: 0x04000744 RID: 1860
		private object _mailsLocker = new object();

		// Token: 0x04000745 RID: 1861
		private object _notificationsLocker = new object();

		// Token: 0x04000746 RID: 1862
		private bool _isActive;

		// Token: 0x04000747 RID: 1863
		private DispatcherTimer _dispatcher;

		// Token: 0x04000748 RID: 1864
		private List<Thread> _threads;

		// Token: 0x04000749 RID: 1865
		private ObservableCollection<ScheduledMail> _mails = new ObservableCollection<ScheduledMail>();

		// Token: 0x0400074A RID: 1866
		private ObservableCollection<Notification> _notifications = new ObservableCollection<Notification>();
	}
}
