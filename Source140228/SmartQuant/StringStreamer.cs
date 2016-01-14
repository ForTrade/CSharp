using System;
using System.IO;
namespace SmartQuant
{
	public class StringStreamer : ObjectStreamer
	{
		public StringStreamer()
		{
			this.typeId = 150;
			this.type = typeof(string);
		}
		public override object Read(BinaryReader reader)
		{
			return reader.ReadString();
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write(obj.ToString());
		}
	}
}
