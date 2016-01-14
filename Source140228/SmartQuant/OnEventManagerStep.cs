using System;
namespace SmartQuant
{
	public class OnEventManagerStep : Event
	{
		public override byte TypeId
		{
			get
			{
				return 211;
			}
		}
		public override string ToString()
		{
			return "OnEventManagerStep";
		}
	}
}
