using System;
using System.Linq;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000ED RID: 237
	internal class Utf7Encoding
	{
		// Token: 0x06000744 RID: 1860 RVA: 0x0002D528 File Offset: 0x0002B728
		public static string Decode(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			string text = input.Replace(<Module>.smethod_2<string>(-2094283250), <Module>.smethod_2<string>(-1261946364));
			for (int num = text.IndexOf('&'); num != -1; num = text.IndexOf('&', num + 1))
			{
				int num2 = text.IndexOf('-', num);
				if (num2 > 0)
				{
					string text2 = text.Substring(num + 1, num2 - num - 1);
					string s = <Module>.smethod_3<string>(168792022) + text2.Replace(',', '/');
					text = text.Replace(<Module>.smethod_6<string>(-943474181) + text2 + <Module>.smethod_5<string>(1205836217), Encoding.UTF7.GetString(Encoding.UTF8.GetBytes(s)));
				}
			}
			return text;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002D5E8 File Offset: 0x0002B7E8
		public static string Encode(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			if (!input.All(new Func<char, bool>(Utf7Encoding.IsPrintableAscii)))
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (char c in input)
				{
					if (!Utf7Encoding.IsPrintableAscii(c))
					{
						stringBuilder2.Append(c);
					}
					else
					{
						if (stringBuilder2.Length > 0)
						{
							stringBuilder.Append(Utf7Encoding.EncodeNonPrintableAsciiString(stringBuilder2.ToString()));
							stringBuilder2.Clear();
						}
						if (c != '&')
						{
							stringBuilder.Append(c);
						}
						else
						{
							stringBuilder.Append(<Module>.smethod_5<string>(1395787583));
						}
					}
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder.Append(Utf7Encoding.EncodeNonPrintableAsciiString(stringBuilder2.ToString()));
				}
				return stringBuilder.ToString();
			}
			return input.Replace(<Module>.smethod_2<string>(-1261946364), <Module>.smethod_6<string>(537429729));
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0000A8EC File Offset: 0x00008AEC
		private static string EncodeNonPrintableAsciiString(string nonAsciiString)
		{
			return Encoding.UTF8.GetString(Encoding.UTF7.GetBytes(nonAsciiString)).Replace('/', ',').Replace('+', '&');
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0000A915 File Offset: 0x00008B15
		private static bool IsPrintableAscii(char c)
		{
			return c >= ' ' && c <= '~';
		}
	}
}
