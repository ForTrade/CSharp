using System;
using System.IO;
namespace SmartQuant
{
	public class FundamentalStreamer : ObjectStreamer
	{
		public FundamentalStreamer()
		{
			this.typeId = 22;
			this.type = typeof(Fundamental);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			Fundamental fundamental = new Fundamental();
			fundamental.dateTime = new DateTime(reader.ReadInt64());
			fundamental.providerId = reader.ReadInt32();
			fundamental.instrumentId = reader.ReadInt32();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				fundamental.fields[i] = reader.ReadDouble();
			}
			return fundamental;
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			Fundamental fundamental = (Fundamental)obj;
			writer.Write(fundamental.dateTime.Ticks);
			writer.Write(fundamental.providerId);
			writer.Write(fundamental.instrumentId);
			if (fundamental.fields != null)
			{
				writer.Write(fundamental.fields.Size);
				for (int i = 0; i < fundamental.fields.Size; i++)
				{
					writer.Write(fundamental.fields[i]);
				}
				return;
			}
			int value2 = 0;
			writer.Write(value2);
		}
	}
}
