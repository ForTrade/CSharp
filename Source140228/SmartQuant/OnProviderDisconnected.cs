using System;
namespace SmartQuant
{
	public class OnProviderDisconnected : Event
	{
		internal Provider provider;
		public override byte TypeId
		{
			get
			{
				return 105;
			}
		}
		public OnProviderDisconnected(Provider provider)
		{
			this.provider = provider;
		}
	}
}
