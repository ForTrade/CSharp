using System;
using System.IO;
using System.Net.Sockets;
namespace SmartQuant
{
	public class NetDataFile : DataFile
	{
		internal string host;
		internal int port = 1000;
		internal TcpClient client;
		public NetDataFile(string name, string host, StreamerManager streamerManager = null) : base(name, streamerManager)
		{
			this.host = host;
		}
		protected override bool OpenFileStream(string name, FileMode mode)
		{
			this.client = new TcpClient(this.host, this.port);
			this.stream = this.client.GetStream();
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(0);
			binaryWriter.Write(name);
			binaryWriter.Write((byte)mode);
			this.stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			BinaryReader binaryReader = new BinaryReader(this.stream);
			long num = binaryReader.ReadInt64();
			return num != 0L;
		}
		protected override void CloseFileStream()
		{
			this.stream.WriteByte(1);
			this.client.Close();
		}
		public override void Open(FileMode mode = FileMode.OpenOrCreate)
		{
			if (this.isOpen)
			{
				Console.WriteLine("DataFile::Open File is already open: " + this.name);
				return;
			}
			if (!this.OpenFileStream(this.name, mode))
			{
				this.beginPosition = 62L;
				this.endPosition = 62L;
				this.keysPosition = 62L;
				this.freePosition = 62L;
				this.keysLength = 0;
				this.freeLength = 0;
				this.keysNumber = 0;
				this.freeNumber = 0;
				this.isModified = true;
				base.WriteHeader();
			}
			else
			{
				if (!base.ReadHeader())
				{
					Console.WriteLine("DataFile::Open Error opening file " + this.name);
					return;
				}
				base.ReadKeys();
				base.ReadFree();
			}
			this.isOpen = true;
		}
		protected internal override void ReadBuffer(byte[] buffer, long position, int length)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(2);
			binaryWriter.Write(position);
			binaryWriter.Write(length);
			this.stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			memoryStream = new MemoryStream(buffer);
			binaryWriter = new BinaryWriter(memoryStream);
			byte[] buffer2 = new byte[8192];
			int num = length;
			while (num != 0)
			{
				int num2;
				if (num < 8192)
				{
					num2 = this.stream.Read(buffer2, 0, num);
				}
				else
				{
					num2 = this.stream.Read(buffer2, 0, 8192);
				}
				if (num2 == 0)
				{
					return;
				}
				num -= num2;
				memoryStream.Write(buffer2, 0, num2);
			}
		}
		protected internal override void WriteBuffer(byte[] buffer, long position, int length)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(3);
			binaryWriter.Write(position);
			binaryWriter.Write(length);
			this.stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			this.stream.Write(buffer, 0, length);
		}
		public override void Flush()
		{
			if (this.isModified)
			{
				base.Flush();
				this.stream.WriteByte(4);
			}
		}
	}
}
