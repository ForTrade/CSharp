using System;
using System.IO;
namespace SmartQuant
{
	public class DateTimeStreamer : ObjectStreamer
	{
		public DateTimeStreamer()
		{
			this.typeId = 153;
			this.type = typeof(DateTime);
		}
		public override object Read(BinaryReader reader)
		{
			return new DateTime(reader.ReadInt64());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			writer.Write(((DateTime)obj).Ticks);
		}
	}
}
