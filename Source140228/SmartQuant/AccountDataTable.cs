using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class AccountDataTable
	{
		public IDictionary<string, AccountDataTableItem> Items
		{
			get;
			private set;
		}
		public AccountDataTable()
		{
			this.Items = new Dictionary<string, AccountDataTableItem>();
		}
	}
}
