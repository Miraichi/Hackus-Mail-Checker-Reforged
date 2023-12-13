using System;
using System.IO;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000F1 RID: 241
	public abstract class ObjectHeaders
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0000AA8E File Offset: 0x00008C8E
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0000AA96 File Offset: 0x00008C96
		public virtual string RawHeaders { get; internal set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0002DFEC File Offset: 0x0002C1EC
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x0000AA9F File Offset: 0x00008C9F
		public virtual HeaderDictionary Headers
		{
			get
			{
				HeaderDictionary result;
				if ((result = this._Headers) == null)
				{
					result = (this._Headers = HeaderDictionary.Parse(this.RawHeaders, this._DefaultEncoding));
				}
				return result;
			}
			internal set
			{
				this._Headers = value;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002E020 File Offset: 0x0002C220
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		public virtual string ContentTransferEncoding
		{
			get
			{
				return this.Headers[<Module>.smethod_6<string>(-1386707483)].Value ?? string.Empty;
			}
			set
			{
				this.Headers.Set(<Module>.smethod_6<string>(-1386707483), new Header(value));
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0002E054 File Offset: 0x0002C254
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x0000AAC5 File Offset: 0x00008CC5
		public virtual string ContentType
		{
			get
			{
				return this.Headers[<Module>.smethod_5<string>(1179211278)].Value.NotEmpty(new string[]
				{
					<Module>.smethod_5<string>(93151217)
				});
			}
			set
			{
				this.Headers.Set(<Module>.smethod_3<string>(1979536001), new Header(value));
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0002E098 File Offset: 0x0002C298
		public virtual string Charset
		{
			get
			{
				return this.Headers[<Module>.smethod_3<string>(-954980546)][<Module>.smethod_2<string>(1452670491)].NotEmpty(new string[]
				{
					this.Headers[<Module>.smethod_6<string>(-1106786680)][<Module>.smethod_4<string>(1869366484)]
				});
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0002E104 File Offset: 0x0002C304
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x0000AAE2 File Offset: 0x00008CE2
		public virtual Encoding Encoding
		{
			get
			{
				Encoding result;
				if ((result = this._Encoding) == null)
				{
					result = (this._Encoding = Utils.ParseCharsetToEncoding(this.Charset, this._DefaultEncoding));
				}
				return result;
			}
			set
			{
				this._DefaultEncoding = (value ?? this._DefaultEncoding);
				if (this._Encoding != null)
				{
					this._Encoding = (value ?? this._DefaultEncoding);
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0000AB0E File Offset: 0x00008D0E
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x0000AB16 File Offset: 0x00008D16
		public virtual string Body { get; set; }

		// Token: 0x0600077E RID: 1918 RVA: 0x0002E138 File Offset: 0x0002C338
		internal void SetBody(string value)
		{
			if (this.ContentTransferEncoding.Is(<Module>.smethod_4<string>(-759910943)))
			{
				value = Utils.DecodeQuotedPrintable(value, this.Encoding);
			}
			else if (this.ContentTransferEncoding.Is(<Module>.smethod_2<string>(1648508631)) && this.ContentType.StartsWith(<Module>.smethod_6<string>(-514079159), StringComparison.OrdinalIgnoreCase) && Utils.IsValidBase64String(ref value, false))
			{
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value)))
				{
					using (StreamReader streamReader = new StreamReader(memoryStream, this.Encoding))
					{
						value = streamReader.ReadToEnd();
					}
				}
				this.ContentTransferEncoding = string.Empty;
			}
			this.Body = value;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000AB1F File Offset: 0x00008D1F
		internal void SetBody(byte[] data)
		{
			this.ContentTransferEncoding = <Module>.smethod_4<string>(-1563599960);
			this.Body = Convert.ToBase64String(data);
		}

		// Token: 0x040003CD RID: 973
		private HeaderDictionary _Headers;

		// Token: 0x040003CE RID: 974
		protected Encoding _DefaultEncoding = Encoding.GetEncoding(1252);

		// Token: 0x040003CF RID: 975
		protected Encoding _Encoding;
	}
}
