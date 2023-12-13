using System;
using System.Collections.ObjectModel;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Viewer.Models
{
	// Token: 0x02000190 RID: 400
	internal class ImapFolder : BindableObject
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0000D2D7 File Offset: 0x0000B4D7
		public ImapFolder(string name, string fullName)
		{
			this.Name = name;
			this.FullName = fullName;
			this.ExtendedName = name;
			this.InnerFolders = new ObservableCollection<ImapFolder>();
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0000D2FF File Offset: 0x0000B4FF
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x0000D307 File Offset: 0x0000B507
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(475439731));
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0000D320 File Offset: 0x0000B520
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x0000D328 File Offset: 0x0000B528
		public string FullName
		{
			get
			{
				return this._fullName;
			}
			set
			{
				this._fullName = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(-914171811));
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0000D341 File Offset: 0x0000B541
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x0000D349 File Offset: 0x0000B549
		public string ExtendedName
		{
			get
			{
				return this._extendedName;
			}
			set
			{
				this._extendedName = value;
				base.OnPropertyChanged(<Module>.smethod_6<string>(568461884));
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x0000D362 File Offset: 0x0000B562
		// (set) Token: 0x06000C1A RID: 3098 RVA: 0x0000D36A File Offset: 0x0000B56A
		public int MessagesCount
		{
			get
			{
				return this._messagesCount;
			}
			set
			{
				this._messagesCount = value;
				base.OnPropertyChanged(<Module>.smethod_3<string>(-1161018111));
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0000D383 File Offset: 0x0000B583
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x0000D38B File Offset: 0x0000B58B
		public ObservableCollection<ImapFolder> InnerFolders
		{
			get
			{
				return this._innerFolders;
			}
			set
			{
				this._innerFolders = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(1983923903));
			}
		}

		// Token: 0x04000671 RID: 1649
		private string _name;

		// Token: 0x04000672 RID: 1650
		private string _fullName;

		// Token: 0x04000673 RID: 1651
		private string _extendedName;

		// Token: 0x04000674 RID: 1652
		private int _messagesCount;

		// Token: 0x04000675 RID: 1653
		private ObservableCollection<ImapFolder> _innerFolders;
	}
}
