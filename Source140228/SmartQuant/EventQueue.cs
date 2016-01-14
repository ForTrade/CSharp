using System;
using System.Threading;
namespace SmartQuant
{
	public class EventQueue : IEventQueue
	{
		internal byte id;
		internal byte type;
		internal byte priority;
		internal string name;
		private Event[] objects;
		private int size;
		private volatile int readIndex;
		private volatile int writeIndex;
		private long fullCount;
		private long emptyCount;
		private long enqueueCount;
		private long dequeueCount;
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
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public byte Priority
		{
			get
			{
				return this.priority;
			}
		}
		public long Count
		{
			get
			{
				return this.enqueueCount - this.dequeueCount;
			}
		}
		public long EnqueueCount
		{
			get
			{
				return this.enqueueCount;
			}
		}
		public long DequeueCount
		{
			get
			{
				return this.dequeueCount;
			}
		}
		public long FullCount
		{
			get
			{
				return this.fullCount;
			}
		}
		public long EmptyCount
		{
			get
			{
				return this.emptyCount;
			}
		}
		public EventQueue(byte id, byte type = 0, byte priority = 2, int size = 100000)
		{
			this.id = id;
			this.type = type;
			this.priority = priority;
			this.size = size;
			this.readIndex = 0;
			this.writeIndex = 0;
			this.objects = new Event[size];
			this.enqueueCount = 0L;
			this.dequeueCount = 0L;
			this.fullCount = 0L;
			this.emptyCount = 0L;
		}
		public Event Peek()
		{
			return this.objects[this.readIndex];
		}
		public DateTime PeekDateTime()
		{
			return this.objects[this.readIndex].dateTime;
		}
		public Event Read()
		{
			Event result = this.objects[this.readIndex];
			this.readIndex = (this.readIndex + 1) % this.size;
			this.dequeueCount += 1L;
			return result;
		}
		public void Write(Event obj)
		{
			this.objects[this.writeIndex] = obj;
			this.writeIndex = (this.writeIndex + 1) % this.size;
			this.enqueueCount += 1L;
		}
		public Event Dequeue()
		{
			while (this.IsEmpty())
			{
				this.emptyCount += 1L;
				Thread.Sleep(1);
			}
			Event result = this.objects[this.readIndex];
			this.readIndex = (this.readIndex + 1) % this.size;
			this.dequeueCount += 1L;
			return result;
		}
		public void Enqueue(Event obj)
		{
			while (this.IsFull())
			{
				this.fullCount += 1L;
				Thread.Sleep(1);
			}
			this.objects[this.writeIndex] = obj;
			this.writeIndex = (this.writeIndex + 1) % this.size;
			this.enqueueCount += 1L;
		}
		public bool IsEmpty()
		{
			return this.readIndex == this.writeIndex;
		}
		public bool IsFull()
		{
			return (this.writeIndex + 1) % this.size == this.readIndex;
		}
		public void Clear()
		{
			this.readIndex = 0;
			this.writeIndex = 0;
			this.enqueueCount = 0L;
			this.dequeueCount = 0L;
			this.fullCount = 0L;
			this.emptyCount = 0L;
		}
		public void ResetCounts()
		{
			this.fullCount = 0L;
			this.emptyCount = 0L;
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Id: ",
				this.id,
				" Count = ",
				this.Count,
				" Enqueue = ",
				this.enqueueCount,
				" Dequeue = ",
				this.dequeueCount
			});
		}
	}
}
