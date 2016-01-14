using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class AccountDataTableItem
	{
		public AccountDataFieldList Values
		{
			get;
			private set;
		}
		public IDictionary<AccountDataKey, AccountDataFieldList> Positions
		{
			get;
			private set;
		}
		public IDictionary<AccountDataKey, AccountDataFieldList> Orders
		{
			get;
			private set;
		}
		public AccountDataTableItem()
		{
			this.Values = new AccountDataFieldList();
			this.Positions = new Dictionary<AccountDataKey, AccountDataFieldList>();
			this.Orders = new Dictionary<AccountDataKey, AccountDataFieldList>();
		}
	}
}
