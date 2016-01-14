using System;
namespace SmartQuant
{
	public class AccountDataEntry
	{
		public string Account
		{
			get;
			private set;
		}
		public AccountData Values
		{
			get;
			private set;
		}
		public AccountData[] Positions
		{
			get;
			private set;
		}
		public AccountData[] Orders
		{
			get;
			private set;
		}
		internal AccountDataEntry(string account, AccountData values, AccountData[] positions, AccountData[] orders)
		{
			this.Account = account;
			this.Values = values;
			this.Positions = positions;
			this.Orders = orders;
		}
	}
}
