using System;
using System.IO;
namespace SmartQuant
{
	public class GroupEventStreamer : ObjectStreamer
	{
		public GroupEventStreamer()
		{
			this.typeId = 52;
			this.type = typeof(GroupEvent);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			int groupId = reader.ReadInt32();
			Event obj = this.streamerManager.Deserialize(reader) as Event;
			return new GroupEvent(obj, groupId);
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			GroupEvent groupEvent = obj as GroupEvent;
			writer.Write((groupEvent.Group == null) ? groupEvent.GroupId : groupEvent.Group.Id);
			this.streamerManager.Serialize(writer, groupEvent.Obj);
		}
	}
}
