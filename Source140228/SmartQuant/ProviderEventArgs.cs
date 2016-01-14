using System;
namespace SmartQuant
{
	public class ProviderEventArgs : EventArgs
	{
		public IProvider Provider
		{
			get;
			private set;
		}
		public ProviderEventArgs(IProvider provider)
		{
			this.Provider = provider;
		}
	}
}
