using System;
using System.Text.RegularExpressions;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000015 RID: 21
	internal static class StringExtension
	{
		// Token: 0x06000065 RID: 101 RVA: 0x000107B0 File Offset: 0x0000E9B0
		public static bool ContainsOne(this string original, params string[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				if (original.Contains(list[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000064A7 File Offset: 0x000046A7
		public static bool ContainsIgnoreCase(this string original, string search)
		{
			return search != null && original.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000107DC File Offset: 0x0000E9DC
		public static bool ContainsOneIgnoreCase(this string original, params string[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				if (original.IndexOf(list[i], StringComparison.OrdinalIgnoreCase) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000064BC File Offset: 0x000046BC
		public static bool EqualsIgnoreCase(this string original, string second)
		{
			return string.Equals(original, second, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0001080C File Offset: 0x0000EA0C
		public static string ToPlainText(this string html)
		{
			return Regex.Replace(Regex.Replace(Regex.Unescape(Regex.Replace(Regex.Replace(Regex.Replace(html, <Module>.smethod_4<string>(557743937), string.Empty, RegexOptions.Compiled), <Module>.smethod_2<string>(-97739575), string.Empty, RegexOptions.Compiled), <Module>.smethod_6<string>(-808550951), string.Empty, RegexOptions.Compiled)), <Module>.smethod_6<string>(-2141710427), <Module>.smethod_2<string>(1849819774)), <Module>.smethod_4<string>(2136272057), <Module>.smethod_2<string>(-364305307)).Replace(<Module>.smethod_3<string>(-1106851411), string.Empty);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000108A4 File Offset: 0x0000EAA4
		public static string Substring(this string self, string left, string right, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal, string fallback = null)
		{
			if (string.IsNullOrEmpty(self) || string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right) || startIndex < 0 || startIndex >= self.Length)
			{
				return fallback;
			}
			int num = self.IndexOf(left, startIndex, comparison);
			if (num == -1)
			{
				return fallback;
			}
			int num2 = num + left.Length;
			int num3 = self.IndexOf(right, num2, comparison);
			if (num3 != -1)
			{
				return self.Substring(num2, num3 - num2);
			}
			return fallback;
		}
	}
}
