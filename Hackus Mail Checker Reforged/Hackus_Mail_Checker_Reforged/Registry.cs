using System;
using System.Globalization;
using System.Linq;
using HandyControl.Tools;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000020 RID: 32
	internal static class Registry
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00011AB0 File Offset: 0x0000FCB0
		public static ValueTuple<string, string> GetSavedCredentials()
		{
			string value = RegistryHelper.GetValue<string>(<Module>.smethod_4<string>(-188245252), <Module>.smethod_6<string>(1875065851), null);
			string value2 = RegistryHelper.GetValue<string>(<Module>.smethod_6<string>(2024540070), <Module>.smethod_2<string>(-546547708), null);
			return new ValueTuple<string, string>(value, value2);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000066C1 File Offset: 0x000048C1
		public static void SaveCredentials(string username, string password)
		{
			RegistryHelper.AddOrUpdateKey<string>(<Module>.smethod_2<string>(-1795053037), <Module>.smethod_5<string>(787785413), username, null);
			RegistryHelper.AddOrUpdateKey<string>(<Module>.smethod_5<string>(-2024528016), <Module>.smethod_5<string>(787785413), password, null);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00011AF8 File Offset: 0x0000FCF8
		public static void SetLanguage()
		{
			if (RegistryHelper.GetValue<string>(<Module>.smethod_6<string>(1431832549), <Module>.smethod_4<string>(1477444617), null) == <Module>.smethod_4<string>(1128797621))
			{
				App.Language = App.Languages.FirstOrDefault((CultureInfo language) => Registry.<>c.smethod_1(Registry.<>c.smethod_0(language), <Module>.smethod_3<string>(11138840)));
				return;
			}
			App.Language = App.Languages.FirstOrDefault((CultureInfo language) => Registry.<>c.smethod_1(Registry.<>c.smethod_0(language), <Module>.smethod_2<string>(1401011641)));
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00011B90 File Offset: 0x0000FD90
		public static void SaveLanguage()
		{
			if (App.Language.Name == <Module>.smethod_2<string>(152506312))
			{
				RegistryHelper.AddOrUpdateKey<string>(<Module>.smethod_4<string>(-1413318057), <Module>.smethod_6<string>(1875065851), <Module>.smethod_2<string>(152506312), null);
				return;
			}
			RegistryHelper.AddOrUpdateKey<string>(<Module>.smethod_3<string>(292081982), <Module>.smethod_6<string>(1875065851), <Module>.smethod_5<string>(1510633816), null);
		}
	}
}
