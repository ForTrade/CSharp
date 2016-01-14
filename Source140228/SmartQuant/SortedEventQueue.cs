using System;
using System.Threading;
namespace SmartQuant
{
	public class SortedEventQueue : IEventQueue
	{
		private EventSortedSet list = new EventSortedSet();
		internal byte id;
		internal byte type;
		internal byte priority;
		internal string name;
		internal DateTime dateTime;
		public byte Id
		{
			get
			{
				return this.id;
			}
		}
		public byte Type
		{
			get
			{
				return this.type;
			}
		}
		public byte Priority
		{
			get
			{
				return this.priority;
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public long Count
		{
			get
			{
				return (long)this.list.Count;
			}
		}
		public long EmptyCount
		{
			get
			{
				throw new NotImplementedException("Not implemented in SortedEventQueue");
			}
		}
		public long FullCount
		{
			get
			{
				throw new NotImplementedException("Not implemented in SortedEventQueue");
			}
		}
		public SortedEventQueue(byte id, byte type = 0, byte priority = 2)
		{
			this.id = id;
			this.type = type;
			this.priority = priority;
		}
		public void Enqueue(Event e)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.list.Add(e);
				this.dateTime = this.list[0].dateTime;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public Event Dequeue()
		{
			bool flag = false;
			Event result;
			try
			{
				Monitor.Enter(this, ref flag);
				result = this.list[0];
				this.list.RemoveAt(0);
				if (this.list.Count != 0)
				{
					this.dateTime = this.list[0].dateTime;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
		public Event Peek()
		{
			bool flag = false;
			Event result;
			try
			{
				Monitor.Enter(this, ref flag);
				result = this.list[0];
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
		public DateTime PeekDateTime()
		{
			return this.dateTime;
		}
		public bool IsEmpty()
		{
			return this.list.Count == 0;
		}
		public bool IsFull()
		{
			return false;
		}
		public void Clear()
		{
			this.list.Clear();
		}
		public void ResetCounts()
		{
		}
		public void Write(Event e)
		{
			throw new NotImplementedException("Not implemented in SortedEventQueue");
		}
		public Event Read()
		{
			bool flag = false;
			Event result;
			try
			{
				Monitor.Enter(this, ref flag);
				result = this.list[0];
				this.list.RemoveAt(0);
				if (this.list.Count > 0)
				{
					this.dateTime = this.list[0].dateTime;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
	}
}
