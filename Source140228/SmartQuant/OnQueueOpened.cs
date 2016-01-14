using System;
namespace SmartQuant
{
	public class OnQueueOpened : Event
	{
		public override byte TypeId
		{
			get
			{
				return 205;
			}
		}
		public OnQueueOpened()
		{
			this.dateTime = DateTime.MinValue;
		}
		public override string ToString()
		{
			return "OnQueueOpened";
		}
	}
}
