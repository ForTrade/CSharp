using System;
namespace SmartQuant
{
	public class OnOrderStatusChanged : Event
	{
		internal Order order;
		public override byte TypeId
		{
			get
			{
				return 116;
			}
		}
		public OnOrderStatusChanged(Order order)
		{
			this.order = order;
		}
	}
}
