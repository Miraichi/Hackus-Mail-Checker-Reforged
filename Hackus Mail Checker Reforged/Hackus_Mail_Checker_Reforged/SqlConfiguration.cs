using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Hackus_Mail_Checker_Reforged.Models;
using Hackus_Mail_Checker_Reforged.Models.Enums;

namespace Hackus_Mail_Checker_Reforged
{
	// Token: 0x02000019 RID: 25
	internal class SqlConfiguration
	{
		// Token: 0x06000075 RID: 117 RVA: 0x000064D7 File Offset: 0x000046D7
		public SqlConfiguration(string connectionString)
		{
			this._connectionString = connectionString;
			this._readerWriteLock = new ReaderWriterLockSlim();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006507 File Offset: 0x00004707
		public SqlConfiguration(string connectionString, string writeConnectionString)
		{
			this._connectionString = connectionString;
			this._writeConnectionString = writeConnectionString;
			this._readerWriteLock = new ReaderWriterLockSlim();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00010910 File Offset: 0x0000EB10
		public Task UploadConfiguration(HashSet<string> toSearch, ProtocolType protocol)
		{
			SqlConfiguration.<UploadConfiguration>d__8 <UploadConfiguration>d__;
			<UploadConfiguration>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UploadConfiguration>d__.<>4__this = this;
			<UploadConfiguration>d__.toSearch = toSearch;
			<UploadConfiguration>d__.protocol = protocol;
			<UploadConfiguration>d__.<>1__state = -1;
			<UploadConfiguration>d__.<>t__builder.Start<SqlConfiguration.<UploadConfiguration>d__8>(ref <UploadConfiguration>d__);
			return <UploadConfiguration>d__.<>t__builder.Task;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00010964 File Offset: 0x0000EB64
		public void Open()
		{
			SQLiteFactory sqliteFactory = (SQLiteFactory)DbProviderFactories.GetFactory(<Module>.smethod_3<string>(619217524));
			this._connection = (SQLiteConnection)sqliteFactory.CreateConnection();
			this._connection.ConnectionString = this._connectionString;
			this._connection.Open();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000109B4 File Offset: 0x0000EBB4
		public Server Find(string domain)
		{
			Server server = this.Find(domain, ProtocolType.IMAP);
			if (server == null)
			{
				server = this.Find(domain, ProtocolType.POP3);
			}
			return server;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000109D8 File Offset: 0x0000EBD8
		public Server Find(string domain, ProtocolType protocol)
		{
			Server result = null;
			if (protocol == ProtocolType.IMAP)
			{
				this._imapServers.TryGetValue(domain, out result);
			}
			else if (protocol == ProtocolType.POP3)
			{
				this._pop3Servers.TryGetValue(domain, out result);
			}
			return result;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00010A14 File Offset: 0x0000EC14
		public Server FindInDatabase(string domain)
		{
			Server server = this.FindInDatabase(domain, ProtocolType.IMAP);
			if (server == null)
			{
				server = this.FindInDatabase(domain, ProtocolType.POP3);
			}
			return server;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00010A38 File Offset: 0x0000EC38
		public Server FindInDatabase(string domain, ProtocolType protocol)
		{
			Server result;
			try
			{
				using (SQLiteConnection sqliteConnection = new SQLiteConnection(this._connectionString))
				{
					sqliteConnection.Open();
					SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand();
					sqliteCommand.CommandText = <Module>.smethod_4<string>(-1800431605);
					sqliteCommand.ExecuteNonQuery();
					using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand2.CommandText = string.Format(<Module>.smethod_2<string>(-1430568235), protocol, domain);
						using (SQLiteDataReader sqliteDataReader = sqliteCommand2.ExecuteReader())
						{
							if (!sqliteDataReader.HasRows)
							{
								result = null;
							}
							else
							{
								sqliteDataReader.Read();
								result = new Server(sqliteDataReader.GetString(0), sqliteDataReader.GetString(1), sqliteDataReader.GetInt32(2), protocol, (sqliteDataReader.GetInt32(3) == 0) ? SocketType.SSL : SocketType.Plain);
							}
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00010B34 File Offset: 0x0000ED34
		public IEnumerable<Server> FindAll(string searchQuery, SearchServerType searchType)
		{
			List<Server> list = new List<Server>();
			try
			{
				this._readerWriteLock.EnterReadLock();
				using (SQLiteConnection sqliteConnection = new SQLiteConnection(this._connectionString))
				{
					sqliteConnection.Open();
					SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand();
					sqliteCommand.CommandText = <Module>.smethod_2<string>(783556846);
					sqliteCommand.ExecuteNonQuery();
					string arg = searchQuery.ToLower();
					using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand2.CommandText = string.Format(<Module>.smethod_6<string>(1703104427), searchType, arg);
						sqliteCommand2.CommandType = CommandType.Text;
						using (SQLiteDataReader sqliteDataReader = sqliteCommand2.ExecuteReader())
						{
							if (sqliteDataReader.HasRows)
							{
								list.AddRange(this.ReadAll(sqliteDataReader, ProtocolType.IMAP));
							}
						}
					}
					using (SQLiteCommand sqliteCommand3 = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand3.CommandText = string.Format(<Module>.smethod_3<string>(2145163483), searchType, arg);
						sqliteCommand3.CommandType = CommandType.Text;
						using (SQLiteDataReader sqliteDataReader2 = sqliteCommand3.ExecuteReader())
						{
							if (sqliteDataReader2.HasRows)
							{
								list.AddRange(this.ReadAll(sqliteDataReader2, ProtocolType.POP3));
							}
						}
					}
				}
			}
			finally
			{
				this._readerWriteLock.ExitReadLock();
			}
			return list;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00010CFC File Offset: 0x0000EEFC
		public void Add(Server server)
		{
			ProtocolType protocol = server.Protocol;
			if (protocol == ProtocolType.IMAP)
			{
				ConcurrentDictionary<string, Server> imapServers = this._imapServers;
				if (imapServers != null)
				{
					imapServers.TryAdd(server.Domain, server);
				}
			}
			else if (protocol == ProtocolType.POP3)
			{
				ConcurrentDictionary<string, Server> pop3Servers = this._pop3Servers;
				if (pop3Servers != null)
				{
					pop3Servers.TryAdd(server.Domain, server);
				}
			}
			try
			{
				this._readerWriteLock.EnterWriteLock();
				using (SQLiteConnection sqliteConnection = new SQLiteConnection(this._writeConnectionString))
				{
					sqliteConnection.Open();
					SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand();
					sqliteCommand.CommandText = <Module>.smethod_6<string>(-664266087);
					sqliteCommand.ExecuteNonQuery();
					using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand2.Transaction = sqliteConnection.BeginTransaction();
						sqliteCommand2.CommandText = string.Format(<Module>.smethod_4<string>(-1418126376), new object[]
						{
							server.Protocol,
							server.Domain,
							server.Hostname,
							server.Port,
							(server.Socket == SocketType.SSL) ? 0 : 1
						});
						sqliteCommand2.CommandType = CommandType.Text;
						sqliteCommand2.ExecuteNonQuery();
						sqliteCommand2.Transaction.Commit();
					}
				}
			}
			finally
			{
				this._readerWriteLock.ExitWriteLock();
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00010E5C File Offset: 0x0000F05C
		public void Update(Server original, Server updated)
		{
			try
			{
				this._readerWriteLock.EnterWriteLock();
				if (original.Protocol == updated.Protocol)
				{
					using (SQLiteConnection sqliteConnection = new SQLiteConnection(this._writeConnectionString))
					{
						sqliteConnection.Open();
						SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand();
						sqliteCommand.CommandText = <Module>.smethod_3<string>(-1547507446);
						sqliteCommand.ExecuteNonQuery();
						using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(sqliteConnection))
						{
							sqliteCommand2.Transaction = sqliteConnection.BeginTransaction();
							sqliteCommand2.CommandText = string.Format(<Module>.smethod_2<string>(1517974662), new object[]
							{
								original.Protocol,
								updated.Hostname,
								updated.Port,
								(updated.Socket == SocketType.SSL) ? 0 : 1,
								original.Domain
							});
							sqliteCommand2.CommandType = CommandType.Text;
							sqliteCommand2.ExecuteNonQuery();
							sqliteCommand2.Transaction.Commit();
							return;
						}
					}
				}
				this.Remove(original);
				this.Add(updated);
			}
			finally
			{
				this._readerWriteLock.ExitWriteLock();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00010F98 File Offset: 0x0000F198
		public void Remove(Server server)
		{
			try
			{
				this._readerWriteLock.EnterWriteLock();
				using (SQLiteConnection sqliteConnection = new SQLiteConnection(this._writeConnectionString))
				{
					sqliteConnection.Open();
					SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand();
					sqliteCommand.CommandText = <Module>.smethod_2<string>(-614551194);
					sqliteCommand.ExecuteNonQuery();
					using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand2.Transaction = sqliteConnection.BeginTransaction();
						sqliteCommand2.CommandText = string.Format(<Module>.smethod_4<string>(-18730073), server.Protocol, server.Domain);
						sqliteCommand2.CommandType = CommandType.Text;
						sqliteCommand2.ExecuteNonQuery();
						sqliteCommand2.Transaction.Commit();
					}
				}
			}
			finally
			{
				this._readerWriteLock.ExitWriteLock();
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000653E File Offset: 0x0000473E
		public void Close()
		{
			this._connection.Close();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0001107C File Offset: 0x0000F27C
		public string GetDatabaseVersion()
		{
			string result;
			try
			{
				using (SQLiteConnection sqliteConnection = new SQLiteConnection(this._connectionString))
				{
					sqliteConnection.Open();
					SQLiteCommand sqliteCommand = sqliteConnection.CreateCommand();
					sqliteCommand.CommandText = <Module>.smethod_5<string>(1526131999);
					sqliteCommand.ExecuteNonQuery();
					using (SQLiteCommand sqliteCommand2 = new SQLiteCommand(sqliteConnection))
					{
						sqliteCommand2.CommandText = <Module>.smethod_2<string>(669317931);
						using (SQLiteDataReader sqliteDataReader = sqliteCommand2.ExecuteReader())
						{
							if (!sqliteDataReader.HasRows)
							{
								result = null;
							}
							else
							{
								sqliteDataReader.Read();
								result = sqliteDataReader.GetString(0);
							}
						}
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000654B File Offset: 0x0000474B
		private Server Read(SQLiteDataReader reader, ProtocolType protocol)
		{
			if (!reader.Read())
			{
				return null;
			}
			return new Server(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), protocol, (reader.GetInt32(3) == 0) ? SocketType.SSL : SocketType.Plain);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0001114C File Offset: 0x0000F34C
		private IEnumerable<Server> ReadAll(SQLiteDataReader reader, ProtocolType protocol)
		{
			List<Server> list = new List<Server>();
			while (reader.Read())
			{
				list.Add(new Server(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), protocol, (reader.GetInt32(3) == 0) ? SocketType.SSL : SocketType.Plain));
			}
			return list;
		}

		// Token: 0x04000042 RID: 66
		private string _connectionString;

		// Token: 0x04000043 RID: 67
		private string _writeConnectionString;

		// Token: 0x04000044 RID: 68
		private SQLiteConnection _connection;

		// Token: 0x04000045 RID: 69
		private ReaderWriterLockSlim _readerWriteLock;

		// Token: 0x04000046 RID: 70
		private ConcurrentDictionary<string, Server> _imapServers = new ConcurrentDictionary<string, Server>();

		// Token: 0x04000047 RID: 71
		private ConcurrentDictionary<string, Server> _pop3Servers = new ConcurrentDictionary<string, Server>();
	}
}
