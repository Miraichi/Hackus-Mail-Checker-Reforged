using System;
using System.IO;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000E0 RID: 224
	public class Attachment : ObjectHeaders
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x0000A1BD File Offset: 0x000083BD
		public Attachment()
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0000A1C5 File Offset: 0x000083C5
		public Attachment(byte[] data, string contentType, string name = null, bool isAttachment = false) : this(contentType, name, isAttachment)
		{
			base.SetBody(data);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0000A1D8 File Offset: 0x000083D8
		public Attachment(string data, string contentType, string name = null, bool isAttachment = false) : this(contentType, name, isAttachment)
		{
			base.SetBody(data);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002BCD4 File Offset: 0x00029ED4
		private Attachment(string contentType, string name, bool isAttachment)
		{
			this.Headers.Add(<Module>.smethod_6<string>(-1106786680), contentType);
			if (!string.IsNullOrEmpty(name))
			{
				Header value = new Header(isAttachment ? <Module>.smethod_5<string>(-714738064) : <Module>.smethod_2<string>(949454611));
				this.Headers.Add(<Module>.smethod_5<string>(-1253199419), value);
				value[isAttachment ? <Module>.smethod_4<string>(-1650761709) : <Module>.smethod_5<string>(1290480718)] = name;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0002BD5C File Offset: 0x00029F5C
		public virtual string Filename
		{
			get
			{
				return this.Headers[<Module>.smethod_5<string>(-1253199419)][<Module>.smethod_4<string>(-1650761709)].NotEmpty(new string[]
				{
					this.Headers[<Module>.smethod_4<string>(450736905)][<Module>.smethod_4<string>(-1760129032)],
					this.Headers[<Module>.smethod_2<string>(-715219161)][<Module>.smethod_2<string>(400003302)],
					this.Headers[<Module>.smethod_4<string>(-425688904)][<Module>.smethod_3<string>(1948829249)]
				});
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0002BE1C File Offset: 0x0002A01C
		private string ContentDisposition
		{
			get
			{
				string result;
				if ((result = this._ContentDisposition) == null)
				{
					result = (this._ContentDisposition = this.Headers[<Module>.smethod_3<string>(-1151248644)].Value.ToLower());
				}
				return result;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0000A1EB File Offset: 0x000083EB
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0000A1F3 File Offset: 0x000083F3
		public virtual bool OnServer { get; internal set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0000A1FC File Offset: 0x000083FC
		internal bool IsAttachment
		{
			get
			{
				return this.ContentDisposition == <Module>.smethod_2<string>(-2047998239) || !string.IsNullOrEmpty(this.Filename);
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002BE60 File Offset: 0x0002A060
		public virtual void Save(string filename)
		{
			using (FileStream fileStream = new FileStream(filename, FileMode.Create))
			{
				this.Save(fileStream);
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002BE98 File Offset: 0x0002A098
		public virtual void Save(Stream stream)
		{
			byte[] data = this.GetData();
			stream.Write(data, 0, data.Length);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002BEB8 File Offset: 0x0002A0B8
		public virtual byte[] GetData()
		{
			string body = this.Body;
			if (this.ContentTransferEncoding.Is(<Module>.smethod_2<string>(1648508631)) && Utils.IsValidBase64String(ref body, false))
			{
				try
				{
					return Convert.FromBase64String(body);
				}
				catch (Exception)
				{
					return this.Encoding.GetBytes(body);
				}
			}
			return this.Encoding.GetBytes(body);
		}

		// Token: 0x04000378 RID: 888
		private string _ContentDisposition;
	}
}
