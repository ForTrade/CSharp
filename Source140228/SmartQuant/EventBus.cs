using System;
using System.Threading;
namespace SmartQuant
{
	public class EventBus
	{
		internal Framework framework;
		internal EventPipe dataPipe;
		internal EventPipe executionPipe;
		internal EventPipe servicePipe;
		internal IEventQueue reminderQueue;
		internal EventBusMode mode = EventBusMode.Simulation;
		internal int attached_count;
		internal EventQueue[] attached = new EventQueue[1024];
		internal bool idle = true;
		private Event e_;
		public EventBusMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				if (this.mode != value)
				{
					this.mode = value;
				}
			}
		}
		public EventBus(Framework framework, EventBusMode mode = EventBusMode.Realtime)
		{
			this.framework = framework;
			this.mode = mode;
			this.dataPipe = new EventPipe(framework, true);
			this.executionPipe = new EventPipe(framework, false);
			this.servicePipe = new EventPipe(framework, false);
		}
		public void Attach(EventBus bus)
		{
			EventQueue eventQueue = new EventQueue(1, 0, 2, 25000);
			eventQueue.name = "attached";
			eventQueue.Enqueue(new OnQueueOpened());
			bus.dataPipe.Add(eventQueue);
			this.attached[this.attached_count++] = eventQueue;
		}
		public Event Dequeue()
		{
			if (this.mode == EventBusMode.Simulation)
			{
				while (true)
				{
					if (!this.dataPipe.IsEmpty() && this.e_ == null)
					{
						Event @event = this.dataPipe.Read();
						if (@event.dateTime < this.framework.clock.DateTime)
						{
							if (@event.TypeId != 205 && @event.TypeId != 206 && @event.TypeId != 108 && @event.TypeId != 109)
							{
								Console.WriteLine(string.Concat(new object[]
								{
									"EventBus::Dequeue Skipping: ",
									@event,
									" ",
									@event.dateTime,
									" ",
									this.framework.clock.DateTime
								}));
								continue;
							}
							@event.dateTime = this.framework.clock.DateTime;
							this.e_ = @event;
						}
						else
						{
							this.e_ = @event;
						}
					}
					if (!this.executionPipe.IsEmpty())
					{
						break;
					}
					if (!this.reminderQueue.IsEmpty() && this.e_ != null && this.reminderQueue.PeekDateTime() <= this.e_.DateTime)
					{
						goto Block_10;
					}
					if (!this.servicePipe.IsEmpty())
					{
						goto Block_11;
					}
					if (this.e_ != null)
					{
						goto IL_183;
					}
					Thread.Sleep(1);
				}
				return this.executionPipe.Read();
				Block_10:
				return this.reminderQueue.Read();
				Block_11:
				return this.servicePipe.Read();
				IL_183:
				Event event2 = this.e_;
				this.e_ = null;
				for (int i = 0; i < this.attached_count; i++)
				{
					this.attached[i].Enqueue(event2);
				}
				return event2;
			}
			while (this.reminderQueue.IsEmpty() || !(this.reminderQueue.PeekDateTime() <= this.framework.clock.DateTime))
			{
				if (!this.executionPipe.IsEmpty())
				{
					return this.executionPipe.Read();
				}
				if (!this.servicePipe.IsEmpty())
				{
					return this.servicePipe.Read();
				}
				if (!this.dataPipe.IsEmpty())
				{
					return this.dataPipe.Read();
				}
				if (this.idle)
				{
					Thread.Sleep(1);
				}
			}
			return this.reminderQueue.Read();
		}
		public void ResetCounts()
		{
		}
		public void Clear()
		{
			this.reminderQueue.Clear();
			this.servicePipe.Clear();
			this.dataPipe.Clear();
			this.executionPipe.Clear();
			for (int i = 0; i < this.attached_count; i++)
			{
				this.attached[i] = null;
			}
			this.attached_count = 0;
		}
	}
}
