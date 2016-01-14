using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
namespace SmartQuant
{
	public class DataFileServerClient
	{
		private static object locker = new object();
		private TcpClient client;
		private FileManager fileManager;
		private Thread thread;
		public DataFileServerClient(TcpClient client, FileManager fileManager)
		{
			this.client = client;
			this.fileManager = fileManager;
			this.thread = new Thread(new ThreadStart(this.ThreadRun));
			this.thread.IsBackground = true;
			this.thread.Start();
		}
		private void ThreadRun()
		{
			Console.WriteLine(DateTime.Now + " Client thread started");
			NetworkStream stream = this.client.GetStream();
			BinaryReader binaryReader = new BinaryReader(stream);
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			FileStream fileStream = null;
			string text = "";
			try
			{
				while (true)
				{
					byte b = binaryReader.ReadByte();
					long offset;
					int num;
					switch (b)
					{
					case 0:
					{
						text = binaryReader.ReadString().Trim();
						FileMode fileMode = (FileMode)binaryReader.ReadByte();
						Console.WriteLine(string.Concat(new object[]
						{
							DateTime.Now,
							" Open file ",
							text,
							" in ",
							fileMode,
							" mode"
						}));
						lock (this.fileManager)
						{
							fileStream = this.fileManager.GetFile(text, fileMode);
							binaryWriter.Write(fileStream.Length);
							break;
						}
						goto IL_10B;
					}
					case 1:
						goto IL_10B;
					case 2:
						goto IL_16B;
					case 3:
					{
						offset = binaryReader.ReadInt64();
						num = binaryReader.ReadInt32();
						byte[] buffer = new byte[8192];
						int num2 = num;
						lock (fileStream)
						{
							fileStream.Seek(offset, SeekOrigin.Begin);
							while (num != 0)
							{
								int num3;
								if (num2 < 8192)
								{
									num3 = stream.Read(buffer, 0, num2);
								}
								else
								{
									num3 = stream.Read(buffer, 0, 8192);
								}
								if (num3 == 0)
								{
									break;
								}
								num2 -= num3;
								fileStream.Write(buffer, 0, num3);
							}
						}
						break;
					}
					case 4:
						Console.WriteLine(DateTime.Now + " Flush file " + text);
						lock (fileStream)
						{
							fileStream.Flush();
							break;
						}
						goto IL_16B;
					}
					IL_250:
					if (b == 1)
					{
						break;
					}
					continue;
					IL_10B:
					Console.WriteLine(DateTime.Now + " Close file " + text);
					goto IL_250;
					IL_16B:
					offset = binaryReader.ReadInt64();
					num = binaryReader.ReadInt32();
					byte[] buffer2 = new byte[num];
					lock (fileStream)
					{
						fileStream.Seek(offset, SeekOrigin.Begin);
						fileStream.Read(buffer2, 0, num);
					}
					stream.Write(buffer2, 0, num);
					goto IL_250;
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(DateTime.Now + " ServerClient exception. Connection closed.");
				Console.WriteLine(value);
			}
			finally
			{
				Console.WriteLine(DateTime.Now + " DataFileServerClient::Finally");
				if (fileStream != null)
				{
					lock (fileStream)
					{
						fileStream.Flush();
					}
				}
				this.client.Close();
			}
		}
	}
}
