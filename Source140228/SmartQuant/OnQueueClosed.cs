using System;
namespace SmartQuant
{
	public class OnQueueClosed : Event
	{
		public override byte TypeId
		{
			get
			{
				return 206;
			}
		}
		public OnQueueClosed()
		{
			this.dateTime = DateTime.MinValue;
		}
		public override string ToString()
		{
			return "OnQueueClosed";
		}
	}
}
