using System;
using System.IO;
using System.Net.Security;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Utilities
{
	// Token: 0x02000106 RID: 262
	public class SslStream : SslStream
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x0000AE12 File Offset: 0x00009012
		public SslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback) : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback)
		{
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0000AE1D File Offset: 0x0000901D
		public new Stream InnerStream
		{
			get
			{
				return base.InnerStream;
			}
		}
	}
}
