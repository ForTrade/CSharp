using System;
namespace SmartQuant
{
	public class Reminder : DataObject
	{
		private ReminderCallback callback;
		private object data;
		public override byte TypeId
		{
			get
			{
				return 15;
			}
		}
		public Reminder(ReminderCallback callback, DateTime dateTime, object data = null) : base(dateTime)
		{
			this.callback = callback;
			this.data = data;
		}
		public void Execute()
		{
			this.callback(this.dateTime, this.data);
		}
		public override string ToString()
		{
			return "Reminder " + this.dateTime;
		}
	}
}
