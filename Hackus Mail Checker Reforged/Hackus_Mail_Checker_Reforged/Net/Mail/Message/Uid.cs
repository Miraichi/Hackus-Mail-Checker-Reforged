using System;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x0200011C RID: 284
	public class Uid
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0000B734 File Offset: 0x00009934
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x0000B73C File Offset: 0x0000993C
		public virtual decimal UID { get; internal set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0000B745 File Offset: 0x00009945
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0000B74D File Offset: 0x0000994D
		public virtual string ItemId { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0000B756 File Offset: 0x00009956
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x0000B75E File Offset: 0x0000995E
		public virtual Folder Folder { get; set; }

		// Token: 0x060008E9 RID: 2281 RVA: 0x0000619C File Offset: 0x0000439C
		public Uid()
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0000B767 File Offset: 0x00009967
		public Uid(long uid)
		{
			this.UID = Convert.ToDecimal(uid);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0000B77B File Offset: 0x0000997B
		public Uid(decimal uid)
		{
			this.UID = uid;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000B78A File Offset: 0x0000998A
		public Uid(string uid, Folder folder)
		{
			this.UID = decimal.Parse(uid);
			this.Folder = folder;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000B7A5 File Offset: 0x000099A5
		public Uid(string uid)
		{
			this.UID = decimal.Parse(uid);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0000B7B9 File Offset: 0x000099B9
		public Uid(int uid)
		{
			this.UID = Convert.ToDecimal(uid);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0000B7CD File Offset: 0x000099CD
		public Uid(double uid)
		{
			this.UID = Convert.ToDecimal(uid);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00035568 File Offset: 0x00033768
		public override int GetHashCode()
		{
			Folder folder = this.Folder;
			if (((folder != null) ? folder.Name : null) == null)
			{
				return this.UID.GetHashCode();
			}
			return this.UID.GetHashCode() ^ this.Folder.Name.GetHashCode();
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000355B8 File Offset: 0x000337B8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is Uid)
			{
				Uid uid = (Uid)obj;
				Folder folder = this.Folder;
				string a = (folder != null) ? folder.Name : null;
				Folder folder2 = uid.Folder;
				string b = (folder2 != null) ? folder2.Name : null;
				return this.UID == uid.UID && a == b;
			}
			return this.method_0(obj);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000B308 File Offset: 0x00009508
		bool method_0(object object_0)
		{
			return base.Equals(object_0);
		}
	}
}
