using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HandyControl.Themes;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000024 RID: 36
	public partial class App : Application
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000067C3 File Offset: 0x000049C3
		public static List<CultureInfo> Languages
		{
			get
			{
				return App._languages;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000067CA File Offset: 0x000049CA
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00011EA4 File Offset: 0x000100A4
		public static CultureInfo Language
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(<Module>.smethod_4<string>(-1519713082));
				}
				if (value == Thread.CurrentThread.CurrentCulture)
				{
					return;
				}
				Thread.CurrentThread.CurrentUICulture = value;
				ResourceDictionary resourceDictionary = new ResourceDictionary();
				if (!(value.Name == <Module>.smethod_2<string>(152506312)))
				{
					resourceDictionary.Source = new Uri(<Module>.smethod_3<string>(-67753887), UriKind.Relative);
				}
				else
				{
					resourceDictionary.Source = new Uri(<Module>.smethod_6<string>(242957937), UriKind.Relative);
				}
				ResourceDictionary resourceDictionary2 = (from d in Application.Current.Resources.MergedDictionaries
				where App.<>c.smethod_1(App.<>c.smethod_0(d), null) && App.<>c.smethod_3(App.<>c.smethod_2(App.<>c.smethod_0(d)), <Module>.smethod_3<string>(1870002658))
				select d).First<ResourceDictionary>();
				if (resourceDictionary2 != null)
				{
					int index = Application.Current.Resources.MergedDictionaries.IndexOf(resourceDictionary2);
					Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary2);
					Application.Current.Resources.MergedDictionaries.Insert(index, resourceDictionary);
					return;
				}
				Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000067D6 File Offset: 0x000049D6
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00011FBC File Offset: 0x000101BC
		public static ApplicationTheme Theme
		{
			get
			{
				return App._theme;
			}
			set
			{
				if (value == App._theme)
				{
					return;
				}
				ResourceDictionary resourceDictionary = new ResourceDictionary();
				if (value != ApplicationTheme.Light)
				{
					if (value == ApplicationTheme.Dark)
					{
						resourceDictionary.Source = new Uri(<Module>.smethod_5<string>(792951474), UriKind.Relative);
						ThemeManager.Current.ApplicationTheme = new ApplicationTheme?(ApplicationTheme.Dark);
					}
				}
				else
				{
					resourceDictionary.Source = new Uri(<Module>.smethod_5<string>(-119848295), UriKind.Relative);
					ThemeManager.Current.ApplicationTheme = new ApplicationTheme?(ApplicationTheme.Light);
				}
				ResourceDictionary resourceDictionary2 = (from d in Application.Current.Resources.MergedDictionaries
				where App.<>c.smethod_1(App.<>c.smethod_0(d), null) && App.<>c.smethod_3(App.<>c.smethod_2(App.<>c.smethod_0(d)), <Module>.smethod_3<string>(1027173232))
				select d).First<ResourceDictionary>();
				if (resourceDictionary2 == null)
				{
					Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
				}
				else
				{
					int index = Application.Current.Resources.MergedDictionaries.IndexOf(resourceDictionary2);
					Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary2);
					Application.Current.Resources.MergedDictionaries.Insert(index, resourceDictionary);
				}
				App._theme = value;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000120CC File Offset: 0x000102CC
		public App()
		{
			App._languages.Clear();
			App._languages.Add(new CultureInfo(<Module>.smethod_5<string>(1510633816)));
			App._languages.Add(new CultureInfo(<Module>.smethod_4<string>(1128797621)));
			ThemeManager.Current.ApplicationTheme = new ApplicationTheme?(ApplicationTheme.Dark);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000067DD File Offset: 0x000049DD
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.SetupExceptionHandling();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000067EC File Offset: 0x000049EC
		private void SetupExceptionHandling()
		{
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
			{
				this.LogUnhandledException((Exception)e.ExceptionObject, <Module>.smethod_2<string>(-1686262334));
			};
			base.DispatcherUnhandledException += delegate(object sender, DispatcherUnhandledExceptionEventArgs e)
			{
				this.LogUnhandledException(e.Exception, <Module>.smethod_5<string>(1166893511));
				if (e.Exception.GetType() != typeof(OutOfMemoryException))
				{
					e.Handled = true;
				}
			};
			TaskScheduler.UnobservedTaskException += delegate(object sender, UnobservedTaskExceptionEventArgs e)
			{
				this.LogUnhandledException(e.Exception, <Module>.smethod_4<string>(1487673262));
				e.SetObserved();
			};
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0001212C File Offset: 0x0001032C
		private void LogUnhandledException(Exception exception, string source)
		{
			try
			{
				string value = <Module>.smethod_4<string>(1027210915) + source + <Module>.smethod_2<string>(769961107);
				using (StreamWriter streamWriter = new StreamWriter(<Module>.smethod_5<string>(-536708088), true))
				{
					streamWriter.WriteLine(value);
					streamWriter.WriteLine(<Module>.smethod_6<string>(1577847198) + exception.Message);
					streamWriter.WriteLine(<Module>.smethod_6<string>(244687722) + exception.StackTrace);
					streamWriter.WriteLine(<Module>.smethod_3<string>(813558183) + exception.Source);
					streamWriter.WriteLine(<Module>.smethod_2<string>(1251408930));
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400006C RID: 108
		private static List<CultureInfo> _languages = new List<CultureInfo>();

		// Token: 0x0400006D RID: 109
		private static ApplicationTheme _theme = ApplicationTheme.Dark;
	}
}
