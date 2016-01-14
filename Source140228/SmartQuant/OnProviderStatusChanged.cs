using System;
namespace SmartQuant
{
	public class OnProviderStatusChanged : Event
	{
		internal Provider provider;
		public override byte TypeId
		{
			get
			{
				return 106;
			}
		}
		public OnProviderStatusChanged(Provider provider)
		{
			this.provider = provider;
		}
	}
}
