using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x02000115 RID: 277
	public class MessageBuilder
	{
		// Token: 0x06000889 RID: 2185 RVA: 0x00033D14 File Offset: 0x00031F14
		public static MailMessage FromHeader(string text)
		{
			NameValueCollection nameValueCollection = MessageBuilder.ParseMailHeader(text);
			MailMessage mailMessage = new MailMessage();
			foreach (object obj in nameValueCollection)
			{
				string name = (string)obj;
				string value = nameValueCollection.GetValues(name)[0];
				try
				{
					mailMessage.Headers.Add(name, value);
				}
				catch
				{
				}
			}
			Match match = Regex.Match(nameValueCollection[<Module>.smethod_6<string>(-1354554277)] ?? string.Empty, <Module>.smethod_5<string>(-1583823563));
			if (match.Success)
			{
				try
				{
					mailMessage.Subject = MessageBuilder.DecodeWords(nameValueCollection[<Module>.smethod_3<string>(-1305046948)]).Replace(Environment.NewLine, "");
					goto IL_ED;
				}
				catch
				{
					mailMessage.Subject = nameValueCollection[<Module>.smethod_2<string>(-1784181404)];
					goto IL_ED;
				}
			}
			mailMessage.Subject = nameValueCollection[<Module>.smethod_4<string>(1255037929)];
			IL_ED:
			match = Regex.Match(nameValueCollection[<Module>.smethod_5<string>(-503726077)] ?? string.Empty, <Module>.smethod_5<string>(-1583823563));
			if (match.Success)
			{
				try
				{
					mailMessage.From = MessageBuilder.DecodeWords(nameValueCollection[<Module>.smethod_4<string>(-750294305)]).Replace(Environment.NewLine, "");
					goto IL_17B;
				}
				catch
				{
					mailMessage.From = nameValueCollection[<Module>.smethod_2<string>(1934118314)];
					goto IL_17B;
				}
			}
			mailMessage.From = nameValueCollection[<Module>.smethod_4<string>(-750294305)];
			IL_17B:
			string text2 = nameValueCollection[<Module>.smethod_2<string>(157979315)];
			DateTime? dateTime = (text2 != null) ? text2.ToNullDate() : null;
			if (dateTime == null)
			{
				Regex[] array = MessageBuilder.rxDates;
				for (int i = 0; i < array.Length; i++)
				{
					match = array[i].Matches(nameValueCollection[<Module>.smethod_5<string>(350657753)] ?? string.Empty).Cast<Match>().LastOrDefault<Match>();
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
			if (dateTime != null)
			{
				mailMessage.Date = dateTime.Value;
			}
			else if (!string.IsNullOrEmpty(nameValueCollection[<Module>.smethod_6<string>(1554647284)]))
			{
				mailMessage.RawDate = nameValueCollection[<Module>.smethod_5<string>(1037741986)];
			}
			else if (!string.IsNullOrEmpty(nameValueCollection[<Module>.smethod_6<string>(664416743)]))
			{
				mailMessage.RawDate = nameValueCollection[<Module>.smethod_5<string>(350657753)];
			}
			else
			{
				mailMessage.Date = DateTime.MinValue;
			}
			return mailMessage;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00033FE8 File Offset: 0x000321E8
		public static MailMessage FromMime822(string text, bool IsEmptyBody, Encoding defaultEncoding, bool SkipAdditionalParts = true)
		{
			StringReader stringReader = new StringReader(text);
			StringBuilder stringBuilder = new StringBuilder();
			string value;
			while (!string.IsNullOrEmpty(value = stringReader.ReadLine()))
			{
				stringBuilder.AppendLine(value);
			}
			MailMessage mailMessage = MessageBuilder.FromHeader(stringBuilder.ToString());
			if (!IsEmptyBody)
			{
				foreach (MimePart mimePart in MessageBuilder.ParseMailBody(stringReader.ReadToEnd(), mailMessage.Headers))
				{
					MessageBuilder.SetBody(mailMessage, mimePart, defaultEncoding, SkipAdditionalParts);
				}
				return mailMessage;
			}
			return mailMessage;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00034064 File Offset: 0x00032264
		public static void SetBody(MailMessage message, MimePart mimePart, Encoding defaultEncoding, bool SkipAdditionalParts = true)
		{
			string text = <Module>.smethod_3<string>(576681594);
			string str = "";
			NameValueCollection nameValueCollection = MessageBuilder.ParseMimeField(mimePart.Headers[<Module>.smethod_2<string>(-715219161)]);
			Match match = Regex.Match(nameValueCollection[<Module>.smethod_3<string>(1898848107)], <Module>.smethod_6<string>(1433153992));
			if (match.Success)
			{
				text = match.Groups[1].Value.ToLower();
				str = match.Groups[2].Value.ToLower();
			}
			if (SkipAdditionalParts && text != <Module>.smethod_3<string>(-710460337))
			{
				return;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text2 in nameValueCollection.AllKeys)
			{
				dictionary.Add(text2, nameValueCollection[text2]);
			}
			if (SkipAdditionalParts && dictionary.ContainsKey(<Module>.smethod_5<string>(1290480718)))
			{
				return;
			}
			Encoding encoding = dictionary.ContainsKey(<Module>.smethod_5<string>(-1828219578)) ? EncodingHelper.ParseEncodingName(dictionary[<Module>.smethod_2<string>(-212003281)]) : defaultEncoding;
			string text3 = mimePart.Headers[<Module>.smethod_3<string>(-954980546)] ?? string.Empty;
			byte[] array = new byte[0];
			if (string.IsNullOrEmpty(text3))
			{
				array = defaultEncoding.GetBytes(mimePart.Body);
			}
			else
			{
				try
				{
					uint num = MessageBuilder.ComputeStringHash(text3);
					if (num <= 2312238551U)
					{
						if (num <= 1571452250U)
						{
							if (num == 522200311U)
							{
								if (!(text3 == <Module>.smethod_6<string>(-2121361349)))
								{
									goto IL_300;
								}
							}
							else
							{
								if (num != 1571452250U)
								{
									goto IL_300;
								}
								if (!(text3 == <Module>.smethod_4<string>(1455326780)))
								{
									goto IL_300;
								}
								goto IL_2F1;
							}
						}
						else if (num != 1759697756U)
						{
							if (num != 2072664378U)
							{
								if (num != 2312238551U)
								{
									goto IL_300;
								}
								if (!(text3 == <Module>.smethod_4<string>(-759910943)))
								{
									goto IL_300;
								}
							}
							else
							{
								if (!(text3 == <Module>.smethod_5<string>(-1810731990)))
								{
									goto IL_300;
								}
								goto IL_2F1;
							}
						}
						else if (!(text3 == <Module>.smethod_3<string>(-1951806684)))
						{
							goto IL_300;
						}
					}
					else if (num > 2724123114U)
					{
						if (num == 3517586327U)
						{
							if (!(text3 == <Module>.smethod_4<string>(-2127921567)))
							{
								goto IL_300;
							}
						}
						else if (num != 4031671994U)
						{
							if (num != 4035089399U)
							{
								goto IL_300;
							}
							if (!(text3 == <Module>.smethod_4<string>(2070267342)))
							{
								goto IL_300;
							}
						}
						else
						{
							if (text3 == <Module>.smethod_5<string>(-340399650))
							{
								goto IL_2F1;
							}
							goto IL_300;
						}
					}
					else if (num != 2592079690U)
					{
						if (num != 2724123114U)
						{
							goto IL_300;
						}
						if (!(text3 == <Module>.smethod_3<string>(-506680891)))
						{
							goto IL_300;
						}
						goto IL_2F1;
					}
					else
					{
						if (text3 == <Module>.smethod_6<string>(-1083690741))
						{
							goto IL_2F1;
						}
						goto IL_300;
					}
					array = encoding.GetBytes(MessageBuilder.QpDecode(mimePart.Body, encoding));
					goto IL_30E;
					IL_2F1:
					array = Convert.FromBase64String(mimePart.Body);
					goto IL_30E;
					IL_300:
					array = defaultEncoding.GetBytes(mimePart.Body);
					IL_30E:;
				}
				catch
				{
					array = defaultEncoding.GetBytes(mimePart.Body);
				}
			}
			try
			{
				if (dictionary.ContainsKey(<Module>.smethod_4<string>(-1760129032)))
				{
					message.AdditionalFiles.Add(new Attachment(text + <Module>.smethod_3<string>(-1872781685) + str, array, dictionary[<Module>.smethod_5<string>(1290480718)]));
				}
				else if (!(text != <Module>.smethod_3<string>(-710460337)))
				{
					message.AlternateViews.Add(new Attachment(text + <Module>.smethod_5<string>(1871461572) + str, encoding.GetString(array)));
				}
				else
				{
					message.AdditionalFiles.Add(new Attachment(text + <Module>.smethod_2<string>(-1740620499) + str, array));
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0003447C File Offset: 0x0003267C
		public static string DecodeWords(string words)
		{
			if (string.IsNullOrEmpty(words))
			{
				return string.Empty;
			}
			MatchCollection matchCollection = Regex.Matches(words, <Module>.smethod_3<string>(1540741269));
			if (matchCollection.Count != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (object obj in matchCollection)
				{
					Match match = (Match)obj;
					if (match.Index > num)
					{
						MessageBuilder.HandleFillData(stringBuilder, words.Substring(num, match.Index - num));
					}
					stringBuilder.Append(MessageBuilder.DecodeWord(match.Groups[0].Value));
					num = match.Index + match.Length;
				}
				MessageBuilder.HandleFillData(stringBuilder, words.Substring(num));
				return stringBuilder.ToString();
			}
			return words;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00034564 File Offset: 0x00032764
		public static void HandleFillData(StringBuilder decoded, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return;
			}
			string text = data.Replace(Environment.NewLine, "");
			if (text.Trim().Length != 0)
			{
				decoded.Append(text);
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000345A0 File Offset: 0x000327A0
		public static string DecodeWord(string word)
		{
			if (string.IsNullOrEmpty(word))
			{
				return string.Empty;
			}
			Match match = Regex.Match(word, <Module>.smethod_5<string>(-523195550));
			if (!match.Success)
			{
				return word;
			}
			Encoding encoding = EncodingHelper.ParseEncodingName(match.Groups[1].Value);
			string value = match.Groups[2].Value;
			string value2 = match.Groups[3].Value;
			if (value == <Module>.smethod_6<string>(1131154326) || value == <Module>.smethod_5<string>(-877267977))
			{
				return MessageBuilder.QDecode(value2, encoding);
			}
			if (!(value == <Module>.smethod_5<string>(579555585)) && !(value == <Module>.smethod_5<string>(414239753)))
			{
				throw new FormatException(<Module>.smethod_6<string>(-1533843183) + word);
			}
			return encoding.GetString(Convert.FromBase64String(value2));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00034688 File Offset: 0x00032888
		public static string QpDecode(string decodeValue, Encoding encoding)
		{
			string @string;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int i = 0;
					while (i < decodeValue.Length)
					{
						char c = decodeValue[i];
						if (c != '=')
						{
							goto IL_49;
						}
						string text = decodeValue.Substring(i + 1, 2);
						try
						{
							if (text != Environment.NewLine)
							{
								memoryStream.WriteByte(Convert.ToByte(text, 16));
							}
							goto IL_80;
						}
						catch (FormatException)
						{
							goto IL_80;
						}
						goto IL_49;
						IL_80:
						i += 2;
						IL_7A:
						i++;
						continue;
						IL_49:
						try
						{
							memoryStream.WriteByte(Convert.ToByte(c));
						}
						catch (OverflowException)
						{
							byte[] bytes = encoding.GetBytes(new char[]
							{
								c
							});
							memoryStream.Write(bytes, 0, bytes.Length);
						}
						goto IL_7A;
					}
					@string = encoding.GetString(memoryStream.ToArray());
				}
			}
			catch
			{
				throw new FormatException(<Module>.smethod_4<string>(1111488103));
			}
			return @string;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00034780 File Offset: 0x00032980
		public static string QDecode(string value, Encoding encoding)
		{
			string @string;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					for (int i = 0; i < value.Length; i++)
					{
						char c = value[i];
						if (c != '=')
						{
							if (c != '_')
							{
								try
								{
									memoryStream.WriteByte(Convert.ToByte(value[i]));
									goto IL_8D;
								}
								catch (OverflowException)
								{
									byte[] bytes = encoding.GetBytes(new char[]
									{
										value[i]
									});
									memoryStream.Write(bytes, 0, bytes.Length);
									goto IL_8D;
								}
							}
							memoryStream.WriteByte(Convert.ToByte(' '));
						}
						else
						{
							string value2 = value.Substring(i + 1, 2);
							memoryStream.WriteByte(Convert.ToByte(value2, 16));
							i += 2;
						}
						IL_8D:;
					}
					@string = encoding.GetString(memoryStream.ToArray());
				}
			}
			catch
			{
				throw new FormatException(<Module>.smethod_5<string>(1645349659));
			}
			return @string;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0003487C File Offset: 0x00032A7C
		public static MimePart[] ParseMailBody(string body, NameValueCollection header)
		{
			NameValueCollection nameValueCollection = MessageBuilder.ParseMimeField(header[<Module>.smethod_6<string>(-1106786680)]);
			if (!string.IsNullOrEmpty(nameValueCollection[<Module>.smethod_2<string>(-1204665765)]))
			{
				return MessageBuilder.ParseMimeParts(new StringReader(body), nameValueCollection[<Module>.smethod_5<string>(-818852038)]);
			}
			return new MimePart[]
			{
				new MimePart
				{
					Body = body,
					Headers = new NameValueCollection
					{
						{
							<Module>.smethod_6<string>(-1106786680),
							header[<Module>.smethod_6<string>(-1106786680)]
						},
						{
							<Module>.smethod_6<string>(-1241813885),
							header[<Module>.smethod_3<string>(-304630476)]
						},
						{
							<Module>.smethod_6<string>(-1386707483),
							header[<Module>.smethod_5<string>(256079387)]
						},
						{
							<Module>.smethod_4<string>(450736905),
							header[<Module>.smethod_2<string>(-2097007356)]
						}
					}
				}
			};
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00034974 File Offset: 0x00032B74
		public static MimePart[] ParseMimeParts(StringReader reader, string boundary)
		{
			List<MimePart> list = new List<MimePart>();
			string value = <Module>.smethod_4<string>(-503234015) + boundary;
			string value2 = <Module>.smethod_5<string>(34337021) + boundary + <Module>.smethod_6<string>(-208211581);
			string text;
			while ((text = reader.ReadLine()) != null)
			{
				if (text.StartsWith(value))
				{
					while (text != null && text.StartsWith(value))
					{
						MimePart mimePart = new MimePart();
						StringBuilder stringBuilder = new StringBuilder();
						while (!string.IsNullOrEmpty(text = reader.ReadLine()))
						{
							stringBuilder.AppendLine(text);
						}
						mimePart.Headers = MessageBuilder.ParseMailHeader(stringBuilder.ToString());
						NameValueCollection nameValueCollection = MessageBuilder.ParseMimeField(mimePart.Headers[<Module>.smethod_2<string>(-715219161)]);
						if (nameValueCollection[<Module>.smethod_6<string>(979541980)] != null)
						{
							list.AddRange(MessageBuilder.ParseInnerBoundary(reader, boundary, nameValueCollection[<Module>.smethod_5<string>(-1891801578)]));
						}
						StringBuilder stringBuilder2 = new StringBuilder();
						while ((text = reader.ReadLine()) != null && !text.StartsWith(value))
						{
							stringBuilder2.AppendLine(text);
						}
						mimePart.Body = stringBuilder2.ToString();
						if (mimePart.Body != null && mimePart.Body.Trim() != string.Empty)
						{
							list.Add(mimePart);
						}
						if (text == null || text.StartsWith(value2))
						{
							break;
						}
					}
					return list.ToArray();
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00034AE4 File Offset: 0x00032CE4
		public static MimePart[] ParseInnerBoundary(StringReader reader, string mainBoundary, string innerBoundary)
		{
			List<MimePart> list = new List<MimePart>();
			string text = <Module>.smethod_4<string>(-503234015) + mainBoundary;
			string text2 = <Module>.smethod_6<string>(-208211581) + innerBoundary;
			string text3;
			while ((text3 = reader.ReadLine()) != null)
			{
				if (text3.StartsWith(text2))
				{
					while (text3 != null)
					{
						if (text3.StartsWith(text2))
						{
							MimePart mimePart = new MimePart();
							StringBuilder stringBuilder = new StringBuilder();
							while (!string.IsNullOrEmpty(text3 = reader.ReadLine()))
							{
								stringBuilder.AppendLine(text3);
							}
							mimePart.Headers = MessageBuilder.ParseMailHeader(stringBuilder.ToString());
							NameValueCollection nameValueCollection = MessageBuilder.ParseMimeField(mimePart.Headers[<Module>.smethod_3<string>(1979536001)]);
							if (nameValueCollection[<Module>.smethod_5<string>(-818852038)] != null)
							{
								list.AddRange(MessageBuilder.ParseInnerBoundary(reader, innerBoundary, nameValueCollection[<Module>.smethod_2<string>(451686943)]));
							}
							StringBuilder stringBuilder2 = new StringBuilder();
							while ((text3 = reader.ReadLine()) != null && !text3.StartsWith(text) && !text3.StartsWith(text2))
							{
								stringBuilder2.AppendLine(text3);
							}
							mimePart.Body = stringBuilder2.ToString();
							if (mimePart.Body != null && mimePart.Body.Trim() != string.Empty)
							{
								list.Add(mimePart);
							}
							if (text3 != null)
							{
								if (!text3.StartsWith(text))
								{
									if (!text3.StartsWith(text2))
									{
										continue;
									}
									text3 = reader.ReadToEnd();
									reader = new StringReader(text2 + Environment.NewLine + text3);
									list.AddRange(MessageBuilder.ParseInnerBoundary(reader, mainBoundary, innerBoundary));
								}
								else
								{
									text3 = reader.ReadToEnd();
									reader = new StringReader(text + Environment.NewLine + text3);
									list.AddRange(MessageBuilder.ParseInnerBoundary(reader, innerBoundary, mainBoundary));
								}
							}
						}
						IL_1BA:
						return list.ToArray();
					}
					goto IL_1BA;
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00034CB4 File Offset: 0x00032EB4
		public static NameValueCollection ParseMailHeader(string header)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			StringReader stringReader = new StringReader(header);
			string text = null;
			string text2;
			while ((text2 = stringReader.ReadLine()) != null)
			{
				if (!(text2 == string.Empty))
				{
					if (text2[0].IsWhiteSpace())
					{
						if (text != null)
						{
							NameValueCollection nameValueCollection2 = nameValueCollection;
							string text3 = text;
							NameValueCollection nameValueCollection3 = nameValueCollection2;
							string name = text3;
							nameValueCollection3[name] += text2.Trim();
						}
					}
					else
					{
						int num = text2.IndexOf(':');
						if (num >= 0)
						{
							text = text2.Substring(0, num);
							string text4 = text2.Substring(num + 1).Trim();
							if (!MessageBuilder._headersSet.Contains(text))
							{
								text4 = MessageBuilder.StripComments(text4);
							}
							nameValueCollection.Add(text, text4);
						}
					}
				}
			}
			return nameValueCollection;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00034D7C File Offset: 0x00032F7C
		public static NameValueCollection ParseMimeField(string field)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (!string.IsNullOrEmpty(field))
			{
				HashSet<string> hashSet = new HashSet<string>();
				try
				{
					foreach (object obj in Regex.Matches(field, <Module>.smethod_6<string>(1719993935)))
					{
						Match match = (Match)obj;
						string text = match.Groups[1].Value.Trim();
						string str = match.Groups[3].Value.Trim(new char[]
						{
							'"'
						});
						NameValueCollection nameValueCollection2 = nameValueCollection;
						string text2 = text;
						NameValueCollection nameValueCollection3 = nameValueCollection2;
						string name = text2;
						nameValueCollection3[name] += str;
						if (match.Groups[2].Value == <Module>.smethod_2<string>(-908283613))
						{
							hashSet.Add(text);
						}
					}
					foreach (string name2 in hashSet)
					{
						try
						{
							nameValueCollection[name2] = MessageBuilder.Rfc2231Decode(nameValueCollection[name2]);
						}
						catch (FormatException)
						{
						}
					}
					Match match2 = Regex.Match(field, <Module>.smethod_6<string>(-204143277));
					nameValueCollection.Add(<Module>.smethod_5<string>(835074596), match2.Success ? match2.Groups[1].Value.Trim() : "");
				}
				catch
				{
					nameValueCollection.Add(<Module>.smethod_3<string>(1898848107), string.Empty);
				}
				return nameValueCollection;
			}
			nameValueCollection.Add(<Module>.smethod_3<string>(1898848107), string.Empty);
			return nameValueCollection;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00034F9C File Offset: 0x0003319C
		public static string Rfc2231Decode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			Match match = Regex.Match(value, <Module>.smethod_3<string>(1180905400));
			if (match.Success)
			{
				string value2 = match.Groups[1].Value;
				string value3 = match.Groups[2].Value;
				string @string;
				try
				{
					Encoding encoding = Encoding.GetEncoding(value2);
					using (MemoryStream memoryStream = new MemoryStream())
					{
						int i = 0;
						while (i < value3.Length)
						{
							if (value3[i] != '%')
							{
								try
								{
									memoryStream.WriteByte(Convert.ToByte(value[i]));
									goto IL_D8;
								}
								catch (OverflowException)
								{
									byte[] bytes = encoding.GetBytes(new char[]
									{
										value[i]
									});
									memoryStream.Write(bytes, 0, bytes.Length);
									goto IL_D8;
								}
								goto IL_B5;
							}
							goto IL_B5;
							IL_D8:
							i++;
							continue;
							IL_B5:
							string value4 = value3.Substring(i + 1, 2);
							memoryStream.WriteByte(Convert.ToByte(value4, 16));
							i += 2;
							goto IL_D8;
						}
						@string = encoding.GetString(memoryStream.ToArray());
					}
				}
				catch
				{
					throw new FormatException(<Module>.smethod_5<string>(436893402));
				}
				return @string;
			}
			return value;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x000350E8 File Offset: 0x000332E8
		public static string StripComments(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < s.Length)
			{
				char c = s[i];
				if (c != '\\')
				{
					goto IL_34;
				}
				if (flag2)
				{
					goto IL_34;
				}
				flag2 = true;
				IL_81:
				i++;
				continue;
				IL_34:
				if (c == '"' && !flag2)
				{
					flag = !flag;
				}
				if (flag || flag2)
				{
					goto IL_56;
				}
				if (c != '(')
				{
					goto IL_56;
				}
				num++;
				IL_7B:
				flag2 = false;
				goto IL_81;
				IL_56:
				if (!flag && !flag2)
				{
					if (c == ')')
					{
						if (num > 0)
						{
							num--;
							goto IL_7B;
						}
					}
				}
				if (num <= 0)
				{
					stringBuilder.Append(c);
					goto IL_7B;
				}
				goto IL_7B;
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00035194 File Offset: 0x00033394
		public static uint ComputeStringHash(string s)
		{
			uint num = 0U;
			if (s != null)
			{
				num = 2166136261U;
				for (int i = 0; i < s.Length; i++)
				{
					num = ((uint)s[i] ^ num) * 16777619U;
				}
			}
			return num;
		}

		// Token: 0x04000435 RID: 1077
		private static readonly HashSet<string> _headersSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
		{
			<Module>.smethod_4<string>(1255037929),
			<Module>.smethod_3<string>(-1909469162),
			<Module>.smethod_6<string>(-202413492),
			<Module>.smethod_2<string>(-1071382899)
		};

		// Token: 0x04000436 RID: 1078
		private static readonly Regex[] rxDates = (from x in new string[]
		{
			<Module>.smethod_5<string>(-2081752944),
			<Module>.smethod_5<string>(123749326)
		}
		select MessageBuilder.<>c.smethod_0(x, RegexOptions.IgnoreCase | RegexOptions.Compiled)).ToArray<Regex>();
	}
}
