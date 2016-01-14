using System;
using System.IO;
namespace SmartQuant
{
	public class QuoteStreamer : ObjectStreamer
	{
		public QuoteStreamer()
		{
			this.typeId = 5;
			this.type = typeof(Quote);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new Quote(new DateTime(reader.ReadInt64()), reader.ReadByte(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32(), reader.ReadDouble(), reader.ReadInt32());
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			Quote quote = (Quote)obj;
			byte value = 0;
			writer.Write(value);
			writer.Write(quote.DateTime.Ticks);
			writer.Write(quote.bid.providerId);
			writer.Write(quote.bid.instrumentId);
			writer.Write(quote.bid.price);
			writer.Write(quote.bid.size);
			writer.Write(quote.ask.price);
			writer.Write(quote.ask.size);
		}
	}
}
