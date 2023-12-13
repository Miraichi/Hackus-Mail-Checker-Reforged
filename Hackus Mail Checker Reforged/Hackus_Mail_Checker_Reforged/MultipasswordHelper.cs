using System;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000013 RID: 19
	public static class MultipasswordHelper
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000106C4 File Offset: 0x0000E8C4
		public static bool IsMultipassword(string login)
		{
			object locker = MultipasswordHelper._locker;
			bool result;
			lock (locker)
			{
				result = (CheckerSettings.Instance.ProcessMultipassword && MailManager.Instance.SavedLogins.Contains(login));
			}
			return result;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00010720 File Offset: 0x0000E920
		public static bool AddLogin(string login)
		{
			object locker = MultipasswordHelper._locker;
			bool result;
			lock (locker)
			{
				result = MailManager.Instance.SavedLogins.Add(login);
			}
			return result;
		}

		// Token: 0x0400003E RID: 62
		private static object _locker = new object();
	}
}
