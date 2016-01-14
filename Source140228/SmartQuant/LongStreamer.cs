using System;
using System.IO;
namespace SmartQuant
{
	public class LongStreamer : ObjectStreamer
	{
		public LongStreamer()
		{
			this.typeId = 151;
			this.type = typeof(long);
		}
		public override object Read(BinaryReader reader)
		{
			return reader.ReadInt64();
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write((long)obj);
		}
	}
}
