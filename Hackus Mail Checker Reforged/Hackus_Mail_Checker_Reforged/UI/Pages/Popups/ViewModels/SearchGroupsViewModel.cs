using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.UI.Pages.Popups.ViewModels
{
	// Token: 0x0200004A RID: 74
	public class SearchGroupsViewModel : BindableObject
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00007692 File Offset: 0x00005892
		public SearchGroupsViewModel()
		{
			this.FilteredRequestGroups.Filter = ((object o) => this.FilterResults(o as RequestGroup));
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000076C7 File Offset: 0x000058C7
		public ICollectionView FilteredRequestGroups
		{
			get
			{
				return CollectionViewSource.GetDefaultView(SearchSettings.Instance.RequestGroups);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000076D8 File Offset: 0x000058D8
		// (set) Token: 0x0600024C RID: 588 RVA: 0x000076E0 File Offset: 0x000058E0
		public string CreateRequestGroupField
		{
			get
			{
				return this._createRequestGroupField;
			}
			set
			{
				this._createRequestGroupField = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1073365632));
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000076F9 File Offset: 0x000058F9
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00007701 File Offset: 0x00005901
		public string FindRequestGroupField
		{
			get
			{
				return this._findRequestGroupField;
			}
			set
			{
				this._findRequestGroupField = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1449587395));
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000771A File Offset: 0x0000591A
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00007722 File Offset: 0x00005922
		public string CreateRequestField
		{
			get
			{
				return this._createRequestField;
			}
			set
			{
				this._createRequestField = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(672542803));
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000773B File Offset: 0x0000593B
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00007743 File Offset: 0x00005943
		public string FindRequestField
		{
			get
			{
				return this._findRequestField;
			}
			set
			{
				this._findRequestField = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-334364932));
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000775C File Offset: 0x0000595C
		// (set) Token: 0x06000254 RID: 596 RVA: 0x00007764 File Offset: 0x00005964
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000777D File Offset: 0x0000597D
		// (set) Token: 0x06000256 RID: 598 RVA: 0x00007785 File Offset: 0x00005985
		public RequestGroup SelectedGroup
		{
			get
			{
				return this._selectedGroup;
			}
			set
			{
				this._selectedGroup = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1957078455));
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00016954 File Offset: 0x00014B54
		public RelayCommand CreateRequestGroupCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._createRequestGroupCommand) == null)
				{
					result = (this._createRequestGroupCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.CreateRequestGroupField))
						{
							return;
						}
						if (!SearchSettings.Instance.RequestGroups.Any((RequestGroup g) => g.Name.EqualsIgnoreCase(this.CreateRequestGroupField)))
						{
							SearchSettings.Instance.RequestGroups.Add(new RequestGroup(this.CreateRequestGroupField));
							this.CreateRequestGroupField = string.Empty;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00016988 File Offset: 0x00014B88
		public RelayCommand DeleteRequestGroupCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._deleteRequestGroupCommand) == null)
				{
					result = (this._deleteRequestGroupCommand = new RelayCommand(delegate(object obj)
					{
						RoutedEventArgs routedEventArgs = obj as RoutedEventArgs;
						MenuItem menuItem = ((routedEventArgs != null) ? routedEventArgs.Source : null) as MenuItem;
						RequestGroup requestGroup = ((menuItem != null) ? menuItem.DataContext : null) as RequestGroup;
						if (requestGroup != null)
						{
							SearchSettings.Instance.RequestGroups.Remove(requestGroup);
							if (this.SelectedGroup == requestGroup)
							{
								this.SelectedGroup = null;
							}
							this.FilteredRequestGroups.Refresh();
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000259 RID: 601 RVA: 0x000169BC File Offset: 0x00014BBC
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
							this.FilteredRequestGroups.Refresh();
						}
						catch
						{
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600025A RID: 602 RVA: 0x000169F0 File Offset: 0x00014BF0
		public RelayCommand ResetSearchCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._resetSearchCommand) == null)
				{
					result = (this._resetSearchCommand = new RelayCommand(delegate(object obj)
					{
						this.FindRequestGroupField = "";
						this.FilteredRequestGroups.Refresh();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00016A24 File Offset: 0x00014C24
		public RelayCommand SelectGroupCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._selectGroupCommand) == null)
				{
					result = (this._selectGroupCommand = new RelayCommand(delegate(object obj)
					{
						RoutedEventArgs routedEventArgs = obj as RoutedEventArgs;
						Button button = ((routedEventArgs != null) ? routedEventArgs.Source : null) as Button;
						RequestGroup requestGroup = ((button != null) ? button.DataContext : null) as RequestGroup;
						if (requestGroup != null)
						{
							this.SelectedGroup = requestGroup;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00016A58 File Offset: 0x00014C58
		public RelayCommand CloseGroupCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._closeGroupCommand) == null)
				{
					result = (this._closeGroupCommand = new RelayCommand(delegate(object obj)
					{
						this.SelectedGroup = null;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00016A8C File Offset: 0x00014C8C
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
									if (!this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_0.smethod_0(r.Sender, request)))
									{
										this.SelectedGroup.Requests.Add(new Request
										{
											Sender = request
										});
									}
									break;
								case 1:
									if (!this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_0.smethod_0(r.Subject, request)))
									{
										this.SelectedGroup.Requests.Add(new Request
										{
											Subject = request
										});
									}
									break;
								case 2:
									if (!this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_0.smethod_0(r.Body, request)))
									{
										this.SelectedGroup.Requests.Add(new Request
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
									if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Sender, parts[0]) && SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Subject, parts[1])))
									{
										this.SelectedGroup.Requests.Add(new Request
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
									if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Sender, parts[0]) && SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Body, parts[1])))
									{
										this.SelectedGroup.Requests.Add(new Request
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
									if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Subject, parts[0]) && SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Body, parts[1])))
									{
										this.SelectedGroup.Requests.Add(new Request
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
									if (parts.Length >= 3 && parts[0].Length != 0 && parts[1].Length != 0 && parts[2].Length != 0 && !this.SelectedGroup.Requests.Any((Request r) => SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Sender, parts[0]) && SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Subject, parts[1]) && SearchGroupsViewModel.<>c__DisplayClass47_1.smethod_0(r.Body, parts[2])))
									{
										this.SelectedGroup.Requests.Add(new Request
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00016AC0 File Offset: 0x00014CC0
		public RelayCommand DeleteRequestCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._deleteRequestCommand) == null)
				{
					result = (this._deleteRequestCommand = new RelayCommand(delegate(object obj)
					{
						RoutedEventArgs routedEventArgs = obj as RoutedEventArgs;
						MenuItem menuItem = ((routedEventArgs != null) ? routedEventArgs.Source : null) as MenuItem;
						Request request = ((menuItem != null) ? menuItem.DataContext : null) as Request;
						if (request != null)
						{
							this.SelectedGroup.Requests.Remove(request);
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00016AF4 File Offset: 0x00014CF4
		public RelayCommand FetchGroupCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._fetchGroupCommand) == null)
				{
					result = (this._fetchGroupCommand = new RelayCommand(delegate(object obj)
					{
						RequestGroup requestGroup = obj as RequestGroup;
						if (requestGroup != null)
						{
							IEnumerator<Request> enumerator = requestGroup.Requests.GetEnumerator();
							try
							{
								while (SearchGroupsViewModel.<>c.smethod_0(enumerator))
								{
									Request request = enumerator.Current;
									if (!SearchSettings.Instance.Requests.Any((Request r) => r.Equals(request)))
									{
										SearchSettings.Instance.Requests.Add(request.Clone());
									}
								}
							}
							finally
							{
								if (enumerator != null)
								{
									SearchGroupsViewModel.<>c.smethod_1(enumerator);
								}
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00016B3C File Offset: 0x00014D3C
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
						if (request != null && !SearchSettings.Instance.Requests.Any((Request r) => r.Equals(request)))
						{
							SearchSettings.Instance.Requests.Add(request.Clone());
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000779E File Offset: 0x0000599E
		private bool FilterResults(RequestGroup requestGroup)
		{
			return requestGroup == null || requestGroup.Name.ContainsIgnoreCase(this.FindRequestGroupField);
		}

		// Token: 0x04000166 RID: 358
		private string _createRequestGroupField;

		// Token: 0x04000167 RID: 359
		private string _findRequestGroupField = string.Empty;

		// Token: 0x04000168 RID: 360
		private string _createRequestField;

		// Token: 0x04000169 RID: 361
		private string _findRequestField = string.Empty;

		// Token: 0x0400016A RID: 362
		private int _selectedRequestTypeIndex;

		// Token: 0x0400016B RID: 363
		private RequestGroup _selectedGroup;

		// Token: 0x0400016C RID: 364
		private RelayCommand _createRequestGroupCommand;

		// Token: 0x0400016D RID: 365
		private RelayCommand _deleteRequestGroupCommand;

		// Token: 0x0400016E RID: 366
		private RelayCommand searchCommand;

		// Token: 0x0400016F RID: 367
		private RelayCommand _resetSearchCommand;

		// Token: 0x04000170 RID: 368
		private RelayCommand _selectGroupCommand;

		// Token: 0x04000171 RID: 369
		private RelayCommand _closeGroupCommand;

		// Token: 0x04000172 RID: 370
		private RelayCommand _createRequestCommand;

		// Token: 0x04000173 RID: 371
		private RelayCommand _deleteRequestCommand;

		// Token: 0x04000174 RID: 372
		private RelayCommand _fetchGroupCommand;

		// Token: 0x04000175 RID: 373
		private RelayCommand _fetchRequestCommand;
	}
}
