using System;
namespace SmartQuant
{
	public class AccountDataSnapshot
	{
		public AccountDataEntry[] Entries
		{
			get;
			private set;
		}
		internal AccountDataSnapshot(AccountDataEntry[] entries)
		{
			this.Entries = entries;
		}
	}
}
