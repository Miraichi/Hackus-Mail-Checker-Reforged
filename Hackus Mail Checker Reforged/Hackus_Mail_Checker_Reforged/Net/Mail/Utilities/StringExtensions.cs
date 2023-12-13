using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Utilities
{
	// Token: 0x0200010B RID: 267
	public static class StringExtensions
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x0003208C File Offset: 0x0003028C
		public static string[] SubstringsOrEmpty(this string self, string left, string right, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal, int limit = 0)
		{
			if (string.IsNullOrEmpty(self))
			{
				return new string[0];
			}
			if (string.IsNullOrEmpty(left))
			{
				throw new ArgumentNullException(<Module>.smethod_2<string>(-2129473509));
			}
			if (string.IsNullOrEmpty(right))
			{
				throw new ArgumentNullException(<Module>.smethod_2<string>(-1297136623));
			}
			if (startIndex >= 0 && startIndex < self.Length)
			{
				int startIndex2 = startIndex;
				int num = limit;
				List<string> list = new List<string>();
				for (;;)
				{
					if (limit > 0)
					{
						num--;
						if (num < 0)
						{
							break;
						}
					}
					int num2 = self.IndexOf(left, startIndex2, comparison);
					if (num2 == -1)
					{
						break;
					}
					int num3 = num2 + left.Length;
					int num4 = self.IndexOf(right, num3, comparison);
					if (num4 == -1)
					{
						break;
					}
					int length = num4 - num3;
					list.Add(self.Substring(num3, length));
					startIndex2 = num4 + right.Length;
				}
				return list.ToArray();
			}
			throw new ArgumentOutOfRangeException(<Module>.smethod_5<string>(-1730857173));
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0003216C File Offset: 0x0003036C
		public static string[] Substrings(this string self, string left, string right, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal, int limit = 0, string[] fallback = null)
		{
			string[] array = self.SubstringsOrEmpty(left, right, startIndex, comparison, limit);
			if (array.Length == 0)
			{
				return fallback;
			}
			return array;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00032190 File Offset: 0x00030390
		public static string[] SubstringsEx(this string self, string left, string right, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal, int limit = 0)
		{
			string[] array = self.SubstringsOrEmpty(left, right, startIndex, comparison, limit);
			if (array.Length == 0)
			{
				throw new SubstringException(string.Concat(new string[]
				{
					<Module>.smethod_2<string>(1616042478),
					left,
					<Module>.smethod_4<string>(-661208798),
					right,
					<Module>.smethod_4<string>(-1450472858)
				}));
			}
			return array;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000321F0 File Offset: 0x000303F0
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

		// Token: 0x06000828 RID: 2088 RVA: 0x0000AEF3 File Offset: 0x000090F3
		public static string SubstringOrEmpty(this string self, string left, string right, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal)
		{
			return self.Substring(left, right, startIndex, comparison, string.Empty);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0003225C File Offset: 0x0003045C
		public static string SubstringEx(this string self, string left, string right, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal)
		{
			string text = self.Substring(left, right, startIndex, comparison, null);
			if (text == null)
			{
				throw new SubstringException(string.Concat(new string[]
				{
					<Module>.smethod_3<string>(-999311643),
					left,
					<Module>.smethod_2<string>(1066591169),
					right,
					<Module>.smethod_4<string>(-1450472858)
				}));
			}
			return text;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000322BC File Offset: 0x000304BC
		public static string SubstringLast(this string self, string right, string left, int startIndex = -1, StringComparison comparison = StringComparison.Ordinal, string notFoundValue = null)
		{
			if (string.IsNullOrEmpty(self) || string.IsNullOrEmpty(right) || string.IsNullOrEmpty(left) || startIndex < -1 || startIndex >= self.Length)
			{
				return notFoundValue;
			}
			if (startIndex == -1)
			{
				startIndex = self.Length - 1;
			}
			int num = self.LastIndexOf(right, startIndex, comparison);
			if (num != -1 && num != 0)
			{
				int num2 = self.LastIndexOf(left, num - 1, comparison);
				if (num2 != -1)
				{
					if (num - num2 != 1)
					{
						int num3 = num2 + left.Length;
						return self.Substring(num3, num - num3);
					}
				}
				return notFoundValue;
			}
			return notFoundValue;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000AF05 File Offset: 0x00009105
		public static string SubstringLastOrEmpty(this string self, string right, string left, int startIndex = -1, StringComparison comparison = StringComparison.Ordinal)
		{
			return self.SubstringLast(right, left, startIndex, comparison, string.Empty);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00032344 File Offset: 0x00030544
		public static string SubstringLastEx(this string self, string right, string left, int startIndex = -1, StringComparison comparison = StringComparison.Ordinal)
		{
			string text = self.SubstringLast(right, left, startIndex, comparison, null);
			if (text == null)
			{
				throw new SubstringException(string.Concat(new string[]
				{
					<Module>.smethod_5<string>(2147250330),
					right,
					<Module>.smethod_5<string>(1577396232),
					left,
					<Module>.smethod_3<string>(-718368501)
				}));
			}
			return text;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0000AF17 File Offset: 0x00009117
		public static bool ContainsInsensitive(this string self, string value)
		{
			return self.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000323A4 File Offset: 0x000305A4
		public static string QuoteString(this string value)
		{
			return <Module>.smethod_2<string>(1588727045) + value.Replace(<Module>.smethod_3<string>(-1591838543), <Module>.smethod_5<string>(-2043602992)).Replace(<Module>.smethod_2<string>(1020182203), <Module>.smethod_5<string>(1021050912)).Replace(<Module>.smethod_5<string>(-1278631467), <Module>.smethod_6<string>(-744853273)).Replace(<Module>.smethod_2<string>(1588727045), <Module>.smethod_4<string>(-939042760)) + <Module>.smethod_3<string>(1298413043);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0000AF27 File Offset: 0x00009127
		public static bool IsWhiteSpace(this char chr)
		{
			if (chr != ' ')
			{
				if (chr != '\t')
				{
					if (chr != '\n')
					{
						return chr == '\r';
					}
				}
			}
			return true;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0000AF41 File Offset: 0x00009141
		public static bool IsAscii(this string s)
		{
			return s.All((char c) => c < '\u007f');
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0000AF68 File Offset: 0x00009168
		public static bool IsBrokenEncoding(this string s)
		{
			return s.Contains('�') || Regex.IsMatch(s, <Module>.smethod_3<string>(205281091));
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0000AF89 File Offset: 0x00009189
		public static bool IsContainsByeBye(this string s)
		{
			return StringExtensions.ExcSearch.Contains(s);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00032430 File Offset: 0x00030630
		public static string NormalizeDate(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			value = StringExtensions.rxTimeZoneName.Replace(value, string.Empty);
			value = StringExtensions.rxTimeZoneColon.Replace(value, (Match match) => StringExtensions.<>c.smethod_4(<Module>.smethod_6<string>(820097393), StringExtensions.<>c.smethod_2(StringExtensions.<>c.smethod_1(StringExtensions.<>c.smethod_0(match), 1)), StringExtensions.<>c.smethod_3(StringExtensions.<>c.smethod_2(StringExtensions.<>c.smethod_1(StringExtensions.<>c.smethod_0(match), 2)), 2, '0'), StringExtensions.<>c.smethod_2(StringExtensions.<>c.smethod_1(StringExtensions.<>c.smethod_0(match), 3))));
			value = StringExtensions.rxNegativeHours.Replace(value, string.Empty);
			Match match2 = StringExtensions.rxTimeZoneMinutes.Match(value);
			try
			{
				if (match2.Captures.Count > 0 && int.Parse(match2.Groups[2].Value) > 60)
				{
					value = value.Substring(0, match2.Index) + match2.Groups[1].Value + <Module>.smethod_4<string>(-579467375);
				}
			}
			catch (FormatException)
			{
			}
			return value;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00032510 File Offset: 0x00030710
		public static DateTime? ToNullDate(this string input)
		{
			input = StringExtensions.NormalizeDate(input);
			DateTime value;
			if (!DateTime.TryParse(input, StringExtensions._enUsCulture, DateTimeStyles.None, out value))
			{
				return null;
			}
			return new DateTime?(value);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0000AF96 File Offset: 0x00009196
		public static string GetRFC2060Date(this DateTime date)
		{
			return date.ToString(<Module>.smethod_2<string>(916988458), StringExtensions._enUsCulture);
		}

		// Token: 0x04000411 RID: 1041
		private static readonly Regex rxTimeZoneName = new Regex(<Module>.smethod_4<string>(999060745), RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000412 RID: 1042
		private static readonly Regex rxTimeZoneColon = new Regex(<Module>.smethod_4<string>(-226012060), RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x04000413 RID: 1043
		private static readonly Regex rxTimeZoneMinutes = new Regex(<Module>.smethod_6<string>(-612780879), RegexOptions.Compiled);

		// Token: 0x04000414 RID: 1044
		private static readonly Regex rxNegativeHours = new Regex(<Module>.smethod_6<string>(-174736932), RegexOptions.Compiled);

		// Token: 0x04000415 RID: 1045
		private static readonly CultureInfo _enUsCulture = CultureInfo.GetCultureInfo(<Module>.smethod_5<string>(1510633816));

		// Token: 0x04000416 RID: 1046
		private static readonly HashSet<string> ExcSearch = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			<Module>.smethod_6<string>(-642187224),
			<Module>.smethod_2<string>(-1272669251),
			<Module>.smethod_6<string>(1579168641)
		};

		// Token: 0x04000417 RID: 1047
		private const int NewLine = 10;

		// Token: 0x04000418 RID: 1048
		private const int CarrieageReturn = 13;
	}
}
