using System;
namespace SmartQuant
{
	public class AccountPosition
	{
		internal byte currencyId;
		internal double value;
		public byte CurrencyId
		{
			get
			{
				return this.currencyId;
			}
		}
		public double Value
		{
			get
			{
				return this.value;
			}
		}
		public AccountPosition(byte currencyId, double value)
		{
			this.currencyId = currencyId;
			this.value = value;
		}
		public AccountPosition(AccountTransaction transaction)
		{
			this.currencyId = transaction.currencyId;
			this.value = transaction.value;
		}
		public void Add(AccountTransaction transaction)
		{
			this.value += transaction.value;
		}
	}
}
