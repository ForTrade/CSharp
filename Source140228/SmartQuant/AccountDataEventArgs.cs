using System;
namespace SmartQuant
{
	public class AccountDataEventArgs : EventArgs
	{
		public AccountData Data
		{
			get;
			private set;
		}
		public AccountDataEventArgs(AccountData data)
		{
			this.Data = data;
		}
	}
}
