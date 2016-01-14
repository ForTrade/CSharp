using System;
using System.IO;
namespace SmartQuant
{
	public class IntStreamer : ObjectStreamer
	{
		public IntStreamer()
		{
			this.typeId = 152;
			this.type = typeof(int);
		}
		public override object Read(BinaryReader reader)
		{
			return reader.ReadInt32();
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write((int)obj);
		}
	}
}
