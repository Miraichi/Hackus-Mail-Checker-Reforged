using System;
using System.Text;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.POP3
{
	// Token: 0x0200010E RID: 270
	public class Pop3Client : MailClient
	{
		// Token: 0x06000843 RID: 2115 RVA: 0x0000AFCB File Offset: 0x000091CB
		public override void Authenticate(string username, string password)
		{
			this.CheckOk(this.SendReceive(<Module>.smethod_4<string>(-1462013254) + username));
			this.CheckOk(this.SendReceive(<Module>.smethod_5<string>(488153875) + password));
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0003265C File Offset: 0x0003085C
		public MailMessage FetchMessage(Uid uid, bool onlyHeaders = true, bool skipAdditionalParts = true)
		{
			this.CheckOk(this.SendReceive(onlyHeaders ? (<Module>.smethod_2<string>(-1822120560) + uid.UID.ToString() + <Module>.smethod_5<string>(-1013969465)) : (<Module>.smethod_3<string>(1379100937) + uid.UID.ToString())));
			MailMessage mailMessage = MessageBuilder.FromMime822(this.LocalStream.ReadMsgAsString(base.Socket, 1048576, this.ReadWriteTimeout), onlyHeaders, Encoding.UTF8, skipAdditionalParts);
			mailMessage.Uid = uid;
			return mailMessage;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000326F0 File Offset: 0x000308F0
		public int GetMessagesCount()
		{
			string text = this.SendReceive(<Module>.smethod_5<string>(-1754305456));
			this.CheckOk(text);
			int result;
			if (!int.TryParse(text.Split(new char[]
			{
				' '
			})[1], out result))
			{
				return -1;
			}
			return result;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00032734 File Offset: 0x00030934
		protected override void CheckOk(string response)
		{
			if (string.IsNullOrEmpty(response))
			{
				throw new MailException();
			}
			if (response.IsBrokenEncoding())
			{
				if (!response.Contains(<Module>.smethod_5<string>(-824018099)))
				{
					throw new EncodingException();
				}
				response = response.Substring(response.IndexOf(<Module>.smethod_6<string>(-492713005)));
				throw new MailException();
			}
			else
			{
				if (!response.StartsWith(<Module>.smethod_4<string>(290838364), StringComparison.OrdinalIgnoreCase))
				{
					throw new MailException(response.Substring(response.IndexOf(' ') + 1).Trim());
				}
				return;
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0000B005 File Offset: 0x00009205
		private string SendReceive(string command)
		{
			this.LocalStream.WriteLine(command ?? "", this.ReadWriteTimeout);
			return base.ReadLine();
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000327BC File Offset: 0x000309BC
		public void DeleteMessage(decimal uid)
		{
			string response = this.SendReceive(<Module>.smethod_3<string>(-2032626850) + uid.ToString());
			this.CheckOk(response);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000327F0 File Offset: 0x000309F0
		public void Quit()
		{
			string response = this.SendReceive(<Module>.smethod_4<string>(731455428));
			this.CheckOk(response);
		}
	}
}
