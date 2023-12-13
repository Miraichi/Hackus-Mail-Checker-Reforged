using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Models.EventArgs;
using Hackus_Mail_Checker_Reforged.Net;
using Hackus_Mail_Checker_Reforged.Services.Background;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.Services.Shared;
using Hackus_Mail_Checker_Reforged.UI.Models;
using Hackus_Mail_Checker_Reforged.UI.Pages.Notifications;
using HandyControl.Controls;
using HandyControl.Data;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x0200007C RID: 124
	internal class ThreadsManager : BindableObject
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x00019C8C File Offset: 0x00017E8C
		private ThreadsManager()
		{
			BindingOperations.EnableCollectionSynchronization(this.Threads, this._locker);
			this.Threads.CollectionChanged += this.OnThreadsCollectionChanged;
			this.ThreadsCancelled = (EventHandler)Delegate.Combine(this.ThreadsCancelled, new EventHandler(this.OnThreadsCancelled));
			this.ThreadCancelled = (EventHandler<ThreadCancelledEventArgs>)Delegate.Combine(this.ThreadCancelled, new EventHandler<ThreadCancelledEventArgs>(this.OnThreadCancelled));
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00009448 File Offset: 0x00007648
		public static ThreadsManager Instance
		{
			get
			{
				ThreadsManager result;
				if ((result = ThreadsManager._instance) == null)
				{
					result = (ThreadsManager._instance = new ThreadsManager());
				}
				return result;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0000945E File Offset: 0x0000765E
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x00009466 File Offset: 0x00007666
		public ObservableCollection<Thread> Threads
		{
			get
			{
				return this._threads;
			}
			set
			{
				this._threads = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-488624271));
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0000947F File Offset: 0x0000767F
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x00009487 File Offset: 0x00007687
		public CheckerState State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(267025275));
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000094A0 File Offset: 0x000076A0
		public int ThreadsCount
		{
			get
			{
				return this.Threads.Count;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00019D34 File Offset: 0x00017F34
		public void Run()
		{
			if (this._state == CheckerState.Stopped)
			{
				this.State = CheckerState.Running;
				StatisticsManager.Instance.ClearResults();
				MailManager.Instance.ClearResults();
				FileManager.CreateResultsDirectory();
				RelayCommand.Validate();
				HostFinder.Refresh();
				HostFinderHandler.Clear();
				BackgroundStopwatch.Start();
				BackgroundSpeedMeter.Start();
				BackgroundRestSaver.Start();
				BackgroundProxyLoader.Start();
				MailManager.Instance.LoadQueue();
				MailManager.Instance.SavedLogins.Clear();
				MailManager.Instance.SavedContacts.Clear();
				int threadsCount = this.GetOptimalThreadsCount();
				ThreadStart <>9__1;
				Task.Run(delegate()
				{
					for (int i = 0; i < threadsCount; i++)
					{
						ThreadStart threadStart_;
						if ((threadStart_ = <>9__1) == null)
						{
							threadStart_ = (<>9__1 = delegate()
							{
								this.SafeExecute(new Action(Worker.Run));
							});
						}
						Thread thread = ThreadsManager.<>c__DisplayClass18_0.smethod_0(threadStart_);
						ThreadsManager.<>c__DisplayClass18_0.smethod_1(thread, true);
						ThreadsManager.<>c__DisplayClass18_0.smethod_2(thread);
						object locker = this._locker;
						bool flag = false;
						try
						{
							ThreadsManager.<>c__DisplayClass18_0.smethod_3(locker, ref flag);
							this.Threads.Add(thread);
						}
						finally
						{
							if (flag)
							{
								ThreadsManager.<>c__DisplayClass18_0.smethod_4(locker);
							}
						}
					}
				});
				return;
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000094AD File Offset: 0x000076AD
		public void Pause()
		{
			if (this._state != CheckerState.Running)
			{
				return;
			}
			this.WaitHandle.Reset();
			this.State = CheckerState.Paused;
			RelayCommand.Validate();
			BackgroundStopwatch.Stop();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000094D5 File Offset: 0x000076D5
		public void Resume()
		{
			if (this._state != CheckerState.Paused)
			{
				return;
			}
			this.WaitHandle.Set();
			this.State = CheckerState.Running;
			RelayCommand.Validate();
			BackgroundStopwatch.Resume();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019DE4 File Offset: 0x00017FE4
		public void Stop()
		{
			if (this._state != CheckerState.Stopped)
			{
				if (this._state != CheckerState.Closing)
				{
					this.State = CheckerState.Closing;
					this.WaitHandle.Set();
					object locker = this._locker;
					lock (locker)
					{
						foreach (Thread thread in this.Threads)
						{
							try
							{
								thread.Abort();
							}
							catch
							{
							}
						}
					}
					return;
				}
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00019E94 File Offset: 0x00018094
		private void SafeExecute(Action action)
		{
			try
			{
				action();
			}
			catch (ThreadAbortException)
			{
				this.ThreadCancelled(this, new ThreadCancelledEventArgs(Thread.CurrentThread));
			}
			catch (Exception exception)
			{
				FileManager.LogUnhandledException(exception, <Module>.smethod_5<string>(-1131597857));
				this.ThreadCancelled(this, new ThreadCancelledEventArgs(Thread.CurrentThread));
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019F08 File Offset: 0x00018108
		private int GetOptimalThreadsCount()
		{
			int num = MailManager.Instance.Count();
			int threads = CheckerSettings.Instance.Threads;
			if (threads > num)
			{
				return num;
			}
			return threads;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000094FE File Offset: 0x000076FE
		private void OnThreadsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.OnPropertyChanged(<Module>.smethod_4<string>(-2045655874));
			if (!this.Threads.Any<Thread>())
			{
				this.ThreadsCancelled(this, null);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00019F34 File Offset: 0x00018134
		private void OnThreadCancelled(object sender, ThreadCancelledEventArgs e)
		{
			object locker = this._locker;
			lock (locker)
			{
				this.Threads.Remove(e.Thread);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000952A File Offset: 0x0000772A
		private void OnThreadsCancelled(object sender, EventArgs e)
		{
			Application.Current.Dispatcher.Invoke(delegate()
			{
				BackgroundSpeedMeter.Stop();
				BackgroundRestSaver.Stop();
				BackgroundProxyLoader.Stop();
				this.State = CheckerState.Stopped;
				RelayCommand.Validate();
				BackgroundStopwatch.Stop();
				BackgroundStopwatch.Show();
				if (CheckerSettings.Instance.NotifyOnCompleted)
				{
					SystemSounds.Beep.Play();
					Notification.Show(new CompleteNotification(), ShowAnimation.Fade, false);
				}
				ReportService.ReportServers();
			});
		}

		// Token: 0x0400026A RID: 618
		private static ThreadsManager _instance;

		// Token: 0x0400026B RID: 619
		private ObservableCollection<Thread> _threads = new ObservableCollection<Thread>();

		// Token: 0x0400026C RID: 620
		private CheckerState _state = CheckerState.Stopped;

		// Token: 0x0400026D RID: 621
		public EventHandler ThreadsCancelled;

		// Token: 0x0400026E RID: 622
		public EventHandler<ThreadCancelledEventArgs> ThreadCancelled;

		// Token: 0x0400026F RID: 623
		public ManualResetEvent WaitHandle = new ManualResetEvent(true);

		// Token: 0x04000270 RID: 624
		private object _locker = new object();
	}
}
