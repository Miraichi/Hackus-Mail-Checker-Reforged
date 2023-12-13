using System;
using System.Collections.Generic;

namespace Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers
{
	// Token: 0x020000CC RID: 204
	public class WebDeRequestHelper : RequestHelper
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00009FD4 File Offset: 0x000081D4
		public override string BaseURL
		{
			get
			{
				return <Module>.smethod_4<string>(428531331);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00009FE0 File Offset: 0x000081E0
		public override string LoginURL
		{
			get
			{
				return <Module>.smethod_2<string>(-1147707449);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00009FEC File Offset: 0x000081EC
		public override string SuccessURL
		{
			get
			{
				return <Module>.smethod_5<string>(1675151254);
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00009FF8 File Offset: 0x000081F8
		public override string ApiURL
		{
			get
			{
				return <Module>.smethod_2<string>(1632188786);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0000A004 File Offset: 0x00008204
		public override string NavigatorURL
		{
			get
			{
				return <Module>.smethod_4<string>(-953467717);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002B680 File Offset: 0x00029880
		public override void SetLoginParameters(IList<KeyValuePair<string, string>> parameters)
		{
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1850962823), <Module>.smethod_5<string>(-36000308)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_5<string>(-1307243931), <Module>.smethod_6<string>(-1697764416)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_5<string>(-899126992), <Module>.smethod_6<string>(-2140997718)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_5<string>(-1284195785), <Module>.smethod_5<string>(1258291461)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_3<string>(2139248894), <Module>.smethod_3<string>(-429649467)));
		}

		// Token: 0x0400034E RID: 846
		public const string BASE_URL = "web.de";

		// Token: 0x0400034F RID: 847
		public const string API_URL = "3c.web.de";

		// Token: 0x04000350 RID: 848
		public const string NAVIGATOR_URL = "navigator.web.de";

		// Token: 0x04000351 RID: 849
		public const string LOGIN_URL = "https://login.web.de/login";

		// Token: 0x04000352 RID: 850
		public const string SUCCESS_URL = "https://navigator.web.de/login";
	}
}
