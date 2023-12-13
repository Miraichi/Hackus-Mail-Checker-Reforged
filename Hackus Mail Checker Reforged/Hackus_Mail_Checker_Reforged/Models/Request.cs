using System;
using System.Collections.Generic;
using System.Linq;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000137 RID: 311
	public class Request : BindableObject
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0000BE10 File Offset: 0x0000A010
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0000BE18 File Offset: 0x0000A018
		public string Sender
		{
			get
			{
				return this._sender;
			}
			set
			{
				this._sender = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(866801588));
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0000BE31 File Offset: 0x0000A031
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x0000BE39 File Offset: 0x0000A039
		public string Subject
		{
			get
			{
				return this._subject;
			}
			set
			{
				this._subject = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1305046948));
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0000BE52 File Offset: 0x0000A052
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0000BE5A File Offset: 0x0000A05A
		public string Body
		{
			get
			{
				return this._body;
			}
			set
			{
				this._body = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(421021987));
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0000BE73 File Offset: 0x0000A073
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0000BE7B File Offset: 0x0000A07B
		public bool CheckDate
		{
			get
			{
				return this._checkDate;
			}
			set
			{
				this._checkDate = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-351728098));
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0000BE94 File Offset: 0x0000A094
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0000BE9C File Offset: 0x0000A09C
		public DateTime? DateFrom
		{
			get
			{
				return this._dateFrom;
			}
			set
			{
				this._dateFrom = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(86395888));
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0000BEB5 File Offset: 0x0000A0B5
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0000BEBD File Offset: 0x0000A0BD
		public DateTime? DateTo
		{
			get
			{
				return this._dateTo;
			}
			set
			{
				this._dateTo = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-1420286107));
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0000BED6 File Offset: 0x0000A0D6
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0000BEDE File Offset: 0x0000A0DE
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1574756136));
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0003A940 File Offset: 0x00038B40
		public void AddSavedUid(Uid uid)
		{
			if (this.SavedUids == null)
			{
				this.SavedUids = new HashSet<Uid>();
			}
			if (this.SavedUids.Any((Uid u) => u.UID == uid.UID))
			{
				return;
			}
			this.SavedUids.Add(uid);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0003A99C File Offset: 0x00038B9C
		public void AddFindedUid(Uid uid)
		{
			if (this.FindedUids == null)
			{
				this.FindedUids = new HashSet<Uid>();
			}
			if (this.FindedUids.Any((Uid u) => u.UID == uid.UID))
			{
				return;
			}
			this.FindedUids.Add(uid);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0003A9F8 File Offset: 0x00038BF8
		public void AddSavedMid(Mid mid)
		{
			if (this.SavedMids == null)
			{
				this.SavedMids = new HashSet<Mid>();
			}
			if (this.SavedMids.Any((Mid u) => Request.<>c__DisplayClass37_0.smethod_0(u.MID, mid.MID)))
			{
				return;
			}
			this.SavedMids.Add(mid);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0003AA54 File Offset: 0x00038C54
		public void AddFindedUid(Mid mid)
		{
			if (this.FindedMids == null)
			{
				this.FindedMids = new HashSet<Mid>();
			}
			if (!this.FindedMids.Any((Mid u) => Request.<>c__DisplayClass38_0.smethod_0(u.MID, mid.MID)))
			{
				this.FindedMids.Add(mid);
				return;
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0003AAB0 File Offset: 0x00038CB0
		public void AddCheckedFolder(string folder)
		{
			if (this.CheckedFolders == null)
			{
				this.CheckedFolders = new HashSet<string>();
			}
			if (!this.CheckedFolders.Any((string f) => Request.<>c__DisplayClass39_0.smethod_0(f, folder)))
			{
				this.CheckedFolders.Add(folder);
				return;
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0003AB0C File Offset: 0x00038D0C
		public Request Clone()
		{
			return new Request
			{
				Sender = this.Sender,
				Body = this.Body,
				Subject = this.Subject,
				SavedUids = this.SavedUids,
				FindedUids = this.FindedUids,
				SavedMids = this.SavedMids,
				FindedMids = this.FindedMids,
				CheckedFolders = this.CheckedFolders,
				CheckDate = this.CheckDate,
				DateFrom = this.DateFrom,
				DateTo = this.DateTo
			};
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0000BEF7 File Offset: 0x0000A0F7
		public bool Equals(Request request)
		{
			return this.Sender == request.Sender && this.Body == request.Body && this.Subject == request.Subject;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003ABA4 File Offset: 0x00038DA4
		public override bool Equals(object obj)
		{
			Request request = obj as Request;
			return request != null && (this.Sender == request.Sender && this.Body == request.Body) && this.Subject == request.Subject;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0003ABF8 File Offset: 0x00038DF8
		public bool IsCombined()
		{
			return (from isActive in new bool[]
			{
				this.Sender != null,
				this.Subject != null,
				this.Body != null
			}
			where isActive
			select isActive).Count<bool>() > 1;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003AC5C File Offset: 0x00038E5C
		public override string ToString()
		{
			string text = "";
			if (!string.IsNullOrWhiteSpace(this.Sender))
			{
				text = text + <Module>.smethod_5<string>(-1299295711) + this.Sender + <Module>.smethod_3<string>(1850728268);
			}
			if (!string.IsNullOrWhiteSpace(this.Subject))
			{
				text += ((text == "") ? (<Module>.smethod_4<string>(-1163809739) + this.Subject + <Module>.smethod_2<string>(-152097740)) : (<Module>.smethod_3<string>(1877381734) + this.Subject + <Module>.smethod_5<string>(-1679198443)));
			}
			if (!string.IsNullOrWhiteSpace(this.Body))
			{
				text += ((text == "") ? (<Module>.smethod_2<string>(-1816771512) + this.Body + <Module>.smethod_5<string>(-1679198443)) : (<Module>.smethod_2<string>(-7819286) + this.Body + <Module>.smethod_4<string>(-1953073799)));
			}
			return text;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0003AD60 File Offset: 0x00038F60
		public override int GetHashCode()
		{
			int num = 330842572;
			if (this.Sender != null)
			{
				num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Sender);
			}
			if (this.Body != null)
			{
				num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Body);
			}
			if (this.Subject != null)
			{
				num = num * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Subject);
			}
			return num;
		}

		// Token: 0x040004A7 RID: 1191
		public HashSet<Uid> SavedUids;

		// Token: 0x040004A8 RID: 1192
		public HashSet<Uid> FindedUids;

		// Token: 0x040004A9 RID: 1193
		public HashSet<Mid> SavedMids;

		// Token: 0x040004AA RID: 1194
		public HashSet<Mid> FindedMids;

		// Token: 0x040004AB RID: 1195
		public HashSet<string> CheckedFolders;

		// Token: 0x040004AC RID: 1196
		public bool IsChecked;

		// Token: 0x040004AD RID: 1197
		public int Errors;

		// Token: 0x040004AE RID: 1198
		private string _sender;

		// Token: 0x040004AF RID: 1199
		private string _subject;

		// Token: 0x040004B0 RID: 1200
		private string _body;

		// Token: 0x040004B1 RID: 1201
		private bool _checkDate;

		// Token: 0x040004B2 RID: 1202
		private DateTime? _dateFrom;

		// Token: 0x040004B3 RID: 1203
		private DateTime? _dateTo;

		// Token: 0x040004B4 RID: 1204
		private int _count;
	}
}
