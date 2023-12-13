using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using DnsClient;
using DnsClient.Protocol;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail;
using Hackus_Mail_Checker_Reforged.Net.Mail.IMAP;
using Hackus_Mail_Checker_Reforged.Net.Mail.POP3;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using xNet;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x02000099 RID: 153
	public class HostFinderContext
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00009974 File Offset: 0x00007B74
		public string Domain
		{
			get
			{
				return this._domain;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001BB94 File Offset: 0x00019D94
		public bool HasWork
		{
			get
			{
				object contextLocker = this._contextLocker;
				bool result;
				lock (contextLocker)
				{
					if (this._serversToPing != null && this._serversToPing.Any<Server>())
					{
						result = true;
					}
					else if (this._webConfigs != null && this._webConfigs.Any<HostFinderContext.WebConfig>())
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
				return result;
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000997C File Offset: 0x00007B7C
		public HostFinderContext(string domain)
		{
			this._domain = domain;
			this._serversToPing = new ConcurrentQueue<Server>();
			this._webConfigs = new ConcurrentQueue<HostFinderContext.WebConfig>();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001BC04 File Offset: 0x00019E04
		private void Start()
		{
			object contextLocker = this._contextLocker;
			lock (contextLocker)
			{
				this.IsStarted = true;
				Server server = this.FindCoincidence();
				if (server != null)
				{
					this.IsHandled = true;
					this.Result = server;
					return;
				}
			}
			if (CheckerSettings.Instance.GuessSubdomainByDomain)
			{
				foreach (Server item in this.GenerateServersByDomain(this._domain))
				{
					this._serversToPing.Enqueue(item);
				}
			}
			if (CheckerSettings.Instance.SearchServerByMX)
			{
				string text;
				if (CheckerSettings.Instance.UseProxyToSearchServer)
				{
					text = this.FindMxRecordByGoogle();
				}
				else
				{
					text = this.FindMXRecordByLookup();
				}
				if (text != null)
				{
					Server server2 = this.FindCoincidence(text);
					if (server2 != null)
					{
						this.IsHandled = true;
						this.Result = server2;
						return;
					}
					if (CheckerSettings.Instance.GuessSubdomainByMX)
					{
						foreach (Server item2 in this.GenerateServersByMx(text))
						{
							this._serversToPing.Enqueue(item2);
						}
						this._serversToPing = new ConcurrentQueue<Server>(this._serversToPing.Distinct<Server>());
					}
				}
			}
			if (CheckerSettings.Instance.SearchServerByAutoConfig)
			{
				this._webConfigs.Enqueue(HostFinderContext.WebConfig.AutoConfig);
			}
			if (CheckerSettings.Instance.SearchServerByAutoDiscover)
			{
				this._webConfigs.Enqueue(HostFinderContext.WebConfig.AutoDiscovery);
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		public void Handle()
		{
			bool flag = false;
			if (!this.IsStarted)
			{
				this.Start();
				flag = true;
			}
			while (!this.IsHandled)
			{
				Server server = this.GetServerWorkItem();
				if (server == null)
				{
					HostFinderContext.WebConfig webConfigWorkItem = this.GetWebConfigWorkItem();
					if (webConfigWorkItem == HostFinderContext.WebConfig.None)
					{
						if (flag)
						{
							while (this._inProcess > 0)
							{
								Thread.Sleep(1000);
							}
							if (this.Result != null)
							{
								ConfigurationManager.Instance.Configuration.Add(this.Result);
							}
							this.IsHandled = true;
							return;
						}
						if (!this.HasWork)
						{
							return;
						}
					}
					else
					{
						Interlocked.Increment(ref this._inProcess);
						if (webConfigWorkItem != HostFinderContext.WebConfig.AutoConfig)
						{
							server = this.FindServerByAutoDiscover("", 0);
						}
						else
						{
							server = this.FindServerByAutoConfig("", 0);
						}
						if (server != null && this.Result == null)
						{
							this.IsHandled = true;
							this.Result = server;
							Interlocked.Decrement(ref this._inProcess);
							return;
						}
						Interlocked.Decrement(ref this._inProcess);
					}
				}
				else
				{
					Interlocked.Increment(ref this._inProcess);
					if (this.ConnectToServer(server) && this.Result == null)
					{
						this.IsHandled = true;
						this.Result = server;
						Interlocked.Decrement(ref this._inProcess);
						return;
					}
					Interlocked.Decrement(ref this._inProcess);
				}
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001BEEC File Offset: 0x0001A0EC
		private HostFinderContext.WebConfig GetWebConfigWorkItem()
		{
			HostFinderContext.WebConfig result = HostFinderContext.WebConfig.None;
			object contextLocker = this._contextLocker;
			lock (contextLocker)
			{
				if (this._webConfigs != null)
				{
					this._webConfigs.TryDequeue(out result);
				}
			}
			return result;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001BF40 File Offset: 0x0001A140
		private Server GetServerWorkItem()
		{
			Server result = null;
			object contextLocker = this._contextLocker;
			lock (contextLocker)
			{
				if (this._serversToPing != null)
				{
					this._serversToPing.TryDequeue(out result);
				}
			}
			return result;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001BF94 File Offset: 0x0001A194
		private List<Server> GenerateServersByDomain(string domain)
		{
			List<Server> list = new List<Server>();
			foreach (string str in HostFinderContext._imapPrefixes)
			{
				list.Add(new Server
				{
					Domain = this._domain,
					Hostname = str + domain,
					Port = 993,
					Socket = Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL,
					Protocol = Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP
				});
				list.Add(new Server
				{
					Domain = this._domain,
					Hostname = str + domain,
					Port = 143,
					Socket = Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.Plain,
					Protocol = Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP
				});
			}
			foreach (string str2 in HostFinderContext._pop3Prefixes)
			{
				list.Add(new Server
				{
					Domain = this._domain,
					Hostname = str2 + domain,
					Port = 995,
					Socket = Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL,
					Protocol = Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.POP3
				});
				list.Add(new Server
				{
					Domain = this._domain,
					Hostname = str2 + domain,
					Port = 110,
					Socket = Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.Plain,
					Protocol = Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.POP3
				});
			}
			return list;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001C11C File Offset: 0x0001A31C
		private List<Server> GenerateServersByMx(string mx)
		{
			List<Server> list = null;
			string[] array = mx.Split(new char[]
			{
				'.'
			});
			array = (from p in array
			where HostFinderContext.<>c.smethod_0(p) > 1
			select p).ToArray<string>();
			int num = array.Length;
			if (num >= 2)
			{
				list = this.GenerateServersByDomain(array[num - 2] + <Module>.smethod_3<string>(2035431732) + array[num - 1]);
				return (from s in list
				select new Server(this._domain, s.Hostname, s.Port, s.Protocol, s.Socket)).ToList<Server>();
			}
			return list;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001C1AC File Offset: 0x0001A3AC
		private Server FindCoincidence()
		{
			KeyValuePair<string, string> keyValuePair = HostFinderContext._domainCoincidences.FirstOrDefault((KeyValuePair<string, string> d) => this._domain.Contains(d.Key));
			if (keyValuePair.Key != null)
			{
				return new Server
				{
					Domain = this._domain,
					Hostname = keyValuePair.Value,
					Port = 993,
					Protocol = Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP,
					Socket = Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL
				};
			}
			return null;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001C214 File Offset: 0x0001A414
		private Server FindCoincidence(string mx)
		{
			KeyValuePair<string, string> keyValuePair = HostFinderContext._recordCoincidences.FirstOrDefault((KeyValuePair<string, string> r) => HostFinderContext.<>c__DisplayClass21_0.smethod_0(mx, r.Key));
			if (keyValuePair.Key == null)
			{
				return null;
			}
			return new Server
			{
				Domain = this._domain,
				Hostname = keyValuePair.Value,
				Port = 993,
				Protocol = Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP,
				Socket = Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL
			};
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001C288 File Offset: 0x0001A488
		private string FindMXRecordByLookup()
		{
			NameServer nameServer = new NameServer(IPAddress.Parse(<Module>.smethod_6<string>(-932791104)));
			LookupClient lookupClient = new LookupClient(new LookupClientOptions(new NameServer[]
			{
				nameServer
			})
			{
				ThrowDnsErrors = false,
				ContinueOnDnsError = true
			});
			string result;
			try
			{
				IDnsQueryResponse dnsQueryResponse = lookupClient.Query(this._domain, 15, 1);
				string text;
				if (dnsQueryResponse == null)
				{
					text = null;
				}
				else
				{
					IReadOnlyList<DnsResourceRecord> answers = dnsQueryResponse.Answers;
					if (answers == null)
					{
						text = null;
					}
					else
					{
						IEnumerable<MxRecord> enumerable = RecordCollectionExtension.MxRecords(answers);
						if (enumerable == null)
						{
							text = null;
						}
						else
						{
							MxRecord mxRecord = enumerable.FirstOrDefault<MxRecord>();
							if (mxRecord == null)
							{
								text = null;
							}
							else
							{
								DnsString exchange = mxRecord.Exchange;
								text = ((exchange != null) ? exchange.Value : null);
							}
						}
					}
				}
				result = text;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001C340 File Offset: 0x0001A540
		public string FindMxRecordByGoogle()
		{
			string result;
			for (;;)
			{
				ThreadsManager.Instance.WaitHandle.WaitOne();
				ProxyClient proxy = ProxyManager.Instance.GetProxy();
				try
				{
					using (HttpRequest httpRequest = new HttpRequest())
					{
						httpRequest.IgnoreProtocolErrors = true;
						httpRequest.ConnectTimeout = 15000;
						httpRequest.Proxy = proxy;
						httpRequest.KeepAlive = false;
						httpRequest.AllowAutoRedirect = false;
						httpRequest.UserAgent = <Module>.smethod_2<string>(-2061593978);
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(-486098232), this._domain);
						httpRequest.AddUrlParam(<Module>.smethod_6<string>(-1967002142), <Module>.smethod_3<string>(1386942965));
						string text = httpRequest.Get(<Module>.smethod_5<string>(1100529352), null).ToString();
						if (!string.IsNullOrEmpty(text))
						{
							Match match = Regex.Match(text, <Module>.smethod_3<string>(-17772745));
							if (!match.Success)
							{
								result = null;
							}
							else
							{
								result = match.Groups[3].Value;
							}
						}
						else
						{
							result = null;
						}
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
					continue;
				}
				break;
			}
			return result;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001C46C File Offset: 0x0001A66C
		public Server FindServerByAutoConfig(string prefix = "", int errors = 0)
		{
			ProxyClient proxyClient = null;
			if (CheckerSettings.Instance.UseProxyToSearchServer)
			{
				proxyClient = ProxyManager.Instance.GetProxy();
			}
			ThreadsManager.Instance.WaitHandle.WaitOne();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					httpRequest.IgnoreProtocolErrors = true;
					httpRequest.ConnectTimeout = 15000;
					httpRequest.Proxy = proxyClient;
					httpRequest.KeepAlive = false;
					httpRequest.AllowAutoRedirect = false;
					httpRequest.UserAgent = <Module>.smethod_4<string>(-966056653);
					httpRequest.AddUrlParam(<Module>.smethod_4<string>(-1058026721), <Module>.smethod_4<string>(-84822525));
					string text = httpRequest.Get(<Module>.smethod_6<string>(1877812712) + prefix + this._domain + <Module>.smethod_6<string>(-343543153), null).ToString();
					if (string.IsNullOrEmpty(text))
					{
						return null;
					}
					if (text.Contains(<Module>.smethod_5<string>(1248357596)))
					{
						foreach (Match match in Regex.Matches(text, <Module>.smethod_2<string>(-364280516), RegexOptions.Singleline))
						{
							if (match.Success)
							{
								string value = match.Groups[1].Value;
								if (value != <Module>.smethod_2<string>(302133814))
								{
									string value2 = match.Groups[2].Value;
									return new Server
									{
										Domain = this._domain,
										Hostname = value2.Substring(<Module>.smethod_4<string>(438147969), <Module>.smethod_2<string>(-1495822824), 0, StringComparison.Ordinal, null),
										Port = int.Parse(value2.Substring(<Module>.smethod_6<string>(398638587), <Module>.smethod_4<string>(-1053218402), 0, StringComparison.Ordinal, null)),
										Socket = ((value2.ContainsIgnoreCase(<Module>.smethod_3<string>(1869936522)) || value2.ContainsIgnoreCase(<Module>.smethod_3<string>(1027107096))) ? Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL : Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.Plain),
										Protocol = (value.Contains(<Module>.smethod_5<string>(-1257967223)) ? Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP : Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.POP3)
									};
								}
							}
						}
					}
					if (string.IsNullOrEmpty(prefix))
					{
						return this.FindServerByAutoConfig(<Module>.smethod_3<string>(-377608614), 0);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (ex is HttpException || ex is SocketException || ex is IOException || ex is ProxyException)
				{
					errors++;
					if (CheckerSettings.Instance.UseProxyToSearchServer && proxyClient != null && errors <= 2)
					{
						return this.FindServerByAutoConfig(prefix, errors);
					}
					return null;
				}
			}
			return null;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001C774 File Offset: 0x0001A974
		public Server FindServerByAutoDiscover(string prefix = "", int errors = 0)
		{
			ProxyClient proxyClient = null;
			if (CheckerSettings.Instance.UseProxyToSearchServer)
			{
				proxyClient = ProxyManager.Instance.GetProxy();
			}
			ThreadsManager.Instance.WaitHandle.WaitOne();
			try
			{
				using (HttpRequest httpRequest = new HttpRequest())
				{
					httpRequest.IgnoreProtocolErrors = true;
					httpRequest.ConnectTimeout = 15000;
					httpRequest.Proxy = proxyClient;
					httpRequest.KeepAlive = false;
					httpRequest.AllowAutoRedirect = false;
					httpRequest.UserAgent = <Module>.smethod_6<string>(-931061319);
					string text = httpRequest.Get(<Module>.smethod_2<string>(1983127431) + prefix + this._domain + <Module>.smethod_3<string>(-939494898), null).ToString();
					if (string.IsNullOrEmpty(text))
					{
						return null;
					}
					if (text.Contains(<Module>.smethod_6<string>(-2125125286)))
					{
						foreach (object obj in Regex.Matches(text, <Module>.smethod_6<string>(-1382943546), RegexOptions.Singleline))
						{
							Match match = (Match)obj;
							if (match.Success)
							{
								string value = match.Groups[1].Value;
								if (!value.Contains(<Module>.smethod_6<string>(245704798)))
								{
									return new Server
									{
										Domain = this._domain,
										Hostname = value.Substring(<Module>.smethod_3<string>(1710223629), <Module>.smethod_5<string>(345889949), 0, StringComparison.Ordinal, null),
										Port = int.Parse(value.Substring(<Module>.smethod_6<string>(1876082927), <Module>.smethod_6<string>(1283375406), 0, StringComparison.Ordinal, null)),
										Socket = (value.Contains(<Module>.smethod_6<string>(690667885)) ? Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL : Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.Plain),
										Protocol = (value.Contains(<Module>.smethod_3<string>(-818264649)) ? Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP : Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.POP3)
									};
								}
							}
						}
					}
					if (string.IsNullOrEmpty(prefix))
					{
						return this.FindServerByAutoDiscover(<Module>.smethod_4<string>(1121017004), 0);
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (ex is HttpException || ex is SocketException || ex is IOException || ex is ProxyException)
				{
					errors++;
					if (CheckerSettings.Instance.UseProxyToSearchServer && proxyClient != null && errors <= 2)
					{
						return this.FindServerByAutoDiscover(prefix, errors);
					}
					return null;
				}
			}
			return null;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001CA38 File Offset: 0x0001AC38
		public bool ConnectToServer(Server server)
		{
			MailClient mailClient = null;
			Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType protocol = server.Protocol;
			if (protocol == Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.IMAP)
			{
				mailClient = new ImapClient();
			}
			else if (protocol == Hackus_Mail_Checker_Reforged.Models.Enums.ProtocolType.POP3)
			{
				mailClient = new Pop3Client();
			}
			mailClient.ReadWriteTimeout = 15000;
			mailClient.ConnectTimeout = 15000;
			int i = 0;
			while (i < 5)
			{
				TcpClient tcp = null;
				if (CheckerSettings.Instance.UseProxyToSearchServer)
				{
					ProxyClient proxy = ProxyManager.Instance.GetProxy();
					OperationResult operationResult = this.ConnectToProxy(server, proxy, out tcp);
					if (operationResult == OperationResult.Error)
					{
						i++;
						continue;
					}
					if (operationResult == OperationResult.HostNotFound)
					{
						return false;
					}
				}
				bool result;
				try
				{
					mailClient.Connect(server.Hostname, server.Port, server.Socket == Hackus_Mail_Checker_Reforged.Models.Enums.SocketType.SSL, tcp);
					mailClient.Dispose();
					result = true;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch
				{
					result = false;
				}
				return result;
			}
			return false;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		public OperationResult ConnectToProxy(Server server, ProxyClient proxyClient, out TcpClient tcpClient)
		{
			tcpClient = null;
			OperationResult result;
			try
			{
				tcpClient = proxyClient.CreateConnection(server.Hostname, server.Port, null);
				if (tcpClient != null && tcpClient.Connected)
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
				if (tcpClient != null && tcpClient.Connected)
				{
					tcpClient.DisposeObject();
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
				if (tcpClient != null && tcpClient.Connected)
				{
					tcpClient.DisposeObject();
				}
				result = OperationResult.Error;
			}
			return result;
		}

		// Token: 0x040002A7 RID: 679
		private object _contextLocker = new object();

		// Token: 0x040002A8 RID: 680
		public string _domain;

		// Token: 0x040002A9 RID: 681
		private int _inProcess;

		// Token: 0x040002AA RID: 682
		private ConcurrentQueue<Server> _serversToPing;

		// Token: 0x040002AB RID: 683
		private ConcurrentQueue<HostFinderContext.WebConfig> _webConfigs;

		// Token: 0x040002AC RID: 684
		public bool IsHandled;

		// Token: 0x040002AD RID: 685
		public bool IsStarted;

		// Token: 0x040002AE RID: 686
		public Server Result;

		// Token: 0x040002AF RID: 687
		public bool IsResultSaved;

		// Token: 0x040002B0 RID: 688
		private static readonly Dictionary<string, string> _domainCoincidences = new Dictionary<string, string>
		{
			{
				<Module>.smethod_4<string>(922651911),
				<Module>.smethod_3<string>(-814409771)
			},
			{
				<Module>.smethod_3<string>(-1657239197),
				<Module>.smethod_4<string>(-302420894)
			},
			{
				<Module>.smethod_3<string>(-571949273),
				<Module>.smethod_6<string>(-805804090)
			},
			{
				<Module>.smethod_4<string>(-1968110763),
				<Module>.smethod_4<string>(-1004523205)
			},
			{
				<Module>.smethod_4<string>(-1793787265),
				<Module>.smethod_3<string>(109239821)
			},
			{
				<Module>.smethod_2<string>(-630846248),
				<Module>.smethod_6<string>(1712770428)
			},
			{
				<Module>.smethod_5<string>(1791586755),
				<Module>.smethod_2<string>(750941947)
			},
			{
				<Module>.smethod_5<string>(-1010394552),
				<Module>.smethod_2<string>(1449995967)
			},
			{
				<Module>.smethod_5<string>(-1770200016),
				<Module>.smethod_3<string>(-693179522)
			},
			{
				<Module>.smethod_5<string>(-667448881),
				<Module>.smethod_6<string>(-2137233781)
			},
			{
				<Module>.smethod_6<string>(824574039),
				<Module>.smethod_3<string>(-371826297)
			},
			{
				<Module>.smethod_3<string>(1073299496),
				<Module>.smethod_5<string>(1569844389)
			},
			{
				<Module>.smethod_3<string>(1956539005),
				<Module>.smethod_4<string>(-1876140695)
			},
			{
				<Module>.smethod_5<string>(-1812323138),
				<Module>.smethod_3<string>(-1174245640)
			}
		};

		// Token: 0x040002B1 RID: 689
		private static readonly Dictionary<string, string> _recordCoincidences = new Dictionary<string, string>
		{
			{
				<Module>.smethod_2<string>(816221327),
				<Module>.smethod_5<string>(-1923194321)
			},
			{
				<Module>.smethod_2<string>(2064726656),
				<Module>.smethod_4<string>(1009813660)
			},
			{
				<Module>.smethod_2<string>(-1846711887),
				<Module>.smethod_6<string>(-805804090)
			},
			{
				<Module>.smethod_6<string>(-216556139),
				<Module>.smethod_6<string>(1712770428)
			},
			{
				<Module>.smethod_4<string>(-1266008452),
				<Module>.smethod_5<string>(203227766)
			},
			{
				<Module>.smethod_6<string>(673370035),
				<Module>.smethod_6<string>(2008259296)
			},
			{
				<Module>.smethod_3<string>(1875718839),
				<Module>.smethod_5<string>(-287546149)
			},
			{
				<Module>.smethod_5<string>(1685881633),
				<Module>.smethod_6<string>(-2137233781)
			},
			{
				<Module>.smethod_5<string>(683669559),
				<Module>.smethod_4<string>(515693080)
			},
			{
				<Module>.smethod_5<string>(-746529064),
				<Module>.smethod_5<string>(277141888)
			},
			{
				<Module>.smethod_4<string>(1193753796),
				<Module>.smethod_5<string>(1569844389)
			},
			{
				<Module>.smethod_5<string>(-1126431796),
				<Module>.smethod_2<string>(-1264620888)
			},
			{
				<Module>.smethod_4<string>(-273570980),
				<Module>.smethod_2<string>(-296277030)
			},
			{
				<Module>.smethod_2<string>(1784565185),
				<Module>.smethod_2<string>(-296277030)
			},
			{
				<Module>.smethod_6<string>(1723149138),
				<Module>.smethod_6<string>(389989662)
			},
			{
				<Module>.smethod_6<string>(-1683621769),
				<Module>.smethod_4<string>(-1939260849)
			},
			{
				<Module>.smethod_5<string>(76858400),
				<Module>.smethod_5<string>(-303044332)
			},
			{
				<Module>.smethod_6<string>(687208315),
				<Module>.smethod_6<string>(94500794)
			},
			{
				<Module>.smethod_6<string>(96230579),
				<Module>.smethod_5<string>(-1822655260)
			},
			{
				<Module>.smethod_3<string>(-335271092),
				<Module>.smethod_4<string>(-1677775602)
			},
			{
				<Module>.smethod_5<string>(-328080003),
				<Module>.smethod_6<string>(-2367166)
			},
			{
				<Module>.smethod_6<string>(-2075978597),
				<Module>.smethod_4<string>(7759550)
			},
			{
				<Module>.smethod_4<string>(361214865),
				<Module>.smethod_6<string>(1035303442)
			},
			{
				<Module>.smethod_4<string>(-1217313255),
				<Module>.smethod_2<string>(-1066033851)
			},
			{
				<Module>.smethod_4<string>(356406546),
				<Module>.smethod_6<string>(2072974050)
			},
			{
				<Module>.smethod_4<string>(-2098547383),
				<Module>.smethod_2<string>(432717365)
			},
			{
				<Module>.smethod_4<string>(-781504510),
				<Module>.smethod_3<string>(1200312062)
			},
			{
				<Module>.smethod_6<string>(-1777030159),
				<Module>.smethod_3<string>(76539494)
			},
			{
				<Module>.smethod_4<string>(1242448993),
				<Module>.smethod_4<string>(1329610742)
			},
			{
				<Module>.smethod_6<string>(-1931693733),
				<Module>.smethod_3<string>(-444936707)
			},
			{
				<Module>.smethod_3<string>(-1287766133),
				<Module>.smethod_2<string>(-1865731047)
			},
			{
				<Module>.smethod_6<string>(736355004),
				<Module>.smethod_3<string>(-1247356050)
			},
			{
				<Module>.smethod_6<string>(1031843872),
				<Module>.smethod_6<string>(1033573657)
			},
			{
				<Module>.smethod_2<string>(748242632),
				<Module>.smethod_6<string>(1181318091)
			},
			{
				<Module>.smethod_5<string>(-1842524870),
				<Module>.smethod_4<string>(-1318899961)
			},
			{
				<Module>.smethod_5<string>(-359871003),
				<Module>.smethod_2<string>(-448579056)
			},
			{
				<Module>.smethod_5<string>(-1119676467),
				<Module>.smethod_5<string>(-1499579199)
			}
		};

		// Token: 0x040002B2 RID: 690
		private static List<string> _imapPrefixes = new List<string>
		{
			<Module>.smethod_4<string>(171854403),
			<Module>.smethod_4<string>(259016152),
			<Module>.smethod_2<string>(666643407),
			<Module>.smethod_6<string>(-1342445782),
			<Module>.smethod_6<string>(140187913)
		};

		// Token: 0x040002B3 RID: 691
		private static List<string> _pop3Prefixes = new List<string>
		{
			<Module>.smethod_6<string>(-487828017),
			<Module>.smethod_6<string>(-340083583),
			<Module>.smethod_2<string>(1118150855),
			<Module>.smethod_5<string>(1465728535),
			<Module>.smethod_3<string>(1764125785)
		};

		// Token: 0x0200009A RID: 154
		private enum WebConfig
		{
			// Token: 0x040002B5 RID: 693
			None,
			// Token: 0x040002B6 RID: 694
			AutoConfig,
			// Token: 0x040002B7 RID: 695
			AutoDiscovery
		}
	}
}
