using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000132 RID: 306
	internal class Folder : BindableObject
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x00006C91 File Offset: 0x00004E91
		public Folder()
		{
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0000BB56 File Offset: 0x00009D56
		public Folder(string name)
		{
			this.Name = name;
			this.IsEnabled = true;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0000BB6C File Offset: 0x00009D6C
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x0000BB74 File Offset: 0x00009D74
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-170040426));
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0000BB8D File Offset: 0x00009D8D
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0000BB95 File Offset: 0x00009D95
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(258746446));
			}
		}

		// Token: 0x04000496 RID: 1174
		private string _name;

		// Token: 0x04000497 RID: 1175
		private bool _isEnabled;
	}
}
