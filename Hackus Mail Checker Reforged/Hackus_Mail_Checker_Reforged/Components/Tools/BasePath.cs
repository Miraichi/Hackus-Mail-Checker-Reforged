using System;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Tools
{
	// Token: 0x02000197 RID: 407
	internal class BasePath : BindableObject
	{
		// Token: 0x06000C2F RID: 3119 RVA: 0x0000D4CC File Offset: 0x0000B6CC
		public BasePath(string fileName, string fullPath)
		{
			this.FileName = fileName;
			this.FullPath = fullPath;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0000D4E2 File Offset: 0x0000B6E2
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0000D4EA File Offset: 0x0000B6EA
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this._fileName = value;
				base.OnPropertyChanged(<Module>.smethod_4<string>(-1983759734));
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0000D503 File Offset: 0x0000B703
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x0000D50B File Offset: 0x0000B70B
		public string FullPath
		{
			get
			{
				return this._fullPath;
			}
			set
			{
				this._fullPath = value;
				base.OnPropertyChanged(<Module>.smethod_5<string>(464312975));
			}
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00041CE0 File Offset: 0x0003FEE0
		public override bool Equals(object obj)
		{
			BasePath basePath = obj as BasePath;
			return basePath != null && (this.FileName == basePath.FileName && this.FullPath == basePath.FullPath);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0000D524 File Offset: 0x0000B724
		public override int GetHashCode()
		{
			return this.FileName.GetHashCode() ^ this.FullPath.GetHashCode();
		}

		// Token: 0x04000687 RID: 1671
		private string _fileName;

		// Token: 0x04000688 RID: 1672
		private string _fullPath;
	}
}
