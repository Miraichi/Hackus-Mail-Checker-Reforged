using System;
using System.Collections.Generic;

namespace Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers
{
	// Token: 0x020000CB RID: 203
	public abstract class RequestHelper
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600065A RID: 1626
		public abstract string BaseURL { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600065B RID: 1627
		public abstract string LoginURL { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600065C RID: 1628
		public abstract string SuccessURL { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600065D RID: 1629
		public abstract string ApiURL { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600065E RID: 1630
		public abstract string NavigatorURL { get; }

		// Token: 0x0600065F RID: 1631
		public abstract void SetLoginParameters(IList<KeyValuePair<string, string>> parameters);

		// Token: 0x06000660 RID: 1632 RVA: 0x0002B62C File Offset: 0x0002982C
		public static RequestHelper Get(string domain)
		{
			if (domain == <Module>.smethod_3<string>(-2067254616) || domain == <Module>.smethod_6<string>(-25871447))
			{
				return new GmxComRequestHelper();
			}
			if (!(domain == <Module>.smethod_2<string>(-995331050)))
			{
				return new GmxNetRequestHelper();
			}
			return new WebDeRequestHelper();
		}
	}
}
