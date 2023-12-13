using System;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000EA RID: 234
	public class Folder
	{
		// Token: 0x06000710 RID: 1808 RVA: 0x0000A512 File Offset: 0x00008712
		public Folder()
		{
			this.Name = string.Empty;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0000A525 File Offset: 0x00008725
		public Folder(string name)
		{
			this.Name = name;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0000A534 File Offset: 0x00008734
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x0000A53C File Offset: 0x0000873C
		public virtual string Name { get; internal set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0000A545 File Offset: 0x00008745
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0000A54D File Offset: 0x0000874D
		public virtual int MessagesCount { get; internal set; }
	}
}
