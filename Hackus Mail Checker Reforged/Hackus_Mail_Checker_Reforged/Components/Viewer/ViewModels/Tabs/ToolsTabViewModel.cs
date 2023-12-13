using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Hackus_Mail_Checker_Reforged.Components.Tools;
using Hackus_Mail_Checker_Reforged.Components.Tools.Views;
using Hackus_Mail_Checker_Reforged.UI.Models;
using Microsoft.Win32;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.ViewModels.Tabs
{
	// Token: 0x0200018D RID: 397
	internal class ToolsTabViewModel : BindableObject, IDisposable
	{
		// Token: 0x06000BE7 RID: 3047 RVA: 0x0000D0EE File Offset: 0x0000B2EE
		public ToolsTabViewModel(Frame frame)
		{
			this._contentFrame = frame;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0000D113 File Offset: 0x0000B313
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0000D11B File Offset: 0x0000B31B
		public ObservableCollection<BasePath> Paths
		{
			get
			{
				return this._paths;
			}
			set
			{
				this._paths = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(122467350));
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0000D134 File Offset: 0x0000B334
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0000D13C File Offset: 0x0000B33C
		public BasePath Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-228149589));
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0000D155 File Offset: 0x0000B355
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x0000D15D File Offset: 0x0000B35D
		public bool IsProcessing
		{
			get
			{
				return this._isProcessing;
			}
			set
			{
				this._isProcessing = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(468309634));
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0000D176 File Offset: 0x0000B376
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x0000D17E File Offset: 0x0000B37E
		public SortType SortType
		{
			get
			{
				return this._sortType;
			}
			set
			{
				this._sortType = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-2026106227));
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0004136C File Offset: 0x0003F56C
		public RelayCommand StartCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._startCommand) == null)
				{
					result = (this._startCommand = new RelayCommand(delegate(object obj)
					{
						ToolsTabViewModel.<<get_StartCommand>b__23_0>d <<get_StartCommand>b__23_0>d;
						<<get_StartCommand>b__23_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_StartCommand>b__23_0>d.<>4__this = this;
						<<get_StartCommand>b__23_0>d.obj = obj;
						<<get_StartCommand>b__23_0>d.<>1__state = -1;
						<<get_StartCommand>b__23_0>d.<>t__builder.Start<ToolsTabViewModel.<<get_StartCommand>b__23_0>d>(ref <<get_StartCommand>b__23_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x000413A0 File Offset: 0x0003F5A0
		public RelayCommand OpenPageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openPageCommand) == null)
				{
					result = (this._openPageCommand = new RelayCommand(delegate(object obj)
					{
						string a = obj as string;
						if (obj == null)
						{
							return;
						}
						if (a == <Module>.smethod_4<string>(-233792677))
						{
							this._contentFrame.Navigate(new MergePage(this));
							return;
						}
						if (a == <Module>.smethod_3<string>(162877433))
						{
							this._contentFrame.Navigate(new NormalizePage(this));
							return;
						}
						if (a == <Module>.smethod_3<string>(-399008851))
						{
							this._contentFrame.Navigate(new ShufflePage(this));
							return;
						}
						if (a == <Module>.smethod_4<string>(-1802704159))
						{
							this._contentFrame.Navigate(new SortPage(this));
							return;
						}
						if (a == <Module>.smethod_3<string>(1046116942))
						{
							this._contentFrame.Navigate(new DistinctPage(this));
							return;
						}
						if (!(a == <Module>.smethod_6<string>(121769012)))
						{
							return;
						}
						this._contentFrame.Navigate(new SortDomainsPage(this));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x000413D4 File Offset: 0x0003F5D4
		public RelayCommand RemovePathCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removePathCommand) == null)
				{
					result = (this._removePathCommand = new RelayCommand(delegate(object obj)
					{
						BasePath basePath = obj as BasePath;
						if (basePath != null)
						{
							this.Paths.Remove(basePath);
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00041408 File Offset: 0x0003F608
		public RelayCommand AddPathCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addPathCommand) == null)
				{
					result = (this._addPathCommand = new RelayCommand(delegate(object obj)
					{
						OpenFileDialog openFileDialog = new OpenFileDialog();
						openFileDialog.Filter = <Module>.smethod_3<string>(182416367);
						openFileDialog.RestoreDirectory = true;
						bool? flag = openFileDialog.ShowDialog();
						if (flag.GetValueOrDefault() & flag != null)
						{
							FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
							if (fileInfo.Exists)
							{
								BasePath item = new BasePath(fileInfo.Name, fileInfo.FullName);
								if (!this.Paths.Contains(item))
								{
									this.Paths.Add(item);
								}
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0004143C File Offset: 0x0003F63C
		public RelayCommand AddSinglePathCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addSinglePathCommand) == null)
				{
					result = (this._addSinglePathCommand = new RelayCommand(delegate(object obj)
					{
						OpenFileDialog openFileDialog = new OpenFileDialog();
						openFileDialog.Filter = <Module>.smethod_5<string>(1119604328);
						openFileDialog.RestoreDirectory = true;
						bool? flag = openFileDialog.ShowDialog();
						if (flag.GetValueOrDefault() & flag != null)
						{
							FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
							if (fileInfo.Exists)
							{
								this.Path = new BasePath(fileInfo.Name, fileInfo.FullName);
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00041470 File Offset: 0x0003F670
		public RelayCommand DropBaseToPathsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._dropBaseToPathsCommand) == null)
				{
					result = (this._dropBaseToPathsCommand = new RelayCommand(delegate(object obj)
					{
						DragEventArgs dragEventArgs = obj as DragEventArgs;
						if (dragEventArgs == null)
						{
							return;
						}
						if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
						{
							string[] array = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop);
							for (int i = 0; i < array.Length; i++)
							{
								FileInfo fileInfo = new FileInfo(array[i]);
								if (fileInfo.Exists && fileInfo.Extension == <Module>.smethod_6<string>(-1352824492))
								{
									BasePath item = new BasePath(fileInfo.Name, fileInfo.FullName);
									if (!this.Paths.Contains(item))
									{
										this.Paths.Add(item);
									}
								}
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x000414A4 File Offset: 0x0003F6A4
		public RelayCommand DropBaseToPathCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._dropBaseToPathCommand) == null)
				{
					result = (this._dropBaseToPathCommand = new RelayCommand(delegate(object obj)
					{
						DragEventArgs dragEventArgs = obj as DragEventArgs;
						if (dragEventArgs != null)
						{
							if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
							{
								FileInfo fileInfo = new FileInfo(((string[])dragEventArgs.Data.GetData(DataFormats.FileDrop))[0]);
								if (fileInfo.Exists && fileInfo.Extension == <Module>.smethod_4<string>(1865170172))
								{
									this.Path = new BasePath(fileInfo.Name, fileInfo.FullName);
								}
							}
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x000414D8 File Offset: 0x0003F6D8
		public RelayCommand ClosePageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._closePageCommand) == null)
				{
					result = (this._closePageCommand = new RelayCommand(delegate(object obj)
					{
						this._cancellationTokenSource.Cancel();
						this.IsProcessing = false;
						this._contentFrame.Content = null;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0000D197 File Offset: 0x0000B397
		public void Dispose()
		{
			if (!this._isDisposed)
			{
				this._cancellationTokenSource.Cancel();
				this._contentFrame = null;
				this._isDisposed = true;
			}
		}

		// Token: 0x04000656 RID: 1622
		private bool _isDisposed;

		// Token: 0x04000657 RID: 1623
		private Frame _contentFrame;

		// Token: 0x04000658 RID: 1624
		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04000659 RID: 1625
		private CancellationToken _cancellationToken;

		// Token: 0x0400065A RID: 1626
		private ObservableCollection<BasePath> _paths = new ObservableCollection<BasePath>();

		// Token: 0x0400065B RID: 1627
		private BasePath _path;

		// Token: 0x0400065C RID: 1628
		private bool _isProcessing;

		// Token: 0x0400065D RID: 1629
		private SortType _sortType;

		// Token: 0x0400065E RID: 1630
		private RelayCommand _startCommand;

		// Token: 0x0400065F RID: 1631
		private RelayCommand _openPageCommand;

		// Token: 0x04000660 RID: 1632
		private RelayCommand _removePathCommand;

		// Token: 0x04000661 RID: 1633
		private RelayCommand _addPathCommand;

		// Token: 0x04000662 RID: 1634
		private RelayCommand _addSinglePathCommand;

		// Token: 0x04000663 RID: 1635
		private RelayCommand _dropBaseToPathsCommand;

		// Token: 0x04000664 RID: 1636
		private RelayCommand _dropBaseToPathCommand;

		// Token: 0x04000665 RID: 1637
		private RelayCommand _closePageCommand;
	}
}
