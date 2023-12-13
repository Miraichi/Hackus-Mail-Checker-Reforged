using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.UI.Models;

namespace Hackus_Mail_Checker_Reforged.Components.Tools
{
	// Token: 0x0200019C RID: 412
	internal class SortDomainsTool : BindableObject, ITool
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x0000D5A8 File Offset: 0x0000B7A8
		public SortDomainsTool(BasePath path, CancellationToken token)
		{
			this._srcPath = path;
			this._cancellationToken = token;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00041FC4 File Offset: 0x000401C4
		public Task<bool> Run()
		{
			SortDomainsTool.<Run>d__3 <Run>d__;
			<Run>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<SortDomainsTool.<Run>d__3>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00006655 File Offset: 0x00004855
		public void OpenDirectory()
		{
		}

		// Token: 0x04000696 RID: 1686
		private CancellationToken _cancellationToken;

		// Token: 0x04000697 RID: 1687
		private BasePath _srcPath;
	}
}
