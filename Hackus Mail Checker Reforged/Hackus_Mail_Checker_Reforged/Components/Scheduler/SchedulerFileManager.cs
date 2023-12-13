using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler
{
	// Token: 0x020001D2 RID: 466
	public static class SchedulerFileManager
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x0000DF70 File Offset: 0x0000C170
		// (set) Token: 0x06000DAA RID: 3498 RVA: 0x0000DF77 File Offset: 0x0000C177
		public static string ResultsPath { get; set; } = Path.Combine(<Module>.smethod_4<string>(732679442), <Module>.smethod_4<string>(1954867686));

		// Token: 0x06000DAB RID: 3499 RVA: 0x00046128 File Offset: 0x00044328
		private static string GetBody(MailMessage message, DownloadMode mode)
		{
			HashSet<Attachment> alternateViews = message.AlternateViews;
			object obj;
			if (alternateViews == null)
			{
				obj = null;
			}
			else
			{
				Attachment attachment = alternateViews.FirstOrDefault((Attachment v) => SchedulerFileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_6<string>(-2006787197)));
				obj = ((attachment != null) ? attachment.Body : null);
			}
			string text = (string)obj;
			HashSet<Attachment> alternateViews2 = message.AlternateViews;
			object obj2;
			if (alternateViews2 == null)
			{
				obj2 = null;
			}
			else
			{
				Attachment attachment2 = alternateViews2.FirstOrDefault((Attachment v) => SchedulerFileManager.<>c.smethod_0(v.ContentType, <Module>.smethod_6<string>(955020623)));
				obj2 = ((attachment2 != null) ? attachment2.Body : null);
			}
			string text2 = (string)obj2;
			string body = message.Body;
			if (mode != DownloadMode.Html)
			{
				string html;
				if (string.IsNullOrEmpty(text))
				{
					if (string.IsNullOrEmpty(text2))
					{
						if (string.IsNullOrEmpty(body))
						{
							return null;
						}
						html = body;
					}
					else
					{
						html = text2;
					}
				}
				else
				{
					html = text;
				}
				return SchedulerFileManager.HtmlToPlainText(WebUtility.HtmlDecode(SchedulerFileManager.HtmlToPlainText(html)));
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (string.IsNullOrEmpty(text2))
			{
				return body;
			}
			return text2;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00046210 File Offset: 0x00044410
		public static void SaveLetter(string login, string password, string request, MailMessage message)
		{
			string body = SchedulerFileManager.GetBody(message, SearchSettings.Instance.DownloadMode);
			if (body == null)
			{
				return;
			}
			string body2 = body;
			string subject = message.Subject;
			DateTime date = message.Date;
			Folder folder = message.Folder;
			string folder2;
			if (folder != null)
			{
				if ((folder2 = folder.Name) != null)
				{
					goto IL_45;
				}
			}
			folder2 = <Module>.smethod_3<string>(-762368918);
			IL_45:
			SchedulerFileManager.SaveLetter(login, password, request, body2, subject, date, folder2);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00046268 File Offset: 0x00044468
		public static void SaveLetter(string login, string password, string request, string body, string subject, DateTime date, string folder = "INBOX")
		{
			string text;
			string str;
			if (SearchSettings.Instance.DownloadMode == DownloadMode.Html)
			{
				text = SchedulerFileManager.GetHtml(login, password, body, subject, date, folder);
				str = <Module>.smethod_2<string>(1539767510);
			}
			else
			{
				text = SchedulerFileManager.GetPlainText(login, password, body, subject, date, folder);
				str = <Module>.smethod_2<string>(829792275);
			}
			string path = Path.Combine(<Module>.smethod_4<string>(732679442), <Module>.smethod_2<string>(-1743195859), SchedulerFileManager.GetSafeFilename(login) + str);
			if (text != null)
			{
				object letterLocker = SchedulerFileManager._letterLocker;
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

		// Token: 0x06000DAE RID: 3502 RVA: 0x0004635C File Offset: 0x0004455C
		private static string GetHtml(string login, string password, string body, string subject, DateTime date, string folder = "INBOX")
		{
			string str = string.Concat(Enumerable.Repeat<string>(<Module>.smethod_4<string>(-41547654), 50));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(<Module>.smethod_6<string>(221487808));
			stringBuilder.AppendLine(str + <Module>.smethod_3<string>(-1191592591));
			stringBuilder.AppendLine(string.Format(<Module>.smethod_2<string>(-78522087), new object[]
			{
				login,
				password,
				subject,
				date,
				folder
			}));
			stringBuilder.AppendLine(str + <Module>.smethod_2<string>(857033335));
			stringBuilder.AppendLine(<Module>.smethod_3<string>(896239652));
			stringBuilder.AppendLine(body + <Module>.smethod_6<string>(372691812));
			return stringBuilder.ToString();
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0004642C File Offset: 0x0004462C
		private static string GetPlainText(string login, string password, string body, string subject, DateTime date, string folder = "INBOX")
		{
			string str = string.Concat(Enumerable.Repeat<string>(<Module>.smethod_6<string>(73743374), 50));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(str + <Module>.smethod_4<string>(219937593));
			stringBuilder.AppendLine(login + <Module>.smethod_6<string>(1827648947) + password);
			stringBuilder.AppendLine(subject ?? "");
			stringBuilder.AppendLine(string.Format(<Module>.smethod_5<string>(603398365), date));
			stringBuilder.AppendLine(folder ?? "");
			stringBuilder.AppendLine(str + <Module>.smethod_5<string>(-1278631467));
			stringBuilder.AppendLine(body + <Module>.smethod_4<string>(219937593));
			return stringBuilder.ToString();
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000464F8 File Offset: 0x000446F8
		private static string HtmlToPlainText(string html)
		{
			return Regex.Replace(Regex.Replace(Regex.Unescape(Regex.Replace(Regex.Replace(Regex.Replace(html, <Module>.smethod_5<string>(1948157853), string.Empty, RegexOptions.Compiled), <Module>.smethod_3<string>(1983523151), string.Empty, RegexOptions.Compiled), <Module>.smethod_2<string>(-1479527770), string.Empty, RegexOptions.Compiled)), <Module>.smethod_4<string>(383420439), <Module>.smethod_4<string>(-405843621)), <Module>.smethod_2<string>(-1612810636), <Module>.smethod_4<string>(-2071533490)).Replace(<Module>.smethod_2<string>(1333008155), string.Empty);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00046590 File Offset: 0x00044790
		private static string GetSafeFilename(string s)
		{
			return s.Replace(<Module>.smethod_3<string>(-1310895401), <Module>.smethod_5<string>(1681510206)).Replace(<Module>.smethod_3<string>(-1872781685), "").Replace(<Module>.smethod_3<string>(2141242469), "").Replace(<Module>.smethod_2<string>(-1691660964), "").Replace(<Module>.smethod_6<string>(1255698846), "").Replace(<Module>.smethod_2<string>(973996356), "").Replace(<Module>.smethod_5<string>(-138923271), "").Replace(<Module>.smethod_6<string>(1403443280), "").Replace(<Module>.smethod_6<string>(-77460630), "");
		}

		// Token: 0x04000752 RID: 1874
		private static object _letterLocker = new object();
	}
}
