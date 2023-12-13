using System;
using System.Runtime.InteropServices;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Proton.GoSrp
{
	// Token: 0x020000D0 RID: 208
	public struct DisposableGoBytes : IDisposable
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x0000A04D File Offset: 0x0000824D
		public void Dispose()
		{
			PInvoke.RtlZeroMemory(this.Data, this.Capacity.ToInt32());
			Marshal.FreeHGlobal(this.Data);
		}

		// Token: 0x0400035A RID: 858
		public IntPtr Data;

		// Token: 0x0400035B RID: 859
		public IntPtr Length;

		// Token: 0x0400035C RID: 860
		public IntPtr Capacity;
	}
}
