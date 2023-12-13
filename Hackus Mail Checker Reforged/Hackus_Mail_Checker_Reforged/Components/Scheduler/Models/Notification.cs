using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Models
{
	// Token: 0x020001DD RID: 477
	public class Notification : BindableObject
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x0000E278 File Offset: 0x0000C478
		// (set) Token: 0x06000E12 RID: 3602 RVA: 0x0000E280 File Offset: 0x0000C480
		public string Address
		{
			get
			{
				return this._address;
			}
			set
			{
				this._address = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(95536999));
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0000E299 File Offset: 0x0000C499
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x0000E2A1 File Offset: 0x0000C4A1
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				this._message = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(470273670));
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0000E2BA File Offset: 0x0000C4BA
		// (set) Token: 0x06000E16 RID: 3606 RVA: 0x0000E2C2 File Offset: 0x0000C4C2
		public DateTime Time
		{
			get
			{
				return this._time;
			}
			set
			{
				this._time = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-831966319));
			}
		}

		// Token: 0x04000787 RID: 1927
		private string _address;

		// Token: 0x04000788 RID: 1928
		private string _message;

		// Token: 0x04000789 RID: 1929
		private DateTime _time;
	}
}
