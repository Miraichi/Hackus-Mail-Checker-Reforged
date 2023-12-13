using System;
using System.Runtime.InteropServices;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Proton.GoSrp
{
	// Token: 0x020000D5 RID: 213
	public class PInvoke
	{
		// Token: 0x0600067C RID: 1660
		[DllImport("Kernel32.dll")]
		public static extern void RtlZeroMemory(IntPtr dest, int size);
	}
}
