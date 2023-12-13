using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Tools
{
	// Token: 0x020001A3 RID: 419
	internal class NormalizeTool : BindableObject, ITool
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x0000D693 File Offset: 0x0000B893
		public NormalizeTool(BasePath path, CancellationToken token)
		{
			this._srcPath = path;
			this._cancellationToken = token;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00042638 File Offset: 0x00040838
		public Task<bool> Run()
		{
			NormalizeTool.<Run>d__4 <Run>d__;
			<Run>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<NormalizeTool.<Run>d__4>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0000D6A9 File Offset: 0x0000B8A9
		public void OpenDirectory()
		{
			Process.Start(<Module>.smethod_2<string>(843412805), <Module>.smethod_3<string>(1167347191) + this._savePath.FullName + <Module>.smethod_6<string>(-77460630));
		}

		// Token: 0x040006B3 RID: 1715
		private CancellationToken _cancellationToken;

		// Token: 0x040006B4 RID: 1716
		private FileInfo _savePath;

		// Token: 0x040006B5 RID: 1717
		private BasePath _srcPath;
	}
}
