using System;
using System.IO;
namespace SmartQuant
{
	public class NewsStreamer : ObjectStreamer
	{
		public NewsStreamer()
		{
			this.typeId = 23;
			this.type = typeof(News);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			return new News
			{
				dateTime = new DateTime(reader.ReadInt64()),
				providerId = reader.ReadInt32(),
				instrumentId = reader.ReadInt32(),
				urgency = reader.ReadByte(),
				url = reader.ReadString(),
				headline = reader.ReadString(),
				text = reader.ReadString()
			};
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			News news = obj as News;
			writer.Write(news.dateTime.Ticks);
			writer.Write(news.providerId);
			writer.Write(news.instrumentId);
			writer.Write(news.urgency);
			writer.Write(news.url);
			writer.Write(news.headline);
			writer.Write(news.text);
		}
	}
}
