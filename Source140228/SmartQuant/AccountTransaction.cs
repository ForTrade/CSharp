using System;
namespace SmartQuant
{
	public class AccountTransaction
	{
		internal DateTime dateTime;
		internal double value;
		internal byte currencyId;
		internal string text;
		public DateTime DateTime
		{
			get
			{
				return this.dateTime;
			}
		}
		public double Value
		{
			get
			{
				return this.value;
			}
		}
		public byte CurrencyId
		{
			get
			{
				return this.currencyId;
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
		}
		public AccountTransaction(DateTime dateTime, double value, byte currencyId, string text)
		{
			this.dateTime = dateTime;
			this.value = value;
			this.currencyId = currencyId;
			this.text = text;
		}
		public AccountTransaction(Fill fill)
		{
			this.dateTime = fill.dateTime;
			this.value = fill.CashFlow;
			this.currencyId = fill.currencyId;
			this.text = fill.text;
		}
	}
}
