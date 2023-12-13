using System;
using System.Collections.Generic;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000F3 RID: 243
	public class SafeDictionary<KT, VT> : Dictionary<KT, VT>
	{
		// Token: 0x06000789 RID: 1929 RVA: 0x0000ABCA File Offset: 0x00008DCA
		public SafeDictionary()
		{
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0000ABD2 File Offset: 0x00008DD2
		public SafeDictionary(IEqualityComparer<KT> comparer) : base(comparer)
		{
		}

		// Token: 0x1700019E RID: 414
		public new virtual VT this[KT key]
		{
			get
			{
				return this.Get(key, default(VT));
			}
			set
			{
				this.Set(key, value);
			}
		}
	}
}
