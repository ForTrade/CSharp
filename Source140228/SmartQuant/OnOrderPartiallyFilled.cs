using System;
namespace SmartQuant
{
	public class OnOrderPartiallyFilled : Event
	{
		internal Order order;
		public override byte TypeId
		{
			get
			{
				return 117;
			}
		}
		public OnOrderPartiallyFilled(Order order)
		{
			this.order = order;
		}
	}
}
