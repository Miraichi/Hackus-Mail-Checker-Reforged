using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;
using Hackus_Mail_Checker_Reforged.Services.Managers;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x0200001B RID: 27
	internal class PlainConfiguration : IConfigurationProvider
	{
		// Token: 0x0600009A RID: 154 RVA: 0x0000662B File Offset: 0x0000482B
		public PlainConfiguration(string path)
		{
			this._configurationPath = path;
			this._servers = new List<Server>(300000);
			this._readerWriteLock = new ReaderWriterLockSlim();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00011718 File Offset: 0x0000F918
		public void Open()
		{
			this._servers.Clear();
			using (StreamReader streamReader = new StreamReader(this._configurationPath))
			{
				string line;
				while ((line = streamReader.ReadLine()) != null)
				{
					Server fromString = Server.GetFromString(line);
					if (fromString != null)
					{
						this._servers.Add(fromString);
					}
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0001177C File Offset: 0x0000F97C
		public Server Find(string domain)
		{
			Server result = null;
			try
			{
				this._readerWriteLock.EnterReadLock();
				result = this._servers.FirstOrDefault((Server s) => s.Domain.EqualsIgnoreCase(domain));
			}
			finally
			{
				this._readerWriteLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000117DC File Offset: 0x0000F9DC
		public Server Find(string domain, ProtocolType protocol)
		{
			Server result = null;
			try
			{
				this._readerWriteLock.EnterReadLock();
				result = this._servers.FirstOrDefault((Server s) => s.Domain.EqualsIgnoreCase(domain) && s.Protocol == protocol);
			}
			finally
			{
				this._readerWriteLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00011844 File Offset: 0x0000FA44
		public IEnumerable<Server> FindAll(string searchQuery, SearchServerType searchType)
		{
			IEnumerable<Server> result = null;
			try
			{
				this._readerWriteLock.EnterReadLock();
				if (searchType != SearchServerType.Domain)
				{
					result = from s in this._servers
					where s.Hostname.ContainsIgnoreCase(searchQuery)
					select s;
				}
				else
				{
					result = from s in this._servers
					where s.Domain.ContainsIgnoreCase(searchQuery)
					select s;
				}
			}
			finally
			{
				this._readerWriteLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000118C0 File Offset: 0x0000FAC0
		public void Add(Server server)
		{
			try
			{
				this._readerWriteLock.EnterWriteLock();
				this._servers.Add(server);
				FileManager.AddToConfiguration(server);
			}
			finally
			{
				this._readerWriteLock.ExitWriteLock();
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00011908 File Offset: 0x0000FB08
		public void Update(Server original, Server updated)
		{
			try
			{
				this._readerWriteLock.EnterWriteLock();
				Server original2 = original.Clone();
				original.Domain = updated.Domain;
				original.Hostname = updated.Hostname;
				original.Port = updated.Port;
				original.Socket = updated.Socket;
				original.Protocol = updated.Protocol;
				FileManager.EditConfiguration(original2, original);
			}
			finally
			{
				this._readerWriteLock.ExitWriteLock();
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00011988 File Offset: 0x0000FB88
		public void Remove(Server server)
		{
			try
			{
				this._readerWriteLock.EnterWriteLock();
				this._servers.Remove(server);
				FileManager.RemoveFromConfiguration(server);
			}
			finally
			{
				this._readerWriteLock.ExitWriteLock();
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006655 File Offset: 0x00004855
		public void Close()
		{
		}

		// Token: 0x04000058 RID: 88
		private string _configurationPath;

		// Token: 0x04000059 RID: 89
		private List<Server> _servers;

		// Token: 0x0400005A RID: 90
		private ReaderWriterLockSlim _readerWriteLock;
	}
}
