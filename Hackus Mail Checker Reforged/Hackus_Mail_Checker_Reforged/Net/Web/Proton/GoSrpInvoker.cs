using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Hackus_Mail_Checker_Reforged.Net.Web.Proton.GoSrp;
using Hackus_Mail_Checker_Reforged.Services.Managers;

namespace Hackus_Mail_Checker_Reforged.Net.Web.Proton
{
	// Token: 0x020000CF RID: 207
	public static class GoSrpInvoker
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0000A032 File Offset: 0x00008232
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x0000A039 File Offset: 0x00008239
		private static string BinaryPath { get; set; }

		// Token: 0x06000670 RID: 1648 RVA: 0x0002B728 File Offset: 0x00029928
		public static void Invoke()
		{
			GoSrpInvoker.BinaryPath = (Environment.Is64BitOperatingSystem ? <Module>.smethod_6<string>(1112839400) : <Module>.smethod_6<string>(-806108457));
			if (!File.Exists(GoSrpInvoker.BinaryPath))
			{
				FileManager.LogUnhandledException(new Exception(<Module>.smethod_3<string>(-670182526)), <Module>.smethod_3<string>(-1513011952));
				Process.GetCurrentProcess().Kill();
				return;
			}
			Environment.SetEnvironmentVariable(<Module>.smethod_2<string>(1664828476), <Module>.smethod_3<string>(-348829301));
			if (GoSrpInvoker.LoadLibrary(GoSrpInvoker.BinaryPath) == IntPtr.Zero)
			{
				GoSrpInvoker.BinaryPath = (Environment.Is64BitOperatingSystem ? <Module>.smethod_6<string>(-806108457) : <Module>.smethod_3<string>(-2115308319));
				if (GoSrpInvoker.LoadLibrary(GoSrpInvoker.BinaryPath) == IntPtr.Zero)
				{
					FileManager.LogUnhandledException(new Exception(<Module>.smethod_2<string>(1798111342)), <Module>.smethod_3<string>(-1513011952));
					Process.GetCurrentProcess().Kill();
					return;
				}
			}
		}

		// Token: 0x06000671 RID: 1649
		[DllImport("Kernel32.dll")]
		public static extern IntPtr LoadLibrary(string lpFileName);

		// Token: 0x06000672 RID: 1650
		[DllImport("GoSrp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GenerateProofs")]
		private static extern GoBytes GenerateProofs_1(int version, GoString username, DisposableGoBytes password, GoString salt, GoString signedModulus, GoString serverEphemeral, int bits);

		// Token: 0x06000673 RID: 1651 RVA: 0x0002B81C File Offset: 0x00029A1C
		public static GoProofs GenerateProofs(int version, string username, string password, string salt, string signedModulus, string serverEphemeral, int bitLength = 2048)
		{
			byte[] buffer;
			using (GoString username2 = username.ToGoString())
			{
				using (DisposableGoBytes password2 = password.ToDisposableGoBytes())
				{
					using (GoString salt2 = salt.ToGoString())
					{
						using (GoString signedModulus2 = signedModulus.ToGoString())
						{
							using (GoString serverEphemeral2 = serverEphemeral.ToGoString())
							{
								object lockedThread = GoSrpInvoker.LockedThread;
								lock (lockedThread)
								{
									buffer = GoSrpInvoker.GenerateProofs_1(version, username2, password2, salt2, signedModulus2, serverEphemeral2, bitLength).ConvertToBytes();
								}
							}
						}
					}
				}
			}
			using (MemoryStream memoryStream = new MemoryStream(buffer))
			{
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				binaryReader.ReadByte();
				byte b = binaryReader.ReadByte();
				if (b == 0)
				{
					ushort count = binaryReader.ReadUInt16();
					byte[] bytes = binaryReader.ReadBytes((int)count);
					File.AppendAllText(<Module>.smethod_3<string>(-910715585), <Module>.smethod_6<string>(-1996712854) + Encoding.UTF8.GetString(bytes) + Environment.NewLine);
					return null;
				}
				if (b == 1)
				{
					ushort count2 = binaryReader.ReadUInt16();
					byte[] inArray = binaryReader.ReadBytes((int)count2);
					count2 = binaryReader.ReadUInt16();
					byte[] inArray2 = binaryReader.ReadBytes((int)count2);
					count2 = binaryReader.ReadUInt16();
					byte[] expectedServerProof = binaryReader.ReadBytes((int)count2);
					return new GoProofs
					{
						ClientProof = Convert.ToBase64String(inArray),
						ClientEphemeral = Convert.ToBase64String(inArray2),
						ExpectedServerProof = expectedServerProof
					};
				}
			}
			return null;
		}

		// Token: 0x04000359 RID: 857
		private static object LockedThread = new object();
	}
}
