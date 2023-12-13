using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Tools
{
	// Token: 0x020001A1 RID: 417
	internal class ShuffleTool : BindableObject, ITool
	{
		// Token: 0x06000C56 RID: 3158 RVA: 0x0000D632 File Offset: 0x0000B832
		public ShuffleTool(BasePath path, CancellationToken token)
		{
			this._srcPath = path;
			this._cancellationToken = token;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000423A8 File Offset: 0x000405A8
		public Task<bool> Run()
		{
			ShuffleTool.<Run>d__4 <Run>d__;
			<Run>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<ShuffleTool.<Run>d__4>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0000D648 File Offset: 0x0000B848
		public void OpenDirectory()
		{
			Process.Start(<Module>.smethod_4<string>(-753878610), <Module>.smethod_3<string>(1167347191) + this._savePath.FullName + <Module>.smethod_4<string>(2069567598));
		}

		// Token: 0x040006A8 RID: 1704
		private CancellationToken _cancellationToken;

		// Token: 0x040006A9 RID: 1705
		private FileInfo _savePath;

		// Token: 0x040006AA RID: 1706
		private BasePath _srcPath;
	}
}
