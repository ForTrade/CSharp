using System;
using System.IO;
namespace SmartQuant
{
	public class GroupStreamer : ObjectStreamer
	{
		public GroupStreamer()
		{
			this.typeId = 50;
			this.type = typeof(Group);
		}
		public override object Read(BinaryReader reader)
		{
			reader.ReadByte();
			string name = reader.ReadString();
			reader.ReadInt32();
			Group group = new Group(name);
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string name2 = reader.ReadString();
				byte type = reader.ReadByte();
				object value = this.streamerManager.Deserialize(reader);
				group.Add(name2, type, value);
			}
			return group;
		}
		public override void Write(BinaryWriter writer, object obj)
		{
			byte value = 0;
			writer.Write(value);
			Group group = obj as Group;
			writer.Write(group.Name);
			writer.Write(group.Id);
			writer.Write(group.Fields.Count);
			foreach (GroupField current in group.Fields.Values)
			{
				writer.Write(current.Name);
				writer.Write(current.Type);
				this.streamerManager.Serialize(writer, current.Value);
			}
		}
	}
}
