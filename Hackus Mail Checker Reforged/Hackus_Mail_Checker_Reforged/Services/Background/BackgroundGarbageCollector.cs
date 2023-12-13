using System;
using System.Runtime;
using System.Threading;

namespace Hackus_Mail_Checker_Reforged.Services.Background
{
	// Token: 0x0200007F RID: 127
	internal static class BackgroundGarbageCollector
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x00009628 File Offset: 0x00007828
		public static void Start()
		{
			BackgroundGarbageCollector._timer = new Timer(new TimerCallback(BackgroundGarbageCollector.Collect), null, 30000, 30000);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000964B File Offset: 0x0000784B
		public static void Stop()
		{
			Timer timer = BackgroundGarbageCollector._timer;
			if (timer != null)
			{
				timer.Change(-1, -1);
			}
			Timer timer2 = BackgroundGarbageCollector._timer;
			if (timer2 == null)
			{
				return;
			}
			timer2.method_0();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000966F File Offset: 0x0000786F
		private static void Collect(object obj)
		{
			GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00009684 File Offset: 0x00007884
		void method_0()
		{
			base.Dispose();
		}

		// Token: 0x0400027C RID: 636
		private static Timer _timer;
	}
}
