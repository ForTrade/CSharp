using System;
using System.IO;
namespace SmartQuant
{
	public class GroupUpdateStreamer : ObjectStreamer
	{
		public GroupUpdateStreamer()
		{
			this.typeId = 51;
			this.type = typeof(GroupUpdate);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			reader.ReadInt32();
			this.streamerManager.Deserialize(reader);
			return null;
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			GroupEvent groupEvent = obj as GroupEvent;
			this.streamerManager.Serialize(writer, groupEvent.Obj);
		}
	}
}
