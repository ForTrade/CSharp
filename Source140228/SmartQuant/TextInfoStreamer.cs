using System;
using System.IO;
namespace SmartQuant
{
	public class TextInfoStreamer : ObjectStreamer
	{
		public TextInfoStreamer()
		{
			this.typeId = 17;
			this.type = typeof(TextInfo);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			DateTime dateTime = new DateTime(reader.ReadInt64());
			string content = reader.ReadString();
			return new TextInfo(dateTime, content);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			TextInfo textInfo = obj as TextInfo;
			writer.Write(textInfo.DateTime.Ticks);
			writer.Write(textInfo.Content);
		}
	}
}
