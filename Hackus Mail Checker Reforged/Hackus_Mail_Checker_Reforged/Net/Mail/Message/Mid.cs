using System;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Message
{
	// Token: 0x02000117 RID: 279
	public class Mid
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0000B272 File Offset: 0x00009472
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x0000B27A File Offset: 0x0000947A
		public virtual string MID { get; internal set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0000B283 File Offset: 0x00009483
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0000B28B File Offset: 0x0000948B
		public virtual string ItemId { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0000B294 File Offset: 0x00009494
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x0000B29C File Offset: 0x0000949C
		public virtual Folder Folder { get; set; }

		// Token: 0x060008A5 RID: 2213 RVA: 0x0000619C File Offset: 0x0000439C
		public Mid()
		{
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000B2A5 File Offset: 0x000094A5
		public Mid(string mid, Folder folder)
		{
			this.MID = mid;
			this.Folder = folder;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000B2BB File Offset: 0x000094BB
		public Mid(string mid)
		{
			this.MID = mid;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000B2CA File Offset: 0x000094CA
		public override int GetHashCode()
		{
			Folder folder = this.Folder;
			if (((folder != null) ? folder.Name : null) == null)
			{
				return this.MID.GetHashCode();
			}
			return this.MID.GetHashCode() ^ this.Folder.Name.GetHashCode();
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00035270 File Offset: 0x00033470
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Mid))
			{
				return this.method_0(obj);
			}
			Mid mid = (Mid)obj;
			Folder folder = this.Folder;
			string a = (folder != null) ? folder.Name : null;
			Folder folder2 = mid.Folder;
			string b = (folder2 != null) ? folder2.Name : null;
			return this.MID == mid.MID && a == b;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000B308 File Offset: 0x00009508
		bool method_0(object object_0)
		{
			return base.Equals(object_0);
		}
	}
}
