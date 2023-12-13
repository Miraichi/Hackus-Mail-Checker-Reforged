using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Utilities
{
	// Token: 0x02000103 RID: 259
	public static class DisposeHelper
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x00030DA8 File Offset: 0x0002EFA8
		public static void DisposeObject(this TcpClient tcpClient)
		{
			if (tcpClient == null)
			{
				return;
			}
			try
			{
				tcpClient.Client.DisposeObject();
				tcpClient.Dispose();
			}
			catch (ThreadAbortException)
			{
				tcpClient.DisposeObject();
				throw;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00030DE8 File Offset: 0x0002EFE8
		public static void DisposeObject(this TcpClient tcpClient, IAsyncResult ar)
		{
			if (tcpClient == null)
			{
				return;
			}
			if (ar != null)
			{
				try
				{
					tcpClient.Client.DisposeObject(ar);
					tcpClient.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				catch (ThreadAbortException)
				{
					tcpClient.DisposeObject(ar);
					throw;
				}
				return;
			}
			tcpClient.DisposeObject();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00030E40 File Offset: 0x0002F040
		public static void DisposeObject(this Socket socket)
		{
			if (socket == null)
			{
				return;
			}
			try
			{
				try
				{
					if (socket.Connected)
					{
						socket.Disconnect(false);
					}
				}
				catch
				{
				}
				socket.Dispose();
			}
			catch (ObjectDisposedException)
			{
			}
			catch (ThreadAbortException)
			{
				socket.DisposeObject();
				throw;
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00030EA4 File Offset: 0x0002F0A4
		public static void DisposeObject(this Socket socket, IAsyncResult ar)
		{
			if (socket == null)
			{
				return;
			}
			if (ar != null)
			{
				try
				{
					try
					{
						if (ar.IsCompleted)
						{
							socket.EndConnect(ar);
						}
					}
					catch
					{
					}
					if (socket.Connected)
					{
						try
						{
							socket.Disconnect(false);
						}
						catch
						{
						}
					}
					socket.Dispose();
				}
				catch (ObjectDisposedException)
				{
				}
				catch (ThreadAbortException)
				{
					socket.DisposeObject(ar);
					throw;
				}
				return;
			}
			socket.DisposeObject();
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00030F30 File Offset: 0x0002F130
		public static void DisposeObject(this NetworkStream nStream)
		{
			if (nStream == null)
			{
				return;
			}
			try
			{
				try
				{
					nStream.Close();
				}
				catch
				{
				}
				nStream.Dispose();
			}
			catch (ThreadAbortException)
			{
				nStream.DisposeObject();
				throw;
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00030F7C File Offset: 0x0002F17C
		public static void DisposeObject(this SslStream stream)
		{
			if (stream == null)
			{
				return;
			}
			try
			{
				try
				{
					stream.Close();
				}
				catch
				{
				}
				stream.Dispose();
			}
			catch (ThreadAbortException)
			{
				stream.DisposeObject();
				throw;
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00030FC8 File Offset: 0x0002F1C8
		public static void DisposeObject(this Stream stream)
		{
			if (stream == null)
			{
				return;
			}
			try
			{
				try
				{
					stream.Close();
				}
				catch
				{
				}
				stream.Dispose();
			}
			catch (ThreadAbortException)
			{
				stream.DisposeObject();
				throw;
			}
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00031014 File Offset: 0x0002F214
		public static void DisposeObject(this Stream stream, IAsyncResult ar)
		{
			if (stream == null)
			{
				return;
			}
			if (ar != null)
			{
				try
				{
					try
					{
						if (ar.IsCompleted)
						{
							stream.EndRead(ar);
						}
					}
					catch
					{
					}
					try
					{
						stream.Close();
					}
					catch
					{
					}
					stream.Dispose();
				}
				catch (ThreadAbortException)
				{
					stream.DisposeObject();
					throw;
				}
				return;
			}
			stream.DisposeObject();
		}

		// Token: 0x040003FA RID: 1018
		private static object _locker = new object();
	}
}
