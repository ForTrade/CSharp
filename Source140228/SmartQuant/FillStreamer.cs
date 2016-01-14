using System;
using System.IO;
namespace SmartQuant
{
	public class FillStreamer : ObjectStreamer
	{
		public FillStreamer()
		{
			this.typeId = 10;
			this.type = typeof(Fill);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new Fill
			{
				dateTime = new DateTime(reader.ReadInt64()),
				instrumentId = reader.ReadInt32(),
				currencyId = reader.ReadByte(),
				side = (OrderSide)Enum.Parse(typeof(OrderSide), reader.ReadString()),
				qty = reader.ReadDouble(),
				price = reader.ReadDouble(),
				text = reader.ReadString()
			};
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			Fill fill = obj as Fill;
			writer.Write(fill.dateTime.Ticks);
			writer.Write(fill.instrumentId);
			writer.Write(fill.currencyId);
			writer.Write(fill.side.ToString());
			writer.Write(fill.qty);
			writer.Write(fill.price);
			writer.Write(fill.text);
		}
	}
}
