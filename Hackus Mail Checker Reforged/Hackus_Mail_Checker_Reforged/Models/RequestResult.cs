using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x0200013F RID: 319
	internal class RequestResult : BindableObject
	{
		// Token: 0x060009E5 RID: 2533 RVA: 0x0000C00B File Offset: 0x0000A20B
		public RequestResult(string request, int count)
		{
			this.Request = request;
			this.Count = count;
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0000C021 File Offset: 0x0000A221
		// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0000C029 File Offset: 0x0000A229
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1226958529));
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0000C042 File Offset: 0x0000A242
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0000C04A File Offset: 0x0000A24A
		public string Request
		{
			get
			{
				return this._request;
			}
			set
			{
				this._request = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(994142225));
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0000C063 File Offset: 0x0000A263
		public RequestResult Clone(RequestResult result)
		{
			return new RequestResult(result.Request, result.Count);
		}

		// Token: 0x040004BE RID: 1214
		private int _count;

		// Token: 0x040004BF RID: 1215
		private string _request;
	}
}
