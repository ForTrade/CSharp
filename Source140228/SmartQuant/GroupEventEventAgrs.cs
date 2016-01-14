using System;
namespace SmartQuant
{
	public class GroupEventEventAgrs : EventArgs
	{
		public GroupEvent GroupEvent
		{
			get;
			private set;
		}
		public GroupEventEventAgrs(GroupEvent groupEvent)
		{
			this.GroupEvent = groupEvent;
		}
	}
}
