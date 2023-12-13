using System;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Captcha;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Helpers
{
	// Token: 0x0200015B RID: 347
	internal static class CaptchaHelpers
	{
		// Token: 0x06000A24 RID: 2596 RVA: 0x0003B24C File Offset: 0x0003944C
		public static ICaptchaClient CreateInstance()
		{
			CaptchaSolvationService captchaSolvationService = WebSettings.Instance.CaptchaSolvationService;
			if (captchaSolvationService == CaptchaSolvationService.AntiCaptcha)
			{
				return new AntiCaptcha();
			}
			if (captchaSolvationService != CaptchaSolvationService.RuCaptcha)
			{
				return null;
			}
			return new RuCaptcha();
		}
	}
}
