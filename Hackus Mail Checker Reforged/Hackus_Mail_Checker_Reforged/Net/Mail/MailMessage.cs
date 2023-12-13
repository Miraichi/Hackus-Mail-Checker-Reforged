using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000EE RID: 238
	public class MailMessage : ObjectHeaders
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x0002D6D0 File Offset: 0x0002B8D0
		public static implicit operator MailMessage(MailMessage msg)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.Subject = msg.Subject;
			mailMessage.Sender = msg.Sender;
			mailMessage.Body = msg.Body;
			mailMessage.IsBodyHtml = msg.ContentType.Contains(<Module>.smethod_2<string>(1569658303));
			mailMessage.From = msg.From;
			foreach (Attachment attachment in msg.Attachments)
			{
				mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.GetData()), attachment.Filename, attachment.ContentType));
			}
			foreach (Attachment attachment2 in msg.AlternateViews)
			{
				mailMessage.AlternateViews.Add(new AlternateView(new MemoryStream(attachment2.GetData()), attachment2.ContentType));
			}
			return mailMessage;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0000A926 File Offset: 0x00008B26
		public MailMessage()
		{
			this.Attachments = new Collection<Attachment>();
			this.AlternateViews = new AlternateViewCollection();
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0000A944 File Offset: 0x00008B44
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0000A94C File Offset: 0x00008B4C
		public virtual DateTime Date { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0000A955 File Offset: 0x00008B55
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0000A95D File Offset: 0x00008B5D
		public virtual int Size { get; internal set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0000A966 File Offset: 0x00008B66
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0000A96E File Offset: 0x00008B6E
		public virtual string Subject { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0000A977 File Offset: 0x00008B77
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x0000A97F File Offset: 0x00008B7F
		public virtual string Folder { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0000A988 File Offset: 0x00008B88
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x0000A990 File Offset: 0x00008B90
		public virtual ICollection<Attachment> Attachments { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0000A999 File Offset: 0x00008B99
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0000A9A1 File Offset: 0x00008BA1
		public virtual AlternateViewCollection AlternateViews { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0000A9AA File Offset: 0x00008BAA
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0000A9B2 File Offset: 0x00008BB2
		public virtual MailAddress From { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0000A9BB File Offset: 0x00008BBB
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0000A9C3 File Offset: 0x00008BC3
		public virtual MailAddress Sender { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0000A9CC File Offset: 0x00008BCC
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		public virtual string MessageID { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0000A9DD File Offset: 0x00008BDD
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0000A9E5 File Offset: 0x00008BE5
		public virtual string Uid { get; internal set; }

		// Token: 0x0600075F RID: 1887 RVA: 0x0002D7E8 File Offset: 0x0002B9E8
		public virtual void Load(string message, bool headersOnly = false, bool saveAttachments = true)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(this._DefaultEncoding.GetBytes(message)))
			{
				this.Load(memoryStream, headersOnly, message.Length, null, saveAttachments);
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0002D844 File Offset: 0x0002BA44
		public virtual void Load(Stream reader, bool headersOnly = false, int maxLength = 0, char? termChar = null, bool saveAttachments = true)
		{
			this._HeadersOnly = headersOnly;
			this.Headers = null;
			this.Body = null;
			StringBuilder stringBuilder = new StringBuilder();
			string text;
			while ((text = reader.ReadLine(ref maxLength, this._DefaultEncoding, termChar, 10000)) != null)
			{
				if (text.Length == 0)
				{
					if (stringBuilder.Length != 0)
					{
						break;
					}
				}
				else
				{
					stringBuilder.AppendLine(text);
				}
			}
			this.RawHeaders = stringBuilder.ToString();
			if (!headersOnly)
			{
				string boundary = this.Headers.GetBoundary();
				if (!string.IsNullOrEmpty(boundary))
				{
					List<Attachment> list = new List<Attachment>();
					string text2 = MailMessage.ParseMime(reader, boundary, ref maxLength, list, this.Encoding, termChar);
					if (!string.IsNullOrEmpty(text2))
					{
						base.SetBody(text2);
					}
					foreach (Attachment attachment in list)
					{
						if (attachment.IsAttachment && saveAttachments)
						{
							this.Attachments.Add(attachment);
						}
						else
						{
							this.AlternateViews.Add(attachment);
						}
					}
					if (maxLength > 0)
					{
						reader.ReadToEnd(maxLength, this.Encoding);
					}
				}
				else
				{
					string body = string.Empty;
					if (maxLength > 0)
					{
						body = reader.ReadToEnd(maxLength, this.Encoding);
					}
					base.SetBody(body);
				}
			}
			else if (maxLength > 0)
			{
				reader.ReadToEnd(maxLength, this.Encoding);
			}
			if ((string.IsNullOrWhiteSpace(this.Body) || this.ContentType.StartsWith(<Module>.smethod_6<string>(-53548007))) && this.AlternateViews.Count > 0)
			{
				Attachment attachment2 = this.AlternateViews.GetTextView() ?? this.AlternateViews.GetHtmlView();
				if (attachment2 != null)
				{
					this.Body = attachment2.Body;
					this.ContentTransferEncoding = attachment2.Headers[<Module>.smethod_6<string>(-1386707483)].RawValue;
					this.ContentType = attachment2.Headers[<Module>.smethod_4<string>(-425688904)].RawValue;
				}
			}
			this.Date = this.Headers.GetDate();
			this.Sender = this.Headers.GetMailAddresses(<Module>.smethod_5<string>(201638498)).FirstOrDefault<MailAddress>();
			this.From = this.Headers.GetMailAddresses(<Module>.smethod_2<string>(1934118314)).FirstOrDefault<MailAddress>();
			this.MessageID = this.Headers[<Module>.smethod_4<string>(1791821373)].RawValue;
			this.Subject = this.Headers[<Module>.smethod_3<string>(-1305046948)].RawValue;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002DAEC File Offset: 0x0002BCEC
		private static string ParseMime(Stream reader, string boundary, ref int maxLength, ICollection<Attachment> attachments, Encoding encoding, char? termChar)
		{
			bool flag = maxLength > 0;
			string text = null;
			string text2 = <Module>.smethod_6<string>(-208211581) + boundary;
			string value = text2 + <Module>.smethod_4<string>(-503234015);
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			do
			{
				if (flag)
				{
					if (maxLength <= 0)
					{
						goto IL_1C8;
					}
				}
				if (text != null)
				{
					stringBuilder.Append(text);
				}
				text = reader.ReadLine(ref maxLength, encoding, termChar, 10000);
				num++;
			}
			while (text != null && !text.StartsWith(text2));
			while (text != null && !text.StartsWith(value) && (!flag || maxLength != 0))
			{
				text = reader.ReadLine(ref maxLength, encoding, termChar, 10000);
				if (text == null)
				{
					break;
				}
				Attachment attachment = new Attachment
				{
					Encoding = encoding
				};
				StringBuilder stringBuilder2 = new StringBuilder();
				while (!text.StartsWith(text2) && text != string.Empty && (!flag || maxLength != 0))
				{
					stringBuilder2.AppendLine(text);
					text = reader.ReadLine(ref maxLength, encoding, termChar, 10000);
					if (text == null)
					{
						break;
					}
				}
				attachment.RawHeaders = stringBuilder2.ToString();
				string boundary2 = attachment.Headers.GetBoundary();
				if (string.IsNullOrEmpty(boundary2))
				{
					text = reader.ReadLine(ref maxLength, attachment.Encoding, termChar, 10000);
					if (text == null)
					{
						break;
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					while (!text.StartsWith(text2) && (!flag || maxLength != 0))
					{
						stringBuilder3.AppendLine(text);
						text = reader.ReadLine(ref maxLength, attachment.Encoding, termChar, 10000);
					}
					attachment.SetBody(stringBuilder3.ToString());
					attachments.Add(attachment);
				}
				else
				{
					MailMessage.ParseMime(reader, boundary2, ref maxLength, attachments, encoding, termChar);
					while (!text.StartsWith(text2) && (!flag || maxLength != 0))
					{
						text = reader.ReadLine(ref maxLength, encoding, termChar, 10000);
					}
				}
			}
			return stringBuilder.ToString();
			IL_1C8:
			return stringBuilder.ToString();
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002DCC8 File Offset: 0x0002BEC8
		public virtual void Save(Stream stream, Encoding encoding = null)
		{
			StreamWriter streamWriter = new StreamWriter(stream, encoding ?? Encoding.Default);
			this.Save(streamWriter);
			streamWriter.Flush();
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002DCF4 File Offset: 0x0002BEF4
		public virtual void Save(TextWriter txt)
		{
			txt.WriteLine(<Module>.smethod_2<string>(-1626431166), ((this.Date == DateTime.MinValue) ? DateTime.Now : this.Date).GetRFC2060Date());
			if (this.Sender != null)
			{
				txt.WriteLine(<Module>.smethod_3<string>(530555330), this.Sender);
			}
			if (this.From != null)
			{
				txt.WriteLine(<Module>.smethod_4<string>(-1118174577), this.From);
			}
			if (!string.IsNullOrEmpty(this.MessageID))
			{
				txt.WriteLine(<Module>.smethod_5<string>(-1675225273), this.MessageID);
			}
			foreach (KeyValuePair<string, Header> keyValuePair in from x in this.Headers
			where !MailMessage.SpecialHeaders.Contains(x.Key, MailMessage.<>c.smethod_0())
			select x)
			{
				txt.WriteLine(<Module>.smethod_3<string>(851908555), keyValuePair.Key, keyValuePair.Value);
			}
			txt.WriteLine(<Module>.smethod_3<string>(-1716989806), this.Subject);
			string boundary = null;
			if (this.Attachments.Any<Attachment>() || this.AlternateViews.Any<Attachment>())
			{
				boundary = string.Format(<Module>.smethod_3<string>(2016091206), Guid.NewGuid());
				txt.WriteLine(<Module>.smethod_5<string>(-762425504), boundary);
			}
			txt.WriteLine();
			if (boundary != null)
			{
				txt.WriteLine(<Module>.smethod_3<string>(1373384756) + boundary);
				txt.WriteLine();
			}
			txt.WriteLine(this.Body);
			this.AlternateViews.Union(this.Attachments).ToList<Attachment>().ForEach(delegate(Attachment att)
			{
				MailMessage.<>c__DisplayClass48_0.smethod_1(txt, MailMessage.<>c__DisplayClass48_0.smethod_0(<Module>.smethod_3<string>(1373384756), boundary));
				MailMessage.<>c__DisplayClass48_0.smethod_1(txt, MailMessage.<>c__DisplayClass48_0.smethod_2(<Module>.smethod_3<string>(-227532916), from h in att.Headers
				select MailMessage.<>c.smethod_1(<Module>.smethod_4<string>(-1553983322), h.Key, h.Value)));
				MailMessage.<>c__DisplayClass48_0.smethod_3(txt);
				MailMessage.<>c__DisplayClass48_0.smethod_1(txt, att.Body);
			});
			if (boundary != null)
			{
				txt.WriteLine(<Module>.smethod_2<string>(1836199244) + boundary + <Module>.smethod_5<string>(34337021));
			}
		}

		// Token: 0x040003BB RID: 955
		private bool _HeadersOnly;

		// Token: 0x040003C6 RID: 966
		private static readonly string[] SpecialHeaders = <Module>.smethod_2<string>(1686596533).Split(new char[]
		{
			','
		});
	}
}
