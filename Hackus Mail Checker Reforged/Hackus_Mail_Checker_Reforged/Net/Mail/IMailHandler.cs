using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000FF RID: 255
	public interface IMailHandler
	{
		// Token: 0x060007D7 RID: 2007
		OperationResult Connect(ProxyClient proxyClient);

		// Token: 0x060007D8 RID: 2008
		OperationResult Login(out ExceptionType exceptionType);

		// Token: 0x060007D9 RID: 2009
		void SearchMessages();

		// Token: 0x060007DA RID: 2010
		void Dispose();

		// Token: 0x060007DB RID: 2011
		OperationResult SelectFolder(Folder folder);
	}
}
