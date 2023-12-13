using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Proton.GoSrp
{
	// Token: 0x020000D1 RID: 209
	public static class Extensions
	{
		// Token: 0x06000676 RID: 1654 RVA: 0x0002BA10 File Offset: 0x00029C10
		internal static GoString ToGoString(this string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			IntPtr intPtr = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, intPtr, bytes.Length);
			return new GoString
			{
				Data = intPtr,
				Length = (IntPtr)bytes.Length
			};
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002BA60 File Offset: 0x00029C60
		public static byte[] ConvertToBytes(this GoBytes goBytes)
		{
			int num = goBytes.Length.ToInt32();
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = Marshal.ReadByte(goBytes.Data, i);
			}
			return array;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002BAA0 File Offset: 0x00029CA0
		internal unsafe static GoBytes ToGoBytes(this string str)
		{
			IntPtr s = Marshal.StringToHGlobalUni(str);
			int maxByteCount = Encoding.UTF8.GetMaxByteCount(str.Length);
			IntPtr data = Marshal.AllocHGlobal(Encoding.UTF8.GetMaxByteCount(str.Length));
			int bytes = Encoding.UTF8.GetBytes((char*)s.ToPointer(), str.Length, (byte*)data.ToPointer(), maxByteCount);
			Marshal.ZeroFreeGlobalAllocUnicode(s);
			return new GoBytes
			{
				Data = data,
				Length = (IntPtr)bytes,
				Capacity = (IntPtr)bytes
			};
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002BB30 File Offset: 0x00029D30
		public unsafe static DisposableGoBytes ToDisposableGoBytes(this string str)
		{
			IntPtr s = Marshal.StringToHGlobalUni(str);
			int maxByteCount = Encoding.UTF8.GetMaxByteCount(str.Length);
			IntPtr data = Marshal.AllocHGlobal(Encoding.UTF8.GetMaxByteCount(str.Length));
			int bytes = Encoding.UTF8.GetBytes((char*)s.ToPointer(), str.Length, (byte*)data.ToPointer(), maxByteCount);
			Marshal.ZeroFreeGlobalAllocUnicode(s);
			return new DisposableGoBytes
			{
				Data = data,
				Length = (IntPtr)bytes,
				Capacity = (IntPtr)bytes
			};
		}
	}
}
