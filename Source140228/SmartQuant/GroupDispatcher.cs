using System;
using System.Collections.Generic;
using System.Threading;
namespace SmartQuant
{
	public class GroupDispatcher
	{
		private Framework framework;
		private IdArray<List<IGroupListener>> listenerTable;
		private List<IGroupListener> listeners;
		private Dictionary<IGroupListener, List<int>> groupByListenerTable;
		public GroupDispatcher(Framework framework)
		{
			this.framework = framework;
			this.listeners = new List<IGroupListener>();
			this.listenerTable = new IdArray<List<IGroupListener>>(1000);
			this.groupByListenerTable = new Dictionary<IGroupListener, List<int>>();
			framework.eventManager.Dispatcher.NewGroup += new GroupEventHandler(this.Dispatcher_NewGroup);
			framework.eventManager.Dispatcher.NewGroupEvent += new GroupEventEventHandler(this.Dispatcher_NewGroupEvent);
			framework.eventManager.Dispatcher.NewGroupUpdate += new GroupUpdateEventHandler(this.Dispatcher_NewGroupUpdate);
			framework.EventManager.Dispatcher.FrameworkCleared += new FrameworkEventHandler(this.Dispatcher_FrameworkCleared);
		}
		private void Dispatcher_FrameworkCleared(object sender, FrameworkEventArgs args)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				foreach (IGroupListener current in this.listeners)
				{
					current.Queue.Enqueue(new OnFrameworkCleared(args.Framework));
				}
				this.listenerTable.Clear();
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		private void Dispatcher_NewGroupUpdate(object sender, GroupUpdateEventAgrs args)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				if (args.GroupUpdate.GroupId != -1)
				{
					List<IGroupListener> list = this.listenerTable[args.GroupUpdate.GroupId];
					if (list != null)
					{
						foreach (IGroupListener current in list)
						{
							current.OnNewGroupUpdate(args.GroupUpdate);
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		private void Dispatcher_NewGroupEvent(object sender, GroupEventEventAgrs args)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				if (args.GroupEvent.Group != null)
				{
					Group group = this.framework.groupManager.Groups[args.GroupEvent.Group.Id];
					group.OnNewGroupEvent(args.GroupEvent);
					List<IGroupListener> list = this.listenerTable[args.GroupEvent.Group.Id];
					if (list != null)
					{
						foreach (IGroupListener current in list)
						{
							current.Queue.Enqueue(args.GroupEvent);
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		private void Dispatcher_NewGroup(object sender, GroupEventAgrs args)
		{
			foreach (IGroupListener current in this.listeners)
			{
				this.ProcessGroup(current, args.Group);
			}
		}
		private void ProcessGroup(IGroupListener listener, Group group)
		{
			if (listener.OnNewGroup(group))
			{
				List<IGroupListener> list = this.listenerTable[group.Id];
				if (list == null)
				{
					list = new List<IGroupListener>();
					this.listenerTable[group.Id] = list;
				}
				this.groupByListenerTable[listener].Add(group.Id);
				list.Add(listener);
				foreach (GroupEvent current in group.Events)
				{
					listener.Queue.Enqueue(current);
				}
			}
		}
		public void AddListener(IGroupListener listener)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.listeners.Add(listener);
				this.groupByListenerTable[listener] = new List<int>();
				foreach (Group current in this.framework.groupManager.GroupList)
				{
					this.ProcessGroup(listener, current);
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public void RemoveListener(IGroupListener listener)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.listeners.Remove(listener);
				foreach (int current in this.groupByListenerTable[listener])
				{
					this.listenerTable[current].Remove(listener);
				}
				this.groupByListenerTable.Remove(listener);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
	}
}
