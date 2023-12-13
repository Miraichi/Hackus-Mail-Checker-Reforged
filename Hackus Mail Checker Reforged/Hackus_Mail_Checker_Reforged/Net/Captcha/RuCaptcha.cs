using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Captcha
{
	// Token: 0x0200012F RID: 303
	internal class RuCaptcha : ICaptchaClient
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x0003A134 File Offset: 0x00038334
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
					httpRequest.AddUrlParam(<Module>.smethod_5<string>(1530899803), WebSettings.Instance.CaptchaSolvationKey);
					httpRequest.AddUrlParam(<Module>.smethod_5<string>(1334194988), <Module>.smethod_4<string>(1387398307));
					string text = httpRequest.Post(<Module>.smethod_3<string>(415041262)).ToString();
					try
					{
						double.Parse(text);
						return new ValueTuple<OperationResult, string>(OperationResult.Ok, text + <Module>.smethod_3<string>(-708731306));
					}
					catch
					{
						return new ValueTuple<OperationResult, string>(OperationResult.Error, text);
					}
				}
			}
			catch
			{
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0003A204 File Offset: 0x00038404
		[return: TupleElementNames(new string[]
		{
			"status",
			"solution"
		})]
		public ValueTuple<OperationResult, string> SolveCaptcha(string base64, string lang, bool onlyLetters = false)
		{
			ValueTuple<OperationResult, string> valueTuple = this.SendCaptchaRequest(base64, lang, onlyLetters);
			OperationResult item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item == OperationResult.Ok)
			{
				return this.GetResult(item2);
			}
			return new ValueTuple<OperationResult, string>(item, null);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0003A23C File Offset: 0x0003843C
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

		// Token: 0x06000975 RID: 2421 RVA: 0x0003A270 File Offset: 0x00038470
		[return: TupleElementNames(new string[]
		{
			"status",
			"solution"
		})]
		public ValueTuple<OperationResult, string> SolveHCaptcha(string siteKey, string pageUrl, string userAgent)
		{
			ValueTuple<OperationResult, string> valueTuple = this.SendHcaptchaRequest(siteKey, pageUrl);
			OperationResult item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item != OperationResult.Ok)
			{
				return new ValueTuple<OperationResult, string>(item, null);
			}
			return this.GetResult(item2);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0003A2A4 File Offset: 0x000384A4
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
						httpRequest.AddParam(<Module>.smethod_4<string>(1461183639), WebSettings.Instance.CaptchaSolvationKey);
						httpRequest.AddParam(<Module>.smethod_6<string>(696161607), <Module>.smethod_2<string>(1648508631));
						httpRequest.AddParam(<Module>.smethod_3<string>(736394487), base64);
						if (lang != null)
						{
							httpRequest.AddParam(<Module>.smethod_2<string>(-203682217), lang);
						}
						if (onlyLetters)
						{
							httpRequest.AddParam(<Module>.smethod_4<string>(-1416202618), 2);
						}
						string text = httpRequest.Post(<Module>.smethod_3<string>(1900577138)).ToString();
						if (text.Contains(<Module>.smethod_6<string>(781329414)))
						{
							string[] array = text.Split(new char[]
							{
								'|'
							});
							return new ValueTuple<OperationResult, string>(OperationResult.Ok, array[1]);
						}
						if (!text.Contains(<Module>.smethod_4<string>(1935022403)) && !text.Contains(<Module>.smethod_5<string>(-1883056844)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
						}
						Thread.Sleep(5000);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0003A41C File Offset: 0x0003861C
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
						httpRequest.AddParam(<Module>.smethod_2<string>(-350759150), WebSettings.Instance.CaptchaSolvationKey);
						httpRequest.AddParam(<Module>.smethod_3<string>(-989674448), <Module>.smethod_2<string>(-1939058790));
						httpRequest.AddParam(<Module>.smethod_3<string>(1475208343), siteKey);
						httpRequest.AddParam(<Module>.smethod_2<string>(1806457197), pageUrl);
						string text = httpRequest.Post(<Module>.smethod_3<string>(1900577138)).ToString();
						if (text.Contains(<Module>.smethod_2<string>(-1735172287)))
						{
							string[] array = text.Split(new char[]
							{
								'|'
							});
							return new ValueTuple<OperationResult, string>(OperationResult.Ok, array[1]);
						}
						if (!text.Contains(<Module>.smethod_6<string>(-1525906967)) && !text.Contains(<Module>.smethod_6<string>(695448898)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
						}
						Thread.Sleep(5000);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0003A560 File Offset: 0x00038760
		[return: TupleElementNames(new string[]
		{
			"status",
			"id"
		})]
		private ValueTuple<OperationResult, string> SendHcaptchaRequest(string siteKey, string pageUrl)
		{
			for (int i = 0; i < 3; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AddParam(<Module>.smethod_2<string>(-350759150), WebSettings.Instance.CaptchaSolvationKey);
						httpRequest.AddParam(<Module>.smethod_5<string>(1867092025), <Module>.smethod_3<string>(-1655576302));
						httpRequest.AddParam(<Module>.smethod_2<string>(8500559), siteKey);
						httpRequest.AddParam(<Module>.smethod_5<string>(892299524), pageUrl);
						string text = httpRequest.Post(<Module>.smethod_2<string>(1877159998)).ToString();
						if (text.Contains(<Module>.smethod_6<string>(781329414)))
						{
							string[] array = text.Split(new char[]
							{
								'|'
							});
							return new ValueTuple<OperationResult, string>(OperationResult.Ok, array[1]);
						}
						if (!text.Contains(<Module>.smethod_5<string>(359402487)) && !text.Contains(<Module>.smethod_2<string>(-557270595)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
						}
						Thread.Sleep(5000);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0003A6A0 File Offset: 0x000388A0
		[return: TupleElementNames(new string[]
		{
			null,
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
						httpRequest.AddUrlParam(<Module>.smethod_5<string>(1530899803), WebSettings.Instance.CaptchaSolvationKey);
						httpRequest.AddUrlParam(<Module>.smethod_4<string>(-975585554), <Module>.smethod_4<string>(971434845));
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(772680489), id);
						string text = httpRequest.Get(<Module>.smethod_4<string>(598134247), null).ToString();
						if (text.Contains(<Module>.smethod_6<string>(781329414)))
						{
							string[] array = text.Split(new char[]
							{
								'|'
							});
							return new ValueTuple<OperationResult, string>(OperationResult.Ok, array[1]);
						}
						if (!text.Contains(<Module>.smethod_6<string>(549434249)))
						{
							return new ValueTuple<OperationResult, string>(OperationResult.Error, null);
						}
						Thread.Sleep(10000);
					}
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, string>(OperationResult.HttpError, null);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0000BAF5 File Offset: 0x00009CF5
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}
	}
}
