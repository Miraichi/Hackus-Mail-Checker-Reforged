using System;
using System.ComponentModel;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Hackus_Mail_Checker_Reforged.Net.Mail.Utilities;

namespace Hackus_Mail_Checker_Reforged.Net.Mail
{
	// Token: 0x020000FB RID: 251
	public abstract class MailClient : IDisposable
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0000ACF5 File Offset: 0x00008EF5
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x0000ACFD File Offset: 0x00008EFD
		public ConnectionState State { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0000AD06 File Offset: 0x00008F06
		public Socket Socket
		{
			get
			{
				return this.TcpClient.Client;
			}
		}

		// Token: 0x060007C1 RID: 1985
		public abstract void Authenticate(string username, string password);

		// Token: 0x060007C2 RID: 1986
		protected abstract void CheckOk(string response);

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002F2D8 File Offset: 0x0002D4D8
		public void Connect(string host, int port, bool ssl, TcpClient tcp = null)
		{
			if (tcp != null)
			{
				this.TcpClient = tcp;
			}
			else
			{
				this.CreateConnection(host, port);
			}
			this.LocalStream = (ssl ? this.StandartSSL(host) : this.TcpClient.GetStream());
			this.CheckOk(this.ReadLine());
			this.isDisposed = false;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002F32C File Offset: 0x0002D52C
		private void CreateConnection(string host, int port)
		{
			this.TcpClient = new TcpClient
			{
				ReceiveTimeout = this.ReadWriteTimeout,
				SendTimeout = this.ReadWriteTimeout,
				NoDelay = true,
				LingerState = new LingerOption(true, 0),
				Client = 
				{
					SendTimeout = this.ReadWriteTimeout,
					ReceiveTimeout = this.ReadWriteTimeout,
					NoDelay = true,
					LingerState = new LingerOption(true, 0)
				}
			};
			IAsyncResult localResult = null;
			Exception connectException = null;
			ManualResetEventSlim connectDoneEvent = new ManualResetEventSlim();
			try
			{
				localResult = this.TcpClient.BeginConnect(host, port, delegate(IAsyncResult ar)
				{
					try
					{
						if (MailClient.<>c__DisplayClass14_0.smethod_0(this.TcpClient) == null)
						{
							this.TcpClient.DisposeObject(ar);
							return;
						}
						MailClient.<>c__DisplayClass14_0.smethod_2(MailClient.<>c__DisplayClass14_0.smethod_1(ar));
						localResult = null;
						MailClient.<>c__DisplayClass14_0.smethod_3(this.TcpClient, ar);
					}
					catch (NullReferenceException)
					{
						this.TcpClient.DisposeObject(ar);
						return;
					}
					catch (ThreadAbortException connectException)
					{
						this.TcpClient.DisposeObject(ar);
						ThreadAbortException connectException = connectException;
					}
					catch (ObjectDisposedException)
					{
						return;
					}
					catch (Exception connectException2)
					{
						ThreadAbortException connectException = connectException2;
					}
					MailClient.<>c__DisplayClass14_0.smethod_4(connectDoneEvent);
				}, this.TcpClient);
			}
			catch
			{
				this.TcpClient.DisposeObject(localResult);
				this.Dispose();
				throw;
			}
			if (!connectDoneEvent.Wait(this.ConnectTimeout))
			{
				this.TcpClient.DisposeObject(localResult);
				this.Dispose();
				throw new TimeoutException();
			}
			if (connectException != null)
			{
				this.TcpClient.DisposeObject(localResult);
				this.Dispose();
				throw connectException;
			}
			if (!this.TcpClient.Connected)
			{
				this.TcpClient.DisposeObject(localResult);
				this.Dispose();
				throw new TimeoutException();
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002F4A8 File Offset: 0x0002D6A8
		private Stream StandartSSL(string host)
		{
			Hackus_Mail_Checker_Reforged.Net.Mail.Utilities.SslStream sslStream = null;
			Stream result;
			try
			{
				sslStream = new Hackus_Mail_Checker_Reforged.Net.Mail.Utilities.SslStream(this.TcpClient.GetStream(), false, (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors err) => true);
				sslStream.AuthenticateAsClient(host, null, SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, false);
				if (!sslStream.IsAuthenticated)
				{
					sslStream.DisposeObject();
					this.Dispose();
					throw new IOException();
				}
				result = sslStream;
			}
			catch (IOException ex)
			{
				sslStream.DisposeObject();
				this.Dispose();
				if (ex.InnerException is Win32Exception)
				{
					if ((ex.InnerException as Win32Exception).ErrorCode != 10060)
					{
						throw new IOException();
					}
					throw new TimeoutException();
				}
				else
				{
					if (ex.InnerException == null)
					{
						throw;
					}
					throw ex.InnerException;
				}
			}
			catch (Win32Exception ex2)
			{
				sslStream.DisposeObject();
				this.Dispose();
				if (ex2.ErrorCode == 10060)
				{
					throw new TimeoutException();
				}
				throw new IOException();
			}
			catch (Exception ex3)
			{
				sslStream.DisposeObject();
				this.Dispose();
				if (!(ex3.InnerException is Win32Exception))
				{
					if (ex3.InnerException != null)
					{
						throw ex3.InnerException;
					}
					throw;
				}
				else
				{
					if ((ex3.InnerException as Win32Exception).ErrorCode == 10060)
					{
						throw new TimeoutException();
					}
					throw new IOException();
				}
			}
			return result;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0000AD13 File Offset: 0x00008F13
		protected string ReadLine()
		{
			return this.LocalStream.ReadLineAsString(this.ReadWriteTimeout);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0000AD26 File Offset: 0x00008F26
		public void Dispose()
		{
			this.Release();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002F600 File Offset: 0x0002D800
		protected virtual void Release()
		{
			try
			{
				if (!this.isDisposed)
				{
					this.LocalStream.DisposeObject();
					this.TcpClient.DisposeObject();
					this.isDisposed = true;
				}
			}
			catch (ThreadAbortException)
			{
				this.isDisposed = false;
				this.Release();
				throw;
			}
			finally
			{
				this.TcpClient = null;
				this.LocalStream = null;
			}
		}

		// Token: 0x040003E7 RID: 999
		private bool isDisposed;

		// Token: 0x040003E8 RID: 1000
		public Stream LocalStream;

		// Token: 0x040003E9 RID: 1001
		public TcpClient TcpClient;

		// Token: 0x040003EB RID: 1003
		public int ConnectTimeout = 10000;

		// Token: 0x040003EC RID: 1004
		public int ReadWriteTimeout = 10000;
	}
}
