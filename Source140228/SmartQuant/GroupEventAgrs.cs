using System;
namespace SmartQuant
{
	public class GroupEventAgrs : EventArgs
	{
		public Group Group
		{
			get;
			private set;
		}
		public GroupEventAgrs(Group group)
		{
			this.Group = group;
		}
	}
}
