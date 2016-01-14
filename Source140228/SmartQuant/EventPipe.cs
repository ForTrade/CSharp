using System;
namespace SmartQuant
{
	public class EventPipe
	{
		private Framework framework;
		private LinkedList<IEventQueue> queues = new LinkedList<IEventQueue>();
		private bool synch;
		public bool Synch
		{
			get
			{
				return this.synch;
			}
			set
			{
				this.synch = value;
			}
		}
		public int Count
		{
			get
			{
				return this.queues.Count;
			}
		}
		public EventPipe(Framework framework, bool synch = false)
		{
			this.framework = framework;
			this.synch = synch;
		}
		public void Add(IEventQueue queue)
		{
			this.queues.Add(queue);
		}
		public bool IsEmpty()
		{
			if (this.queues.Count == 0)
			{
				return true;
			}
			if (this.synch)
			{
				for (LinkedListNode<IEventQueue> linkedListNode = this.queues.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (linkedListNode.Data.IsEmpty())
					{
						return true;
					}
				}
				return false;
			}
			for (LinkedListNode<IEventQueue> linkedListNode2 = this.queues.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
			{
				if (!linkedListNode2.Data.IsEmpty())
				{
					return false;
				}
			}
			return true;
		}
		public Event Read()
		{
			if (this.synch)
			{
				DateTime t = DateTime.MaxValue;
				LinkedListNode<IEventQueue> linkedListNode = this.queues.First;
				LinkedListNode<IEventQueue> linkedListNode2 = null;
				LinkedListNode<IEventQueue> linkedListNode3 = null;
				while (linkedListNode != null)
				{
					Event @event = linkedListNode.Data.Peek();
					if (@event.TypeId == 206)
					{
						if (linkedListNode2 == null)
						{
							this.queues.First = linkedListNode.Next;
						}
						else
						{
							linkedListNode2.Next = linkedListNode.Next;
						}
						this.queues.Count--;
						if (this.queues.Count == 0 && this.framework.Mode == FrameworkMode.Simulation && linkedListNode.Data.Name != "Simulator stop queue")
						{
							EventQueue eventQueue = new EventQueue(1, 0, 2, 10);
							eventQueue.name = "Simulator stop queue";
							eventQueue.Enqueue(new OnQueueOpened());
							eventQueue.Enqueue(new OnSimulatorStop());
							eventQueue.Enqueue(new OnQueueClosed());
							this.Add(eventQueue);
						}
						linkedListNode3 = linkedListNode;
						break;
					}
					DateTime dateTime = @event.dateTime;
					if (dateTime <= t)
					{
						linkedListNode3 = linkedListNode;
						t = dateTime;
					}
					linkedListNode2 = linkedListNode;
					linkedListNode = linkedListNode.Next;
				}
				return linkedListNode3.Data.Read();
			}
			LinkedListNode<IEventQueue> linkedListNode4 = this.queues.First;
			LinkedListNode<IEventQueue> linkedListNode5 = null;
			while (linkedListNode4 != null)
			{
				if (!linkedListNode4.Data.IsEmpty())
				{
					Event event2 = linkedListNode4.Data.Read();
					if (event2.TypeId == 206)
					{
						if (linkedListNode5 == null)
						{
							this.queues.First = linkedListNode4.Next;
						}
						else
						{
							linkedListNode5.Next = linkedListNode4.Next;
						}
						this.queues.Count--;
					}
					return event2;
				}
				linkedListNode5 = linkedListNode4;
				linkedListNode4 = linkedListNode4.Next;
			}
			return null;
		}
		public Event Dequeue()
		{
			return null;
		}
		public void Clear()
		{
			this.queues.Clear();
		}
	}
}
