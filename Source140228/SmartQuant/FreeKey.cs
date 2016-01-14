using System;
using System.IO;
namespace SmartQuant
{
	internal class FreeKey : IComparable<FreeKey>
	{
		internal DataFile file;
		internal string label = "FKey";
		internal long position = -1L;
		internal int length = -1;
		public FreeKey()
		{
		}
		public FreeKey(DataFile file, long position = -1L, int length = -1)
		{
			this.file = file;
			this.position = position;
			this.length = length;
		}
		public FreeKey(ObjectKey key)
		{
			this.file = key.file;
			this.position = key.position;
			this.length = key.recLength;
		}
		internal void Write(BinaryWriter writer)
		{
			writer.Write(this.label);
			writer.Write(this.position);
			writer.Write(this.length);
		}
		internal void Read(BinaryReader reader, bool readLabel = true)
		{
			if (readLabel)
			{
				this.label = reader.ReadString();
				if (this.label != "FKey")
				{
					Console.WriteLine("FreeKey::ReadKey This is not FreeKey! label = " + this.label);
				}
			}
			this.position = reader.ReadInt64();
			this.length = reader.ReadInt32();
		}
		public int CompareTo(FreeKey other)
		{
			if (other == null)
			{
				return 1;
			}
			return this.length.CompareTo(other.length);
		}
	}
}
