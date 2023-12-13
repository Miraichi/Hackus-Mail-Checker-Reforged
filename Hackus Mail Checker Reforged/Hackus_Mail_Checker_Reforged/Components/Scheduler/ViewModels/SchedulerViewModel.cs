using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Views;
using Hackus_Mail_Checker_Reforged.Components.Viewer;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using Hackus_Mail_Checker_Reforged.UI.Pages.Popups;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.ViewModels
{
	// Token: 0x020001D5 RID: 469
	public class SchedulerViewModel : BindableObject
	{
		// Token: 0x06000DBC RID: 3516 RVA: 0x0000DFD3 File Offset: 0x0000C1D3
		public SchedulerViewModel()
		{
			this.FilteredExitedRequests.Filter = ((object o) => this.FilterResults(o as Request));
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0000E008 File Offset: 0x0000C208
		public SchedulerSettings SchedulerSettings
		{
			get
			{
				return SchedulerSettings.Instance;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0000E00F File Offset: 0x0000C20F
		public Scheduler Scheduler
		{
			get
			{
				return Scheduler.Instance;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0000E016 File Offset: 0x0000C216
		public ICollectionView FilteredExitedRequests
		{
			get
			{
				return CollectionViewSource.GetDefaultView(this.ExistedRequests);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0000E023 File Offset: 0x0000C223
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0000E02B File Offset: 0x0000C22B
		public bool IsWorking
		{
			get
			{
				return this._isWorking;
			}
			set
			{
				this._isWorking = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1037860423));
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0000E044 File Offset: 0x0000C244
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0000E04C File Offset: 0x0000C24C
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

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0000E065 File Offset: 0x0000C265
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x0000E06D File Offset: 0x0000C26D
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1500479806));
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0000E086 File Offset: 0x0000C286
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x0000E08E File Offset: 0x0000C28E
		public ObservableCollection<Request> ExistedRequests
		{
			get
			{
				return this._existedRequests;
			}
			set
			{
				this._existedRequests = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-608929445));
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0000E0A7 File Offset: 0x0000C2A7
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x0000E0AF File Offset: 0x0000C2AF
		public int SelectedRequestTypeIndex
		{
			get
			{
				return this._selectedRequestTypeIndex;
			}
			set
			{
				this._selectedRequestTypeIndex = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1274883212));
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x0000E0D0 File Offset: 0x0000C2D0
		public string CreateRequestField
		{
			get
			{
				return this._createRequestField;
			}
			set
			{
				this._createRequestField = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-893236362));
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x0000E0E9 File Offset: 0x0000C2E9
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x0000E0F1 File Offset: 0x0000C2F1
		public string FindRequestField
		{
			get
			{
				return this._findRequestField;
			}
			set
			{
				this._findRequestField = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1736065788));
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0004673C File Offset: 0x0004493C
		public RelayCommand StartCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._startCommand) == null)
				{
					result = (this._startCommand = new RelayCommand(delegate(object obj)
					{
						if (this._isWorking)
						{
							return;
						}
						if (this.SchedulerSettings.UseProxy && !ProxyManager.Instance.Any())
						{
							this.SendSimpleErrorMessage(ResourceHelper.GetResource<string>(<Module>.smethod_2<string>(1471912770)));
							return;
						}
						if (!this.Scheduler.Mails.Any<ScheduledMail>())
						{
							this.SendSimpleErrorMessage(ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(1009561737)));
							return;
						}
						if (this.SchedulerSettings.Requests.Any<Request>())
						{
							Scheduler.Instance.Start();
							this.IsWorking = true;
							return;
						}
						this.SendSimpleErrorMessage(ResourceHelper.GetResource<string>(<Module>.smethod_4<string>(-1657230575)));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x00046770 File Offset: 0x00044970
		public RelayCommand StopCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._stopCommand) == null)
				{
					result = (this._stopCommand = new RelayCommand(delegate(object obj)
					{
						if (!this._isWorking)
						{
							return;
						}
						Scheduler.Instance.Stop();
						this.IsWorking = false;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x000467A4 File Offset: 0x000449A4
		public RelayCommand AddMailCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addMailCommand) == null)
				{
					result = (this._addMailCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.Address) || string.IsNullOrWhiteSpace(this.Password))
						{
							this.SendSimpleErrorMessage(ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(-616148098)));
							return;
						}
						if (this.Scheduler.Mails.FirstOrDefault((ScheduledMail m) => m.Address.EqualsIgnoreCase(this.Address)) != null)
						{
							this.SendSimpleErrorMessage(<Module>.smethod_3<string>(-957040257));
							return;
						}
						ScheduledMail fromString = ScheduledMail.GetFromString(this.Address + <Module>.smethod_6<string>(1827648947) + this.Password);
						if (fromString == null)
						{
							return;
						}
						fromString.Status = MailStatus.Stopped;
						this.Scheduler.Mails.Add(fromString);
						this.Address = string.Empty;
						this.Password = string.Empty;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x000467D8 File Offset: 0x000449D8
		public RelayCommand RemoveMailCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeMailCommand) == null)
				{
					result = (this._removeMailCommand = new RelayCommand(delegate(object obj)
					{
						ScheduledMail scheduledMail = obj as ScheduledMail;
						if (scheduledMail != null && scheduledMail.Status != MailStatus.Processing)
						{
							if (scheduledMail.Status != MailStatus.Waiting)
							{
								this.Scheduler.Mails.Remove(scheduledMail);
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0004680C File Offset: 0x00044A0C
		public RelayCommand OpenThroughEmailViewerCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openThroughEmailViewerCommand) == null)
				{
					result = (this._openThroughEmailViewerCommand = new RelayCommand(delegate(object obj)
					{
						ScheduledMail scheduledMail = obj as ScheduledMail;
						if (scheduledMail != null)
						{
							MailboxResult mailboxResult = new MailboxResult(scheduledMail.Address, scheduledMail.Password);
							ViewerController.Instance.Show(false);
							ViewerController.Instance.OpenNewTab(mailboxResult);
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00046854 File Offset: 0x00044A54
		public RelayCommand LoadMailsFromBaseCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._loadMailsFromBaseCommand) == null)
				{
					result = (this._loadMailsFromBaseCommand = new RelayCommand(delegate(object obj)
					{
						SchedulerViewModel.<<get_LoadMailsFromBaseCommand>b__52_0>d <<get_LoadMailsFromBaseCommand>b__52_0>d;
						<<get_LoadMailsFromBaseCommand>b__52_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_LoadMailsFromBaseCommand>b__52_0>d.<>4__this = this;
						<<get_LoadMailsFromBaseCommand>b__52_0>d.obj = obj;
						<<get_LoadMailsFromBaseCommand>b__52_0>d.<>1__state = -1;
						<<get_LoadMailsFromBaseCommand>b__52_0>d.<>t__builder.Start<SchedulerViewModel.<<get_LoadMailsFromBaseCommand>b__52_0>d>(ref <<get_LoadMailsFromBaseCommand>b__52_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00046888 File Offset: 0x00044A88
		public RelayCommand OpenRequestsPopupCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openRequestsPopupCommand) == null)
				{
					result = (this._openRequestsPopupCommand = new RelayCommand(delegate(object obj)
					{
						this.RefreshExistedRequests();
						AddRequestsPopup addRequestsPopup = SingleOpenHelper.CreateControl<AddRequestsPopup>();
						addRequestsPopup.DataContext = this;
						Point position = Mouse.GetPosition(Application.Current.Windows.OfType<SchedulerWindow>().First<SchedulerWindow>());
						PopupWindow window = new PopupWindow
						{
							Left = position.X,
							Top = position.Y,
							PopupElement = addRequestsPopup,
							Owner = Application.Current.Windows.OfType<SchedulerWindow>().First<SchedulerWindow>()
						};
						addRequestsPopup.Canceled += delegate(object sender, EventArgs e)
						{
							SchedulerViewModel.<>c__DisplayClass55_0.smethod_0(window);
						};
						window.Show(window, false);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x000468BC File Offset: 0x00044ABC
		public RelayCommand SearchCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.searchCommand) == null)
				{
					result = (this.searchCommand = new RelayCommand(delegate(object obj)
					{
						try
						{
							this.FilteredExitedRequests.Refresh();
						}
						catch
						{
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x000468F0 File Offset: 0x00044AF0
		public RelayCommand ResetSearchCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._resetSearchCommand) == null)
				{
					result = (this._resetSearchCommand = new RelayCommand(delegate(object obj)
					{
						this.FindRequestField = "";
						this.FilteredExitedRequests.Refresh();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00046924 File Offset: 0x00044B24
		public RelayCommand CreateRequestCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._createRequestCommand) == null)
				{
					result = (this._createRequestCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.CreateRequestField))
						{
							return;
						}
						string[] array = this.CreateRequestField.Split(new char[]
						{
							';'
						});
						for (int i = 0; i < array.Length; i++)
						{
							string request = array[i];
							if (!string.IsNullOrWhiteSpace(request))
							{
								switch (this.SelectedRequestTypeIndex)
								{
								case 0:
									if (!SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_0.smethod_0(r.Sender, request)))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Sender = request
										});
									}
									break;
								case 1:
									if (!SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_0.smethod_0(r.Subject, request)))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Subject = request
										});
									}
									break;
								case 2:
									if (!SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_0.smethod_0(r.Body, request)))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Body = request
										});
									}
									break;
								case 3:
								{
									string[] parts = request.Split(new char[]
									{
										'='
									});
									if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Sender, parts[0]) && SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Subject, parts[1])))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Sender = parts[0],
											Subject = parts[1]
										});
									}
									break;
								}
								case 4:
								{
									string[] parts = request.Split(new char[]
									{
										'='
									});
									if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Sender, parts[0]) && SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Body, parts[1])))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Sender = parts[0],
											Body = parts[1]
										});
									}
									break;
								}
								case 5:
								{
									string[] parts = request.Split(new char[]
									{
										'='
									});
									if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Subject, parts[0]) && SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Body, parts[1])))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Subject = parts[0],
											Body = parts[1]
										});
									}
									break;
								}
								case 6:
								{
									string[] parts = request.Split(new char[]
									{
										'='
									});
									if (parts.Length >= 3 && parts[0].Length != 0 && parts[1].Length != 0 && parts[2].Length != 0 && !SchedulerSettings.Instance.Requests.Any((Request r) => SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Sender, parts[0]) && SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Subject, parts[1]) && SchedulerViewModel.<>c__DisplayClass64_1.smethod_0(r.Body, parts[2])))
									{
										SchedulerSettings.Instance.Requests.Add(new Request
										{
											Sender = parts[0],
											Subject = parts[1],
											Body = parts[2]
										});
									}
									break;
								}
								}
							}
						}
						this.CreateRequestField = string.Empty;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00046958 File Offset: 0x00044B58
		public RelayCommand RemoveRequestCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeRequestCommand) == null)
				{
					result = (this._removeRequestCommand = new RelayCommand(delegate(object obj)
					{
						Request request = obj as Request;
						if (request != null)
						{
							this.SchedulerSettings.Requests.Remove(request);
							this.FilteredExitedRequests.Refresh();
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0004698C File Offset: 0x00044B8C
		public RelayCommand FetchRequestCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._fetchRequestCommand) == null)
				{
					result = (this._fetchRequestCommand = new RelayCommand(delegate(object obj)
					{
						Request request = obj as Request;
						if (request != null && !SchedulerSettings.Instance.Requests.Any((Request r) => r.Equals(request)))
						{
							SchedulerSettings.Instance.Requests.Add(request.Clone());
							this.FilteredExitedRequests.Refresh();
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x000469C0 File Offset: 0x00044BC0
		public RelayCommand RemoveAllMailsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeAllMailsCommand) == null)
				{
					result = (this._removeAllMailsCommand = new RelayCommand(delegate(object obj)
					{
						this.Scheduler.RemoveAllMails();
					}, (object obj) => this.Scheduler.Mails.Any<ScheduledMail>()));
				}
				return result;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00046A00 File Offset: 0x00044C00
		public RelayCommand OpenResultsFolderCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openResultsFolderCommand) == null)
				{
					result = (this._openResultsFolderCommand = new RelayCommand(delegate(object obj)
					{
						DirectoryInfo fileSystemInfo_ = SchedulerViewModel.<>c.smethod_0(SchedulerFileManager.ResultsPath);
						SchedulerViewModel.<>c.smethod_2(<Module>.smethod_4<string>(-753878610), SchedulerViewModel.<>c.smethod_1(fileSystemInfo_));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00046A48 File Offset: 0x00044C48
		private bool FilterResults(Request request)
		{
			return !SchedulerSettings.Instance.Requests.Contains(request) && (request == null || (request.Sender != null && request.Sender.ContainsIgnoreCase(this.FindRequestField)) || (request.Body != null && request.Body.ContainsIgnoreCase(this.FindRequestField)) || (request.Subject != null && request.Subject.ContainsIgnoreCase(this.FindRequestField)));
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00046AC0 File Offset: 0x00044CC0
		private void RefreshExistedRequests()
		{
			HashSet<Request> hashSet = new HashSet<Request>();
			if (SearchSettings.Instance.Requests.Any<Request>())
			{
				hashSet.AddRange(from a in SearchSettings.Instance.Requests
				select a.Clone());
			}
			if (SearchSettings.Instance.RequestGroups.Any<RequestGroup>())
			{
				foreach (RequestGroup requestGroup in SearchSettings.Instance.RequestGroups)
				{
					hashSet.AddRange(from a in requestGroup.Requests
					select a.Clone());
				}
			}
			this.ExistedRequests.Clear();
			foreach (Request item in hashSet)
			{
				this.ExistedRequests.Add(item);
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00046BE8 File Offset: 0x00044DE8
		private void SendSimpleErrorMessage(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_4<string>(161013751)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_5<string>(-917405454),
				IconKey = <Module>.smethod_5<string>(-1297308186),
				StyleKey = <Module>.smethod_5<string>(375297047),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(-656558181))
			});
		}

		// Token: 0x0400075D RID: 1885
		private bool _isWorking;

		// Token: 0x0400075E RID: 1886
		private string _address;

		// Token: 0x0400075F RID: 1887
		private string _password;

		// Token: 0x04000760 RID: 1888
		private ObservableCollection<Request> _existedRequests = new ObservableCollection<Request>();

		// Token: 0x04000761 RID: 1889
		private int _selectedRequestTypeIndex;

		// Token: 0x04000762 RID: 1890
		private string _createRequestField;

		// Token: 0x04000763 RID: 1891
		private string _findRequestField = string.Empty;

		// Token: 0x04000764 RID: 1892
		private RelayCommand _startCommand;

		// Token: 0x04000765 RID: 1893
		private RelayCommand _stopCommand;

		// Token: 0x04000766 RID: 1894
		private RelayCommand _addMailCommand;

		// Token: 0x04000767 RID: 1895
		private RelayCommand _removeMailCommand;

		// Token: 0x04000768 RID: 1896
		private RelayCommand _openThroughEmailViewerCommand;

		// Token: 0x04000769 RID: 1897
		private RelayCommand _loadMailsFromBaseCommand;

		// Token: 0x0400076A RID: 1898
		private RelayCommand _openRequestsPopupCommand;

		// Token: 0x0400076B RID: 1899
		private RelayCommand searchCommand;

		// Token: 0x0400076C RID: 1900
		private RelayCommand _resetSearchCommand;

		// Token: 0x0400076D RID: 1901
		private RelayCommand _createRequestCommand;

		// Token: 0x0400076E RID: 1902
		private RelayCommand _removeRequestCommand;

		// Token: 0x0400076F RID: 1903
		private RelayCommand _fetchRequestCommand;

		// Token: 0x04000770 RID: 1904
		private RelayCommand _removeAllMailsCommand;

		// Token: 0x04000771 RID: 1905
		private RelayCommand _openResultsFolderCommand;
	}
}
