using System;
namespace SmartQuant
{
	public class OnOrderReplaced : Event
	{
		internal Order order;
		public override byte TypeId
		{
			get
			{
				return 119;
			}
		}
		public OnOrderReplaced(Order order)
		{
			this.order = order;
		}
	}
}
