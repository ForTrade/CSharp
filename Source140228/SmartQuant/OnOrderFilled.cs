using System;
namespace SmartQuant
{
	public class OnOrderFilled : Event
	{
		internal Order order;
		public override byte TypeId
		{
			get
			{
				return 118;
			}
		}
		public OnOrderFilled(Order order)
		{
			this.order = order;
		}
	}
}
