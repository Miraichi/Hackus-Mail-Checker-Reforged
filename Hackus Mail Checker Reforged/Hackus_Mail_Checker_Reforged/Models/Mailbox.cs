using System;

namespace Hackus_Mail_Checker_Reforged.Models
{
	// Token: 0x02000133 RID: 307
	public class Mailbox
	{
		// Token: 0x06000988 RID: 2440 RVA: 0x0000619C File Offset: 0x0000439C
		public Mailbox()
		{
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0000BBAE File Offset: 0x00009DAE
		public Mailbox(string address, string password)
		{
			this.Address = address;
			this.Password = password;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0000BBC4 File Offset: 0x00009DC4
		public Mailbox(string address, string password, string domain) : this(address, password)
		{
			this.Domain = domain;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0000BBD5 File Offset: 0x00009DD5
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0000BBDD File Offset: 0x00009DDD
		public string Address { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0000BBE6 File Offset: 0x00009DE6
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0000BBEE File Offset: 0x00009DEE
		public string Password { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0000BBF7 File Offset: 0x00009DF7
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0000BBFF File Offset: 0x00009DFF
		public string Domain { get; set; }

		// Token: 0x06000991 RID: 2449 RVA: 0x0003A7CC File Offset: 0x000389CC
		public static Mailbox GetFromString(string value)
		{
			string[] array;
			if (value.Contains(<Module>.smethod_6<string>(1827648947)))
			{
				array = value.Split(new char[]
				{
					':'
				});
			}
			else
			{
				if (!value.Contains(<Module>.smethod_6<string>(-635980793)))
				{
					return null;
				}
				array = value.Split(new char[]
				{
					';'
				});
			}
			if (array.Length < 2 || string.IsNullOrWhiteSpace(array[0]) || string.IsNullOrWhiteSpace(array[1]))
			{
				return null;
			}
			string[] array2 = array[0].Split(new char[]
			{
				'@'
			});
			if (array2.Length < 2)
			{
				return null;
			}
			return new Mailbox(array[0], array[1], array2[1].ToLower());
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0003A870 File Offset: 0x00038A70
		public static Mailbox Get(string address, string password)
		{
			if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(password))
			{
				return null;
			}
			string[] array = address.Split(new char[]
			{
				'@'
			});
			if (array.Length >= 2)
			{
				return new Mailbox(address, password, array[1]);
			}
			return null;
		}
	}
}
