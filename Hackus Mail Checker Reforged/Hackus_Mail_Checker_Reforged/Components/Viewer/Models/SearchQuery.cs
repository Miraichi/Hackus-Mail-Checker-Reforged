using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Models
{
	// Token: 0x02000193 RID: 403
	internal class SearchQuery : BindableObject
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0000D3A4 File Offset: 0x0000B5A4
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		public string Query
		{
			get
			{
				return this._query;
			}
			set
			{
				this._query = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-277778602));
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0000D3C5 File Offset: 0x0000B5C5
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0000D3CD File Offset: 0x0000B5CD
		public DateTime? DateFrom
		{
			get
			{
				return this.dateFrom;
			}
			set
			{
				this.dateFrom = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-827578586));
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0000D3E6 File Offset: 0x0000B5E6
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0000D3EE File Offset: 0x0000B5EE
		public DateTime? DateTo
		{
			get
			{
				return this.dateTo;
			}
			set
			{
				this.dateTo = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1420286107));
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0000D407 File Offset: 0x0000B607
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0000D40F File Offset: 0x0000B60F
		public SearchType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-828389526));
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0000D428 File Offset: 0x0000B628
		public SearchQuery Clone()
		{
			return new SearchQuery
			{
				Query = this.Query,
				DateFrom = this.DateFrom,
				DateTo = this.DateTo,
				Type = this.Type
			};
		}

		// Token: 0x04000679 RID: 1657
		private string _query;

		// Token: 0x0400067A RID: 1658
		private DateTime? dateFrom;

		// Token: 0x0400067B RID: 1659
		private DateTime? dateTo;

		// Token: 0x0400067C RID: 1660
		private SearchType _type;
	}
}
