using System;
namespace SmartQuant
{
	public class GroupEvent : DataObject
	{
		public Group Group
		{
			get;
			set;
		}
		public Event Obj
		{
			get;
			private set;
		}
		public int GroupId
		{
			get;
			private set;
		}
		public override byte TypeId
		{
			get
			{
				return 52;
			}
		}
		public GroupEvent(Event obj, Group group)
		{
			this.Obj = obj;
			this.Group = group;
			if (group != null)
			{
				this.GroupId = group.Id;
				return;
			}
			this.GroupId = -1;
		}
		internal GroupEvent(Event obj, int groupId)
		{
			this.Obj = obj;
			this.GroupId = groupId;
		}
	}
}
