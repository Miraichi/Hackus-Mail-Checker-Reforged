using System;
using System.Collections.ObjectModel;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x0200013E RID: 318
	public class RequestGroup : BindableObject
	{
		// Token: 0x060009E0 RID: 2528 RVA: 0x0000BFAF File Offset: 0x0000A1AF
		public RequestGroup(string name)
		{
			this.Name = name;
			this.Requests = new ObservableCollection<Request>();
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0000BFC9 File Offset: 0x0000A1C9
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0000BFD1 File Offset: 0x0000A1D1
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(1843052335));
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0000BFEA File Offset: 0x0000A1EA
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0000BFF2 File Offset: 0x0000A1F2
		public ObservableCollection<Request> Requests
		{
			get
			{
				return this._requests;
			}
			set
			{
				this._requests = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1897553056));
			}
		}

		// Token: 0x040004BC RID: 1212
		private string _name;

		// Token: 0x040004BD RID: 1213
		private ObservableCollection<Request> _requests;
	}
}
