using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Shared;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x0200009D RID: 157
	public static class HostFinderHandler
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x0001D1AC File Offset: 0x0001B3AC
		public static Server Find(string domain)
		{
			Func<HostFinderContext, bool> <>9__0;
			Server result;
			for (;;)
			{
				HostFinderContext hostFinderContext = null;
				object locker = HostFinderHandler._locker;
				lock (locker)
				{
					IEnumerable<HostFinderContext> contexts = HostFinderHandler._contexts;
					Func<HostFinderContext, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((HostFinderContext c) => HostFinderHandler.<>c__DisplayClass4_0.smethod_0(c.Domain, domain)));
					}
					hostFinderContext = contexts.FirstOrDefault(predicate);
					if (hostFinderContext == null)
					{
						hostFinderContext = new HostFinderContext(domain);
						HostFinderHandler._contexts.Add(hostFinderContext);
					}
				}
				hostFinderContext.Handle();
				locker = HostFinderHandler._locker;
				lock (locker)
				{
					if (hostFinderContext.IsHandled)
					{
						if (hostFinderContext.Result == null)
						{
							result = null;
							break;
						}
						if (hostFinderContext.IsResultSaved)
						{
							result = hostFinderContext.Result;
							break;
						}
						try
						{
							ReportService.AddServer(hostFinderContext.Result);
							ConfigurationManager.Instance.Add(hostFinderContext.Result);
						}
						catch
						{
						}
						finally
						{
							hostFinderContext.IsResultSaved = true;
						}
						result = hostFinderContext.Result;
						break;
					}
				}
				Thread.Sleep(1000);
			}
			return result;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00009A21 File Offset: 0x00007C21
		public static void Clear()
		{
			HostFinderHandler._contexts.Clear();
		}

		// Token: 0x040002BB RID: 699
		private static object _locker = new object();

		// Token: 0x040002BC RID: 700
		private static List<HostFinderContext> _contexts = new List<HostFinderContext>();
	}
}
