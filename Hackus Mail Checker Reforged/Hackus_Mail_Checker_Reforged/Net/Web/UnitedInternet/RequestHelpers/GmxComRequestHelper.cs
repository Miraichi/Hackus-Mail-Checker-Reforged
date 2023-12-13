using System;
using System.Collections.Generic;

namespace Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers
{
	// Token: 0x020000C9 RID: 201
	public class GmxComRequestHelper : RequestHelper
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00009F59 File Offset: 0x00008159
		public override string BaseURL
		{
			get
			{
				return <Module>.smethod_5<string>(-673013199);
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00009F65 File Offset: 0x00008165
		public override string LoginURL
		{
			get
			{
				return <Module>.smethod_6<string>(1982008154);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00009F71 File Offset: 0x00008171
		public override string SuccessURL
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00009F78 File Offset: 0x00008178
		public override string ApiURL
		{
			get
			{
				return <Module>.smethod_3<string>(-241091125);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00009F84 File Offset: 0x00008184
		public override string NavigatorURL
		{
			get
			{
				return <Module>.smethod_6<string>(-682581013);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0002B514 File Offset: 0x00029714
		public override void SetLoginParameters(IList<KeyValuePair<string, string>> parameters)
		{
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_6<string>(-1858634421), <Module>.smethod_2<string>(-2053471242)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_5<string>(-1307243931), <Module>.smethod_4<string>(1559186040)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_2<string>(726424993), <Module>.smethod_3<string>(1565797976)));
			parameters.Add(new KeyValuePair<string, string>(<Module>.smethod_2<string>(-756006378), <Module>.smethod_2<string>(1324835837)));
		}

		// Token: 0x04000344 RID: 836
		public const string BASE_URL = "gmx.com";

		// Token: 0x04000345 RID: 837
		public const string API_URL = "3c-bs.gmx.com";

		// Token: 0x04000346 RID: 838
		public const string NAVIGATOR_URL = "navigator-bs.gmx.com";

		// Token: 0x04000347 RID: 839
		public const string LOGIN_URL = "https://login.gmx.com/login";

		// Token: 0x04000348 RID: 840
		public const string SUCCESS_URL = "";
	}
}
