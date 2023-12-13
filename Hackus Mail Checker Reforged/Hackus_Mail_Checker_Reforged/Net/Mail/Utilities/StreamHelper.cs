using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Hackus_Mail_Checker_Reforged.Net.Mail.Utilities
{
	// Token: 0x02000107 RID: 263
	public static class StreamHelper
	{
		// Token: 0x060007F5 RID: 2037 RVA: 0x0003108C File Offset: 0x0002F28C
		public static byte[] ReadLineAsBytes(this Stream stream, int readTimeOut = 15000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_3<string>(2010242753));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = readTimeOut;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				for (;;)
				{
					int num = stream.ReadByte();
					if (num == -1 && memoryStream.Length > 0L)
					{
						break;
					}
					if (num == -1)
					{
						if (memoryStream.Length == 0L)
						{
							goto IL_7C;
						}
					}
					if (num != 13)
					{
						if (num == 10)
						{
							goto IL_92;
						}
						memoryStream.WriteByte((byte)num);
					}
				}
				return memoryStream.ToArray();
				IL_7C:
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_6<string>(-1996104120));
				IL_92:
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00031154 File Offset: 0x0002F354
		public static byte[] ReadBytesByOne(this Stream stream, int MaxBufferLenght = 1048576, int ReadTimeOut = 15000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_6<string>(-1996104120));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = ReadTimeOut;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				do
				{
					int num = stream.ReadByte();
					if (num == -1)
					{
						if (memoryStream.Length > 0L)
						{
							goto IL_90;
						}
					}
					if (num == -1 && memoryStream.Length == 0L)
					{
						goto IL_71;
					}
					memoryStream.WriteByte((byte)num);
				}
				while (memoryStream.Length != (long)MaxBufferLenght);
				return memoryStream.ToArray();
				IL_71:
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_3<string>(2010242753));
				IL_90:
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00031218 File Offset: 0x0002F418
		public static byte[] ReadBytesByBuffer(this Stream stream, int MaxBufferLength = 1048576, int ReadTimeOut = 15000)
		{
			if (stream.CanRead)
			{
				if (stream.CanTimeout)
				{
					stream.ReadTimeout = ReadTimeOut;
				}
				ManualResetEventSlim readDoneEvent = new ManualResetEventSlim();
				Exception innerException = null;
				IAsyncResult asyncResult = null;
				byte[] result;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int rData = 0;
					byte[] array = new byte[MaxBufferLength];
					try
					{
						AsyncCallback asyncCallback = null;
						AsyncCallback <>9__0;
						for (;;)
						{
							if (asyncResult == null)
							{
								goto IL_6E;
							}
							if (asyncResult.IsCompleted)
							{
								goto IL_6E;
							}
							IL_C0:
							if (innerException != null)
							{
								goto IL_198;
							}
							if (!readDoneEvent.Wait(ReadTimeOut))
							{
								goto IL_17C;
							}
							if (rData == 0)
							{
								if (asyncResult.IsCompleted)
								{
									break;
								}
								continue;
							}
							else
							{
								if (rData == 0 && memoryStream.Length == 0L)
								{
									goto IL_160;
								}
								if (rData <= 0)
								{
									continue;
								}
								memoryStream.Write(array, 0, rData);
								if (memoryStream.Length != (long)MaxBufferLength)
								{
									continue;
								}
								goto IL_1E5;
							}
							IL_6E:
							readDoneEvent = new ManualResetEventSlim();
							Stream stream2 = stream;
							byte[] buffer = array;
							int offset = 0;
							AsyncCallback callback;
							if ((callback = asyncCallback) == null)
							{
								AsyncCallback asyncCallback2;
								if ((asyncCallback2 = <>9__0) == null)
								{
									asyncCallback2 = (<>9__0 = delegate(IAsyncResult innerAr)
									{
										try
										{
											StreamHelper.<>c__DisplayClass6_0.smethod_1(StreamHelper.<>c__DisplayClass6_0.smethod_0(innerAr));
											rData = StreamHelper.<>c__DisplayClass6_0.smethod_2(stream, innerAr);
										}
										catch (Exception ex)
										{
											if (ex is NullReferenceException || ex is ObjectDisposedException)
											{
												innerException = StreamHelper.<>c__DisplayClass6_0.smethod_3(<Module>.smethod_5<string>(-16128818), ex);
												return;
											}
											innerException = ex;
										}
										StreamHelper.<>c__DisplayClass6_0.smethod_4(readDoneEvent);
									});
								}
								asyncCallback = (callback = asyncCallback2);
							}
							asyncResult = stream2.BeginRead(buffer, offset, MaxBufferLength, callback, null);
							goto IL_C0;
						}
						if (memoryStream.Length != (long)MaxBufferLength)
						{
							stream.DisposeObject(asyncResult);
							throw new IOException(<Module>.smethod_2<string>(-1762264601));
						}
						return memoryStream.ToArray();
						IL_160:
						stream.DisposeObject(asyncResult);
						throw new IOException(<Module>.smethod_2<string>(-1762264601));
						IL_17C:
						stream.DisposeObject(asyncResult);
						throw new TimeoutException(<Module>.smethod_2<string>(1433800077));
						IL_198:
						stream.DisposeObject(asyncResult);
						if (!(innerException is IOException) && !(innerException is SocketException))
						{
							throw innerException;
						}
						throw new IOException(<Module>.smethod_3<string>(-1401485034) + innerException.Message);
						IL_1E5:
						result = memoryStream.ToArray();
					}
					catch (NotSupportedException innerException)
					{
						stream.DisposeObject(asyncResult);
						NotSupportedException innerException3;
						throw new IOException(<Module>.smethod_5<string>(-16128818), innerException3);
					}
					catch (EndOfStreamException innerException2)
					{
						if (memoryStream.Length != (long)MaxBufferLength)
						{
							stream.DisposeObject(asyncResult);
							throw new IOException(<Module>.smethod_6<string>(-1996104120), innerException2);
						}
						result = memoryStream.ToArray();
					}
				}
				return result;
			}
			stream.DisposeObject();
			throw new IOException(<Module>.smethod_5<string>(-16128818));
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000314E0 File Offset: 0x0002F6E0
		public static string ReadLineAsString(this Stream stream, int ReadTimeOut = 15000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_4<string>(-322178440));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = ReadTimeOut;
			}
			int num = -1;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				for (;;)
				{
					try
					{
						num = stream.ReadByte();
					}
					catch (EndOfStreamException)
					{
						if (memoryStream.Length <= 0L)
						{
							stream.DisposeObject();
							throw new IOException(<Module>.smethod_6<string>(-1996104120));
						}
						return Encoding.UTF8.GetString(memoryStream.ToArray());
					}
					if (num == -1)
					{
						goto IL_96;
					}
					byte b = (byte)num;
					if (b != 13)
					{
						if (b == 10)
						{
							break;
						}
						memoryStream.WriteByte(b);
					}
				}
				return Encoding.UTF8.GetString(memoryStream.ToArray());
				IL_96:
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_4<string>(-322178440));
			}
			string result;
			return result;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0000AE25 File Offset: 0x00009025
		public static IEnumerable<string> ReadLines(this Stream stream, int ReadTimeOut = 15000)
		{
			if (!StreamHelper.<ReadLines>d__8.smethod_1(stream))
			{
				goto IL_63;
			}
			if (StreamHelper.<ReadLines>d__8.smethod_3(stream))
			{
				StreamHelper.<ReadLines>d__8.smethod_4(stream, ReadTimeOut);
			}
			StreamReader reader2 = StreamHelper.<ReadLines>d__8.smethod_6(stream, StreamHelper.<ReadLines>d__8.smethod_5(), true, 4096, true);
			for (;;)
			{
				string text;
				try
				{
					IL_8A:
					if ((text = StreamHelper.<ReadLines>d__8.smethod_7(reader2)) == null)
					{
						break;
					}
					goto IL_AA;
					IL_63:
					stream.DisposeObject();
					throw StreamHelper.<ReadLines>d__8.smethod_2(<Module>.smethod_4<string>(-322178440));
					IL_7E:
					int num;
					if (num != 1)
					{
						goto IL_BC;
					}
					goto IL_8A;
				}
				finally
				{
					if (reader2 != null)
					{
						StreamHelper.<ReadLines>d__8.smethod_8(reader2);
					}
				}
				break;
				IL_AA:
				yield return text;
				goto IL_7E;
			}
			reader2 = null;
			yield break;
			IL_BC:
			yield break;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000315D8 File Offset: 0x0002F7D8
		public static string ReadToEnd(this Stream stream, int ReadTimeOut = 15000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_4<string>(-322178440));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = ReadTimeOut;
			}
			string result;
			using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0003164C File Offset: 0x0002F84C
		public static string ReadLine(this Stream stream, int ReadTimeOut = 15000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_5<string>(-16128818));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = ReadTimeOut;
			}
			string result;
			using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
			{
				result = streamReader.ReadLine();
			}
			return result;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000316C0 File Offset: 0x0002F8C0
		public static string ReadMsgAsString(this Stream stream, Socket Sck, int MaxBufferLenght = 1048576, int ReadTimeOut = 30000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_2<string>(-1762264601));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = ReadTimeOut;
			}
			ManualResetEventSlim readDoneEvent = new ManualResetEventSlim();
			Exception innerException = null;
			IAsyncResult asyncResult = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int rData = 0;
				byte[] array = new byte[MaxBufferLenght];
				try
				{
					AsyncCallback asyncCallback = null;
					AsyncCallback <>9__0;
					for (;;)
					{
						if (asyncResult == null || asyncResult.IsCompleted)
						{
							readDoneEvent = new ManualResetEventSlim();
							Stream stream2 = stream;
							byte[] buffer = array;
							int offset = 0;
							AsyncCallback callback;
							if ((callback = asyncCallback) == null)
							{
								AsyncCallback asyncCallback2;
								if ((asyncCallback2 = <>9__0) == null)
								{
									asyncCallback2 = (<>9__0 = delegate(IAsyncResult innerAr)
									{
										try
										{
											StreamHelper.<>c__DisplayClass11_0.smethod_1(StreamHelper.<>c__DisplayClass11_0.smethod_0(innerAr));
											rData = StreamHelper.<>c__DisplayClass11_0.smethod_2(stream, innerAr);
										}
										catch (Exception ex)
										{
											if (ex is NullReferenceException || ex is ObjectDisposedException)
											{
												innerException = StreamHelper.<>c__DisplayClass11_0.smethod_3(<Module>.smethod_5<string>(-16128818));
												return;
											}
											innerException = ex;
										}
										StreamHelper.<>c__DisplayClass11_0.smethod_4(readDoneEvent);
									});
								}
								asyncCallback = (callback = asyncCallback2);
							}
							asyncResult = stream2.BeginRead(buffer, offset, MaxBufferLenght, callback, null);
						}
						if (innerException != null)
						{
							goto IL_30A;
						}
						if (!readDoneEvent.Wait(ReadTimeOut))
						{
							goto IL_2EE;
						}
						if (rData != 0)
						{
							if (rData == 0 && memoryStream.Length == 0L)
							{
								goto IL_23A;
							}
							if (rData > 0)
							{
								memoryStream.Write(array, 0, rData);
								int rData2 = rData;
								if (rData2 != 1)
								{
									if (rData2 != 2)
									{
										if (array[rData - 1] == 10 && array[rData - 2] == 13)
										{
											byte b = array[rData - 3];
											if (b <= 46)
											{
												if (b != 41)
												{
													if (b != 46)
													{
														continue;
													}
												}
												if (Sck.WaitAvailableData(500, 50))
												{
													goto IL_256;
												}
											}
											else if (b - 100 <= 1 || b == 116)
											{
												if (Sck.WaitAvailableData(500, 50))
												{
													break;
												}
											}
										}
									}
									else if (array[rData - 1] == 10 && array[rData - 2] == 13)
									{
										if (Sck.WaitAvailableData(500, 50))
										{
											goto IL_284;
										}
									}
								}
								else if (array[rData - 1] == 10)
								{
									if (Sck.WaitAvailableData(500, 50))
									{
										goto IL_29B;
									}
								}
							}
						}
						else if (asyncResult.IsCompleted)
						{
							goto IL_2B2;
						}
					}
					return Encoding.UTF8.GetString(memoryStream.ToArray());
					IL_23A:
					stream.DisposeObject(asyncResult);
					throw new IOException(<Module>.smethod_3<string>(2010242753));
					IL_256:
					return Encoding.UTF8.GetString(memoryStream.ToArray());
					IL_284:
					return Encoding.UTF8.GetString(memoryStream.ToArray());
					IL_29B:
					return Encoding.UTF8.GetString(memoryStream.ToArray());
					IL_2B2:
					if (memoryStream.Length <= 0L || !Sck.WaitAvailableData(500, 50))
					{
						stream.DisposeObject(asyncResult);
						throw new IOException(<Module>.smethod_3<string>(2010242753));
					}
					return Encoding.UTF8.GetString(memoryStream.ToArray());
					IL_2EE:
					stream.DisposeObject(asyncResult);
					throw new IOException(<Module>.smethod_4<string>(1082026182));
					IL_30A:
					stream.DisposeObject(asyncResult);
					if (!(innerException is IOException) && !(innerException is SocketException))
					{
						throw innerException;
					}
					throw new IOException(<Module>.smethod_3<string>(2010242753));
				}
				catch (NotSupportedException)
				{
					stream.DisposeObject(asyncResult);
					throw new IOException(<Module>.smethod_2<string>(-1762264601));
				}
				catch (EndOfStreamException)
				{
					if (memoryStream.Length <= 0L)
					{
						stream.DisposeObject(asyncResult);
						throw new IOException(<Module>.smethod_3<string>(2010242753));
					}
					return Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
			string result;
			return result;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00031AE4 File Offset: 0x0002FCE4
		public static string ReadPartAsString(this Stream stream, Socket Sck, int MaxBufferLenght = 1048576, int ReadTimeOut = 30000)
		{
			if (!stream.CanRead)
			{
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_2<string>(-1762264601));
			}
			if (stream.CanTimeout)
			{
				stream.ReadTimeout = ReadTimeOut;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = 0;
				byte[] array = new byte[MaxBufferLenght];
				IAsyncResult asyncResult = stream.BeginRead(array, 0, MaxBufferLenght, null, null);
				for (;;)
				{
					try
					{
						num = stream.EndRead(asyncResult);
					}
					catch (EndOfStreamException)
					{
						if (memoryStream.Length <= 0L)
						{
							stream.DisposeObject();
							throw new IOException(<Module>.smethod_2<string>(-1762264601));
						}
						return Encoding.UTF8.GetString(memoryStream.ToArray());
					}
					if (num == 0)
					{
						if (asyncResult.IsCompleted)
						{
							break;
						}
					}
					else
					{
						if (num == 0 && memoryStream.Length == 0L)
						{
							goto IL_1CB;
						}
						if (num > 0)
						{
							memoryStream.Write(array, 0, num);
							if (num != 1)
							{
								if (num != 2)
								{
									if (array[num - 1] == 10 && array[num - 2] == 13)
									{
										byte b = array[num - 3];
										if (b <= 46)
										{
											if (b != 41)
											{
												if (b == 46)
												{
													goto Block_16;
												}
											}
											else
											{
												if (Sck.Available == 0)
												{
													goto Block_17;
												}
												goto IL_16B;
											}
										}
										else if (b - 100 <= 1 || b == 116)
										{
											goto IL_1F5;
										}
										Encoding.UTF8.GetString(memoryStream.ToArray());
										Console.WriteLine((int)array[num - 3]);
									}
								}
								else if (array[num - 1] == 10)
								{
									if (array[num - 2] == 13)
									{
										goto IL_209;
									}
								}
							}
							else if (array[num - 1] == 10)
							{
								goto IL_21D;
							}
						}
						IL_16B:
						if (asyncResult.IsCompleted)
						{
							asyncResult = stream.BeginRead(array, 0, MaxBufferLenght, null, null);
						}
					}
				}
				if (memoryStream.Length > 0L && Sck.Available == 0)
				{
					return Encoding.UTF8.GetString(memoryStream.ToArray());
				}
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_5<string>(-16128818));
				Block_16:
				goto IL_1F5;
				Block_17:
				return Encoding.UTF8.GetString(memoryStream.ToArray());
				IL_1CB:
				stream.DisposeObject();
				throw new IOException(<Module>.smethod_6<string>(-1996104120));
				IL_1F5:
				return Encoding.UTF8.GetString(memoryStream.ToArray());
				IL_209:
				return Encoding.UTF8.GetString(memoryStream.ToArray());
				IL_21D:
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
			string result;
			return result;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00031D68 File Offset: 0x0002FF68
		public static string ReadLineAsAscii(this Stream stream, int ReadTimeOut = 15000)
		{
			byte[] array = stream.ReadLineAsBytes(ReadTimeOut);
			if (array != null)
			{
				return Encoding.ASCII.GetString(array);
			}
			return null;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00031D90 File Offset: 0x0002FF90
		public static void WriteLine(this Stream stream, string line, int WriteTimeOut = 15000)
		{
			if (stream.CanTimeout)
			{
				stream.WriteTimeout = WriteTimeOut;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(line + Environment.NewLine);
			stream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00031DD0 File Offset: 0x0002FFD0
		public static void WaitAvailableData(this NetworkStream nStream, int TimeOnWaitingResponse = 1500, int TimeInterval = 150)
		{
			int num = 0;
			while (!nStream.DataAvailable)
			{
				Thread.Sleep(TimeInterval);
				num += TimeInterval;
				if (num > TimeOnWaitingResponse)
				{
					break;
				}
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00031DF8 File Offset: 0x0002FFF8
		public static bool WaitAvailableData(this Socket Sck, int TimeOnWaitingResponse = 1500, int TimeInterval = 150)
		{
			int num = 0;
			while (Sck.Available == 0)
			{
				Thread.Sleep(TimeInterval);
				num += TimeInterval;
				if (num > TimeOnWaitingResponse)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0000AE3C File Offset: 0x0000903C
		public static void Write(this Stream stream, byte[] bytes, int WriteTimeOut = 15000)
		{
			if (stream.CanTimeout)
			{
				stream.WriteTimeout = WriteTimeOut;
			}
			stream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x040003FB RID: 1019
		private const int StreamException = -1;

		// Token: 0x040003FC RID: 1020
		private const int NewLine = 10;

		// Token: 0x040003FD RID: 1021
		private const int CarrieageReturn = 13;

		// Token: 0x040003FE RID: 1022
		private const int Dot = 46;
	}
}
