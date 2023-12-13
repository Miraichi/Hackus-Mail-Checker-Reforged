using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using Microsoft.Win32;

namespace Hackus_Mail_Checker_Reforged.UI.ViewModels
{
	// Token: 0x02000040 RID: 64
	internal class ProxyLoaderViewModel : BindableObject
	{
		// Token: 0x060001FC RID: 508 RVA: 0x00007322 File Offset: 0x00005522
		public ProxyLoaderViewModel()
		{
			this.UpdateLocalSettings();
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00007330 File Offset: 0x00005530
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00007338 File Offset: 0x00005538
		public bool LoadFromFile
		{
			get
			{
				return this._loadFromFile;
			}
			set
			{
				this._loadFromFile = value;
				RelayCommand.Validate();
				base.OnPropertyChanged(<Module>.smethod_5<string>(-1638664589));
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00007356 File Offset: 0x00005556
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000735E File Offset: 0x0000555E
		public bool LoadFromWeb
		{
			get
			{
				return this._loadFromWeb;
			}
			set
			{
				this._loadFromWeb = value;
				RelayCommand.Validate();
				base.OnPropertyChanged(<Module>.smethod_6<string>(1211437145));
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000737C File Offset: 0x0000557C
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00007384 File Offset: 0x00005584
		public string LocalFilePath
		{
			get
			{
				return this._localFilePath;
			}
			set
			{
				this._localFilePath = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-318069865));
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000739D File Offset: 0x0000559D
		// (set) Token: 0x06000204 RID: 516 RVA: 0x000073A5 File Offset: 0x000055A5
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this._fileName = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(2099633534));
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000073BE File Offset: 0x000055BE
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000073C6 File Offset: 0x000055C6
		public string WebTextSources
		{
			get
			{
				return this._webTextSources;
			}
			set
			{
				this._webTextSources = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(153570918));
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000207 RID: 519 RVA: 0x000073DF File Offset: 0x000055DF
		// (set) Token: 0x06000208 RID: 520 RVA: 0x000073E7 File Offset: 0x000055E7
		public ProxyType LocalProxyType
		{
			get
			{
				return this._localProxyType;
			}
			set
			{
				this._localProxyType = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1797225526));
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00007400 File Offset: 0x00005600
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00007408 File Offset: 0x00005608
		public bool UseAutoUpdate
		{
			get
			{
				return this._useAutoUpdate;
			}
			set
			{
				this._useAutoUpdate = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-276385905));
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007421 File Offset: 0x00005621
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00007429 File Offset: 0x00005629
		public int UpdateDelay
		{
			get
			{
				return this._UpdateDelay;
			}
			set
			{
				this._UpdateDelay = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-721348992));
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007442 File Offset: 0x00005642
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000744A File Offset: 0x0000564A
		public bool UseAuthentication
		{
			get
			{
				return this._useAuthentication;
			}
			set
			{
				this._useAuthentication = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(176998475));
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00007463 File Offset: 0x00005663
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000746B File Offset: 0x0000566B
		public string Login
		{
			get
			{
				return this._login;
			}
			set
			{
				this._login = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(329300501));
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00007484 File Offset: 0x00005684
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000748C File Offset: 0x0000568C
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00015DEC File Offset: 0x00013FEC
		public RelayCommand EnableLoadingFromWebCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._enableLoadingFromWebCommand) == null)
				{
					result = (this._enableLoadingFromWebCommand = new RelayCommand(delegate(object obj)
					{
						this.LoadFromWeb = true;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00015E20 File Offset: 0x00014020
		public RelayCommand DisableLoadingFromWebCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._disableLoadingFromWebCommand) == null)
				{
					result = (this._disableLoadingFromWebCommand = new RelayCommand(delegate(object obj)
					{
						this.LoadFromWeb = false;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00015E54 File Offset: 0x00014054
		public RelayCommand EnableLoadingFromFileCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._enableLoadingFromFileCommand) == null)
				{
					result = (this._enableLoadingFromFileCommand = new RelayCommand(delegate(object obj)
					{
						this.LoadFromFile = true;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00015E88 File Offset: 0x00014088
		public RelayCommand DisableLoadingFromFileCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._disableLoadingFromFileCommand) == null)
				{
					result = (this._disableLoadingFromFileCommand = new RelayCommand(delegate(object obj)
					{
						this.LoadFromFile = false;
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00015EBC File Offset: 0x000140BC
		public RelayCommand SelectFilePathCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._selectFilePathCommand) == null)
				{
					result = (this._selectFilePathCommand = new RelayCommand(delegate(object obj)
					{
						OpenFileDialog openFileDialog = new OpenFileDialog();
						openFileDialog.Filter = <Module>.smethod_3<string>(182416367);
						openFileDialog.RestoreDirectory = true;
						bool? flag = openFileDialog.ShowDialog();
						if (flag.GetValueOrDefault() & flag != null)
						{
							FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
							if (!fileInfo.Exists)
							{
								this.SendSimpleErrorMessage(<Module>.smethod_5<string>(561671620));
								return;
							}
							this.LocalFilePath = fileInfo.FullName;
							this.FileName = fileInfo.Name;
							if (!this.LoadFromFile)
							{
								this.LoadFromFile = true;
							}
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00015EF0 File Offset: 0x000140F0
		public RelayCommand LoadProxyCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._loadProxyCommand) == null)
				{
					result = (this._loadProxyCommand = new RelayCommand(delegate(object obj)
					{
						ProxyLoaderViewModel.<<get_LoadProxyCommand>b__62_0>d <<get_LoadProxyCommand>b__62_0>d;
						<<get_LoadProxyCommand>b__62_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_LoadProxyCommand>b__62_0>d.<>4__this = this;
						<<get_LoadProxyCommand>b__62_0>d.<>1__state = -1;
						<<get_LoadProxyCommand>b__62_0>d.<>t__builder.Start<ProxyLoaderViewModel.<<get_LoadProxyCommand>b__62_0>d>(ref <<get_LoadProxyCommand>b__62_0>d);
					}, (object obj) => this.LoadFromFile || (this.LoadFromWeb && !string.IsNullOrEmpty(this.WebTextSources))));
				}
				return result;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00015F30 File Offset: 0x00014130
		public void UpdateGlobalSettings()
		{
			ProxySettings.Instance.UseWebSources = this.LoadFromWeb;
			ProxySettings.Instance.WebLinks = this.WebTextSources;
			ProxySettings.Instance.UseAutoUpdate = this.UseAutoUpdate;
			ProxySettings.Instance.UpdateDelay = this.UpdateDelay;
			ProxySettings.Instance.UseAuthentication = this.UseAuthentication;
			ProxySettings.Instance.Login = this.Login;
			ProxySettings.Instance.Password = this.Password;
			ProxySettings.Instance.ProxyType = this.LocalProxyType;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00015FC0 File Offset: 0x000141C0
		public void UpdateLocalSettings()
		{
			this.LoadFromWeb = ProxySettings.Instance.UseWebSources;
			this.WebTextSources = ProxySettings.Instance.WebLinks;
			this.UseAutoUpdate = ProxySettings.Instance.UseAutoUpdate;
			this.UpdateDelay = ProxySettings.Instance.UpdateDelay;
			this.UseAuthentication = ProxySettings.Instance.UseAuthentication;
			this.Login = ProxySettings.Instance.Login;
			this.Password = ProxySettings.Instance.Password;
			this.LocalProxyType = ProxySettings.Instance.ProxyType;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00016050 File Offset: 0x00014250
		private void SendSimpleErrorMessage(string text)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = text,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_2<string>(-1052487694)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_2<string>(196017635),
				IconKey = <Module>.smethod_6<string>(1292737040),
				StyleKey = <Module>.smethod_6<string>(1294466825),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_4<string>(-1596646186))
			});
		}

		// Token: 0x0400012F RID: 303
		private bool _loadFromFile;

		// Token: 0x04000130 RID: 304
		private bool _loadFromWeb;

		// Token: 0x04000131 RID: 305
		private string _localFilePath;

		// Token: 0x04000132 RID: 306
		private string _fileName;

		// Token: 0x04000133 RID: 307
		private string _webTextSources;

		// Token: 0x04000134 RID: 308
		private ProxyType _localProxyType;

		// Token: 0x04000135 RID: 309
		private bool _useAutoUpdate;

		// Token: 0x04000136 RID: 310
		private int _UpdateDelay;

		// Token: 0x04000137 RID: 311
		private bool _useAuthentication;

		// Token: 0x04000138 RID: 312
		private string _login;

		// Token: 0x04000139 RID: 313
		private string _password;

		// Token: 0x0400013A RID: 314
		private RelayCommand _enableLoadingFromWebCommand;

		// Token: 0x0400013B RID: 315
		private RelayCommand _disableLoadingFromWebCommand;

		// Token: 0x0400013C RID: 316
		private RelayCommand _enableLoadingFromFileCommand;

		// Token: 0x0400013D RID: 317
		private RelayCommand _disableLoadingFromFileCommand;

		// Token: 0x0400013E RID: 318
		private RelayCommand _selectFilePathCommand;

		// Token: 0x0400013F RID: 319
		private RelayCommand _loadProxyCommand;
	}
}
