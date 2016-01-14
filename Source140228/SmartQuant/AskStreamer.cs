using System;
using System.IO;
namespace SmartQuant
{
	public class AskStreamer : ObjectStreamer
	{
		public AskStreamer()
		{
			this.typeId = 3;
			this.type = typeof(Ask);
		}
		public override object Read(BinaryReader reader)
		{
			if (reader.ReadByte() == 0)
			{
				return new Ask(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
			}
			return new Ask(new DateTime(reader.ReadInt64()), new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Ask ask = (Ask)obj;
			byte b = 0;
			if (ask.exchangeDateTime.Ticks != 0L)
			{
				b = 1;
			}
			writer.Write(b);
			writer.Write(ask.dateTime.Ticks);
			if (b == 1)
			{
				writer.Write(ask.exchangeDateTime.Ticks);
			}
			writer.Write(ask.providerId);
			writer.Write(ask.instrumentId);
			writer.Write(ask.price);
			writer.Write(ask.size);
		}
	}
}
