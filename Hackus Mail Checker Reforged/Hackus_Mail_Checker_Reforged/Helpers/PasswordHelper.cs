using System;
using System.Security.Cryptography;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Helpers
{
	// Token: 0x02000158 RID: 344
	public static class PasswordHelper
	{
		// Token: 0x06000A19 RID: 2585 RVA: 0x0003B0D0 File Offset: 0x000392D0
		public static string Generate(int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			using (RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				byte[] array = new byte[4];
				while (length-- > 0)
				{
					rngcryptoServiceProvider.GetBytes(array);
					uint num = BitConverter.ToUInt32(array, 0);
					stringBuilder.Append(<Module>.smethod_3<string>(-289343236)[(int)(num % (uint)<Module>.smethod_4<string>(622787849).Length)]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400051C RID: 1308
		private const string _valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	}
}
