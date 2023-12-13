using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000130 RID: 304
	internal class DomainFilter : BindableObject
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0000BB14 File Offset: 0x00009D14
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0000BB1C File Offset: 0x00009D1C
		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				this._domain = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1334223077));
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0000BB35 File Offset: 0x00009D35
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0000BB3D File Offset: 0x00009D3D
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(855342463));
			}
		}

		// Token: 0x04000491 RID: 1169
		private string _domain;

		// Token: 0x04000492 RID: 1170
		private bool _isEnabled;
	}
}
