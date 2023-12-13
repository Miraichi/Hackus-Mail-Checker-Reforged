using System;
using Hackus_Mail_Checker_Reforged.Components.Scheduler.Models.Contexts;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Scheduler.Models
{
	// Token: 0x020001DE RID: 478
	public class ScheduledMail : BindableObject
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0000E2DB File Offset: 0x0000C4DB
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x0000E2E3 File Offset: 0x0000C4E3
		public string Address
		{
			get
			{
				return this._address;
			}
			set
			{
				this._address = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(1587104857));
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x0000E304 File Offset: 0x0000C504
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this._password = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(-2024528016));
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0000E31D File Offset: 0x0000C51D
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x0000E325 File Offset: 0x0000C525
		public MailStatus Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(1417505023));
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0000E33E File Offset: 0x0000C53E
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0000E346 File Offset: 0x0000C546
		public DateTime? LastExecuted
		{
			get
			{
				return this._lastExecuted;
			}
			set
			{
				this._lastExecuted = value;
				base.OnPropertyChanged(<Module>.smethod_2<string>(-1590869042));
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0000E35F File Offset: 0x0000C55F
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x0000E367 File Offset: 0x0000C567
		public Server Server { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0000E370 File Offset: 0x0000C570
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0000E378 File Offset: 0x0000C578
		public MailContext Context { get; set; }

		// Token: 0x06000E24 RID: 3620 RVA: 0x00047674 File Offset: 0x00045874
		public string GetDomain()
		{
			string[] array = this.Address.Split(new char[]
			{
				'@'
			});
			if (array.Length == 2)
			{
				return array[1];
			}
			return null;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000476A4 File Offset: 0x000458A4
		public static ScheduledMail GetFromString(string value)
		{
			string[] array;
			if (value.Contains(<Module>.smethod_3<string>(-1905482012)))
			{
				array = value.Split(new char[]
				{
					':'
				});
			}
			else
			{
				if (!value.Contains(<Module>.smethod_4<string>(90200717)))
				{
					return null;
				}
				array = value.Split(new char[]
				{
					';'
				});
			}
			if (array.Length < 2 || string.IsNullOrWhiteSpace(array[0]) || string.IsNullOrWhiteSpace(array[1]))
			{
				return null;
			}
			if (array[0].Split(new char[]
			{
				'@'
			}).Length < 2)
			{
				return null;
			}
			return new ScheduledMail
			{
				Address = array[0],
				Password = array[1]
			};
		}

		// Token: 0x0400078A RID: 1930
		private string _address;

		// Token: 0x0400078B RID: 1931
		private string _password;

		// Token: 0x0400078C RID: 1932
		private MailStatus _status;

		// Token: 0x0400078D RID: 1933
		private DateTime? _lastExecuted;
	}
}
