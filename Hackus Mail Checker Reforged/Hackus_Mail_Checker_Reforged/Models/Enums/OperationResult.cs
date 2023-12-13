using System;

namespace Hackus_Mail_Checker_Reforged.Models.Enums
{
	// Token: 0x02000150 RID: 336
	public enum OperationResult
	{
		// Token: 0x040004FC RID: 1276
		Ok,
		// Token: 0x040004FD RID: 1277
		Bad,
		// Token: 0x040004FE RID: 1278
		Error,
		// Token: 0x040004FF RID: 1279
		HttpError,
		// Token: 0x04000500 RID: 1280
		Blocked,
		// Token: 0x04000501 RID: 1281
		TwoFactor,
		// Token: 0x04000502 RID: 1282
		Multipassword,
		// Token: 0x04000503 RID: 1283
		Captcha,
		// Token: 0x04000504 RID: 1284
		ReCaptcha,
		// Token: 0x04000505 RID: 1285
		HostNotFound,
		// Token: 0x04000506 RID: 1286
		ServerDisabled,
		// Token: 0x04000507 RID: 1287
		Retry,
		// Token: 0x04000508 RID: 1288
		RetryOnNewConnection
	}
}
