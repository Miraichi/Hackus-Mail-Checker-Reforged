using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using Hackus_Mail_Checker_Reforged.Components.Scheduler;
using Hackus_Mail_Checker_Reforged.Components.Viewer;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using Hackus_Mail_Checker_Reforged.UI.Pages.Overlays;
using Hackus_Mail_Checker_Reforged.UI.Pages.Popups;
using Hackus_Mail_Checker_Reforged.UI.Pages.Settings;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using Microsoft.Win32;

namespace Hackus_Mail_Checker_Reforged.UI.ViewModels
{
	// Token: 0x02000035 RID: 53
	internal class MainViewModel : BindableObject
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00006CFF File Offset: 0x00004EFF
		private MainViewModel()
		{
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00006D3E File Offset: 0x00004F3E
		public static MainViewModel Instance
		{
			get
			{
				MainViewModel result;
				if ((result = MainViewModel._instance) == null)
				{
					result = (MainViewModel._instance = new MainViewModel());
				}
				return result;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006D54 File Offset: 0x00004F54
		public MailManager MailManager
		{
			get
			{
				return MailManager.Instance;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006D5B File Offset: 0x00004F5B
		public CheckerSettings CheckerSettings
		{
			get
			{
				return CheckerSettings.Instance;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006D62 File Offset: 0x00004F62
		public SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00006D69 File Offset: 0x00004F69
		public ProxySettings ProxySettings
		{
			get
			{
				return ProxySettings.Instance;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00006D70 File Offset: 0x00004F70
		public WebSettings WebSettings
		{
			get
			{
				return WebSettings.Instance;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00006D77 File Offset: 0x00004F77
		public StatisticsManager StatisticsManager
		{
			get
			{
				return StatisticsManager.Instance;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00006D7E File Offset: 0x00004F7E
		public ThreadsManager ThreadsManager
		{
			get
			{
				return ThreadsManager.Instance;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00006D85 File Offset: 0x00004F85
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00006D8D File Offset: 0x00004F8D
		public ProxyLoaderViewModel ProxyLoaderViewModel
		{
			get
			{
				return this._proxyLoaderViewModel;
			}
			set
			{
				this._proxyLoaderViewModel = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1391886552));
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006DA6 File Offset: 0x00004FA6
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00006DAE File Offset: 0x00004FAE
		public string IgnoreDomainFilterInput
		{
			get
			{
				return this._ignoreDomainFilterInput;
			}
			set
			{
				this._ignoreDomainFilterInput = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1953323914));
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006DC7 File Offset: 0x00004FC7
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00006DCF File Offset: 0x00004FCF
		public string RebruteDomainFilterInput
		{
			get
			{
				return this._rebruteDomainFilterInput;
			}
			set
			{
				this._rebruteDomainFilterInput = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1950103960));
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00006DE8 File Offset: 0x00004FE8
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00006DF0 File Offset: 0x00004FF0
		public string AttachmentsFilterInput
		{
			get
			{
				return this._attachmentsFilterInput;
			}
			set
			{
				this._attachmentsFilterInput = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1003567084));
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00006E09 File Offset: 0x00005009
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00006E11 File Offset: 0x00005011
		public string AddSearchRequestInput
		{
			get
			{
				return this._addSearchRequestInput;
			}
			set
			{
				this._addSearchRequestInput = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(2034685351));
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006E2A File Offset: 0x0000502A
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00006E32 File Offset: 0x00005032
		public string FolderInput
		{
			get
			{
				return this._folderInput;
			}
			set
			{
				this._folderInput = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(902088105));
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00006E4B File Offset: 0x0000504B
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00006E53 File Offset: 0x00005053
		public bool IsAnticaptchaLoading
		{
			get
			{
				return this._isAnticaptchaLoading;
			}
			set
			{
				this._isAnticaptchaLoading = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(340201821));
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00006E6C File Offset: 0x0000506C
		public string Language
		{
			get
			{
				if (App.Language.Name == <Module>.smethod_3<string>(11138840))
				{
					return <Module>.smethod_3<string>(-502627605);
				}
				return <Module>.smethod_6<string>(-1199890703);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00013738 File Offset: 0x00011938
		public RelayCommand InitializeCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._initializeCommand) == null)
				{
					result = (this._initializeCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(GlobalSettingsPage).TypeHandle), FrameType.MainSettings);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00013780 File Offset: 0x00011980
		public RelayCommand StartCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._startCommand) == null)
				{
					result = (this._startCommand = new RelayCommand(delegate(object obj)
					{
						MainViewModel.<<get_StartCommand>b__53_0>d <<get_StartCommand>b__53_0>d;
						<<get_StartCommand>b__53_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_StartCommand>b__53_0>d.<>4__this = this;
						<<get_StartCommand>b__53_0>d.<>1__state = -1;
						<<get_StartCommand>b__53_0>d.<>t__builder.Start<MainViewModel.<<get_StartCommand>b__53_0>d>(ref <<get_StartCommand>b__53_0>d);
					}, (object obj) => MailManager.Instance.Any() && ThreadsManager.Instance.State == CheckerState.Stopped && (!ProxySettings.Instance.UseProxy || ProxyManager.Instance.Any() || !MainViewModel.<>c.smethod_1(ProxySettings.Instance.WebLinks))));
				}
				return result;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000137D0 File Offset: 0x000119D0
		public RelayCommand PauseCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._pauseCommand) == null)
				{
					result = (this._pauseCommand = new RelayCommand(delegate(object obj)
					{
						this.ThreadsManager.Pause();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00013804 File Offset: 0x00011A04
		public RelayCommand ResumeCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._resumeCommand) == null)
				{
					result = (this._resumeCommand = new RelayCommand(delegate(object obj)
					{
						this.ThreadsManager.Resume();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00013838 File Offset: 0x00011A38
		public RelayCommand StopCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._stopCommand) == null)
				{
					result = (this._stopCommand = new RelayCommand(delegate(object obj)
					{
						this.ThreadsManager.Stop();
					}, delegate(object obj)
					{
						if (ThreadsManager.Instance.State != CheckerState.Running)
						{
							if (ThreadsManager.Instance.State != CheckerState.Paused)
							{
								return false;
							}
						}
						return true;
					}));
				}
				return result;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00013888 File Offset: 0x00011A88
		public RelayCommand OpenProxyLoaderPageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openProxyLoaderPageCommand) == null)
				{
					result = (this._openProxyLoaderPageCommand = new RelayCommand(delegate(object obj)
					{
						if (this.ProxyLoaderViewModel == null)
						{
							this.ProxyLoaderViewModel = new ProxyLoaderViewModel();
						}
						PagesManager.Instance.OpenCachedPage(typeof(ProxyLoaderPage), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000138BC File Offset: 0x00011ABC
		public RelayCommand OpenWebSettingsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openWebSettingsCommand) == null)
				{
					result = (this._openWebSettingsCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(WebSettingsPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00013904 File Offset: 0x00011B04
		public RelayCommand SelectSettingsPageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._selectSettingsPageCommand) == null)
				{
					result = (this._selectSettingsPageCommand = new RelayCommand(delegate(object obj)
					{
						string string_ = obj as string;
						if (MainViewModel.<>c.smethod_2(string_, <Module>.smethod_4<string>(-1916443268)))
						{
							PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(GlobalSettingsPage).TypeHandle), FrameType.MainSettings);
							return;
						}
						if (MainViewModel.<>c.smethod_2(string_, <Module>.smethod_5<string>(-394840539)))
						{
							PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(SearchSettingsPage).TypeHandle), FrameType.MainSettings);
							return;
						}
						if (MainViewModel.<>c.smethod_2(string_, <Module>.smethod_3<string>(-1460904963)))
						{
							PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(RequestsSettingsPage).TypeHandle), FrameType.MainSettings);
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0001394C File Offset: 0x00011B4C
		public RelayCommand ChangeThemeCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._changeThemeCommand) == null)
				{
					result = (this._changeThemeCommand = new RelayCommand(delegate(object obj)
					{
						if (App.Theme == ApplicationTheme.Light)
						{
							App.Theme = ApplicationTheme.Dark;
							return;
						}
						App.Theme = ApplicationTheme.Light;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00013994 File Offset: 0x00011B94
		public RelayCommand OpenDomainsFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openDomainsFilterCommand) == null)
				{
					result = (this._openDomainsFilterCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(DomainsFilterOverlayPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000139DC File Offset: 0x00011BDC
		public RelayCommand OpenRebruteDomainsFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openRebruteDomainsFilterCommand) == null)
				{
					result = (this._openRebruteDomainsFilterCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(RebruteDomainsFilterOverlayPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00013A24 File Offset: 0x00011C24
		public RelayCommand OpenAttachmentsFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openAttachmentsFilterCommand) == null)
				{
					result = (this._openAttachmentsFilterCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(AttachmentsFilterOverlayPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00013A6C File Offset: 0x00011C6C
		public RelayCommand OpenRequestsManagerCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openRequestsManagerCommand) == null)
				{
					result = (this._openRequestsManagerCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(RequestsManagerPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00013AB4 File Offset: 0x00011CB4
		public RelayCommand AddIgnoreDomainFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addIgnoreDomainFilterCommand) == null)
				{
					result = (this._addIgnoreDomainFilterCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.IgnoreDomainFilterInput))
						{
							return;
						}
						if (!CheckerSettings.Instance.IgnoreDomainFilters.Any((DomainFilter filter) => filter.Domain.EqualsIgnoreCase(this.IgnoreDomainFilterInput)))
						{
							CheckerSettings.Instance.IgnoreDomainFilters.Add(new DomainFilter
							{
								Domain = this.IgnoreDomainFilterInput,
								IsEnabled = true
							});
						}
						this.IgnoreDomainFilterInput = "";
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00013AE8 File Offset: 0x00011CE8
		public RelayCommand AddRebruteDomainFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addRebruteDomainFilterCommand) == null)
				{
					result = (this._addRebruteDomainFilterCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.RebruteDomainFilterInput))
						{
							return;
						}
						if (!CheckerSettings.Instance.RebruteDomainFilters.Any((DomainFilter filter) => filter.Domain.EqualsIgnoreCase(this.RebruteDomainFilterInput)))
						{
							CheckerSettings.Instance.RebruteDomainFilters.Add(new DomainFilter
							{
								Domain = this.RebruteDomainFilterInput,
								IsEnabled = true
							});
						}
						this.RebruteDomainFilterInput = "";
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00013B1C File Offset: 0x00011D1C
		public RelayCommand AddAttachmentsFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addAttachmentsFilterCommand) == null)
				{
					result = (this._addAttachmentsFilterCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.AttachmentsFilterInput))
						{
							return;
						}
						if (!SearchSettings.Instance.AttachmentFilters.Any((string filter) => filter.EqualsIgnoreCase(this.AttachmentsFilterInput)))
						{
							SearchSettings.Instance.AttachmentFilters.Add(this.AttachmentsFilterInput);
						}
						this.AttachmentsFilterInput = "";
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00013B50 File Offset: 0x00011D50
		public RelayCommand AddFolderCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addFolderCommand) == null)
				{
					result = (this._addFolderCommand = new RelayCommand(delegate(object obj)
					{
						if (string.IsNullOrWhiteSpace(this.FolderInput))
						{
							return;
						}
						if (!SearchSettings.Instance.Folders.Any((Folder folder) => folder.Name.EqualsIgnoreCase(this.FolderInput)))
						{
							SearchSettings.Instance.Folders.Add(new Folder
							{
								Name = this.FolderInput,
								IsEnabled = true
							});
						}
						this.FolderInput = "";
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00013B84 File Offset: 0x00011D84
		public RelayCommand RemoveIgnoreDomainFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeIgnoreDomainFilterCommand) == null)
				{
					result = (this._removeIgnoreDomainFilterCommand = new RelayCommand(delegate(object obj)
					{
						DomainFilter domainFilter = obj as DomainFilter;
						if (domainFilter == null)
						{
							return;
						}
						CheckerSettings.Instance.IgnoreDomainFilters.Remove(domainFilter);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00013BCC File Offset: 0x00011DCC
		public RelayCommand RemoveRebruteDomainFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeRebruteDomainFilterCommand) == null)
				{
					result = (this._removeRebruteDomainFilterCommand = new RelayCommand(delegate(object obj)
					{
						DomainFilter domainFilter = obj as DomainFilter;
						if (domainFilter == null)
						{
							return;
						}
						CheckerSettings.Instance.RebruteDomainFilters.Remove(domainFilter);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00013C14 File Offset: 0x00011E14
		public RelayCommand RemoveAttachmentsFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeAttachmentsFilterCommand) == null)
				{
					result = (this._removeAttachmentsFilterCommand = new RelayCommand(delegate(object obj)
					{
						string text = obj as string;
						if (text == null)
						{
							return;
						}
						SearchSettings.Instance.AttachmentFilters.Remove(text);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00013C5C File Offset: 0x00011E5C
		public RelayCommand RemoveFolderCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeFolderCommand) == null)
				{
					result = (this._removeFolderCommand = new RelayCommand(delegate(object obj)
					{
						Folder folder = obj as Folder;
						if (folder != null)
						{
							SearchSettings.Instance.Folders.Remove(folder);
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00013CA4 File Offset: 0x00011EA4
		public RelayCommand CloseOverlayCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._closeOverlayCommand) == null)
				{
					result = (this._closeOverlayCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.ClearFrame(FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00013CEC File Offset: 0x00011EEC
		public RelayCommand AddSearchRequestCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._addSearchRequestCommand) == null)
				{
					result = (this._addSearchRequestCommand = new RelayCommand(delegate(object obj)
					{
						string text = obj as string;
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(this.AddSearchRequestInput))
						{
							string[] array = this.AddSearchRequestInput.Split(new char[]
							{
								';'
							});
							for (int i = 0; i < array.Length; i++)
							{
								string request = array[i];
								if (!string.IsNullOrWhiteSpace(request))
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
									if (num > 2179143197U)
									{
										if (num <= 2692293871U)
										{
											if (num != 2194958255U)
											{
												if (num == 2692293871U)
												{
													if (text == <Module>.smethod_5<string>(-1850869467) && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_0.smethod_0(r.Subject, request)))
													{
														this.SearchSettings.Requests.Add(new Request
														{
															Subject = request
														});
													}
												}
											}
											else if (text == <Module>.smethod_6<string>(275823852))
											{
												string[] parts = request.Split(new char[]
												{
													'='
												});
												if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Sender, parts[0]) && MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Subject, parts[1])))
												{
													this.SearchSettings.Requests.Add(new Request
													{
														Sender = parts[0],
														Subject = parts[1]
													});
												}
											}
										}
										else if (num == 3398851933U)
										{
											if (text == <Module>.smethod_2<string>(163377945))
											{
												string[] parts = request.Split(new char[]
												{
													'='
												});
												if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Subject, parts[0]) && MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Body, parts[1])))
												{
													this.SearchSettings.Requests.Add(new Request
													{
														Subject = parts[0],
														Body = parts[1]
													});
												}
											}
										}
										else if (num == 3849582724U)
										{
											if (text == <Module>.smethod_4<string>(-410651940) && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_0.smethod_0(r.Sender, request)))
											{
												this.SearchSettings.Requests.Add(new Request
												{
													Sender = request
												});
											}
										}
									}
									else if (num != 1116344469U)
									{
										if (num == 1742621218U)
										{
											if (text == <Module>.smethod_6<string>(-1797787579))
											{
												string[] parts = request.Split(new char[]
												{
													'='
												});
												if (parts.Length >= 2 && parts[0].Length != 0 && parts[1].Length != 0 && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Sender, parts[0]) && MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Body, parts[1])))
												{
													this.SearchSettings.Requests.Add(new Request
													{
														Sender = parts[0],
														Body = parts[1]
													});
												}
											}
										}
										else if (num == 2179143197U)
										{
											if (text == <Module>.smethod_4<string>(-851269004))
											{
												string[] parts = request.Split(new char[]
												{
													'='
												});
												if (parts.Length >= 3 && parts[0].Length != 0 && parts[1].Length != 0 && parts[2].Length != 0 && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Sender, parts[0]) && MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Subject, parts[1]) && MainViewModel.<>c__DisplayClass116_1.smethod_0(r.Body, parts[2])))
												{
													this.SearchSettings.Requests.Add(new Request
													{
														Sender = parts[0],
														Subject = parts[1],
														Body = parts[2]
													});
												}
											}
										}
									}
									else if (text == <Module>.smethod_2<string>(-535676075) && !this.SearchSettings.Requests.Any((Request r) => MainViewModel.<>c__DisplayClass116_0.smethod_0(r.Body, request)))
									{
										this.SearchSettings.Requests.Add(new Request
										{
											Body = request
										});
									}
								}
							}
							this.AddSearchRequestInput = string.Empty;
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00013D20 File Offset: 0x00011F20
		public RelayCommand RemoveSearchRequestCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._removeSearchRequestCommand) == null)
				{
					result = (this._removeSearchRequestCommand = new RelayCommand(delegate(object obj)
					{
						Request request = obj as Request;
						if (request == null)
						{
							return;
						}
						this.SearchSettings.Requests.Remove(request);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00013D54 File Offset: 0x00011F54
		public RelayCommand ChangeSearchRequestDateCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._changeSearchRequestDateCommand) == null)
				{
					result = (this._changeSearchRequestDateCommand = new RelayCommand(delegate(object obj)
					{
						Request request = obj as Request;
						if (request != null)
						{
							request.CheckDate = !request.CheckDate;
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00013D9C File Offset: 0x00011F9C
		public RelayCommand LoadBaseCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._loadBaseCommand) == null)
				{
					result = (this._loadBaseCommand = new RelayCommand(delegate(object obj)
					{
						MainViewModel.<<get_LoadBaseCommand>b__125_0>d <<get_LoadBaseCommand>b__125_0>d;
						<<get_LoadBaseCommand>b__125_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_LoadBaseCommand>b__125_0>d.<>4__this = this;
						<<get_LoadBaseCommand>b__125_0>d.obj = obj;
						<<get_LoadBaseCommand>b__125_0>d.<>1__state = -1;
						<<get_LoadBaseCommand>b__125_0>d.<>t__builder.Start<MainViewModel.<<get_LoadBaseCommand>b__125_0>d>(ref <<get_LoadBaseCommand>b__125_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00013DD0 File Offset: 0x00011FD0
		public RelayCommand DropBaseCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._dropBaseCommand) == null)
				{
					result = (this._dropBaseCommand = new RelayCommand(delegate(object obj)
					{
						DragEventArgs dragEventArgs = obj as DragEventArgs;
						if (dragEventArgs != null)
						{
							if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
							{
								FileInfo fileInfo = new FileInfo(((string[])dragEventArgs.Data.GetData(DataFormats.FileDrop))[0]);
								if (fileInfo.Exists && fileInfo.Extension == <Module>.smethod_5<string>(1457383938))
								{
									this.LoadBaseCommand.Execute(fileInfo.FullName);
								}
							}
							return;
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00013E04 File Offset: 0x00012004
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
							this.MailManager.FilteredMailboxResults.Refresh();
						}
						catch
						{
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00013E38 File Offset: 0x00012038
		public RelayCommand ResetSearchCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._resetSearchCommand) == null)
				{
					result = (this._resetSearchCommand = new RelayCommand(delegate(object obj)
					{
						this.MailManager.SearchQuery = "";
						this.MailManager.FilteredMailboxResults.Refresh();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00013E6C File Offset: 0x0001206C
		public RelayCommand EditConfigurationCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._editConfigurationCommand) == null)
				{
					result = (this._editConfigurationCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(ConfigurationPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00013EB4 File Offset: 0x000120B4
		public RelayCommand SetConfigurationProviderCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._setConfigurationProviderCommand) == null)
				{
					result = (this._setConfigurationProviderCommand = new RelayCommand(delegate(object obj)
					{
						if (obj as string == <Module>.smethod_5<string>(1647335304))
						{
							if (this.SendSimpleAskMessage(ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(501842153))) == MessageBoxResult.Yes)
							{
								this.CheckerSettings.ConfigurationProviderType = ConfigurationProviderType.TXT;
								this.CloseApplicationCommand.Execute(null);
								return;
							}
						}
						else
						{
							OpenFileDialog openFileDialog = new OpenFileDialog();
							openFileDialog.Filter = <Module>.smethod_3<string>(1104138520);
							openFileDialog.RestoreDirectory = true;
							bool? flag = openFileDialog.ShowDialog();
							if (flag.GetValueOrDefault() & flag != null)
							{
								if (this.SendSimpleAskMessage(ResourceHelper.GetResource<string>(<Module>.smethod_6<string>(1608983328))) == MessageBoxResult.Yes)
								{
									this.CheckerSettings.ConfigurationProviderType = ConfigurationProviderType.SQL;
									this.CheckerSettings.ConfigurationDatabasePath = openFileDialog.FileName;
									this.CloseApplicationCommand.Execute(null);
								}
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00013EE8 File Offset: 0x000120E8
		public RelayCommand ImportRequestsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._importRequestsCommand) == null)
				{
					result = (this._importRequestsCommand = new RelayCommand(delegate(object obj)
					{
						OpenFileDialog openFileDialog = new OpenFileDialog();
						openFileDialog.DefaultExt = <Module>.smethod_2<string>(979394986);
						openFileDialog.Filter = <Module>.smethod_4<string>(1937906964);
						bool? flag = openFileDialog.ShowDialog();
						if (flag.GetValueOrDefault() & flag != null)
						{
							using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
							{
								string input;
								while ((input = streamReader.ReadLine()) != null)
								{
									string from = null;
									Match match = Regex.Match(input, <Module>.smethod_3<string>(-1424349758));
									if (match.Success)
									{
										from = match.Groups[1].Value;
									}
									string subject = null;
									Match match2 = Regex.Match(input, <Module>.smethod_4<string>(799995908));
									if (match2.Success)
									{
										subject = match2.Groups[1].Value;
									}
									string body = null;
									Match match3 = Regex.Match(input, <Module>.smethod_5<string>(1921532914));
									if (match3.Success)
									{
										body = match3.Groups[1].Value;
									}
									bool checkDate = false;
									Match match4 = Regex.Match(input, <Module>.smethod_3<string>(-219757024));
									if (match4.Success)
									{
										checkDate = (match4.Groups[1].Value == <Module>.smethod_3<string>(663482485));
									}
									DateTime minValue = DateTime.MinValue;
									Match match5 = Regex.Match(input, <Module>.smethod_6<string>(1605523758));
									if (match5.Success)
									{
										DateTime.TryParse(match5.Groups[1].Value, out minValue);
									}
									DateTime minValue2 = DateTime.MinValue;
									Match match6 = Regex.Match(input, <Module>.smethod_2<string>(-1482251876));
									if (match6.Success)
									{
										DateTime.TryParse(match6.Groups[1].Value, out minValue2);
									}
									Request request = new Request
									{
										Sender = from,
										Subject = subject,
										Body = body,
										CheckDate = checkDate
									};
									if (minValue != DateTime.MinValue)
									{
										request.DateFrom = new DateTime?(minValue);
									}
									if (minValue2 != DateTime.MinValue)
									{
										request.DateTo = new DateTime?(minValue2);
									}
									if (!this.SearchSettings.Requests.Any((Request r) => r.Sender.EqualsIgnoreCase(from) && r.Subject.EqualsIgnoreCase(subject) && r.Body.EqualsIgnoreCase(body)))
									{
										this.SearchSettings.Requests.Add(request);
									}
								}
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00013F1C File Offset: 0x0001211C
		public RelayCommand ExportRequestsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._exportRequestsCommand) == null)
				{
					result = (this._exportRequestsCommand = new RelayCommand(delegate(object obj)
					{
						SaveFileDialog saveFileDialog = MainViewModel.<>c.smethod_3();
						MainViewModel.<>c.smethod_4(saveFileDialog, <Module>.smethod_6<string>(1897553056));
						MainViewModel.<>c.smethod_5(saveFileDialog, <Module>.smethod_6<string>(1906201981));
						MainViewModel.<>c.smethod_6(saveFileDialog, <Module>.smethod_2<string>(-367029413));
						bool? flag = MainViewModel.<>c.smethod_7(saveFileDialog);
						if (flag.GetValueOrDefault() & flag != null)
						{
							string string_ = MainViewModel.<>c.smethod_8(saveFileDialog);
							if (SearchSettings.Instance.Requests.Count > 0)
							{
								StreamWriter streamWriter = MainViewModel.<>c.smethod_9(string_, false);
								try
								{
									IEnumerator<Request> enumerator = SearchSettings.Instance.Requests.GetEnumerator();
									try
									{
										while (MainViewModel.<>c.smethod_13(enumerator))
										{
											Request request = enumerator.Current;
											if (request.Sender != null)
											{
												MainViewModel.<>c.smethod_11(streamWriter, MainViewModel.<>c.smethod_10(<Module>.smethod_4<string>(-1040017459), request.Sender, <Module>.smethod_6<string>(1158830886)));
											}
											if (request.Subject != null)
											{
												MainViewModel.<>c.smethod_11(streamWriter, MainViewModel.<>c.smethod_10(<Module>.smethod_2<string>(215061586), request.Subject, <Module>.smethod_2<string>(1879735358)));
											}
											if (request.Body != null)
											{
												MainViewModel.<>c.smethod_11(streamWriter, MainViewModel.<>c.smethod_10(<Module>.smethod_3<string>(-539182810), request.Body, <Module>.smethod_5<string>(487757498)));
											}
											if (request.CheckDate)
											{
												MainViewModel.<>c.smethod_11(streamWriter, <Module>.smethod_4<string>(1875398817));
											}
											if (request.DateFrom != null)
											{
												MainViewModel.<>c.smethod_11(streamWriter, MainViewModel.<>c.smethod_10(<Module>.smethod_5<string>(1970411365), request.DateFrom.Value.ToString(<Module>.smethod_5<string>(1590508633)), <Module>.smethod_5<string>(1210605901)));
											}
											if (request.DateTo != null)
											{
												MainViewModel.<>c.smethod_11(streamWriter, MainViewModel.<>c.smethod_10(<Module>.smethod_4<string>(-51776299), request.DateTo.Value.ToString(<Module>.smethod_2<string>(-1104146544)), <Module>.smethod_2<string>(-821260967)));
											}
											MainViewModel.<>c.smethod_11(streamWriter, MainViewModel.<>c.smethod_12());
										}
									}
									finally
									{
										if (enumerator != null)
										{
											MainViewModel.<>c.smethod_14(enumerator);
										}
									}
								}
								finally
								{
									if (streamWriter != null)
									{
										MainViewModel.<>c.smethod_14(streamWriter);
									}
								}
							}
						}
					}, (object obj) => this.SearchSettings.Requests.Any<Request>()));
				}
				return result;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00013F6C File Offset: 0x0001216C
		public RelayCommand ManageRequestGroupsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._manageRequestGroupsCommand) == null)
				{
					result = (this._manageRequestGroupsCommand = new RelayCommand(delegate(object obj)
					{
						SearchGroups frameworkElement_ = SingleOpenHelper.CreateControl<SearchGroups>();
						PopupWindow popupWindow = MainViewModel.<>c.smethod_15();
						MainViewModel.<>c.smethod_16(popupWindow, frameworkElement_);
						MainViewModel.<>c.smethod_17(popupWindow, popupWindow, false);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00013FB4 File Offset: 0x000121B4
		public RelayCommand ClearRequestsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._clearRequestsCommand) == null)
				{
					result = (this._clearRequestsCommand = new RelayCommand(delegate(object obj)
					{
						this.SearchSettings.Requests.Clear();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00013FE8 File Offset: 0x000121E8
		public RelayCommand OpenViewerCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openViewerCommand) == null)
				{
					result = (this._openViewerCommand = new RelayCommand(delegate(object obj)
					{
						ViewerController.Instance.Show(true);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00014030 File Offset: 0x00012230
		public RelayCommand OpenSchedulerCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openSchedulerCommand) == null)
				{
					result = (this._openSchedulerCommand = new RelayCommand(delegate(object obj)
					{
						SchedulerController.Instance.Show();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00014078 File Offset: 0x00012278
		public RelayCommand OpenToolsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openToolsCommand) == null)
				{
					result = (this._openToolsCommand = new RelayCommand(delegate(object obj)
					{
						ViewerController.Instance.ShowTools();
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000140C0 File Offset: 0x000122C0
		public RelayCommand OpenResultsFolderCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openResultsFolderCommand) == null)
				{
					result = (this._openResultsFolderCommand = new RelayCommand(delegate(object obj)
					{
						if (!MainViewModel.<>c.smethod_1(FileManager.ResultsPath))
						{
							DirectoryInfo fileSystemInfo_ = MainViewModel.<>c.smethod_18(FileManager.ResultsPath);
							MainViewModel.<>c.smethod_20(<Module>.smethod_2<string>(843412805), MainViewModel.<>c.smethod_19(fileSystemInfo_));
							return;
						}
						DirectoryInfo fileSystemInfo_2 = MainViewModel.<>c.smethod_18(<Module>.smethod_4<string>(732679442));
						MainViewModel.<>c.smethod_20(<Module>.smethod_5<string>(-1981610260), MainViewModel.<>c.smethod_19(fileSystemInfo_2));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00014108 File Offset: 0x00012308
		public RelayCommand SwitchLanguageCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._switchLanguageCommand) == null)
				{
					result = (this._switchLanguageCommand = new RelayCommand(delegate(object obj)
					{
						if (this.Language == <Module>.smethod_3<string>(-502627605))
						{
							App.Language = App.Languages.First((CultureInfo lang) => MainViewModel.<>c.smethod_2(MainViewModel.<>c.smethod_21(lang), <Module>.smethod_5<string>(1510633816)));
						}
						else
						{
							App.Language = App.Languages.First((CultureInfo lang) => MainViewModel.<>c.smethod_2(MainViewModel.<>c.smethod_21(lang), <Module>.smethod_4<string>(1128797621)));
						}
						Registry.SaveLanguage();
						base.OnPropertyChanged(<Module>.smethod_2<string>(1847095668));
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0001413C File Offset: 0x0001233C
		public RelayCommand OpenThroughEmailViewerCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._openThroughEmailViewerCommand) == null)
				{
					result = (this._openThroughEmailViewerCommand = new RelayCommand(delegate(object obj)
					{
						MailboxResult mailboxResult = obj as MailboxResult;
						if (mailboxResult == null)
						{
							return;
						}
						ViewerController.Instance.Show(false);
						ViewerController.Instance.OpenNewTab(mailboxResult);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00014184 File Offset: 0x00012384
		public RelayCommand ShowStatisticDetailsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._showStatisticDetailsCommand) == null)
				{
					result = (this._showStatisticDetailsCommand = new RelayCommand(delegate(object obj)
					{
						PagesManager.Instance.OpenCachedPage(MainViewModel.<>c.smethod_0(typeof(StatisticsDetailsPage).TypeHandle), FrameType.MainOverlay);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000141CC File Offset: 0x000123CC
		public RelayCommand GetCaptchaBalanceCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._getCaptchaBalanceCommand) == null)
				{
					result = (this._getCaptchaBalanceCommand = new RelayCommand(delegate(object obj)
					{
						MainViewModel.<<get_GetCaptchaBalanceCommand>b__176_0>d <<get_GetCaptchaBalanceCommand>b__176_0>d;
						<<get_GetCaptchaBalanceCommand>b__176_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_GetCaptchaBalanceCommand>b__176_0>d.<>4__this = this;
						<<get_GetCaptchaBalanceCommand>b__176_0>d.<>1__state = -1;
						<<get_GetCaptchaBalanceCommand>b__176_0>d.<>t__builder.Start<MainViewModel.<<get_GetCaptchaBalanceCommand>b__176_0>d>(ref <<get_GetCaptchaBalanceCommand>b__176_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00014200 File Offset: 0x00012400
		public RelayCommand OpenTelegramCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.openTelegramCommand) == null)
				{
					result = (this.openTelegramCommand = new RelayCommand(delegate(object obj)
					{
						try
						{
							MainViewModel.<>c.smethod_22(<Module>.smethod_4<string>(819841191));
						}
						catch
						{
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00014248 File Offset: 0x00012448
		public RelayCommand CloseApplicationCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._closeApplicationCommand) == null)
				{
					result = (this._closeApplicationCommand = new RelayCommand(delegate(object obj)
					{
						if (this.ThreadsManager.State == CheckerState.Running || this.ThreadsManager.State == CheckerState.Paused)
						{
							if (this.SendSimpleAskMessage(ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(1868075219))) != MessageBoxResult.Yes)
							{
								return;
							}
						}
						SettingsManager.SaveSettings();
						Environment.Exit(0);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001427C File Offset: 0x0001247C
		private void SendSimpleErrorMessage(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(-416025122)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_3<string>(1310043813),
				IconKey = <Module>.smethod_5<string>(-1297308186),
				StyleKey = <Module>.smethod_5<string>(375297047),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(-1867162284))
			});
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000142FC File Offset: 0x000124FC
		private void SendSimpleSuccessMessage(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_2<string>(1694768851)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_2<string>(-935524673),
				IconKey = <Module>.smethod_5<string>(1098145450),
				StyleKey = <Module>.smethod_4<string>(69043683),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_2<string>(862431965))
			});
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0001437C File Offset: 0x0001257C
		private MessageBoxResult SendSimpleAskMessage(string text)
		{
			return HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_4<string>(194671984)),
				Button = MessageBoxButton.YesNo,
				IconBrushKey = <Module>.smethod_6<string>(-761846756),
				IconKey = <Module>.smethod_2<string>(-402393209),
				StyleKey = <Module>.smethod_2<string>(-1218410250),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(-1867162284))
			});
		}

		// Token: 0x040000BD RID: 189
		private static MainViewModel _instance;

		// Token: 0x040000BE RID: 190
		private ProxyLoaderViewModel _proxyLoaderViewModel;

		// Token: 0x040000BF RID: 191
		private string _ignoreDomainFilterInput = string.Empty;

		// Token: 0x040000C0 RID: 192
		private string _rebruteDomainFilterInput = string.Empty;

		// Token: 0x040000C1 RID: 193
		private string _attachmentsFilterInput = string.Empty;

		// Token: 0x040000C2 RID: 194
		private string _addSearchRequestInput = string.Empty;

		// Token: 0x040000C3 RID: 195
		private string _folderInput = string.Empty;

		// Token: 0x040000C4 RID: 196
		private bool _isAnticaptchaLoading;

		// Token: 0x040000C5 RID: 197
		private RelayCommand _initializeCommand;

		// Token: 0x040000C6 RID: 198
		private RelayCommand _startCommand;

		// Token: 0x040000C7 RID: 199
		private RelayCommand _pauseCommand;

		// Token: 0x040000C8 RID: 200
		private RelayCommand _resumeCommand;

		// Token: 0x040000C9 RID: 201
		private RelayCommand _stopCommand;

		// Token: 0x040000CA RID: 202
		private RelayCommand _openProxyLoaderPageCommand;

		// Token: 0x040000CB RID: 203
		private RelayCommand _openWebSettingsCommand;

		// Token: 0x040000CC RID: 204
		private RelayCommand _selectSettingsPageCommand;

		// Token: 0x040000CD RID: 205
		private RelayCommand _changeThemeCommand;

		// Token: 0x040000CE RID: 206
		private RelayCommand _openDomainsFilterCommand;

		// Token: 0x040000CF RID: 207
		private RelayCommand _openRebruteDomainsFilterCommand;

		// Token: 0x040000D0 RID: 208
		private RelayCommand _openAttachmentsFilterCommand;

		// Token: 0x040000D1 RID: 209
		private RelayCommand _openRequestsManagerCommand;

		// Token: 0x040000D2 RID: 210
		private RelayCommand _addIgnoreDomainFilterCommand;

		// Token: 0x040000D3 RID: 211
		private RelayCommand _addRebruteDomainFilterCommand;

		// Token: 0x040000D4 RID: 212
		private RelayCommand _addAttachmentsFilterCommand;

		// Token: 0x040000D5 RID: 213
		private RelayCommand _addFolderCommand;

		// Token: 0x040000D6 RID: 214
		private RelayCommand _removeIgnoreDomainFilterCommand;

		// Token: 0x040000D7 RID: 215
		private RelayCommand _removeRebruteDomainFilterCommand;

		// Token: 0x040000D8 RID: 216
		private RelayCommand _removeAttachmentsFilterCommand;

		// Token: 0x040000D9 RID: 217
		private RelayCommand _removeFolderCommand;

		// Token: 0x040000DA RID: 218
		private RelayCommand _closeOverlayCommand;

		// Token: 0x040000DB RID: 219
		private RelayCommand _addSearchRequestCommand;

		// Token: 0x040000DC RID: 220
		private RelayCommand _removeSearchRequestCommand;

		// Token: 0x040000DD RID: 221
		private RelayCommand _changeSearchRequestDateCommand;

		// Token: 0x040000DE RID: 222
		private RelayCommand _loadBaseCommand;

		// Token: 0x040000DF RID: 223
		private RelayCommand _dropBaseCommand;

		// Token: 0x040000E0 RID: 224
		private RelayCommand searchCommand;

		// Token: 0x040000E1 RID: 225
		private RelayCommand _resetSearchCommand;

		// Token: 0x040000E2 RID: 226
		private RelayCommand _editConfigurationCommand;

		// Token: 0x040000E3 RID: 227
		private RelayCommand _setConfigurationProviderCommand;

		// Token: 0x040000E4 RID: 228
		private RelayCommand _importRequestsCommand;

		// Token: 0x040000E5 RID: 229
		private RelayCommand _exportRequestsCommand;

		// Token: 0x040000E6 RID: 230
		private RelayCommand _manageRequestGroupsCommand;

		// Token: 0x040000E7 RID: 231
		private RelayCommand _clearRequestsCommand;

		// Token: 0x040000E8 RID: 232
		private RelayCommand _openViewerCommand;

		// Token: 0x040000E9 RID: 233
		private RelayCommand _openSchedulerCommand;

		// Token: 0x040000EA RID: 234
		private RelayCommand _openToolsCommand;

		// Token: 0x040000EB RID: 235
		private RelayCommand _openResultsFolderCommand;

		// Token: 0x040000EC RID: 236
		private RelayCommand _switchLanguageCommand;

		// Token: 0x040000ED RID: 237
		private RelayCommand _openThroughEmailViewerCommand;

		// Token: 0x040000EE RID: 238
		private RelayCommand _showStatisticDetailsCommand;

		// Token: 0x040000EF RID: 239
		private RelayCommand _getCaptchaBalanceCommand;

		// Token: 0x040000F0 RID: 240
		private RelayCommand openTelegramCommand;

		// Token: 0x040000F1 RID: 241
		private RelayCommand _closeApplicationCommand;
	}
}
