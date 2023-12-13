using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000F4 RID: 244
	internal static class Utils
	{
		// Token: 0x0600078D RID: 1933 RVA: 0x0002E34C File Offset: 0x0002C54C
		public static NameValueCollection ParseImapHeader(string data)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			string text = null;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (data != null)
			{
				foreach (char c in data)
				{
					if (c != ' ')
					{
						if (c == '(')
						{
							if (num > 0)
							{
								stringBuilder.Append(c);
							}
							num++;
						}
						else if (c != ')')
						{
							stringBuilder.Append(c);
						}
						else
						{
							num--;
							if (num > 0)
							{
								stringBuilder.Append(c);
							}
						}
					}
					else if (text == null)
					{
						text = stringBuilder.ToString();
						stringBuilder.Clear();
					}
					else if (num != 0)
					{
						stringBuilder.Append(c);
					}
					else
					{
						nameValueCollection[text] = stringBuilder.ToString();
						text = null;
						stringBuilder.Clear();
					}
				}
			}
			if (text != null)
			{
				nameValueCollection[text] = stringBuilder.ToString();
			}
			return nameValueCollection;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002E424 File Offset: 0x0002C624
		internal static byte[] Read(this Stream stream, int len)
		{
			byte[] array = new byte[len];
			int num = 0;
			int num2;
			while (num < len && (num2 = stream.Read(array, num, len - num)) > 0)
			{
				num += num2;
			}
			return array;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0002E458 File Offset: 0x0002C658
		public static string ReadLine(this Stream stream, ref int maxLength, Encoding encoding, char? termChar, int ReadTimeout = 10000)
		{
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = CheckerSettings.Instance.Timeout * 1000;
			}
			bool flag = maxLength > 0;
			byte b = 0;
			bool flag2 = false;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				do
				{
					byte b2 = b;
					int num = stream.ReadByte();
					if (num == -1)
					{
						break;
					}
					flag2 = true;
					b = (byte)num;
					if (flag)
					{
						maxLength--;
					}
					if (flag && memoryStream.Length == 1L)
					{
						int num2 = (int)b;
						char? c = termChar;
						int? num3 = (c != null) ? new int?((int)c.GetValueOrDefault()) : null;
						if (num2 == num3.GetValueOrDefault() & num3 != null)
						{
							int num4 = (int)b2;
							c = termChar;
							num3 = ((c != null) ? new int?((int)c.GetValueOrDefault()) : null);
							if (num4 == num3.GetValueOrDefault() & num3 != null)
							{
								maxLength++;
								continue;
							}
						}
					}
					if (b != 10)
					{
						if (b != 13)
						{
							memoryStream.WriteByte(b);
							if (flag && maxLength == 0)
							{
								break;
							}
							continue;
						}
					}
				}
				while (memoryStream.Length == 0L && b == 10);
				if (memoryStream.Length == 0L && !flag2)
				{
					result = null;
				}
				else
				{
					result = encoding.GetString(memoryStream.ToArray());
				}
			}
			return result;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002E5CC File Offset: 0x0002C7CC
		internal static string ReadToEnd(this Stream stream, int maxLength, Encoding encoding)
		{
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = CheckerSettings.Instance.Timeout * 1000;
			}
			byte[] array = new byte[8192];
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num;
				do
				{
					int count = (maxLength == 0) ? array.Length : Math.Min(maxLength - (int)memoryStream.Length, array.Length);
					num = stream.Read(array, 0, count);
					memoryStream.Write(array, 0, num);
					if (maxLength > 0 && memoryStream.Length == (long)maxLength)
					{
						break;
					}
				}
				while (num > 0);
				@string = encoding.GetString(memoryStream.ToArray());
			}
			return @string;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002E678 File Offset: 0x0002C878
		public static string ToQuotedString(this string str)
		{
			return <Module>.smethod_4<string>(2069567598) + str.Replace(<Module>.smethod_2<string>(1722009911), <Module>.smethod_4<string>(142392482)).Replace(<Module>.smethod_3<string>(-953053107), <Module>.smethod_6<string>(-1543100842)).Replace(<Module>.smethod_4<string>(219937593), <Module>.smethod_5<string>(570016217)).Replace(<Module>.smethod_2<string>(1588727045), <Module>.smethod_4<string>(-939042760)) + <Module>.smethod_2<string>(1588727045);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002E704 File Offset: 0x0002C904
		internal static string DecodeBase64(string data, Encoding encoding = null)
		{
			if (!Utils.IsValidBase64String(ref data, false))
			{
				return data;
			}
			byte[] bytes = Convert.FromBase64String(data);
			return (encoding ?? Encoding.Default).GetString(bytes);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0002E734 File Offset: 0x0002C934
		internal static bool IsValidBase64String(ref string param, bool strictPadding = false)
		{
			if (param == null)
			{
				return false;
			}
			param = param.Replace(<Module>.smethod_2<string>(1020182203), string.Empty).Replace(<Module>.smethod_3<string>(-227532916), string.Empty);
			int num = param.Length;
			int num2 = num % 4;
			if (num2 != 0)
			{
				if (strictPadding)
				{
					return false;
				}
				if (num2 > 2)
				{
					num2 %= 2;
				}
				param += new string('=', num2);
				num += num2;
			}
			if (num == 0)
			{
				return false;
			}
			string text = param.TrimEnd(new char[]
			{
				'='
			});
			int length = text.Length;
			if (num - length <= 2)
			{
				foreach (char item in text)
				{
					if (!Utils.Base64Characters.Contains(item))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002E800 File Offset: 0x0002CA00
		internal static string DecodeWords(string encodedWords, Encoding @default = null)
		{
			if (string.IsNullOrEmpty(encodedWords))
			{
				return string.Empty;
			}
			string text = encodedWords;
			foreach (Match match in Regex.Matches(encodedWords, <Module>.smethod_3<string>(-1795882533)))
			{
				if (match.Success)
				{
					string value = match.Value;
					string value2 = match.Groups[<Module>.smethod_4<string>(465161862)].Value;
					string value3 = match.Groups[<Module>.smethod_3<string>(1977608562)].Value;
					Encoding encoding = Utils.ParseCharsetToEncoding(match.Groups[<Module>.smethod_5<string>(-1828219578)].Value, @default);
					string a = value3.ToUpperInvariant();
					string newValue;
					if (a == <Module>.smethod_2<string>(1036502048))
					{
						newValue = Utils.DecodeBase64(value2, encoding);
					}
					else
					{
						if (!(a == <Module>.smethod_2<string>(-728666154)))
						{
							throw new ArgumentException(<Module>.smethod_6<string>(1610304771) + value3 + <Module>.smethod_6<string>(1165341684));
						}
						newValue = Utils.DecodeQuotedPrintable(value2, encoding);
					}
					text = text.Replace(value, newValue);
				}
			}
			return text;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002E958 File Offset: 0x0002CB58
		public static Encoding ParseCharsetToEncoding(string characterSet, Encoding @default)
		{
			if (string.IsNullOrEmpty(characterSet))
			{
				return @default ?? Encoding.Default;
			}
			string text = characterSet.ToUpperInvariant();
			if (!text.Contains(<Module>.smethod_6<string>(-1648721702)) && !text.Contains(<Module>.smethod_6<string>(-166088007)))
			{
				return (from x in Encoding.GetEncodings()
				where Utils.<>c__DisplayClass13_0.smethod_0(x).Is(characterSet)
				select Utils.<>c.smethod_0(x)).FirstOrDefault<Encoding>() ?? (@default ?? Encoding.Default);
			}
			text = text.Replace(<Module>.smethod_3<string>(-454772310), "");
			text = text.Replace(<Module>.smethod_6<string>(-1648721702), "");
			text = text.Replace(<Module>.smethod_6<string>(2018333639), "");
			int codepageNumber = int.Parse(text, CultureInfo.InvariantCulture);
			return (from x in Encoding.GetEncodings()
			where Utils.<>c__DisplayClass13_1.smethod_0(x) == codepageNumber
			select Utils.<>c.smethod_0(x)).FirstOrDefault<Encoding>() ?? (@default ?? Encoding.Default);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002EAB4 File Offset: 0x0002CCB4
		internal static string NotEmpty(this string input, params string[] others)
		{
			if (!string.IsNullOrEmpty(input))
			{
				return input;
			}
			foreach (string text in others)
			{
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002EAF0 File Offset: 0x0002CCF0
		internal static DateTime? ToNullDate(this string input)
		{
			input = Utils.NormalizeDate(input);
			DateTime value;
			if (DateTime.TryParse(input, Utils._enUsCulture, DateTimeStyles.None, out value))
			{
				return new DateTime?(value);
			}
			return null;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002EB28 File Offset: 0x0002CD28
		public static string NormalizeDate(string value)
		{
			/*
An exception occurred when decompiling this method (06000798)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String Hackus_Mail_Checker_Reforged.Net.Mail.Utils::NormalizeDate(System.String)

 ---> System.ArgumentOutOfRangeException: Non-negative number required. (Parameter 'length')
   at System.Array.Copy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length, Boolean reliable)
   at System.Array.Copy(Array sourceArray, Array destinationArray, Int32 length)
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackSlot.ModifyStack(StackSlot[] stack, Int32 popCount, Int32 pushCount, ByteCode pushDefinition) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 48
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 387
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 269
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 112
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000ABE5 File Offset: 0x00008DE5
		internal static string GetRFC2060Date(this DateTime date)
		{
			return date.ToString(<Module>.smethod_2<string>(-995231886), Utils._enUsCulture);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0000ABFD File Offset: 0x00008DFD
		internal static string GetRFC2822Date(this DateTime date)
		{
			return date.ToString(<Module>.smethod_2<string>(669441886), Utils._enUsCulture);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0000AC15 File Offset: 0x00008E15
		internal static bool EndsWithWhiteSpace(this string line)
		{
			return !string.IsNullOrEmpty(line) && line[line.Length - 1].IsWhiteSpace();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0000AC34 File Offset: 0x00008E34
		internal static bool IsWhiteSpace(this char chr)
		{
			if (chr != ' ' && chr != '\t')
			{
				if (chr != '\n')
				{
					return chr == '\r';
				}
			}
			return true;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002EBDC File Offset: 0x0002CDDC
		internal static string TrimStartOnce(this string line)
		{
			string text = line;
			if (string.IsNullOrEmpty(text))
			{
				text = text.Substring(1, text.Length - 1);
			}
			return text;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002EC04 File Offset: 0x0002CE04
		internal static string TrimEndOnce(this string line)
		{
			string text = line;
			if (text.EndsWithWhiteSpace())
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002EC2C File Offset: 0x0002CE2C
		public static string DecodeQuotedPrintable(string value, Encoding encoding = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.Default;
			}
			if (value.IndexOf('_') > -1 && value.IndexOf(' ') == -1)
			{
				value = value.Replace('_', ' ');
			}
			byte[] bytes = Encoding.ASCII.GetBytes(value);
			byte b = Convert.ToByte('=');
			int num = 0;
			int i = 0;
			while (i < bytes.Length)
			{
				byte b2 = bytes[i];
				if (b2 != b)
				{
					goto IL_C1;
				}
				if (i + 2 >= bytes.Length)
				{
					goto IL_C1;
				}
				byte b3 = bytes[i + 1];
				byte b4 = bytes[i + 2];
				if (b3 != 10 && b3 != 13)
				{
					if (!byte.TryParse(value.Substring(i + 1, 2), NumberStyles.HexNumber, null, out b2))
					{
						bytes[i] = b;
						num++;
					}
					else
					{
						bytes[num] = b2;
						num++;
						i += 2;
					}
				}
				else
				{
					i++;
					if (b4 == 10 || b4 == 13)
					{
						i++;
					}
				}
				IL_CA:
				i++;
				continue;
				IL_C1:
				bytes[num] = b2;
				num++;
				goto IL_CA;
			}
			value = encoding.GetString(bytes, 0, num);
			return value;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002ED1C File Offset: 0x0002CF1C
		public static void TryDispose<T>(ref T obj) where T : class, IDisposable
		{
			try
			{
				if (obj != null)
				{
					obj.Dispose();
				}
			}
			catch
			{
			}
			obj = default(T);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002ED60 File Offset: 0x0002CF60
		internal static VT Get<KT, VT>(this IDictionary<KT, VT> dictionary, KT key, VT defaultValue = default(VT))
		{
			if (dictionary == null)
			{
				return defaultValue;
			}
			VT result;
			if (dictionary.TryGetValue(key, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002ED80 File Offset: 0x0002CF80
		internal static void Set<KT, VT>(this IDictionary<KT, VT> dictionary, KT key, VT value)
		{
			if (!dictionary.ContainsKey(key))
			{
				lock (dictionary)
				{
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, value);
						return;
					}
				}
			}
			dictionary[key] = value;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000AC4E File Offset: 0x00008E4E
		internal static void Fire<T>(this EventHandler<T> events, object sender, T args) where T : EventArgs
		{
			if (events == null)
			{
				return;
			}
			events(sender, args);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002EDDC File Offset: 0x0002CFDC
		internal static MailAddress ToEmailAddress(this string input)
		{
			MailAddress result;
			try
			{
				result = new MailAddress(input);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000064BC File Offset: 0x000046BC
		internal static bool Is(this string input, string other)
		{
			return string.Equals(input, other, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002EE08 File Offset: 0x0002D008
		internal static bool IsTwoFactor(string response)
		{
			return response != null && Utils.TwoFactorArguments.Any((string arg) => response.ContainsIgnoreCase(arg));
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002EE44 File Offset: 0x0002D044
		internal static bool IsBlocked(string response)
		{
			return response != null && Utils.BlockArguments.Any((string arg) => response.ContainsIgnoreCase(arg));
		}

		// Token: 0x040003D2 RID: 978
		private static CultureInfo _enUsCulture = CultureInfo.GetCultureInfo(<Module>.smethod_4<string>(1215959370));

		// Token: 0x040003D3 RID: 979
		private const char Base64Padding = '=';

		// Token: 0x040003D4 RID: 980
		private static readonly HashSet<char> Base64Characters = new HashSet<char>
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'+',
			'/'
		};

		// Token: 0x040003D5 RID: 981
		private static readonly string[] BlockArguments = new string[]
		{
			<Module>.smethod_6<string>(871582601),
			<Module>.smethod_5<string>(-851837809),
			<Module>.smethod_5<string>(-2053933234),
			<Module>.smethod_5<string>(188526097),
			<Module>.smethod_5<string>(2051082696),
			<Module>.smethod_3<string>(-93009002),
			<Module>.smethod_4<string>(-845760941),
			<Module>.smethod_6<string>(1316545688),
			<Module>.smethod_5<string>(-1331084831),
			<Module>.smethod_6<string>(-1497517698),
			<Module>.smethod_3<string>(-52598919),
			<Module>.smethod_3<string>(1392526874),
			<Module>.smethod_3<string>(830640590),
			<Module>.smethod_5<string>(-1906104990),
			<Module>.smethod_4<string>(563252000),
			<Module>.smethod_5<string>(-613402489),
			<Module>.smethod_6<string>(-2095414574),
			<Module>.smethod_5<string>(1059202744),
			<Module>.smethod_2<string>(-1095875062)
		};

		// Token: 0x040003D6 RID: 982
		private static readonly string[] TwoFactorArguments = new string[]
		{
			<Module>.smethod_3<string>(-2122885803),
			<Module>.smethod_3<string>(911460756),
			<Module>.smethod_4<string>(1788324805)
		};

		// Token: 0x040003D7 RID: 983
		private static Regex rxTimeZoneName = new Regex(<Module>.smethod_3<string>(-212311812), RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x040003D8 RID: 984
		private static Regex rxTimeZoneColon = new Regex(<Module>.smethod_3<string>(1232813981), RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x040003D9 RID: 985
		private static Regex rxTimeZoneMinutes = new Regex(<Module>.smethod_2<string>(1101930174), RegexOptions.Compiled);

		// Token: 0x040003DA RID: 986
		private static Regex rxNegativeHours = new Regex(<Module>.smethod_4<string>(-593892332), RegexOptions.Compiled);
	}
}
