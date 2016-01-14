using System;
using System.IO;
namespace SmartQuant
{
	internal class DataBuffer
	{
		internal MemoryStream stream;
		internal BinaryReader reader;
		internal BinaryWriter writer;
		public int Length
		{
			get
			{
				return (int)this.stream.Length;
			}
		}
		public DataBuffer()
		{
			this.stream = new MemoryStream();
			this.reader = new BinaryReader(this.stream);
			this.writer = new BinaryWriter(this.stream);
		}
		public byte[] GetBuffer()
		{
			return this.stream.GetBuffer();
		}
		public void CopyTo(Stream stream)
		{
			stream.Write(this.stream.GetBuffer(), 0, (int)this.stream.Length);
		}
		public void Write(bool value)
		{
			this.writer.Write(value);
		}
		public void Write(byte value)
		{
			this.writer.Write(value);
		}
		public void Write(sbyte value)
		{
			this.writer.Write(value);
		}
		public void Write(short value)
		{
			this.writer.Write(value);
		}
		public void Write(int value)
		{
			this.writer.Write(value);
		}
		public void Write(long value)
		{
			this.writer.Write(value);
		}
		public void Write(DateTime value)
		{
			this.writer.Write(value.Ticks);
		}
		public void Write(string value)
		{
			this.writer.Write(value);
		}
		public void Write(byte[] buffer, int position, int count)
		{
			this.stream.Write(buffer, position, count);
		}
		public void Write(byte[] buffer, int count)
		{
			this.stream.Write(buffer, 0, count);
		}
		public byte ReadByte()
		{
			return this.reader.ReadByte();
		}
		public sbyte ReadSByte()
		{
			return this.reader.ReadSByte();
		}
		public short ReadInt16()
		{
			return this.reader.ReadInt16();
		}
		public int ReadInt32()
		{
			return this.reader.ReadInt32();
		}
		public long ReadInt64()
		{
			return this.reader.ReadInt64();
		}
		public bool ReadBoolean()
		{
			return this.reader.ReadBoolean();
		}
		public DateTime ReadDateTime()
		{
			return new DateTime(this.reader.ReadInt64());
		}
		public string ReadString()
		{
			return this.reader.ReadString();
		}
		public void Clear()
		{
			this.stream.SetLength(0L);
		}
	}
}
