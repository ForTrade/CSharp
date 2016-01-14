using System;
namespace SmartQuant
{
	public class GroupUpdateEventAgrs : EventArgs
	{
		public GroupUpdate GroupUpdate
		{
			get;
			private set;
		}
		public GroupUpdateEventAgrs(GroupUpdate groupUpdate)
		{
			this.GroupUpdate = groupUpdate;
		}
	}
}
