using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000E1 RID: 225
	internal abstract class Client
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0000A225 File Offset: 0x00008425
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0000A22D File Offset: 0x0000842D
		public virtual string Host { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0000A236 File Offset: 0x00008436
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x0000A23E File Offset: 0x0000843E
		public virtual int Port { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0000A247 File Offset: 0x00008447
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0000A24F File Offset: 0x0000844F
		public virtual bool Ssl { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0000A258 File Offset: 0x00008458
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0000A260 File Offset: 0x00008460
		public virtual bool IsConnected { get; private set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0000A269 File Offset: 0x00008469
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x0000A271 File Offset: 0x00008471
		public virtual bool IsAuthenticated { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0000A27A File Offset: 0x0000847A
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x0000A282 File Offset: 0x00008482
		public virtual bool IsDisposed { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0000A28B File Offset: 0x0000848B
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x0000A293 File Offset: 0x00008493
		public virtual Encoding Encoding { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0000A29C File Offset: 0x0000849C
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0000A2A4 File Offset: 0x000084A4
		public int ServerTimeout { get; set; }

		// Token: 0x060006C5 RID: 1733 RVA: 0x0000A2AD File Offset: 0x000084AD
		public Client()
		{
			this.Encoding = Encoding.GetEncoding(1252);
			this.ServerTimeout = 10000;
		}

		// Token: 0x060006C6 RID: 1734
		internal abstract void OnLogin(string username, string password);

		// Token: 0x060006C7 RID: 1735
		internal abstract void CheckResultOK(string result);

		// Token: 0x060006C8 RID: 1736
		internal abstract void OnLogout();

		// Token: 0x060006C9 RID: 1737 RVA: 0x0000A2D0 File Offset: 0x000084D0
		protected virtual void OnConnected(string result)
		{
			this.CheckResultOK(result);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002BF24 File Offset: 0x0002A124
		public virtual void Connect(string hostname, int port, bool ssl, TcpClient tcp = null)
		{
			try
			{
				this.Host = hostname;
				this.Port = port;
				this.Ssl = ssl;
				if (tcp != null)
				{
					this._Connection = tcp;
				}
				else
				{
					this._Connection = new TcpClient(hostname, port);
				}
				this._Stream = this._Connection.GetStream();
				if (ssl)
				{
					RemoteCertificateValidationCallback userCertificateValidationCallback = (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors err) => true;
					SslStream sslStream = new SslStream(this._Stream, false, userCertificateValidationCallback);
					this._Stream = sslStream;
					sslStream.AuthenticateAsClient(hostname);
				}
				this.OnConnected(this.GetResponse());
				this.IsConnected = true;
				this.Host = hostname;
			}
			catch (Exception)
			{
				this.IsConnected = false;
				Utils.TryDispose<Stream>(ref this._Stream);
				Utils.TryDispose<TcpClient>(ref this._Connection);
				throw;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002C000 File Offset: 0x0002A200
		protected virtual void SendCommand(string command)
		{
			byte[] bytes = Encoding.Default.GetBytes(command + <Module>.smethod_6<string>(-1253514038));
			this._Stream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0000A2D9 File Offset: 0x000084D9
		public virtual void Login(string username, string password)
		{
			if (!this.IsConnected)
			{
				throw new Exception(<Module>.smethod_2<string>(-1397953336));
			}
			this.IsAuthenticated = false;
			this.OnLogin(username, password);
			this.IsAuthenticated = true;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0000A309 File Offset: 0x00008509
		protected virtual string SendCommandGetResponse(string command)
		{
			this.SendCommand(command);
			return this.GetResponse();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000A318 File Offset: 0x00008518
		protected virtual void SendCommandCheckOK(string command)
		{
			this.CheckResultOK(this.SendCommandGetResponse(command));
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000A327 File Offset: 0x00008527
		protected virtual string GetResponse()
		{
			return this.GetResponse(this.ServerTimeout);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002C038 File Offset: 0x0002A238
		protected virtual string GetResponse(int timeout)
		{
			int num = 0;
			return this._Stream.ReadLine(ref num, this.Encoding, null, timeout);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0000A335 File Offset: 0x00008535
		public virtual void Disconnect()
		{
			if (!this.IsConnected)
			{
				return;
			}
			if (this.IsAuthenticated)
			{
				this.OnLogout();
			}
			this.IsConnected = false;
			Utils.TryDispose<Stream>(ref this._Stream);
			Utils.TryDispose<TcpClient>(ref this._Connection);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0000A36B File Offset: 0x0000856B
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002C064 File Offset: 0x0002A264
		protected virtual void Dispose(bool disposing)
		{
			lock (this)
			{
				if (!this.IsDisposed && disposing)
				{
					this.IsDisposed = true;
					this.Disconnect();
				}
			}
			this._Stream = null;
			this._Connection = null;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
		protected virtual void CheckConnectionStatus()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!this.IsConnected)
			{
				throw new Exception(<Module>.smethod_5<string>(1902059681));
			}
			if (!this.IsAuthenticated)
			{
				throw new Exception(<Module>.smethod_6<string>(78628362));
			}
		}

		// Token: 0x0400037A RID: 890
		protected TcpClient _Connection;

		// Token: 0x0400037B RID: 891
		protected Stream _Stream;
	}
}
