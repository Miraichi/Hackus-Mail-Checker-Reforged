using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Tools
{
	// Token: 0x0200019E RID: 414
	internal class SortTool : BindableObject, ITool
	{
		// Token: 0x06000C48 RID: 3144 RVA: 0x0000D5CC File Offset: 0x0000B7CC
		public SortTool(BasePath path, CancellationToken token)
		{
			this._srcPath = path;
			this._cancellationToken = token;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000420E8 File Offset: 0x000402E8
		public Task<bool> Run()
		{
			SortTool.<Run>d__4 <Run>d__;
			<Run>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<SortTool.<Run>d__4>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0000D5E2 File Offset: 0x0000B7E2
		public void OpenDirectory()
		{
			Process.Start(<Module>.smethod_6<string>(-1308867158), <Module>.smethod_3<string>(1167347191) + this._savePath.FullName + <Module>.smethod_6<string>(-77460630));
		}

		// Token: 0x0400069C RID: 1692
		private CancellationToken _cancellationToken;

		// Token: 0x0400069D RID: 1693
		private FileInfo _savePath;

		// Token: 0x0400069E RID: 1694
		private BasePath _srcPath;
	}
}
