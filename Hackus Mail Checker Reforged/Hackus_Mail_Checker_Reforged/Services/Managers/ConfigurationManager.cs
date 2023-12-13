using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x0200006C RID: 108
	internal class ConfigurationManager : BindableObject
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x00006C91 File Offset: 0x00004E91
		private ConfigurationManager()
		{
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00008D0B File Offset: 0x00006F0B
		public static ConfigurationManager Instance
		{
			get
			{
				ConfigurationManager result;
				if ((result = ConfigurationManager._instance) == null)
				{
					result = (ConfigurationManager._instance = new ConfigurationManager());
				}
				return result;
			}
		}

		// Token: 0x04000219 RID: 537
		private static ConfigurationManager _instance;

		// Token: 0x0400021A RID: 538
		public SqlConfiguration Configuration;
	}
}
