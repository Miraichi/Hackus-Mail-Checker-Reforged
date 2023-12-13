using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000E8 RID: 232
	internal class Imap : Client
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x0000A475 File Offset: 0x00008675
		private string GetTag()
		{
			this._tag++;
			return string.Format(<Module>.smethod_2<string>(302084232), this._tag);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0000A49F File Offset: 0x0000869F
		internal override void CheckResultOK(string response)
		{
			if (!this.IsResultOK(response))
			{
				response = response.Substring(response.IndexOf(<Module>.smethod_4<string>(-405843621))).Trim();
				throw new Exception(response);
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002C734 File Offset: 0x0002A934
		internal bool IsResultOK(string response)
		{
			response = response.Substring(response.IndexOf(<Module>.smethod_6<string>(820097393))).Trim();
			return response.ToUpper().StartsWith(<Module>.smethod_4<string>(2007671458)) || response.ToUpper().StartsWith(<Module>.smethod_5<string>(466694997));
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002C78C File Offset: 0x0002A98C
		internal bool IsLoginOK(string response)
		{
			response = response.Substring(response.IndexOf(<Module>.smethod_3<string>(2023933234))).Trim();
			string text = response.ToUpper();
			return !text.StartsWith(<Module>.smethod_3<string>(-2072970797)) && !text.StartsWith(<Module>.smethod_4<string>(-1398893100)) && !text.StartsWith(<Module>.smethod_2<string>(1449946385));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002C7F8 File Offset: 0x0002A9F8
		internal override void OnLogin(string login, string password)
		{
			string command = string.Empty;
			string text = string.Empty;
			string tag = this.GetTag();
			command = string.Concat(new string[]
			{
				tag,
				<Module>.smethod_4<string>(2106810136),
				login.ToQuotedString(),
				<Module>.smethod_4<string>(-405843621),
				password.ToQuotedString()
			});
			text = this.SendCommandGetResponse(command);
			if (this.IsResultOK(text))
			{
				return;
			}
			if (text.StartsWith(<Module>.smethod_2<string>(-764178696)) && text.EndsWith(<Module>.smethod_2<string>(68158190)))
			{
				throw new AuthenticationException(Utils.DecodeBase64(text.Substring(2), Encoding.UTF7));
			}
			throw new AuthenticationException(text);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002C8A8 File Offset: 0x0002AAA8
		public virtual Folder SelectMailbox(string name)
		{
			string command = this.GetTag() + <Module>.smethod_4<string>(-1311731351) + Utf7Encoding.Encode(name).ToQuotedString();
			string result = this.SendCommandGetResponse(command);
			this.CheckResultOK(result);
			this._selectedMailbox = new Folder(name);
			return this._selectedMailbox;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
		public virtual List<Folder> ListMailboxes(string reference, string pattern)
		{
			List<Folder> list = new List<Folder>();
			string command = string.Concat(new string[]
			{
				this.GetTag(),
				<Module>.smethod_5<string>(81626204),
				reference.ToQuotedString(),
				<Module>.smethod_6<string>(820097393),
				pattern.ToQuotedString()
			});
			string text = base.SendCommandGetResponse(command);
			if (!text.StartsWith(<Module>.smethod_3<string>(2141242469)))
			{
				throw new Exception();
			}
			Match match = Regex.Match(text, <Module>.smethod_6<string>(-1263180039));
			while (match.Groups.Count > 1)
			{
				Folder item = new Folder(Utf7Encoding.Decode(match.Groups[3].Value));
				list.Add(item);
				match = Regex.Match(this.GetResponse(), <Module>.smethod_3<string>(1700520298));
			}
			return list;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002C9C8 File Offset: 0x0002ABC8
		public virtual int GetMessagesCount()
		{
			string command = this.GetTag() + <Module>.smethod_2<string>(217760901) + Utf7Encoding.Encode(this._selectedMailbox.Name).ToQuotedString() + <Module>.smethod_4<string>(503203);
			string text = this.SendCommandGetResponse(command);
			string pattern = <Module>.smethod_2<string>(-2029003870);
			int result = 0;
			while (text.StartsWith(<Module>.smethod_2<string>(-908283613)))
			{
				Match match = Regex.Match(text, pattern);
				if (match.Groups.Count > 1)
				{
					result = Convert.ToInt32(match.Groups[1].ToString());
				}
				text = this.GetResponse();
				match = Regex.Match(text, pattern);
			}
			return result;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0000A4CE File Offset: 0x000086CE
		public virtual string[] Search(SearchCondition criteria, bool uid = true)
		{
			return this.Search(criteria.ToString());
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002CA74 File Offset: 0x0002AC74
		public virtual string[] Search(string criteria)
		{
			if (this._selectedMailbox == null)
			{
				this._selectedMailbox = new Folder(<Module>.smethod_4<string>(-1159001420));
			}
			string tag = this.GetTag();
			string command = tag + <Module>.smethod_5<string>(-1095039053) + criteria;
			string text = this.SendCommandGetResponse(command);
			if (!text.StartsWith(<Module>.smethod_3<string>(1500397322), StringComparison.InvariantCultureIgnoreCase) && !this.IsResultOK(text))
			{
				throw new Exception(text);
			}
			string response;
			while (!(response = this.GetResponse()).StartsWith(tag))
			{
				text = text + Environment.NewLine + response;
			}
			return (from x in Regex.Match(text, <Module>.smethod_5<string>(387614814), RegexOptions.Multiline).Groups[1].Value.Trim().Split(new char[]
			{
				' '
			})
			where !Imap.<>c.smethod_0(x)
			select x).ToArray<string>();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002CB60 File Offset: 0x0002AD60
		public virtual MailMessage GetMessage(string uid, bool headersOnly, bool saveAttachments = true)
		{
			/*
An exception occurred when decompiling this method (06000700)

ICSharpCode.Decompiler.DecompilerException: Error decompiling Hackus_Mail_Checker_Reforged.Net.Mail.MailMessage Hackus_Mail_Checker_Reforged.Net.Mail.Imap::GetMessage(System.String,System.Boolean,System.Boolean)

 ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.IntroducePropertyAccessInstructions(ILExpression expr, ILExpression parentExpr, Int32 posInParent) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1587
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.IntroducePropertyAccessInstructions(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1579
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 244
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002CDAC File Offset: 0x0002AFAC
		public virtual List<MailMessage> GetMessages(string start, string end, bool headersOnly)
		{
			/*
An exception occurred when decompiling this method (06000701)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Collections.Generic.List`1<Hackus_Mail_Checker_Reforged.Net.Mail.MailMessage> Hackus_Mail_Checker_Reforged.Net.Mail.Imap::GetMessages(System.String,System.String,System.Boolean)

 ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.IntroducePropertyAccessInstructions(ILExpression expr, ILExpression parentExpr, Int32 posInParent) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1587
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.IntroducePropertyAccessInstructions(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 1579
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 244
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002CFE8 File Offset: 0x0002B1E8
		public List<AttachmentMessageInfo> GetAttachmentMessages(string messagesSet)
		{
			string tag = this.GetTag();
			string command = tag + <Module>.smethod_3<string>(1652400459) + messagesSet + <Module>.smethod_2<string>(-163043746);
			List<AttachmentMessageInfo> list = new List<AttachmentMessageInfo>();
			this.SendCommand(command);
			for (;;)
			{
				string response = this.GetResponse();
				if (string.IsNullOrEmpty(response) || response.Contains(tag + <Module>.smethod_2<string>(-1735172287)))
				{
					break;
				}
				if (response[0] == '*')
				{
					if (response.Contains(<Module>.smethod_5<string>(-1073580175)))
					{
						AttachmentMessageInfo attachmentMessageInfo = new AttachmentMessageInfo();
						Match match = Regex.Match(response, <Module>.smethod_6<string>(836378167));
						if (match.Success)
						{
							attachmentMessageInfo.Uid = match.Groups[1].Value;
							foreach (object obj in Regex.Matches(response, <Module>.smethod_5<string>(-2060294066)))
							{
								Match match2 = (Match)obj;
								if (match2.Success)
								{
									if (attachmentMessageInfo.Filenames == null)
									{
										attachmentMessageInfo.Filenames = new List<string>();
									}
									attachmentMessageInfo.Filenames.Add(match2.Groups[1].Value);
								}
							}
							if (attachmentMessageInfo.HasAttachments)
							{
								list.Add(attachmentMessageInfo);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002D160 File Offset: 0x0002B360
		public void SetMessageFlags(string messageSet, string flags)
		{
			string command = string.Concat(new string[]
			{
				this.GetTag(),
				<Module>.smethod_5<string>(2044721864),
				messageSet,
				<Module>.smethod_2<string>(-878417611),
				flags,
				<Module>.smethod_5<string>(-726659454)
			});
			string result = this.SendCommandGetResponse(command);
			this.CheckResultOK(result);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002D1C0 File Offset: 0x0002B3C0
		public void Expunge()
		{
			string command = this.GetTag() + <Module>.smethod_3<string>(-273791452);
			this.SendCommandGetResponse(command);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002D1EC File Offset: 0x0002B3EC
		public void DeleteMessages(List<MailMessage> messages)
		{
			try
			{
				string messageSet = string.Join(<Module>.smethod_3<string>(-854885990), from m in messages
				select m.Uid);
				this.SetMessageFlags(messageSet, <Module>.smethod_5<string>(905013668));
				this.Expunge();
			}
			catch
			{
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0000A4DC File Offset: 0x000086DC
		protected override string SendCommandGetResponse(string command)
		{
			return this.SendCommandGetResponse(command, this._selectedMailbox);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002D25C File Offset: 0x0002B45C
		protected virtual string SendCommandGetResponse(string command, Folder mailbox)
		{
			string response = base.SendCommandGetResponse(command);
			return this.HandleUntaggedResponse(response, mailbox);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002D27C File Offset: 0x0002B47C
		protected virtual string HandleUntaggedResponse(string response, Folder mailbox)
		{
			while (response.StartsWith(<Module>.smethod_6<string>(-1558364540)))
			{
				if (mailbox != null && !Regex.Match(response, <Module>.smethod_3<string>(-1116620878)).Success && !Regex.Match(response, <Module>.smethod_2<string>(1485310181)).Success && !Regex.Match(response, <Module>.smethod_3<string>(1773630708)).Success && !Regex.Match(response, <Module>.smethod_4<string>(-1098941301)).Success && !Regex.Match(response, <Module>.smethod_5<string>(-619763321)).Success && !response.StartsWith(<Module>.smethod_4<string>(1094527381)) && !response.StartsWith(<Module>.smethod_3<string>(409325081)))
				{
					return response;
				}
				response = this.GetResponse();
			}
			return response;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002D350 File Offset: 0x0002B550
		internal override void OnLogout()
		{
			if (this.IsConnected)
			{
				try
				{
					this.SendCommand(this.GetTag() + <Module>.smethod_4<string>(392425070));
				}
				catch
				{
				}
			}
		}

		// Token: 0x0400038E RID: 910
		private int _tag;

		// Token: 0x0400038F RID: 911
		private Folder _selectedMailbox;
	}
}
