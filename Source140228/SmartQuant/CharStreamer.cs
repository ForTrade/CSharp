using System;
using System.IO;
namespace SmartQuant
{
	public class CharStreamer : ObjectStreamer
	{
		public CharStreamer()
		{
			this.typeId = 154;
			this.type = typeof(char);
		}
		public override object Read(BinaryReader reader)
		{
			return reader.ReadChar();
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write((char)obj);
		}
	}
}
