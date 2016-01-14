using System;
using System.IO;
namespace SmartQuant
{
	public class BooleanStreamer : ObjectStreamer
	{
		public BooleanStreamer()
		{
			this.typeId = 155;
			this.type = typeof(bool);
		}
		public override object Read(BinaryReader reader)
		{
			return reader.ReadBoolean();
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write((bool)obj);
		}
	}
}
