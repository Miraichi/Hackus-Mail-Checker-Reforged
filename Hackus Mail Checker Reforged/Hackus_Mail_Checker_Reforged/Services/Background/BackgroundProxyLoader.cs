using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;

namespace Hackus_Mail_Checker_Reforged.Services.Background
{
	// Token: 0x02000080 RID: 128
	public static class BackgroundProxyLoader
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001A1C8 File Offset: 0x000183C8
		public static void Start()
		{
			if (ProxySettings.Instance.UseAutoUpdate && ProxySettings.Instance.UseWebSources && !string.IsNullOrWhiteSpace(ProxySettings.Instance.WebLinks))
			{
				int updateDelay = ProxySettings.Instance.UpdateDelay;
				BackgroundProxyLoader._timer = new Timer(new TimerCallback(BackgroundProxyLoader.Load), null, updateDelay * 60000, updateDelay * 60000);
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000968C File Offset: 0x0000788C
		public static void Stop()
		{
			Timer timer = BackgroundProxyLoader._timer;
			if (timer != null)
			{
				timer.Change(-1, -1);
			}
			Timer timer2 = BackgroundProxyLoader._timer;
			if (timer2 == null)
			{
				return;
			}
			timer2.method_0();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001A230 File Offset: 0x00018430
		public static Task<bool> LoadManually(string proxyFilePath, string proxyUrls)
		{
			BackgroundProxyLoader.<LoadManually>d__4 <LoadManually>d__;
			<LoadManually>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<LoadManually>d__.proxyFilePath = proxyFilePath;
			<LoadManually>d__.proxyUrls = proxyUrls;
			<LoadManually>d__.<>1__state = -1;
			<LoadManually>d__.<>t__builder.Start<BackgroundProxyLoader.<LoadManually>d__4>(ref <LoadManually>d__);
			return <LoadManually>d__.<>t__builder.Task;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001A27C File Offset: 0x0001847C
		public static void Load(object obj)
		{
			List<Proxy> list = new List<Proxy>();
			if (ProxySettings.Instance.UseProxy && ProxySettings.Instance.UseWebSources && !string.IsNullOrEmpty(ProxySettings.Instance.WebLinks))
			{
				using (StringReader stringReader = new StringReader(ProxySettings.Instance.WebLinks))
				{
					string url;
					while ((url = stringReader.ReadLine()) != null)
					{
						try
						{
							List<Proxy> result = BackgroundProxyLoader.GetFromUrl(url).Result;
							if (result.Any<Proxy>())
							{
								list = list.Union(result).ToList<Proxy>();
							}
						}
						catch
						{
						}
					}
				}
			}
			if (list.Any<Proxy>())
			{
				ProxyManager.Instance.UploadProxies(list);
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001A338 File Offset: 0x00018538
		private static Task<List<Proxy>> GetFromFile(string filePath)
		{
			BackgroundProxyLoader.<GetFromFile>d__6 <GetFromFile>d__;
			<GetFromFile>d__.<>t__builder = AsyncTaskMethodBuilder<List<Proxy>>.Create();
			<GetFromFile>d__.filePath = filePath;
			<GetFromFile>d__.<>1__state = -1;
			<GetFromFile>d__.<>t__builder.Start<BackgroundProxyLoader.<GetFromFile>d__6>(ref <GetFromFile>d__);
			return <GetFromFile>d__.<>t__builder.Task;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001A37C File Offset: 0x0001857C
		private static Task<List<Proxy>> GetFromUrl(string url)
		{
			BackgroundProxyLoader.<GetFromUrl>d__7 <GetFromUrl>d__;
			<GetFromUrl>d__.<>t__builder = AsyncTaskMethodBuilder<List<Proxy>>.Create();
			<GetFromUrl>d__.url = url;
			<GetFromUrl>d__.<>1__state = -1;
			<GetFromUrl>d__.<>t__builder.Start<BackgroundProxyLoader.<GetFromUrl>d__7>(ref <GetFromUrl>d__);
			return <GetFromUrl>d__.<>t__builder.Task;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001A3C0 File Offset: 0x000185C0
		private static Proxy GetProxyFromString(string line)
		{
			if (string.IsNullOrEmpty(line))
			{
				return null;
			}
			string[] array = line.Split(new char[]
			{
				':'
			});
			if (array.Length < 2 || string.IsNullOrWhiteSpace(array[0]) || string.IsNullOrWhiteSpace(array[1]))
			{
				return null;
			}
			int port;
			if (!int.TryParse(array[1], out port))
			{
				return null;
			}
			if (array.Length < 4)
			{
				return new Proxy(array[0], port);
			}
			return new Proxy(array[0], port, array[2], array[3]);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00009684 File Offset: 0x00007884
		void method_0()
		{
			base.Dispose();
		}

		// Token: 0x0400027D RID: 637
		private static Timer _timer;

		// Token: 0x0400027E RID: 638
		private static HttpClient _httpClient = new HttpClient();
	}
}
