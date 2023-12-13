using System;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000014 RID: 20
	internal static class RandomExtensions
	{
		// Token: 0x06000064 RID: 100 RVA: 0x0001076C File Offset: 0x0000E96C
		public static void Shuffle<T>(this Random rng, T[] array)
		{
			int i = array.Length;
			while (i > 1)
			{
				int num = rng.Next(i--);
				T t = array[i];
				array[i] = array[num];
				array[num] = t;
			}
		}
	}
}
