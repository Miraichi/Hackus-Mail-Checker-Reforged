using System;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000F2 RID: 242
	internal class Pop3 : Client
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x0000AB55 File Offset: 0x00008D55
		internal override void CheckResultOK(string result)
		{
			if (!result.StartsWith(<Module>.smethod_3<string>(-1876702699), StringComparison.OrdinalIgnoreCase))
			{
				throw new AuthenticationException(result.Substring(result.IndexOf(' ') + 1).Trim());
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000AB85 File Offset: 0x00008D85
		internal override void OnLogin(string username, string password)
		{
			this.SendCommandCheckOK(<Module>.smethod_6<string>(-1989793714) + username);
			this.SendCommandCheckOK(<Module>.smethod_5<string>(488153875) + password);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002E20C File Offset: 0x0002C40C
		internal override void OnLogout()
		{
			if (this._Stream != null)
			{
				try
				{
					this.SendCommand(<Module>.smethod_3<string>(1294492029));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002E248 File Offset: 0x0002C448
		public virtual int GetMessageCount()
		{
			this.CheckConnectionStatus();
			string text = this.SendCommandGetResponse(<Module>.smethod_3<string>(1013548887));
			this.CheckResultOK(text);
			return int.Parse(text.Split(new char[]
			{
				' '
			})[1]);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002E28C File Offset: 0x0002C48C
		public virtual MailMessage GetMessage(int index, bool headersOnly = false, bool saveAttachments = true)
		{
			return this.GetMessage((index + 1).ToString(), headersOnly, saveAttachments);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002E2AC File Offset: 0x0002C4AC
		public virtual MailMessage GetMessage(string uid, bool headersOnly = false, bool saveAttachments = true)
		{
			this.CheckConnectionStatus();
			string text = this.SendCommandGetResponse(string.Format(headersOnly ? <Module>.smethod_5<string>(-81700223) : <Module>.smethod_2<string>(1186104759), uid));
			this.CheckResultOK(text);
			StringBuilder stringBuilder = new StringBuilder();
			while ((text = this.GetResponse()) != <Module>.smethod_3<string>(2035431732))
			{
				stringBuilder.AppendLine(text);
			}
			text = stringBuilder.ToString();
			MailMessage mailMessage = new MailMessage();
			mailMessage.Load(text, headersOnly, saveAttachments);
			return mailMessage;
		}

		// Token: 0x040003D1 RID: 977
		private static Regex rxOctets = new Regex(<Module>.smethod_5<string>(-461602955), RegexOptions.IgnoreCase);
	}
}
