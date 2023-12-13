using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000E3 RID: 227
	public struct Header
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x0002C11C File Offset: 0x0002A31C
		public Header(string value)
		{
			this = default(Header);
			this._values = new SafeDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			string rawValue;
			if ((rawValue = value) == null)
			{
				rawValue = (value = string.Empty);
			}
			this._rawValue = rawValue;
			this._values[string.Empty] = this.RawValue;
			int num = value.IndexOf(';');
			if (num > 0)
			{
				this._values[string.Empty] = value.Substring(0, num).Trim();
				value = value.Substring(num).Trim();
				Header.ParseValues(this._values, value);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0000A386 File Offset: 0x00008586
		public string Value
		{
			get
			{
				return this[string.Empty] ?? string.Empty;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0000A39C File Offset: 0x0000859C
		public string RawValue
		{
			get
			{
				return this._rawValue ?? string.Empty;
			}
		}

		// Token: 0x17000186 RID: 390
		public string this[string name]
		{
			get
			{
				return this._values.Get(name, string.Empty);
			}
			set
			{
				this._values.Set(name, value);
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002C1B0 File Offset: 0x0002A3B0
		public static void ParseValues(IDictionary<string, string> result, string header)
		{
			while (header.Length > 0)
			{
				int num = header.IndexOf('=');
				if (num < 0)
				{
					num = header.Length;
				}
				string key = header.Substring(0, num).Trim().Trim(new char[]
				{
					';',
					','
				}).Trim();
				string text;
				header = (text = header.Substring(Math.Min(header.Length, num + 1)).Trim());
				if (text.StartsWith(<Module>.smethod_2<string>(1588727045)))
				{
					Header.ProcessValue(1, ref header, ref text, new char[]
					{
						'"'
					});
				}
				else if (text.StartsWith(<Module>.smethod_5<string>(-942044748)))
				{
					Header.ProcessValue(1, ref header, ref text, new char[]
					{
						'\''
					});
				}
				else
				{
					Header.ProcessValue(0, ref header, ref text, new char[]
					{
						' ',
						',',
						';'
					});
				}
				result.Set(key, text);
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002C29C File Offset: 0x0002A49C
		private static void ProcessValue(int skip, ref string header, ref string value, params char[] lookFor)
		{
			int num = value.IndexOfAny(lookFor, skip);
			if (num < 0)
			{
				num = value.Length;
			}
			header = header.Substring(Math.Min(num + 1, header.Length));
			value = value.Substring(skip, num - skip);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
		public override string ToString()
		{
			IEnumerable<string> enumerable = from x in this._values
			where !Header.<>c.smethod_0(x.Key)
			select Header.<>c.smethod_1(x.Key, <Module>.smethod_2<string>(990316201), x.Value);
			return this.Value + (enumerable.Any<string>() ? (<Module>.smethod_4<string>(1574223004) + string.Join(<Module>.smethod_6<string>(-1556939122), enumerable)) : null);
		}

		// Token: 0x04000386 RID: 902
		private string _rawValue;

		// Token: 0x04000387 RID: 903
		private SafeDictionary<string, string> _values;
	}
}
