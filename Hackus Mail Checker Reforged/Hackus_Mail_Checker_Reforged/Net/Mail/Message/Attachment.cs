using System;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x02000112 RID: 274
	public class Attachment
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0000B070 File Offset: 0x00009270
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0000B078 File Offset: 0x00009278
		public string ContentType { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0000B081 File Offset: 0x00009281
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x0000B089 File Offset: 0x00009289
		public object Body { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0000B092 File Offset: 0x00009292
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0000B09A File Offset: 0x0000929A
		public string Name { get; set; }

		// Token: 0x06000864 RID: 2148 RVA: 0x0000619C File Offset: 0x0000439C
		public Attachment()
		{
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000B0A3 File Offset: 0x000092A3
		public Attachment(string contentType, string body)
		{
			this.ContentType = contentType;
			this.Body = body;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0000B0B9 File Offset: 0x000092B9
		public Attachment(string contentType, string body, string name)
		{
			this.ContentType = contentType;
			this.Body = body;
			this.Name = name;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0000B0B9 File Offset: 0x000092B9
		public Attachment(string contentType, byte[] body, string name)
		{
			this.ContentType = contentType;
			this.Body = body;
			this.Name = name;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0000B0A3 File Offset: 0x000092A3
		public Attachment(string contentType, byte[] body)
		{
			this.ContentType = contentType;
			this.Body = body;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00033B54 File Offset: 0x00031D54
		public override bool Equals(object obj)
		{
			Attachment attachment = obj as Attachment;
			if (attachment != null)
			{
				if (attachment.Body == this.Body)
				{
					return attachment.Name == this.Name;
				}
			}
			return false;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000B0D6 File Offset: 0x000092D6
		public override int GetHashCode()
		{
			return this.Body.GetHashCode();
		}
	}
}
