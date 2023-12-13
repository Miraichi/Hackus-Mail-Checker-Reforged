using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Background;

namespace Hackus_Mail_Checker_Reforged.Services.Shared
{
	// Token: 0x0200006A RID: 106
	public static class ReportService
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00017ACC File Offset: 0x00015CCC
		static ReportService()
		{
			ReportService._httpClient.BaseAddress = new Uri(<Module>.smethod_3<string>(-831690586));
			ReportService._httpClient.DefaultRequestHeaders.Add(<Module>.smethod_3<string>(-1674520012), BackgroundAuthenticator.Instance.Token);
			ReportService._httpClient.Timeout = TimeSpan.FromSeconds(30.0);
			ReportService._locker = new object();
			ReportService._foundServers = new HashSet<Server>();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00017B4C File Offset: 0x00015D4C
		public static void AddServer(Server server)
		{
			object locker = ReportService._locker;
			lock (locker)
			{
				ReportService._foundServers.Add(server);
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00008CA1 File Offset: 0x00006EA1
		public static void ReportServers()
		{
			if (!ReportService._foundServers.Any<Server>())
			{
				return;
			}
			Task.Run(delegate()
			{
				try
				{
					StringContent httpContent_ = null;
					object locker = ReportService._locker;
					bool flag = false;
					try
					{
						ReportService.<>c.smethod_0(locker, ref flag);
						httpContent_ = ReportService.<>c.smethod_3(ReportService.<>c.smethod_1(ReportService._foundServers), ReportService.<>c.smethod_2(), <Module>.smethod_2<string>(1371120848));
						ReportService._foundServers.Clear();
					}
					finally
					{
						if (flag)
						{
							ReportService.<>c.smethod_4(locker);
						}
					}
					ReportService.<>c.smethod_5(ReportService._httpClient, <Module>.smethod_3<string>(301653041), httpContent_);
				}
				catch
				{
				}
			});
		}

		// Token: 0x04000214 RID: 532
		private static object _locker;

		// Token: 0x04000215 RID: 533
		private static HashSet<Server> _foundServers;

		// Token: 0x04000216 RID: 534
		private static HttpClient _httpClient = new HttpClient();
	}
}
