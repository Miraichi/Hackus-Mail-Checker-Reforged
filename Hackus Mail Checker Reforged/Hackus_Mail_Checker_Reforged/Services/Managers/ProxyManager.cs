using System;
using System.Collections.Generic;
using System.Linq;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.UI.Models;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Services.Managers
{
	// Token: 0x02000075 RID: 117
	internal class ProxyManager : BindableObject
	{
		// Token: 0x0600041B RID: 1051 RVA: 0x00009037 File Offset: 0x00007237
		private ProxyManager()
		{
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00009060 File Offset: 0x00007260
		public static ProxyManager Instance
		{
			get
			{
				ProxyManager result;
				if ((result = ProxyManager._instance) == null)
				{
					result = (ProxyManager._instance = new ProxyManager());
				}
				return result;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00006D69 File Offset: 0x00004F69
		public ProxySettings ProxySettings
		{
			get
			{
				return ProxySettings.Instance;
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00019574 File Offset: 0x00017774
		public ProxyClient GetProxy()
		{
			if (this._proxies != null && this._proxies.Any<Proxy>())
			{
				object locker = this._locker;
				Proxy proxy;
				lock (locker)
				{
					proxy = this._proxies.ElementAtOrDefault(this._random.Next(StatisticsManager.Instance.LoadedProxy));
				}
				string text;
				if (!proxy.UseAuthentication)
				{
					if (this.ProxySettings.UseAuthentication)
					{
						text = string.Format(<Module>.smethod_3<string>(-546958702), new object[]
						{
							proxy.Host,
							proxy.Port,
							this.ProxySettings.Login,
							this.ProxySettings.Password
						});
					}
					else
					{
						text = string.Format(<Module>.smethod_5<string>(56194156), proxy.Host, proxy.Port);
					}
				}
				else
				{
					text = string.Format(<Module>.smethod_6<string>(2140435665), new object[]
					{
						proxy.Host,
						proxy.Port,
						proxy.Username,
						proxy.Password
					});
				}
				ProxyClient proxyClient = ProxyClient.Parse((ProxyType)Enum.Parse(typeof(ProxyType), this.ProxySettings.ProxyType.ToString(), true), text);
				proxyClient.ConnectTimeout = CheckerSettings.Instance.Timeout * 1000;
				proxyClient.ReadWriteTimeout = CheckerSettings.Instance.Timeout * 1000;
				return proxyClient;
			}
			return null;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00019714 File Offset: 0x00017914
		public void UploadProxies(List<Proxy> proxies)
		{
			if (proxies != null && proxies.Any<Proxy>())
			{
				object locker = this._locker;
				lock (locker)
				{
					this._proxies = proxies;
					StatisticsManager.Instance.LoadedProxy = proxies.Count;
				}
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00009076 File Offset: 0x00007276
		public int Count()
		{
			return this._proxies.Count;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00009083 File Offset: 0x00007283
		public bool Any()
		{
			return this._proxies.Any<Proxy>();
		}

		// Token: 0x04000241 RID: 577
		private static ProxyManager _instance;

		// Token: 0x04000242 RID: 578
		private object _locker = new object();

		// Token: 0x04000243 RID: 579
		private Random _random = new Random();

		// Token: 0x04000244 RID: 580
		private List<Proxy> _proxies = new List<Proxy>();
	}
}
