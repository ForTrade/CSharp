using System;
using System.IO;
namespace SmartQuant
{
	public class DataObjectStreamer : ObjectStreamer
	{
		public DataObjectStreamer()
		{
			this.typeId = 0;
			this.type = typeof(DataObject);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			DateTime dateTime = new DateTime(reader.ReadInt64());
			return new DataObject(dateTime);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			writer.Write((obj as DataObject).dateTime.Ticks);
		}
	}
}
