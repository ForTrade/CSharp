using System;
namespace SmartQuant
{
	public class OnOrderCancelled : Event
	{
		internal Order order;
		public override byte TypeId
		{
			get
			{
				return 120;
			}
		}
		public OnOrderCancelled(Order order)
		{
			this.order = order;
		}
	}
}
