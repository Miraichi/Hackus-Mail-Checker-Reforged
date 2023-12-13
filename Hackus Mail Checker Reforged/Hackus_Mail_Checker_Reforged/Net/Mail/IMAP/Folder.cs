using System;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.IMAP
{
	// Token: 0x0200012A RID: 298
	public class Folder
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0000B988 File Offset: 0x00009B88
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x0000B990 File Offset: 0x00009B90
		public string Name { get; set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0000B999 File Offset: 0x00009B99
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0000B9A1 File Offset: 0x00009BA1
		public int MessageCount { get; set; } = -1;

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0000B9AA File Offset: 0x00009BAA
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x0000B9B2 File Offset: 0x00009BB2
		public bool IsDeletedMessages { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0000B9BB File Offset: 0x00009BBB
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0000B9C3 File Offset: 0x00009BC3
		public bool IsSearchedAttachments { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0000B9CC File Offset: 0x00009BCC
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0000B9D4 File Offset: 0x00009BD4
		public bool IsParsedContacts { get; set; }

		// Token: 0x06000943 RID: 2371 RVA: 0x0000B9DD File Offset: 0x00009BDD
		public static Folder Parse(string name)
		{
			name;
			-1;
			return new Folder();
		}
	}
}
