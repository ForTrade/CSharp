using System;
namespace SmartQuant
{
	public class OnOrderDone : Event
	{
		internal Order order;
		public override byte TypeId
		{
			get
			{
				return 121;
			}
		}
		public OnOrderDone(Order order)
		{
			this.order = order;
		}
	}
}
