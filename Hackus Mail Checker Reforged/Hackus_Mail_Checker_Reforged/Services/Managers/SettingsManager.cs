using System;
using System.IO;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x02000076 RID: 118
	public static class SettingsManager
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x00019770 File Offset: 0x00017970
		public static void LoadSettings()
		{
			if (!File.Exists(FileManager.SettingsPath))
			{
				return;
			}
			string text = null;
			using (StreamReader streamReader = new StreamReader(FileManager.SettingsPath))
			{
				text = streamReader.ReadToEnd();
			}
			if (text != null)
			{
				Settings settings = JsonConvert.DeserializeObject<Settings>(text, new JsonSerializerSettings
				{
					ObjectCreationHandling = 2
				});
				CheckerSettings.Instance = settings.CheckerSettings;
				SearchSettings.Instance = settings.SearchSettings;
				ProxySettings.Instance = settings.ProxySettings;
				ViewerSettings.Instance = settings.ViewerSettings;
				WebSettings.Instance = settings.WebSettings;
				SchedulerSettings.Instance = settings.SchedulerSettings;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00019810 File Offset: 0x00017A10
		public static void SaveSettings()
		{
			string value = JsonConvert.SerializeObject(new Settings(1), 1);
			using (StreamWriter streamWriter = new StreamWriter(FileManager.SettingsPath, false))
			{
				streamWriter.Write(value);
			}
		}
	}
}
