using System;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000B0 RID: 176
	public class RediffmailClient
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00009C69 File Offset: 0x00007E69
		public RediffmailClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0002384C File Offset: 0x00021A4C
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

		// Token: 0x0600059B RID: 1435 RVA: 0x000238BC File Offset: 0x00021ABC
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

		// Token: 0x0600059C RID: 1436 RVA: 0x000238DC File Offset: 0x00021ADC
		private OperationResult Authenticate()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = false;
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

		// Token: 0x0600059D RID: 1437 RVA: 0x00023944 File Offset: 0x00021B44
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_3<string>(1998876527);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00009C78 File Offset: 0x00007E78
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002F1 RID: 753
		private Mailbox _mailbox;

		// Token: 0x040002F2 RID: 754
		private ProxyClient _proxyClient;

		// Token: 0x040002F3 RID: 755
		private CookieDictionary _cookies;
	}
}
