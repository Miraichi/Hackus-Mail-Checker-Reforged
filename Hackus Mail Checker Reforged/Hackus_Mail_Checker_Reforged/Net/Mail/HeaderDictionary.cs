using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000E5 RID: 229
	public class HeaderDictionary : SafeDictionary<string, Header>
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x0000A40B File Offset: 0x0000860B
		public HeaderDictionary() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002C378 File Offset: 0x0002A578
		public virtual string GetBoundary()
		{
			return this[<Module>.smethod_4<string>(-425688904)][<Module>.smethod_4<string>(784958944)];
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002C3A8 File Offset: 0x0002A5A8
		public virtual DateTime GetDate()
		{
			DateTime? dateTime = this[<Module>.smethod_6<string>(1554647284)].RawValue.ToNullDate();
			if (dateTime == null)
			{
				Regex[] array = HeaderDictionary.rxDates;
				for (int i = 0; i < array.Length; i++)
				{
					Match match = array[i].Matches(this[<Module>.smethod_3<string>(-1913257904)].RawValue ?? string.Empty).Cast<Match>().LastOrDefault<Match>();
					if (match != null)
					{
						dateTime = match.Value.ToNullDate();
						if (dateTime != null)
						{
							break;
						}
					}
				}
			}
			if (dateTime == null)
			{
				return DateTime.MinValue;
			}
			return dateTime.Value;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002C458 File Offset: 0x0002A658
		public virtual T GetEnum<T>(string name) where T : struct, IConvertible
		{
			string value = this[name].RawValue;
			if (string.IsNullOrEmpty(value))
			{
				return default(T);
			}
			return Enum.GetValues(typeof(T)).Cast<T>().ToArray<T>().FirstOrDefault((T x) => HeaderDictionary.<>c__DisplayClass4_0<T>.\u202A\u206C\u206A\u206D\u206D\u206B\u202C\u202A\u200D\u200F\u200D\u200E\u200B\u206E\u206E\u202B\u206E\u202D\u206F\u202D\u202E\u200E\u206B\u206D\u202A\u206B\u200B\u202B\u200D\u200C\u200B\u202C\u206A\u200F\u202B\u206F\u202E\u200C\u206F\u202E(x.ToString(), value, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0000A418 File Offset: 0x00008618
		public virtual void Add(string name, string value)
		{
			this[name] = new Header(value);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0000A427 File Offset: 0x00008627
		public virtual void Add(string name, DateTime value)
		{
			this[name] = new Header(value.GetRFC2060Date());
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002C4C4 File Offset: 0x0002A6C4
		public virtual MailAddress[] GetMailAddresses(string header)
		{
			List<MailAddress> list = new List<MailAddress>();
			string text = this[header].RawValue.Trim();
			int num = 0;
			int i = 0;
			while (i < text.Length)
			{
				i = HeaderDictionary.IndexOfExcludingQuoted(text, ',', i);
				if (i == -1)
				{
					i = text.Length;
				}
				MailAddress mailAddress = text.Substring(num, i - num).Trim().ToEmailAddress();
				if (mailAddress == null)
				{
					i++;
				}
				else
				{
					list.Add(mailAddress);
					num = i + 1;
					i = num;
				}
			}
			return list.ToArray();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002C550 File Offset: 0x0002A750
		public static HeaderDictionary Parse(string headers, Encoding encoding)
		{
			headers = Utils.DecodeWords(headers, encoding);
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			string[] array = headers.Split(new char[]
			{
				'\r',
				'\n'
			}, StringSplitOptions.RemoveEmptyEntries);
			string text = null;
			foreach (string text2 in array)
			{
				if (text != null && (text2[0] == '\t' || text2[0] == ' '))
				{
					Dictionary<string, string> dictionary2 = dictionary;
					string key = text;
					dictionary2[key] += text2.TrimStartOnce();
				}
				else
				{
					if (text != null)
					{
						dictionary[text] = dictionary[text].TrimEndOnce();
					}
					int num = text2.IndexOf(':');
					if (num > -1)
					{
						text = text2.Substring(0, num).TrimStartOnce();
						string value = text2.Substring(num + 1).TrimStartOnce();
						dictionary.Set(text, value);
					}
				}
			}
			HeaderDictionary headerDictionary = new HeaderDictionary();
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				headerDictionary.Add(keyValuePair.Key, new Header(keyValuePair.Value));
			}
			return headerDictionary;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002C698 File Offset: 0x0002A898
		private static int IndexOfExcludingQuoted(string str, char searchChar, int startIndex)
		{
			bool flag = false;
			for (int i = startIndex; i < str.Length; i++)
			{
				if (flag)
				{
					if (str[i] == '"')
					{
						flag = false;
					}
				}
				else
				{
					if (str[i] == searchChar)
					{
						return i;
					}
					if (str[i] == '"')
					{
						flag = true;
					}
				}
			}
			return -1;
		}

		// Token: 0x0400038B RID: 907
		private static Regex[] rxDates = (from x in new string[]
		{
			<Module>.smethod_5<string>(-2081752944),
			<Module>.smethod_2<string>(984818407)
		}
		select HeaderDictionary.<>c.smethod_0(x, RegexOptions.IgnoreCase | RegexOptions.Compiled)).ToArray<Regex>();
	}
}
