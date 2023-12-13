using System;

namespace Hackus_Mail_Checker_Reforged.Helpers
{
	// Token: 0x02000157 RID: 343
	internal static class DateHelpers
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x0003B034 File Offset: 0x00039234
		public static DateTime UnixTimeStampToDate(long unixTimeStamp)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			result = result.AddSeconds((double)unixTimeStamp).ToLocalTime();
			return result;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0003B068 File Offset: 0x00039268
		public static DateTime UnixTimeStampToDateInMilliseconds(long unixTimeStamp)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			result = result.AddMilliseconds((double)unixTimeStamp).ToLocalTime();
			return result;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0003B09C File Offset: 0x0003929C
		public static double DateToUnixTimeStamp(DateTime date)
		{
			return (TimeZoneInfo.ConvertTimeToUtc(date) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}
	}
}
