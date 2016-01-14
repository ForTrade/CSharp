using System;
using System.Drawing;
using System.IO;
namespace SmartQuant
{
	public class ColorStreamer : ObjectStreamer
	{
		public ColorStreamer()
		{
			this.typeId = 156;
			this.type = typeof(Color);
		}
		public override object Read(BinaryReader reader)
		{
			return Color.FromArgb(reader.ReadInt32());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write(((Color)obj).ToArgb());
		}
	}
}
