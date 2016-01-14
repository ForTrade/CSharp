using System;
using System.Collections.Generic;
using System.Threading;
namespace SmartQuant
{
	public class GroupManager
	{
		private Framework framework;
		private int nextGroupId;
		public IdArray<Group> Groups
		{
			get;
			private set;
		}
		public List<Group> GroupList
		{
			get;
			private set;
		}
		public GroupManager(Framework framework)
		{
			this.framework = framework;
			this.Groups = new IdArray<Group>(1000);
			this.GroupList = new List<Group>();
		}
		public void Add(Group group)
		{
			group.Id = Interlocked.Increment(ref this.nextGroupId);
			this.Groups.Add(group.Id, group);
			this.GroupList.Add(group);
			group.Framework = this.framework;
			this.framework.eventServer.OnLog(group);
		}
		internal void OnGroup(Group group)
		{
		}
		internal void OnGroupEvent(GroupEvent groupEvent)
		{
		}
	}
}
