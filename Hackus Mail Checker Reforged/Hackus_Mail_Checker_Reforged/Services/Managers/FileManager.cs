using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x0200006D RID: 109
	internal static class FileManager
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00008D21 File Offset: 0x00006F21
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00008D28 File Offset: 0x00006F28
		public static string BaseFileName { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00008D30 File Offset: 0x00006F30
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x00008D37 File Offset: 0x00006F37
		public static string ResultsPath { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00008D3F File Offset: 0x00006F3F
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x00008D46 File Offset: 0x00006F46
		public static string ConfigurationPath { get; set; } = Path.Combine(<Module>.smethod_2<string>(2070174868), <Module>.smethod_4<string>(-685338130));

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00008D4E File Offset: 0x00006F4E
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00008D55 File Offset: 0x00006F55
		public static string SettingsPath { get; set; } = Path.Combine(<Module>.smethod_4<string>(-772499879), <Module>.smethod_5<string>(-1949024626));

		// Token: 0x060003CB RID: 971 RVA: 0x00017C9C File Offset: 0x00015E9C
		public static void CreateResultsDirectory()
		{
			string str = DateTime.Now.ToString(<Module>.smethod_6<string>(345015252), CultureInfo.InvariantCulture);
			FileManager.ResultsPath = Path.GetFullPath(Path.Combine(<Module>.smethod_4<string>(732679442), str + <Module>.smethod_3<string>(-1062652586) + FileManager.BaseFileName));
			Directory.CreateDirectory(FileManager.ResultsPath);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00017D00 File Offset: 0x00015F00
		public static void SaveRest()
		{
			object restLocker = FileManager._restLocker;
			lock (restLocker)
			{
				try
				{
					List<Mailbox> list = MailManager.Instance.QueueToList();
					if (list != null)
					{
						using (StreamWriter streamWriter = new StreamWriter(Path.Combine(FileManager.ResultsPath, <Module>.smethod_2<string>(1354801003)), false))
						{
							foreach (Mailbox mailbox in list)
							{
								streamWriter.WriteLine(mailbox.Address + <Module>.smethod_2<string>(-1691660964) + mailbox.Password);
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00017DEC File Offset: 0x00015FEC
		public static void SaveStatistics(string login, string password, OperationResult type)
		{
			object statisticsLocker = FileManager._statisticsLocker;
			lock (statisticsLocker)
			{
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(Path.Combine(FileManager.ResultsPath, string.Format(<Module>.smethod_4<string>(-249529385), type)), true))
					{
						streamWriter.WriteLine(login + <Module>.smethod_2<string>(-1691660964) + password);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00017E8C File Offset: 0x0001608C
		public static void SaveContact(string contact)
		{
			object statisticsLocker = FileManager._statisticsLocker;
			lock (statisticsLocker)
			{
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(Path.Combine(FileManager.ResultsPath, <Module>.smethod_5<string>(446429010)), true))
					{
						streamWriter.WriteLine(contact);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00017F10 File Offset: 0x00016110
		public static void SaveFound(string login, string password, string request, int count)
		{
			object foundLocker = FileManager._foundLocker;
			lock (foundLocker)
			{
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(Path.Combine(FileManager.ResultsPath, FileManager.GetSafeFilename(request) + <Module>.smethod_2<string>(829792275)), true))
					{
						streamWriter.WriteLine(string.Format(<Module>.smethod_3<string>(-1022242503), login, password, count));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00017FB4 File Offset: 0x000161B4
		public static void SaveLetter(string login, string password, string request, string from, string body, string subject, DateTime date, string folder = "INBOX")
		{
			string text;
			string str;
			if (SearchSettings.Instance.DownloadMode != DownloadMode.Html)
			{
				text = FileManager.GetPlainText(login, password, from, body, subject, date, folder);
				str = <Module>.smethod_2<string>(829792275);
			}
			else
			{
				text = FileManager.GetHtml(login, password, from, body, subject, date, folder);
				str = <Module>.smethod_4<string>(-1881561021);
			}
			string path;
			if (!SearchSettings.Instance.DownloadIntoSingleFile)
			{
				path = Path.Combine(FileManager.ResultsPath, FileManager.GetSafeFilename(request), login + str);
			}
			else
			{
				path = Path.Combine(FileManager.ResultsPath, <Module>.smethod_4<string>(-1794399272), FileManager.GetSafeFilename(request) + str);
			}
			if (text != null)
			{
				object letterLocker = FileManager._letterLocker;
				lock (letterLocker)
				{
					try
					{
						Directory.CreateDirectory(Path.GetDirectoryName(path));
						using (StreamWriter streamWriter = new StreamWriter(path, true))
						{
							streamWriter.WriteLine(text);
						}
					}
					catch
					{
					}
				}
				return;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000180CC File Offset: 0x000162CC
		public static void SaveLetter(string login, string password, string request, System.Net.Mail.MailMessage message)
		{
			string text;
			if (message.IsBodyHtml && SearchSettings.Instance.DownloadMode == DownloadMode.Html)
			{
				text = WebUtility.HtmlDecode(FileManager.HtmlToPlainText(message.Body));
			}
			else if (message.IsBodyHtml && SearchSettings.Instance.DownloadMode == DownloadMode.Plain)
			{
				text = FileManager.HtmlToPlainText(WebUtility.HtmlDecode(FileManager.HtmlToPlainText(message.Body)));
			}
			else
			{
				text = message.Body;
			}
			if (text == null)
			{
				return;
			}
			FileManager.SaveLetter(login, password, request, message.Sender.ToString(), text, message.Subject, DateTime.Parse(message.Headers[<Module>.smethod_3<string>(494066261)]), <Module>.smethod_4<string>(-521243277));
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00018178 File Offset: 0x00016378
		public static void SaveLetter(string login, string password, string request, Hackus_Mail_Checker_Reforged.Net.Mail.MailMessage message)
		{
			string body = FileManager.GetBody(message, SearchSettings.Instance.DownloadMode);
			if (body == null)
			{
				return;
			}
			FileManager.SaveLetter(login, password, request, message.Sender.ToString(), body, message.Subject, message.Date, message.Folder ?? <Module>.smethod_4<string>(-521243277));
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000181D0 File Offset: 0x000163D0
		public static void SaveLetter(string login, string password, string request, Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage message)
		{
			string body = FileManager.GetBody(message, SearchSettings.Instance.DownloadMode);
			if (body != null)
			{
				string from = message.From;
				string body2 = body;
				string subject = message.Subject;
				DateTime date = message.Date;
				Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder folder = message.Folder;
				string folder2;
				if (folder != null)
				{
					if ((folder2 = folder.Name) != null)
					{
						goto IL_4A;
					}
				}
				folder2 = <Module>.smethod_4<string>(-521243277);
				IL_4A:
				FileManager.SaveLetter(login, password, request, from, body2, subject, date, folder2);
				return;
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00018230 File Offset: 0x00016430
		public static void SaveEazyWebLetter(string login, string password, string request, Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage message)
		{
			string eazyWebBody = FileManager.GetEazyWebBody(message, SearchSettings.Instance.DownloadMode);
			if (eazyWebBody != null)
			{
				string from = message.From;
				string body = eazyWebBody;
				string subject = message.Subject;
				DateTime date = message.Date;
				Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder folder = message.Folder;
				string folder2;
				if (folder != null)
				{
					if ((folder2 = folder.Name) != null)
					{
						goto IL_4A;
					}
				}
				folder2 = <Module>.smethod_6<string>(2132499449);
				IL_4A:
				FileManager.SaveLetter(login, password, request, from, body, subject, date, folder2);
				return;
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00018290 File Offset: 0x00016490
		public static void SaveWebLetter(string login, string password, string request, Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage message)
		{
			string webBody = FileManager.GetWebBody(message, SearchSettings.Instance.DownloadMode);
			if (webBody == null)
			{
				return;
			}
			string from = message.From;
			string body = webBody;
			string subject = message.Subject;
			DateTime date = message.Date;
			Hackus_Mail_Checker_Reforged.Net.Mail.IMAP.Folder folder = message.Folder;
			string folder2;
			if (folder != null)
			{
				if ((folder2 = folder.Name) != null)
				{
					goto IL_4B;
				}
			}
			folder2 = <Module>.smethod_2<string>(-1971847226);
			IL_4B:
			FileManager.SaveLetter(login, password, request, from, body, subject, date, folder2);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000182F0 File Offset: 0x000164F0
		private static string GetBody(Hackus_Mail_Checker_Reforged.Net.Mail.MailMessage message, DownloadMode mode)
		{
			Hackus_Mail_Checker_Reforged.Net.Mail.AlternateViewCollection alternateViews = message.AlternateViews;
			string text;
			if (alternateViews == null)
			{
				text = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Attachment htmlView = alternateViews.GetHtmlView();
				text = ((htmlView != null) ? htmlView.Body : null);
			}
			string text2 = text;
			Hackus_Mail_Checker_Reforged.Net.Mail.AlternateViewCollection alternateViews2 = message.AlternateViews;
			string text3;
			if (alternateViews2 == null)
			{
				text3 = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Attachment textView = alternateViews2.GetTextView();
				text3 = ((textView != null) ? textView.Body : null);
			}
			string text4 = text3;
			string body = message.Body;
			if (mode != DownloadMode.Html)
			{
				string html;
				if (string.IsNullOrEmpty(text2))
				{
					if (!string.IsNullOrEmpty(text4))
					{
						html = text4;
					}
					else
					{
						if (string.IsNullOrEmpty(body))
						{
							return null;
						}
						html = body;
					}
				}
				else
				{
					html = text2;
				}
				return FileManager.HtmlToPlainText(WebUtility.HtmlDecode(FileManager.HtmlToPlainText(html)));
			}
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			if (string.IsNullOrEmpty(text4))
			{
				return body;
			}
			return text4;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00018390 File Offset: 0x00016590
		private static string GetBody(Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage message, DownloadMode mode)
		{
			HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> alternateViews = message.AlternateViews;
			object obj;
			if (alternateViews == null)
			{
				obj = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment = alternateViews.FirstOrDefault((Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment v) => FileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_5<string>(473053949)));
				obj = ((attachment != null) ? attachment.Body : null);
			}
			string text = (string)obj;
			HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> alternateViews2 = message.AlternateViews;
			object obj2;
			if (alternateViews2 == null)
			{
				obj2 = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment2 = alternateViews2.FirstOrDefault((Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment v) => FileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_2<string>(-1988142280)));
				obj2 = ((attachment2 != null) ? attachment2.Body : null);
			}
			string text2 = (string)obj2;
			string body = message.Body;
			if (mode != DownloadMode.Html)
			{
				string html;
				if (string.IsNullOrEmpty(text))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						html = text2;
					}
					else
					{
						if (string.IsNullOrEmpty(body))
						{
							return null;
						}
						html = body;
					}
				}
				else
				{
					html = text;
				}
				return FileManager.HtmlToPlainText(WebUtility.HtmlDecode(FileManager.HtmlToPlainText(html)));
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			return body;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00018478 File Offset: 0x00016678
		private static string GetEazyWebBody(Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage message, DownloadMode mode)
		{
			HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> alternateViews = message.AlternateViews;
			object obj;
			if (alternateViews == null)
			{
				obj = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment = alternateViews.FirstOrDefault((Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment v) => FileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_2<string>(642151244)));
				obj = ((attachment != null) ? attachment.Body : null);
			}
			string text = (string)obj;
			HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> alternateViews2 = message.AlternateViews;
			object obj2;
			if (alternateViews2 == null)
			{
				obj2 = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment2 = alternateViews2.FirstOrDefault((Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment v) => FileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_5<string>(93151217)));
				obj2 = ((attachment2 != null) ? attachment2.Body : null);
			}
			string text2 = (string)obj2;
			string result;
			if (mode != DownloadMode.Html)
			{
				if (text2 == null)
				{
					result = FileManager.HtmlToPlainText(WebUtility.HtmlDecode(FileManager.HtmlToPlainText(text)));
				}
				else
				{
					result = text2;
				}
			}
			else if (text == null)
			{
				result = text2;
			}
			else
			{
				result = WebUtility.HtmlDecode(text);
			}
			return result;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00018540 File Offset: 0x00016740
		private static string GetWebBody(Hackus_Mail_Checker_Reforged.Net.Mail.Message.MailMessage message, DownloadMode mode)
		{
			HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> alternateViews = message.AlternateViews;
			object obj;
			if (alternateViews == null)
			{
				obj = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment = alternateViews.FirstOrDefault((Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment v) => FileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_4<string>(1173908513)));
				obj = ((attachment != null) ? attachment.Body : null);
			}
			string text = (string)obj;
			HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> alternateViews2 = message.AlternateViews;
			object obj2;
			if (alternateViews2 == null)
			{
				obj2 = null;
			}
			else
			{
				Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment2 = alternateViews2.FirstOrDefault((Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment v) => FileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_6<string>(955020623)));
				obj2 = ((attachment2 != null) ? attachment2.Body : null);
			}
			string text2 = (string)obj2;
			string result;
			if (mode == DownloadMode.Html)
			{
				if (text != null)
				{
					result = WebUtility.HtmlDecode(FileManager.HtmlToPlainText(text));
				}
				else
				{
					result = text2;
				}
			}
			else if (text2 == null)
			{
				result = FileManager.HtmlToPlainText(WebUtility.HtmlDecode(FileManager.HtmlToPlainText(text)));
			}
			else
			{
				result = text2;
			}
			return result;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001860C File Offset: 0x0001680C
		private static string GetHtml(string login, string password, string from, string body, string subject, DateTime date, string folder = "INBOX")
		{
			string str = string.Concat(Enumerable.Repeat<string>(<Module>.smethod_4<string>(-41547654), 50));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(<Module>.smethod_3<string>(-67820023));
			stringBuilder.AppendLine(str + <Module>.smethod_3<string>(-1191592591));
			stringBuilder.AppendLine(string.Format(<Module>.smethod_2<string>(2105538664), new object[]
			{
				login,
				password,
				from.Replace(<Module>.smethod_5<string>(240979461), <Module>.smethod_5<string>(-328874637)).Replace(<Module>.smethod_2<string>(-1656297168), <Module>.smethod_5<string>(-708777369)),
				subject,
				date,
				folder
			}));
			stringBuilder.AppendLine(str + <Module>.smethod_2<string>(857033335));
			stringBuilder.AppendLine(<Module>.smethod_3<string>(896239652));
			stringBuilder.AppendLine(body + <Module>.smethod_5<string>(773876498));
			return stringBuilder.ToString();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00018714 File Offset: 0x00016914
		private static string GetPlainText(string login, string password, string from, string body, string subject, DateTime date, string folder = "INBOX")
		{
			string str = string.Concat(Enumerable.Repeat<string>(<Module>.smethod_4<string>(-41547654), 50));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(str + <Module>.smethod_5<string>(-1278631467));
			stringBuilder.AppendLine(<Module>.smethod_5<string>(-1848485565) + login + <Module>.smethod_6<string>(1827648947) + password);
			stringBuilder.AppendLine(<Module>.smethod_2<string>(1988575643) + from);
			stringBuilder.AppendLine(<Module>.smethod_4<string>(403877729) + subject);
			stringBuilder.AppendLine(string.Format(<Module>.smethod_4<string>(-385386331), date));
			stringBuilder.AppendLine(<Module>.smethod_5<string>(768710437) + folder);
			stringBuilder.AppendLine(str + <Module>.smethod_3<string>(-227532916));
			stringBuilder.AppendLine(body + <Module>.smethod_3<string>(-227532916));
			return stringBuilder.ToString();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00018810 File Offset: 0x00016A10
		private static string HtmlToPlainText(string html)
		{
			return Regex.Replace(Regex.Replace(Regex.Unescape(Regex.Replace(Regex.Replace(Regex.Replace(html, <Module>.smethod_5<string>(1948157853), string.Empty, RegexOptions.Compiled), <Module>.smethod_2<string>(-97739575), string.Empty, RegexOptions.Compiled), <Module>.smethod_5<string>(238595559), string.Empty, RegexOptions.Compiled)), <Module>.smethod_3<string>(-1709147778), <Module>.smethod_2<string>(1849819774)), <Module>.smethod_5<string>(-331258539), <Module>.smethod_4<string>(-2071533490)).Replace(<Module>.smethod_3<string>(-1106851411), string.Empty);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00008D5D File Offset: 0x00006F5D
		private static string RemoveEmptyLines(string lines)
		{
			return Regex.Replace(lines, <Module>.smethod_2<string>(-775000747), string.Empty).TrimEnd(Array.Empty<char>());
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000188A8 File Offset: 0x00016AA8
		private static string GetSafeFilename(string s)
		{
			return s.Replace(<Module>.smethod_5<string>(-2043602992), <Module>.smethod_4<string>(1105980040)).Replace(<Module>.smethod_6<string>(-1706108974), "").Replace(<Module>.smethod_4<string>(316715980), "").Replace(<Module>.smethod_6<string>(1827648947), "").Replace(<Module>.smethod_4<string>(-1436135638), "").Replace(<Module>.smethod_5<string>(240979461), "").Replace(<Module>.smethod_4<string>(568584589), "").Replace(<Module>.smethod_6<string>(1403443280), "").Replace(<Module>.smethod_2<string>(1588727045), "");
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00018970 File Offset: 0x00016B70
		public static void RemoveFromConfiguration(Server server)
		{
			object configLocker = FileManager._configLocker;
			lock (configLocker)
			{
				try
				{
					string oldValue = string.Format(<Module>.smethod_5<string>(541802010), new object[]
					{
						server.Domain,
						server.Hostname,
						server.Protocol.ToString().ToUpper(),
						server.Port,
						(server.Socket == SocketType.SSL) ? <Module>.smethod_4<string>(1274271205) : <Module>.smethod_5<string>(884747681)
					});
					File.WriteAllText(FileManager.ConfigurationPath, File.ReadAllText(FileManager.ConfigurationPath).Replace(oldValue, ""));
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00018A4C File Offset: 0x00016C4C
		public static void AddToConfiguration(Server server)
		{
			object configLocker = FileManager._configLocker;
			lock (configLocker)
			{
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(FileManager.ConfigurationPath, true))
					{
						streamWriter.WriteLine(string.Format(<Module>.smethod_3<string>(1017469901), new object[]
						{
							server.Domain,
							server.Hostname,
							server.Protocol.ToString().ToUpper(),
							server.Port,
							(server.Socket == SocketType.SSL) ? <Module>.smethod_5<string>(-415504783) : <Module>.smethod_4<string>(752524725)
						}));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00018B30 File Offset: 0x00016D30
		public static void EditConfiguration(Server original, Server replace)
		{
			object configLocker = FileManager._configLocker;
			lock (configLocker)
			{
				try
				{
					string oldValue = string.Format(<Module>.smethod_6<string>(2145625020), new object[]
					{
						original.Domain,
						original.Hostname,
						original.Protocol.ToString().ToUpper(),
						original.Port,
						(original.Socket == SocketType.SSL) ? <Module>.smethod_4<string>(1274271205) : <Module>.smethod_5<string>(884747681)
					});
					string newValue = string.Format(<Module>.smethod_5<string>(541802010), new object[]
					{
						replace.Domain,
						replace.Hostname,
						replace.Protocol.ToString().ToUpper(),
						replace.Port,
						(replace.Socket == SocketType.SSL) ? <Module>.smethod_2<string>(816196536) : <Module>.smethod_3<string>(-65892584)
					});
					File.WriteAllText(FileManager.ConfigurationPath, File.ReadAllText(FileManager.ConfigurationPath).Replace(oldValue, newValue));
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00018C94 File Offset: 0x00016E94
		public static void SaveAttachments(ICollection<Hackus_Mail_Checker_Reforged.Net.Mail.Attachment> attachments, string mailAddress)
		{
			if (attachments == null)
			{
				return;
			}
			foreach (Hackus_Mail_Checker_Reforged.Net.Mail.Attachment attachment in attachments)
			{
				if (attachment.IsAttachment)
				{
					FileManager.SaveAttachment(attachment, mailAddress);
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00018CEC File Offset: 0x00016EEC
		public static void SaveAttachments(HashSet<Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment> attachments, string mailAddress)
		{
			if (attachments == null)
			{
				return;
			}
			foreach (Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment in attachments)
			{
				FileManager.SaveAttachment(attachment, mailAddress);
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00018D40 File Offset: 0x00016F40
		private static void SaveAttachment(Hackus_Mail_Checker_Reforged.Net.Mail.Attachment attachment, string mailAddress)
		{
			string text = Path.Combine(FileManager.ResultsPath, <Module>.smethod_6<string>(-223475279), mailAddress);
			string path = Path.Combine(text, attachment.Filename);
			try
			{
				if (!SearchSettings.Instance.UseAttachmentFilters || SearchSettings.Instance.AttachmentFilters.Any((string filter) => attachment.Filename.ContainsIgnoreCase(filter)))
				{
					Directory.CreateDirectory(text);
					attachment.Save(FileManager.MakeUnique(path).FullName);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00018DE0 File Offset: 0x00016FE0
		public static void SaveAttachment(Hackus_Mail_Checker_Reforged.Net.Mail.Message.Attachment attachment, string mailAddress)
		{
			string text = Path.Combine(FileManager.ResultsPath, <Module>.smethod_4<string>(1716112283), mailAddress);
			string name = (!string.IsNullOrWhiteSpace(attachment.Name)) ? attachment.Name : <Module>.smethod_2<string>(740070314);
			string path = Path.Combine(text, name);
			try
			{
				if (!SearchSettings.Instance.UseAttachmentFilters || SearchSettings.Instance.AttachmentFilters.Any((string filter) => name.ContainsIgnoreCase(filter)))
				{
					Directory.CreateDirectory(text);
					byte[] array = (byte[])attachment.Body;
					if (array != null && array.Length != 0)
					{
						using (FileStream fileStream = new FileStream(FileManager.MakeUnique(path).FullName, FileMode.Create))
						{
							fileStream.Write(array, 0, array.Length);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00018ED4 File Offset: 0x000170D4
		public static FileInfo MakeUnique(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);
			int num = 1;
			while (File.Exists(path))
			{
				path = Path.Combine(directoryName, fileNameWithoutExtension + <Module>.smethod_3<string>(2023933234) + num.ToString() + extension);
				num++;
			}
			return new FileInfo(path);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00018F2C File Offset: 0x0001712C
		public static void LogUnhandledException(Exception exception, string source)
		{
			try
			{
				string value = <Module>.smethod_6<string>(2015891145) + source + <Module>.smethod_6<string>(90024148);
				using (StreamWriter streamWriter = new StreamWriter(<Module>.smethod_4<string>(-1427743014), true))
				{
					streamWriter.WriteLine(value);
					streamWriter.WriteLine(<Module>.smethod_4<string>(-1059862742) + exception.Message);
					streamWriter.WriteLine(<Module>.smethod_2<string>(-796793595) + exception.StackTrace);
					streamWriter.WriteLine(<Module>.smethod_3<string>(813558183) + exception.Source);
					streamWriter.WriteLine(<Module>.smethod_4<string>(-491781356));
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00018FFC File Offset: 0x000171FC
		public static void LogExtendedUnhandledException(Exception exception, string source)
		{
			try
			{
				string value = <Module>.smethod_5<string>(2085653975) + source + <Module>.smethod_4<string>(325108604);
				using (StreamWriter streamWriter = new StreamWriter(<Module>.smethod_4<string>(-1427743014), true))
				{
					streamWriter.WriteLine(value);
					streamWriter.WriteLine(<Module>.smethod_5<string>(254888376) + exception.Message);
					streamWriter.WriteLine(<Module>.smethod_4<string>(-1849126802) + exception.StackTrace);
					streamWriter.WriteLine(<Module>.smethod_3<string>(813558183) + exception.Source);
					streamWriter.WriteLine(<Module>.smethod_6<string>(394161941));
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400021B RID: 539
		private static object _statisticsLocker = new object();

		// Token: 0x0400021C RID: 540
		private static object _foundLocker = new object();

		// Token: 0x0400021D RID: 541
		private static object _letterLocker = new object();

		// Token: 0x0400021E RID: 542
		private static object _restLocker = new object();

		// Token: 0x0400021F RID: 543
		private static object _configLocker = new object();
	}
}
