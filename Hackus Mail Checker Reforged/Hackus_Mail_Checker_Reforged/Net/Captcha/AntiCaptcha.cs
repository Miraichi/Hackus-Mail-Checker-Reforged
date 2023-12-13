using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Captcha
{
	// Token: 0x0200012D RID: 301
	internal class AntiCaptcha : ICaptchaClient
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x00039990 File Offset: 0x00037B90
		[return: TupleElementNames(new string[]
		{
			"status",
			"result"
		})]
		public ValueTuple<OperationResult, string> GetBalance()
		{
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					string text = <Module>.smethod_3<string>(211063408) + WebSettings.Instance.CaptchaSolvationKey + <Module>.smethod_5<string>(1961668511);
					string text2 = httpRequest.Post(<Module>.smethod_3<string>(1656189201), text, <Module>.smethod_2<string>(1371120848)).ToString();
					if (text2.Contains(<Module>.smethod_3<string>(-2036481728)) || text2.Contains(<Module>.smethod_4<string>(1140338017)))
					{
						Match match = Regex.Match(text2, <Module>.smethod_5<string>(643137585));
						if (match.Success)
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value + <Module>.smethod_4<string>(1401823264));
						}
					}
					Match match2 = Regex.Match(text2, <Module>.smethod_2<string>(-538251435));
					if (match2.Success)
					{
						return new ValueTuple<OperationResult, string>(OperationResult.Error, match2.Groups[1].Value);
					}
					return new ValueTuple<OperationResult, string>(OperationResult.Error, <Module>.smethod_2<string>(-1503871187));
				}
			}
			catch
			{
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00039AEC File Offset: 0x00037CEC
		[return: TupleElementNames(new string[]
		{
			"status",
			"solution"
		})]
		public ValueTuple<OperationResult, string> SolveCaptcha(string base64, string lang, bool onlyLetters = false)
		{
			if (lang == null)
			{
				lang = <Module>.smethod_3<string>(-1051087952);
			}
			string lang2 = (lang == <Module>.smethod_3<string>(-10129125)) ? <Module>.smethod_5<string>(-1256376075) : lang;
			ValueTuple<OperationResult, string> valueTuple = this.SendCaptchaRequest(base64, lang2, onlyLetters);
			OperationResult item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item == OperationResult.Ok)
			{
				return this.GetResult(item2);
			}
			return new ValueTuple<OperationResult, string>(item, null);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00039B50 File Offset: 0x00037D50
		[return: TupleElementNames(new string[]
		{
			"status",
			"solution"
		})]
		public ValueTuple<OperationResult, string> SolveRecaptchaV2Proxyless(string siteKey, string pageUrl)
		{
			ValueTuple<OperationResult, string> valueTuple = this.SendRecaptchaV2ProxylessRequest(siteKey, pageUrl);
			OperationResult item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item == OperationResult.Ok)
			{
				return this.GetResult(item2);
			}
			return new ValueTuple<OperationResult, string>(item, null);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00039B84 File Offset: 0x00037D84
		[return: TupleElementNames(new string[]
		{
			"status",
			"solution"
		})]
		public ValueTuple<OperationResult, string> SolveHCaptcha(string siteKey, string pageUrl, string userAgent)
		{
			ValueTuple<OperationResult, string> valueTuple = this.SendHCaptchaRequest(siteKey, pageUrl, userAgent);
			OperationResult item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item == OperationResult.Ok)
			{
				return this.GetResult(item2);
			}
			return new ValueTuple<OperationResult, string>(item, null);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00039BBC File Offset: 0x00037DBC
		[return: TupleElementNames(new string[]
		{
			"status",
			"id"
		})]
		private ValueTuple<OperationResult, string> SendCaptchaRequest(string base64, string lang, bool onlyLetters)
		{
			for (int i = 0; i < 3; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						string text = string.Concat(new string[]
						{
							<Module>.smethod_2<string>(1409307914),
							lang,
							<Module>.smethod_5<string>(-2016181539),
							WebSettings.Instance.CaptchaSolvationKey,
							<Module>.smethod_5<string>(-723479038),
							base64,
							<Module>.smethod_3<string>(1255943249),
							(onlyLetters ? 2 : 0).ToString(),
							<Module>.smethod_2<string>(-1104022589)
						});
						string text2 = httpRequest.Post(<Module>.smethod_3<string>(413113823), text, <Module>.smethod_6<string>(-989874009)).ToString();
						if (text2.Contains(<Module>.smethod_5<string>(2130957513)))
						{
							Match match = Regex.Match(text2, <Module>.smethod_6<string>(-1671208907));
							if (match.Success)
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
							}
						}
						return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00039D28 File Offset: 0x00037F28
		[return: TupleElementNames(new string[]
		{
			"status",
			"id"
		})]
		private ValueTuple<OperationResult, string> SendRecaptchaV2ProxylessRequest(string siteKey, string pageUrl)
		{
			for (int i = 0; i < 3; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						string text = string.Concat(new string[]
						{
							<Module>.smethod_3<string>(211063408),
							WebSettings.Instance.CaptchaSolvationKey,
							<Module>.smethod_3<string>(-1834431313),
							pageUrl,
							<Module>.smethod_6<string>(1587817566),
							siteKey,
							<Module>.smethod_6<string>(-485793865)
						});
						string text2 = httpRequest.Post(<Module>.smethod_4<string>(1576146762), text, <Module>.smethod_4<string>(-423852883)).ToString();
						if (text2.Contains(<Module>.smethod_3<string>(-2036481728)))
						{
							Match match = Regex.Match(text2, <Module>.smethod_4<string>(-1421010937));
							if (match.Success)
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
							}
						}
						return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00039E58 File Offset: 0x00038058
		[return: TupleElementNames(new string[]
		{
			"status",
			"id"
		})]
		private ValueTuple<OperationResult, string> SendHCaptchaRequest(string siteKey, string pageUrl, string userAgent)
		{
			for (int i = 0; i < 3; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						string text = string.Concat(new string[]
						{
							<Module>.smethod_6<string>(263307015),
							WebSettings.Instance.CaptchaSolvationKey,
							<Module>.smethod_3<string>(1377173498),
							pageUrl,
							<Module>.smethod_6<string>(1587817566),
							siteKey,
							<Module>.smethod_3<string>(1417583581),
							userAgent,
							<Module>.smethod_6<string>(-485793865)
						});
						string text2 = httpRequest.Post(<Module>.smethod_6<string>(-776093378), text, <Module>.smethod_6<string>(-989874009)).ToString();
						if (text2.Contains(<Module>.smethod_4<string>(167133821)))
						{
							Match match = Regex.Match(text2, <Module>.smethod_2<string>(1711237442));
							if (match.Success)
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
							}
						}
						return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00039F9C File Offset: 0x0003819C
		[return: TupleElementNames(new string[]
		{
			"status",
			"result"
		})]
		private ValueTuple<OperationResult, string> GetResult(string id)
		{
			if (id == null)
			{
				return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
			}
			for (int i = 0; i < 20; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						string text = string.Concat(new string[]
						{
							<Module>.smethod_5<string>(-644398855),
							WebSettings.Instance.CaptchaSolvationKey,
							<Module>.smethod_2<string>(-652490350),
							id,
							<Module>.smethod_3<string>(-1035668440)
						});
						string text2 = httpRequest.Post(<Module>.smethod_3<string>(-1994144206), text, <Module>.smethod_6<string>(-989874009)).ToString();
						if (text2.Contains(<Module>.smethod_4<string>(-1677687865)))
						{
							Match match = Regex.Match(text2, <Module>.smethod_2<string>(362088937));
							if (match.Success)
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
							}
							match = Regex.Match(text2, <Module>.smethod_3<string>(936517463));
							if (match.Success)
							{
								return new ValueTuple<OperationResult, string>(OperationResult.Ok, match.Groups[1].Value);
							}
						}
						else if (text2.Contains(<Module>.smethod_6<string>(-1970157345)))
						{
							Thread.Sleep(10000);
							goto IL_143;
						}
						return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
					}
				}
				catch
				{
				}
				IL_143:;
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0000BAF5 File Offset: 0x00009CF5
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}
	}
}
