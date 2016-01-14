using System;
namespace SmartQuant
{
	public interface IGroupListener
	{
		PermanentQueue<Event> Queue
		{
			get;
		}
		bool OnNewGroup(Group group);
		void OnNewGroupEvent(GroupEvent groupEvent);
		void OnNewGroupUpdate(GroupUpdate groupUpdate);
	}
}
