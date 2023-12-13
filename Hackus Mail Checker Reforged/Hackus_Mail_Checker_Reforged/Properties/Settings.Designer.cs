using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Hackus_Mail_Checker_Reforged.Properties
{
	// Token: 0x02000027 RID: 39
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00006981 File Offset: 0x00004B81
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000074 RID: 116
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
