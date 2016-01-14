using System;
namespace SmartQuant
{
	public interface IProvider
	{
		ProviderStatus Status
		{
			get;
		}
		byte Id
		{
			get;
		}
		string Name
		{
			get;
		}
		void Connect();
		void Disconnect();
	}
}
