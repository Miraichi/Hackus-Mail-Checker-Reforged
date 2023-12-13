using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using DnsClient;
using DnsClient.Protocol;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Net.Mail;
using Hackus_Mail_Checker_Reforged.Services.Managers;
using Hackus_Mail_Checker_Reforged.Services.Settings;
using Hackus_Mail_Checker_Reforged.Services.Shared;

namespace Hackus_Mail_Checker_Reforged.Net
{
	// Token: 0x02000093 RID: 147
	internal static class HostFinder
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x0001B260 File Offset: 0x00019460
		public static Server GetServer(Mailbox mailbox)
		{
			string text = mailbox.Domain.ToLower();
			bool flag = false;
			HostFinderServer server = null;
			object locker = HostFinder._locker;
			lock (locker)
			{
				if (!(flag = HostFinder._servers.TryGetValue(text, out server)))
				{
					server = new HostFinderServer(HostFinderStatus.InProgress);
					HostFinder._servers.AddOrUpdate(text, server, (string key, HostFinderServer oldValue) => server);
				}
			}
			if (!flag)
			{
				Server server2 = HostFinder.FindServer(text);
				if (server2 == null)
				{
					server.Status = HostFinderStatus.NotFound;
				}
				else
				{
					server.Server = server2;
					server.Status = HostFinderStatus.Found;
					ConfigurationManager.Instance.Configuration.Add(server.Server);
					ReportService.AddServer(server.Server);
				}
			}
			switch (server.Status)
			{
			case HostFinderStatus.Found:
				return server.Server;
			case HostFinderStatus.InProgress:
				Thread.Sleep(100);
				MailManager.Instance.AddToQueue(mailbox);
				return null;
			case HostFinderStatus.NotFound:
				StatisticsManager.Instance.IncrementNoHost();
				FileManager.SaveStatistics(mailbox.Address, mailbox.Password, OperationResult.HostNotFound);
				return null;
			default:
				return null;
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000098D5 File Offset: 0x00007AD5
		public static void Refresh()
		{
			HostFinder._servers = new ConcurrentDictionary<string, HostFinderServer>();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001B3B8 File Offset: 0x000195B8
		private static Server FindServer(string domain)
		{
			/*
An exception occurred when decompiling this method (060004E7)

ICSharpCode.Decompiler.DecompilerException: Error decompiling Hackus_Mail_Checker_Reforged.Models.Server Hackus_Mail_Checker_Reforged.Net.HostFinder::FindServer(System.String)

 ---> System.Exception: Inconsistent stack size at IL_BF
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 443
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 269
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 112
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1533
*/;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001B4C8 File Offset: 0x000196C8
		private static List<Server> GenerateServersByRecords(string domain, string mx)
		{
			List<Server> list = new List<Server>();
			string[] array = mx.Split(new char[]
			{
				'.'
			});
			int num = array.Length;
			if (num >= 2)
			{
				list = HostFinder.GenerateServersByDomain(array[num - 2] + <Module>.smethod_6<string>(1711040643) + array[num - 1]);
				return (from s in list
				select new Server(domain, s.Hostname, s.Port, s.Protocol, s.Socket)).ToList<Server>();
			}
			return list;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001B53C File Offset: 0x0001973C
		private static List<Server> GenerateServersByDomain(string domain)
		{
			HostFinder.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.domain = domain;
			CS$<>8__locals1.servers = new List<Server>();
			switch (CheckerSettings.Instance.GuessProtocolType)
			{
			case ExtendedProtocolType.Imap:
				switch (CheckerSettings.Instance.GuessSocketType)
				{
				case ExtendedSocketType.Ssl:
					HostFinder.smethod_0(true, ref CS$<>8__locals1);
					break;
				case ExtendedSocketType.Plain:
					HostFinder.smethod_0(false, ref CS$<>8__locals1);
					break;
				case ExtendedSocketType.Both:
					HostFinder.smethod_0(true, ref CS$<>8__locals1);
					HostFinder.smethod_0(false, ref CS$<>8__locals1);
					break;
				}
				break;
			case ExtendedProtocolType.Pop3:
				switch (CheckerSettings.Instance.GuessSocketType)
				{
				case ExtendedSocketType.Ssl:
					HostFinder.smethod_1(true, ref CS$<>8__locals1);
					break;
				case ExtendedSocketType.Plain:
					HostFinder.smethod_1(false, ref CS$<>8__locals1);
					break;
				case ExtendedSocketType.Both:
					HostFinder.smethod_1(true, ref CS$<>8__locals1);
					HostFinder.smethod_1(false, ref CS$<>8__locals1);
					break;
				}
				break;
			case ExtendedProtocolType.Both:
				switch (CheckerSettings.Instance.GuessSocketType)
				{
				case ExtendedSocketType.Ssl:
					HostFinder.smethod_0(true, ref CS$<>8__locals1);
					HostFinder.smethod_1(true, ref CS$<>8__locals1);
					break;
				case ExtendedSocketType.Plain:
					HostFinder.smethod_0(false, ref CS$<>8__locals1);
					HostFinder.smethod_1(false, ref CS$<>8__locals1);
					break;
				case ExtendedSocketType.Both:
					HostFinder.smethod_0(true, ref CS$<>8__locals1);
					HostFinder.smethod_0(false, ref CS$<>8__locals1);
					HostFinder.smethod_1(true, ref CS$<>8__locals1);
					HostFinder.smethod_1(false, ref CS$<>8__locals1);
					break;
				}
				break;
			}
			return CS$<>8__locals1.servers;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000098E1 File Offset: 0x00007AE1
		private static string GetMx(string domain)
		{
			MxRecord mxRecord = RecordCollectionExtension.MxRecords(new LookupClient
			{
				UseCache = false,
				ThrowDnsErrors = false
			}.Query(domain, 15, 1).Answers).FirstOrDefault<MxRecord>();
			return (mxRecord != null) ? mxRecord.Exchange : null;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001B684 File Offset: 0x00019884
		private static Server GetDomainCoincidence(string domain)
		{
			KeyValuePair<string, string> keyValuePair = HostFinder.DomainCoincidences.FirstOrDefault((KeyValuePair<string, string> c) => domain.ContainsIgnoreCase(c.Key));
			if (keyValuePair.Key != null)
			{
				return new Server
				{
					Domain = domain,
					Hostname = keyValuePair.Value,
					Port = 993,
					Protocol = ProtocolType.IMAP,
					Socket = SocketType.SSL
				};
			}
			return null;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001B6F8 File Offset: 0x000198F8
		private static Server GetRecordCoincidence(string domain, string mx)
		{
			KeyValuePair<string, string> keyValuePair = HostFinder.RecordCoincidences.FirstOrDefault((KeyValuePair<string, string> c) => mx.ContainsIgnoreCase(c.Key));
			if (keyValuePair.Key == null)
			{
				return null;
			}
			return new Server
			{
				Domain = domain,
				Hostname = keyValuePair.Value,
				Port = 993,
				Protocol = ProtocolType.IMAP,
				Socket = SocketType.SSL
			};
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001B768 File Offset: 0x00019968
		private static bool CheckConnection(Server server)
		{
			Client client = null;
			ProtocolType protocol = server.Protocol;
			if (protocol == ProtocolType.IMAP)
			{
				client = new Imap();
			}
			else if (protocol == ProtocolType.POP3)
			{
				client = new Pop3();
			}
			client.ServerTimeout = 3000;
			try
			{
				client.Connect(server.Hostname, server.Port, server.Socket == SocketType.SSL, null);
				return true;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001BA64 File Offset: 0x00019C64
		[CompilerGenerated]
		internal static void smethod_0(bool ssl, ref HostFinder.<>c__DisplayClass6_0 <>c__DisplayClass6_0_0)
		{
			int port = ssl ? 993 : 143;
			SocketType socket = ssl ? SocketType.SSL : SocketType.Plain;
			<>c__DisplayClass6_0_0.servers.Add(new Server(<>c__DisplayClass6_0_0.domain, <Module>.smethod_3<string>(422883290) + <>c__DisplayClass6_0_0.domain, port, ProtocolType.IMAP, socket));
			<>c__DisplayClass6_0_0.servers.Add(new Server(<>c__DisplayClass6_0_0.domain, <Module>.smethod_4<string>(259016152) + <>c__DisplayClass6_0_0.domain, port, ProtocolType.IMAP, socket));
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		[CompilerGenerated]
		internal static void smethod_1(bool ssl, ref HostFinder.<>c__DisplayClass6_0 <>c__DisplayClass6_0_0)
		{
			int port = ssl ? 995 : 110;
			SocketType socket = ssl ? SocketType.SSL : SocketType.Plain;
			<>c__DisplayClass6_0_0.servers.Add(new Server(<>c__DisplayClass6_0_0.domain, <Module>.smethod_5<string>(-2017772687) + <>c__DisplayClass6_0_0.domain, port, ProtocolType.POP3, socket));
			<>c__DisplayClass6_0_0.servers.Add(new Server(<>c__DisplayClass6_0_0.domain, <Module>.smethod_6<string>(-340083583) + <>c__DisplayClass6_0_0.domain, port, ProtocolType.POP3, socket));
			<>c__DisplayClass6_0_0.servers.Add(new Server(<>c__DisplayClass6_0_0.domain, <Module>.smethod_3<string>(-2146015071) + <>c__DisplayClass6_0_0.domain, port, ProtocolType.POP3, socket));
		}

		// Token: 0x0400029D RID: 669
		private static object _locker = new object();

		// Token: 0x0400029E RID: 670
		private static ConcurrentDictionary<string, HostFinderServer> _servers = new ConcurrentDictionary<string, HostFinderServer>();

		// Token: 0x0400029F RID: 671
		private static Dictionary<string, string> DomainCoincidences = new Dictionary<string, string>
		{
			{
				<Module>.smethod_4<string>(922651911),
				<Module>.smethod_5<string>(1838875938)
			},
			{
				<Module>.smethod_6<string>(-1547985830),
				<Module>.smethod_5<string>(1269021840)
			},
			{
				<Module>.smethod_5<string>(-1353340223),
				<Module>.smethod_2<string>(-1297260578)
			},
			{
				<Module>.smethod_4<string>(-1793787265),
				<Module>.smethod_5<string>(-440540454)
			},
			{
				<Module>.smethod_3<string>(1273422472),
				<Module>.smethod_6<string>(1712770428)
			},
			{
				<Module>.smethod_6<string>(1860514862),
				<Module>.smethod_6<string>(2008259296)
			},
			{
				<Module>.smethod_4<string>(1973401218),
				<Module>.smethod_4<string>(2060562967)
			},
			{
				<Module>.smethod_3<string>(-1255065806),
				<Module>.smethod_3<string>(-1536008948)
			},
			{
				<Module>.smethod_5<string>(-857400247),
				<Module>.smethod_6<string>(-1699189834)
			},
			{
				<Module>.smethod_5<string>(1949747121),
				<Module>.smethod_6<string>(-810993445)
			},
			{
				<Module>.smethod_4<string>(1193753796),
				<Module>.smethod_4<string>(-1876140695)
			},
			{
				<Module>.smethod_4<string>(404489736),
				<Module>.smethod_2<string>(-1264620888)
			}
		};

		// Token: 0x040002A0 RID: 672
		private static Dictionary<string, string> RecordCoincidences = new Dictionary<string, string>
		{
			{
				<Module>.smethod_2<string>(816221327),
				<Module>.smethod_6<string>(1563296209)
			},
			{
				<Module>.smethod_6<string>(-1106482313),
				<Module>.smethod_3<string>(-814409771)
			},
			{
				<Module>.smethod_6<string>(376151382),
				<Module>.smethod_3<string>(2075841815)
			},
			{
				<Module>.smethod_6<string>(-216556139),
				<Module>.smethod_6<string>(1712770428)
			},
			{
				<Module>.smethod_2<string>(650298771),
				<Module>.smethod_5<string>(203227766)
			},
			{
				<Module>.smethod_3<string>(-812482332),
				<Module>.smethod_4<string>(-1798595584)
			},
			{
				<Module>.smethod_4<string>(1973401218),
				<Module>.smethod_5<string>(-287546149)
			},
			{
				<Module>.smethod_4<string>(51034421),
				<Module>.smethod_4<string>(-481552711)
			},
			{
				<Module>.smethod_3<string>(351700319),
				<Module>.smethod_3<string>(-371826297)
			},
			{
				<Module>.smethod_2<string>(117167307),
				<Module>.smethod_4<string>(-1876140695)
			},
			{
				<Module>.smethod_4<string>(-651067890),
				<Module>.smethod_3<string>(-1174245640)
			}
		};
	}
}
