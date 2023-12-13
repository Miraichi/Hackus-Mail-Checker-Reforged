using System;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Utilities
{
	// Token: 0x0200010D RID: 269
	public class SubstringException : Exception
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x000064C6 File Offset: 0x000046C6
		public SubstringException()
		{
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000064CE File Offset: 0x000046CE
		public SubstringException(string message) : base(message)
		{
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000AFC1 File Offset: 0x000091C1
		public SubstringException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
