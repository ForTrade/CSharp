using System;
using System.Diagnostics;
using System.Threading;
namespace SmartQuant
{
	public class Clock
	{
		internal Framework framework;
		private DateTime dateTime = DateTime.MinValue;
		internal ClockMode mode = ClockMode.Simulation;
		internal ClockResolution resolution;
		private long hires_offset;
		private Stopwatch hires_watch;
		private bool isStandalone;
		internal IEventQueue reminderQueue = new SortedEventQueue(3, 0, 2);
		internal Thread reminderThread;
		public long Ticks
		{
			get
			{
				if (this.mode != ClockMode.Realtime)
				{
					return this.dateTime.Ticks;
				}
				if (this.resolution == ClockResolution.Normal)
				{
					return DateTime.Now.Ticks;
				}
				return this.hires_offset + (long)((double)this.hires_watch.ElapsedTicks / (double)Stopwatch.Frequency * 10000000.0);
			}
		}
		public DateTime DateTime
		{
			get
			{
				if (this.mode != ClockMode.Realtime)
				{
					return this.dateTime;
				}
				if (this.resolution == ClockResolution.Normal)
				{
					return DateTime.Now;
				}
				return new DateTime(this.hires_offset + (long)((double)this.hires_watch.ElapsedTicks / (double)Stopwatch.Frequency * 10000000.0));
			}
			internal set
			{
				if (this.mode != ClockMode.Simulation)
				{
					Console.WriteLine("Clock::DateTime Can not set dateTime because Clock is not in the Simulation mode");
					return;
				}
				if (value != this.dateTime)
				{
					if (value < this.dateTime)
					{
						Console.WriteLine("Clock::DateTime incorrect set order");
						return;
					}
					if (this.isStandalone)
					{
						while (!this.reminderQueue.IsEmpty() && this.reminderQueue.PeekDateTime() < value)
						{
							Reminder reminder = (Reminder)this.reminderQueue.Read();
							this.dateTime = reminder.dateTime;
							reminder.Execute();
						}
					}
					this.dateTime = value;
				}
			}
		}
		public ClockMode Mode
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
					if (this.mode == ClockMode.Simulation)
					{
						this.dateTime = DateTime.MinValue;
					}
				}
			}
		}
		public ClockResolution Resolution
		{
			get
			{
				return this.resolution;
			}
			set
			{
				this.resolution = value;
			}
		}
		public Clock(Framework framework, ClockMode mode = ClockMode.Realtime, bool isStandalone = false)
		{
			this.framework = framework;
			this.mode = mode;
			this.isStandalone = isStandalone;
			this.hires_offset = DateTime.Now.Ticks;
			this.hires_watch = Stopwatch.StartNew();
			if (isStandalone)
			{
				this.reminderThread = new Thread(new ThreadStart(this.ThreadRun));
				this.reminderThread.Name = "Clock Thread";
				this.reminderThread.IsBackground = true;
				this.reminderThread.Start();
			}
		}
		private void ThreadRun()
		{
			Console.WriteLine(DateTime.Now + " Clock thread started");
			bool flag = false;
			while (true)
			{
				if (this.mode == ClockMode.Realtime)
				{
					if (!this.reminderQueue.IsEmpty())
					{
						long ticks = this.reminderQueue.PeekDateTime().Ticks;
						long ticks2 = this.framework.clock.Ticks;
						if (ticks <= ticks2)
						{
							((Reminder)this.reminderQueue.Read()).Execute();
						}
						else
						{
							if (ticks - ticks2 < 15000L)
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						Thread.SpinWait(1);
					}
					else
					{
						Thread.Sleep(1);
					}
				}
				else
				{
					Thread.Sleep(10);
				}
			}
		}
		public void AddReminder(Reminder reminder)
		{
			this.reminderQueue.Enqueue(reminder);
		}
		public void AddReminder(ReminderCallback callback, DateTime dateTime, object data = null)
		{
			this.AddReminder(new Reminder(callback, dateTime, data));
		}
		public void Clear()
		{
			this.dateTime = DateTime.MinValue;
		}
		public string GetModeAsString()
		{
			switch (this.mode)
			{
			case ClockMode.Realtime:
				return "Realtime";
			case ClockMode.Simulation:
				return "Simulation";
			default:
				return "Undefined";
			}
		}
	}
}
