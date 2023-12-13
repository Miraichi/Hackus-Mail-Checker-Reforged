using System;
using System.Runtime.InteropServices;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Proton.GoSrp
{
	// Token: 0x020000D4 RID: 212
	public struct GoString : IDisposable
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x0000A070 File Offset: 0x00008270
		public void Dispose()
		{
			PInvoke.RtlZeroMemory(this.Data, this.Length.ToInt32());
			Marshal.FreeHGlobal(this.Data);
		}

		// Token: 0x04000363 RID: 867
		public IntPtr Data;

		// Token: 0x04000364 RID: 868
		public IntPtr Length;
	}
}
