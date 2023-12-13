using System;
using Hackus_Mail_Checker_Reforged.UI.Models;
using MailBee.Mime;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Models
{
	// Token: 0x0200018F RID: 399
	internal class Attachment : BindableObject
	{
		// Token: 0x06000C07 RID: 3079 RVA: 0x0000D1FF File Offset: 0x0000B3FF
		public Attachment(Attachment attachment)
		{
			this.InnerAttachment = attachment;
			this.Filename = attachment.Filename;
			this.ContentType = attachment.ContentType;
			this.Size = attachment.Size;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0000D232 File Offset: 0x0000B432
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0000D23A File Offset: 0x0000B43A
		public string Filename
		{
			get
			{
				return this._filename;
			}
			set
			{
				this._filename = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1541218912));
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0000D253 File Offset: 0x0000B453
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0000D25B File Offset: 0x0000B45B
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				this._contentType = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(538227097));
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0000D274 File Offset: 0x0000B474
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0000D27C File Offset: 0x0000B47C
		public int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-920485052));
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0000D295 File Offset: 0x0000B495
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x0000D29D File Offset: 0x0000B49D
		public Attachment InnerAttachment
		{
			get
			{
				return this._innerAttachment;
			}
			set
			{
				this._innerAttachment = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1977027657));
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0000D2B6 File Offset: 0x0000B4B6
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x0000D2BE File Offset: 0x0000B4BE
		public bool IsSaved
		{
			get
			{
				return this._isSaved;
			}
			set
			{
				this._isSaved = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1640978232));
			}
		}

		// Token: 0x0400066C RID: 1644
		private string _filename;

		// Token: 0x0400066D RID: 1645
		private string _contentType;

		// Token: 0x0400066E RID: 1646
		private int _size;

		// Token: 0x0400066F RID: 1647
		private Attachment _innerAttachment;

		// Token: 0x04000670 RID: 1648
		private bool _isSaved;
	}
}
