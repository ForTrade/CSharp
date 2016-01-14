using System;
namespace SmartQuant
{
	public class Event
	{
		protected internal DateTime dateTime;
		public DateTime DateTime
		{
			get
			{
				return this.dateTime;
			}
			set
			{
				this.dateTime = value;
			}
		}
		public virtual byte TypeId
		{
			get
			{
				return 0;
			}
		}
		public override string ToString()
		{
			return this.dateTime + " " + base.GetType();
		}
	}
}
