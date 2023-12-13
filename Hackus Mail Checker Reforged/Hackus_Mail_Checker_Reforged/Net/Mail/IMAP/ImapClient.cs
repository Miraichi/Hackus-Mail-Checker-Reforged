using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.IMAP
{
	// Token: 0x0200012B RID: 299
	public class ImapClient : MailClient
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0000B9F5 File Offset: 0x00009BF5
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x0000B9FD File Offset: 0x00009BFD
		private int _commandNumber { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0000BA06 File Offset: 0x00009C06
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x0000BA0E File Offset: 0x00009C0E
		private string _tag { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0000BA17 File Offset: 0x00009C17
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x0000BA39 File Offset: 0x00009C39
		private string Tag
		{
			get
			{
				return string.Format(<Module>.smethod_4<string>(-1440244213), this._tag, this._commandNumber);
			}
			set
			{
				this._tag = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0000BA42 File Offset: 0x00009C42
		private string NewTag
		{
			get
			{
				this._commandNumber++;
				return string.Format(<Module>.smethod_4<string>(-1440244213), this._tag, this._commandNumber);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0000BA72 File Offset: 0x00009C72
		// (set) Token: 0x0600094D RID: 2381 RVA: 0x0000BA7A File Offset: 0x00009C7A
		public Folder SelectedFolder { get; private set; }

		// Token: 0x0600094E RID: 2382 RVA: 0x00038B40 File Offset: 0x00036D40
		public override void Authenticate(string username, string password)
		{
			string[] source = this.SendReceive(<Module>.smethod_6<string>(1401409128) + username.ToQuotedString() + <Module>.smethod_5<string>(-521209905) + password.QuoteString());
			this.CheckOk(source.Last<string>());
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00038B88 File Offset: 0x00036D88
		public int SelectFolder(Folder folder)
		{
			string[] array = this.SendReceive(<Module>.smethod_4<string>(2065459023) + folder.Name + <Module>.smethod_4<string>(2069567598));
			this.CheckOk(array.Last<string>());
			if (array.Last<string>().IsServerDisabled())
			{
				throw new AlertException();
			}
			for (int i = 0; i < array.Length; i++)
			{
				Match match = Regex.Match(array[i], <Module>.smethod_2<string>(-595532034));
				if (match.Success)
				{
					folder.MessageCount = int.Parse(match.Value);
					IL_81:
					this.SelectedFolder = folder;
					return folder.MessageCount;
				}
			}
			goto IL_81;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00038C24 File Offset: 0x00036E24
		public int GetMessagesCount(Folder folder)
		{
			string[] array = this.SendReceive(<Module>.smethod_6<string>(-1708143126) + folder.Name.ToQuotedString() + <Module>.smethod_2<string>(601289654));
			this.CheckOk(array.Last<string>());
			for (int i = 0; i < array.Length; i++)
			{
				Match match = Regex.Match(array[i], <Module>.smethod_5<string>(-335233589));
				if (match.Success)
				{
					folder.MessageCount = int.Parse(match.Groups[1].Value);
					return folder.MessageCount;
				}
			}
			return -1;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00038CB8 File Offset: 0x00036EB8
		public List<Folder> GetAllFolders()
		{
			List<Folder> list = new List<Folder>
			{
				Folder.Parse(<Module>.smethod_6<string>(2132499449))
			};
			string[] array = this.SendReceive(<Module>.smethod_4<string>(-123201340));
			this.CheckOk(array.Last<string>());
			if (array.Length == 0)
			{
				return list;
			}
			list.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].StartsWith(<Module>.smethod_2<string>(-606254921)))
				{
					Match match;
					if ((match = Regex.Match(array[i], <Module>.smethod_2<string>(642250408))).Success)
					{
						list.Add(Folder.Parse(match.Groups[1].Value.Replace(<Module>.smethod_2<string>(1588727045), "")));
					}
					else if ((match = Regex.Match(array[i], <Module>.smethod_6<string>(1568789931))).Success)
					{
						list.Add(Folder.Parse(match.Groups[1].Value.Replace(<Module>.smethod_3<string>(1298413043), "")));
					}
				}
			}
			return list;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00038DD0 File Offset: 0x00036FD0
		public Uid[] Search(SearchCondition criteria)
		{
			HashSet<Uid> hashSet = new HashSet<Uid>();
			string text = criteria.ToString();
			string line = new StringReader(text).ReadLine();
			this.LocalStream.WriteLine(this.NewTag + <Module>.smethod_5<string>(547764585) + (text.Contains(Environment.NewLine) ? <Module>.smethod_6<string>(977812195) : string.Empty) + text, this.ReadWriteTimeout);
			string text2;
			while ((text2 = base.ReadLine()) != null)
			{
				if (text2.StartsWith(<Module>.smethod_3<string>(2141242469)))
				{
					while (text2.StartsWith(<Module>.smethod_2<string>(-908283613)))
					{
						Match match = Regex.Match(text2, <Module>.smethod_3<string>(540126389), RegexOptions.Multiline);
						if (match.Success)
						{
							string[] source = match.Groups[1].Value.Trim().Split(new char[]
							{
								' '
							});
							hashSet.UnionWith(from x in source
							select new Uid(x, this.SelectedFolder));
						}
						text2 = base.ReadLine();
					}
					this.CheckOk(text2);
					return hashSet.ToArray<Uid>();
				}
				if (!text2.StartsWith(<Module>.smethod_4<string>(387616751)))
				{
					throw new MailException();
				}
				this.LocalStream.WriteLine(line, this.ReadWriteTimeout);
				text2 = base.ReadLine();
			}
			return new Uid[0];
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00038F1C File Offset: 0x0003711C
		public MailMessage FetchFrom(Uid uid)
		{
			MailMessage mailMessage = null;
			string text = string.Empty;
			this.Send(<Module>.smethod_4<string>(1579031323) + uid.UID.ToString() + <Module>.smethod_4<string>(46313839));
			while ((text = base.ReadLine()).StartsWith(<Module>.smethod_4<string>(316715980)))
			{
				if (Regex.Match(text, <Module>.smethod_6<string>(89615806)).Success)
				{
					text = this.LocalStream.ReadPartAsString(base.Socket, 1048576, this.ReadWriteTimeout);
					int length;
					if ((length = text.IndexOf(Environment.NewLine + this.Tag + <Module>.smethod_4<string>(-1145100733))) > 0)
					{
						mailMessage = MessageBuilder.FromHeader(text.Substring(0, length));
						mailMessage.Uid = uid;
						text = text.Substring(text.IndexOf(this.Tag + <Module>.smethod_5<string>(-950387465)));
						if (!text.EndsWith(<Module>.smethod_4<string>(-2071533490)))
						{
							text += base.ReadLine();
						}
					}
					else
					{
						for (;;)
						{
							if (text.Contains(Environment.NewLine + this.Tag + <Module>.smethod_5<string>(-950387465)))
							{
								if (text.EndsWith(<Module>.smethod_5<string>(1531298060)) || base.Socket.Available == 0)
								{
									break;
								}
							}
							text += base.ReadLine();
						}
						if ((length = text.IndexOf(Environment.NewLine + this.Tag + <Module>.smethod_3<string>(-1917178918))) <= 0)
						{
							throw new IOException(<Module>.smethod_3<string>(2010242753));
						}
						mailMessage = MessageBuilder.FromHeader(text.Substring(0, length));
						mailMessage.Uid = uid;
						text = text.Substring(text.IndexOf(this.Tag + <Module>.smethod_2<string>(1776516822)));
					}
					IL_1D1:
					this.CheckOk(text);
					return mailMessage;
				}
			}
			goto IL_1D1;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00039104 File Offset: 0x00037304
		public MailMessage FetchMessage(Uid uid, bool onlyHeaders = true, bool skipAdditionalParts = true)
		{
			MailMessage mailMessage = null;
			string text = string.Empty;
			string text2 = string.Empty;
			this.Send(string.Concat(new string[]
			{
				<Module>.smethod_4<string>(1579031323),
				uid.UID.ToString(),
				<Module>.smethod_3<string>(2096845236),
				onlyHeaders ? <Module>.smethod_4<string>(1658500192) : <Module>.smethod_5<string>(532266402),
				<Module>.smethod_6<string>(90024148)
			}));
			while ((text = base.ReadLine()).StartsWith(<Module>.smethod_2<string>(-908283613)))
			{
				if (Regex.Match(text, <Module>.smethod_6<string>(89615806)).Success)
				{
					text = this.LocalStream.ReadMsgAsString(base.Socket, 1048576, this.ReadWriteTimeout);
					int num;
					if ((num = text.IndexOf(this.Tag + <Module>.smethod_5<string>(-950387465), 0, StringComparison.Ordinal)) <= 0)
					{
						for (;;)
						{
							if (text.Contains(Environment.NewLine + this.Tag + <Module>.smethod_3<string>(-1917178918)))
							{
								if (text.EndsWith(<Module>.smethod_4<string>(-2071533490)) || base.Socket.Available == 0)
								{
									break;
								}
							}
							text += base.ReadLine();
						}
						if ((num = text.IndexOf(<Module>.smethod_5<string>(-37587696) + uid.UID.ToString() + <Module>.smethod_2<string>(769961107), 0, StringComparison.Ordinal)) >= 0 || (num = text.IndexOf(<Module>.smethod_4<string>(325108604) + Environment.NewLine + this.Tag + <Module>.smethod_3<string>(-1917178918), 0, StringComparison.Ordinal)) >= 0)
						{
							if (num <= 0)
							{
								throw new IOException(<Module>.smethod_2<string>(-1762264601));
							}
							mailMessage = MessageBuilder.FromMime822(text.Substring(0, num), onlyHeaders, Encoding.UTF8, skipAdditionalParts);
							mailMessage.Uid = uid;
							mailMessage.Folder = uid.Folder;
							text2 = text.Substring(text.IndexOf(this.Tag + <Module>.smethod_6<string>(-626619159), 0, StringComparison.Ordinal));
						}
					}
					else
					{
						text2 = text.Substring(num);
						if ((num = text.IndexOf(<Module>.smethod_5<string>(-37587696) + uid.UID.ToString() + <Module>.smethod_2<string>(769961107), 0, StringComparison.Ordinal)) >= 0 || (num = text.IndexOf(<Module>.smethod_3<string>(-829763147) + Environment.NewLine + this.Tag + <Module>.smethod_2<string>(1776516822), 0, StringComparison.Ordinal)) >= 0)
						{
							mailMessage = MessageBuilder.FromMime822(text.Substring(0, num), onlyHeaders, Encoding.UTF8, skipAdditionalParts);
							mailMessage.Uid = uid;
							mailMessage.Folder = uid.Folder;
							if (!text2.EndsWith(<Module>.smethod_5<string>(1531298060)))
							{
								text2 += base.ReadLine();
							}
						}
					}
					IL_2CC:
					this.CheckOk(string.IsNullOrEmpty(text2) ? text : text2);
					return mailMessage;
				}
			}
			goto IL_2CC;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000393F0 File Offset: 0x000375F0
		public MailMessage FetchSubject(Uid uid)
		{
			MailMessage mailMessage = null;
			string text = string.Empty;
			this.Send(<Module>.smethod_5<string>(2060220047) + uid.UID.ToString() + <Module>.smethod_4<string>(2099117256));
			while ((text = base.ReadLine()).StartsWith(<Module>.smethod_3<string>(2141242469)))
			{
				if (Regex.Match(text, <Module>.smethod_4<string>(-1178758966)).Success)
				{
					text = this.LocalStream.ReadPartAsString(base.Socket, 1048576, this.ReadWriteTimeout);
					int length;
					if ((length = text.IndexOf(Environment.NewLine + this.Tag)) > 0)
					{
						mailMessage = MessageBuilder.FromHeader(text.Substring(0, length));
						mailMessage.Uid = uid;
						text = text.Substring(text.IndexOf(this.Tag + <Module>.smethod_4<string>(-1145100733)));
						if (!text.EndsWith(<Module>.smethod_5<string>(1531298060)))
						{
							text += base.ReadLine();
						}
					}
					else
					{
						for (;;)
						{
							if (text.Contains(Environment.NewLine + this.Tag + <Module>.smethod_3<string>(-1917178918)))
							{
								if (text.EndsWith(<Module>.smethod_5<string>(1531298060)))
								{
									break;
								}
								if (base.Socket.Available == 0)
								{
									break;
								}
							}
							text += base.ReadLine();
						}
						if ((length = text.IndexOf(Environment.NewLine + this.Tag)) <= 0)
						{
							throw new IOException(<Module>.smethod_6<string>(-1996104120));
						}
						mailMessage = MessageBuilder.FromHeader(text.Substring(0, length));
						mailMessage.Uid = uid;
						text = text.Substring(text.IndexOf(this.Tag + <Module>.smethod_6<string>(-626619159)));
					}
					IL_1BD:
					this.CheckOk(text);
					return mailMessage;
				}
			}
			goto IL_1BD;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000395C4 File Offset: 0x000377C4
		public AttachmentMessageInfo[] FetchAttachmentMessagesInfo(string messageSet)
		{
			string[] array = this.SendReceive(<Module>.smethod_3<string>(1652400459) + messageSet + <Module>.smethod_3<string>(-916497902));
			this.CheckOk(array.Last<string>());
			List<AttachmentMessageInfo> list = new List<AttachmentMessageInfo>();
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					AttachmentMessageInfo attachmentMessageInfo = new AttachmentMessageInfo();
					Match match = Regex.Match(array[i], <Module>.smethod_2<string>(-1961000384));
					if (match.Success)
					{
						attachmentMessageInfo.Uid = match.Groups[1].Value;
						foreach (object obj in Regex.Matches(array[i], <Module>.smethod_4<string>(-1195719688)))
						{
							Match match2 = (Match)obj;
							if (match2.Success)
							{
								if (attachmentMessageInfo.Filenames == null)
								{
									attachmentMessageInfo.Filenames = new List<string>();
								}
								attachmentMessageInfo.Filenames.Add(match2.Groups[1].Value);
							}
						}
						if (attachmentMessageInfo.HasAttachments)
						{
							list.Add(attachmentMessageInfo);
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0003970C File Offset: 0x0003790C
		public string[] FetchFromHeaders(string messageSet)
		{
			List<string> list = new List<string>();
			this.Send(<Module>.smethod_3<string>(1652400459) + messageSet + <Module>.smethod_5<string>(-1161797709));
			string text = string.Empty;
			while ((text = base.ReadLine()).StartsWith(<Module>.smethod_5<string>(1301607474)))
			{
				if (Regex.Match(text, <Module>.smethod_3<string>(-1988361889)).Success)
				{
					string text2 = string.Empty;
					while (text != <Module>.smethod_4<string>(325108604))
					{
						text = base.ReadLine();
						text2 += text;
					}
					Match match = Regex.Match(text2, <Module>.smethod_4<string>(1130721379));
					if (match.Success)
					{
						list.Add(match.Groups[1].Value);
					}
				}
			}
			this.CheckOk(text);
			return list.ToArray();
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000397E0 File Offset: 0x000379E0
		public void DeleteMessages(Uid[] uids)
		{
			string str = string.Join(<Module>.smethod_2<string>(438041622), from u in uids
			select u.UID.ToString());
			this.Send(<Module>.smethod_6<string>(-347307090) + str + <Module>.smethod_2<string>(-870096547));
			string text = base.ReadLine();
			while (text.StartsWith(<Module>.smethod_2<string>(-908283613)))
			{
				text = base.ReadLine();
			}
			this.CheckOk(text);
			this.Expunge();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00039874 File Offset: 0x00037A74
		public void Expunge()
		{
			this.Send(<Module>.smethod_5<string>(-957542931));
			string text = base.ReadLine();
			while (text.StartsWith(<Module>.smethod_3<string>(2141242469)))
			{
				text = base.ReadLine();
			}
			this.CheckOk(text);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000398BC File Offset: 0x00037ABC
		protected override void CheckOk(string response)
		{
			if (string.IsNullOrEmpty(response))
			{
				throw new ArgumentNullException();
			}
			if (response.IsBrokenEncoding())
			{
				throw new EncodingException();
			}
			if (!response.Substring(response.IndexOf(' ')).Trim().ToUpper().StartsWith(<Module>.smethod_2<string>(-1735172287)))
			{
				throw new MailException(response);
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0000BA83 File Offset: 0x00009C83
		private string[] SendReceive(string command)
		{
			this.LocalStream.WriteLine(this.NewTag + <Module>.smethod_3<string>(2023933234) + command, this.ReadWriteTimeout);
			return this.ReadResponse();
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000BAB2 File Offset: 0x00009CB2
		private void Send(string command)
		{
			this.LocalStream.WriteLine(this.NewTag + <Module>.smethod_2<string>(1849819774) + command, this.ReadWriteTimeout);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00039918 File Offset: 0x00037B18
		private string[] ReadResponse()
		{
			List<string> list = new List<string>();
			string text = base.ReadLine();
			list.Add(text);
			if (!text.IsContainsByeBye())
			{
				while (text.StartsWith(<Module>.smethod_2<string>(-908283613)))
				{
					text = base.ReadLine();
					list.Add(text);
				}
				return list.ToArray();
			}
			throw new MailException();
		}
	}
}
