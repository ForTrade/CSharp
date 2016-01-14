using System;
namespace SmartQuant
{
	public class OnConnect : Event
	{
		public override byte TypeId
		{
			get
			{
				return 201;
			}
		}
	}
}
