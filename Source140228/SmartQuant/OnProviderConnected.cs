using System;
namespace SmartQuant
{
	public class OnProviderConnected : Event
	{
		internal Provider provider;
		public override byte TypeId
		{
			get
			{
				return 104;
			}
		}
		public OnProviderConnected(Provider provider)
		{
			this.provider = provider;
		}
	}
}
