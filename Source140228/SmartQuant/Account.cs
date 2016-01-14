using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class Account
	{
		private Framework framework;
		internal byte currencyId = 1;
		private IdArray<AccountPosition> positionArray = new IdArray<AccountPosition>(1000);
		private List<AccountPosition> positions = new List<AccountPosition>();
		private List<AccountTransaction> transactions = new List<AccountTransaction>();
		public byte CurrencyId
		{
			get
			{
				return this.currencyId;
			}
			set
			{
				this.currencyId = value;
			}
		}
		public List<AccountPosition> Positions
		{
			get
			{
				return this.positions;
			}
		}
		public List<AccountTransaction> Transactions
		{
			get
			{
				return this.transactions;
			}
		}
		public double Value
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < this.positions.Count; i++)
				{
					num += this.framework.currencyConverter.Convert(this.positions[i].Value, this.positions[i].currencyId, this.currencyId);
				}
				return num;
			}
		}
		public Account(Framework framework)
		{
			this.framework = framework;
		}
		public void Add(AccountTransaction transaction)
		{
			AccountPosition accountPosition = this.positionArray[(int)transaction.CurrencyId];
			if (accountPosition == null)
			{
				accountPosition = new AccountPosition(transaction);
				this.positionArray[(int)accountPosition.CurrencyId] = accountPosition;
				this.positions.Add(accountPosition);
			}
			else
			{
				accountPosition.Add(transaction);
			}
			this.transactions.Add(transaction);
		}
		public void Add(Fill fill)
		{
			this.Add(new AccountTransaction(fill));
		}
		public void Add(DateTime dateTime, double value, byte currencyId, string text)
		{
			this.Add(new AccountTransaction(dateTime, value, currencyId, text));
		}
		public void Deposit(DateTime dateTime, double value, byte currencyId, string text)
		{
			this.Add(dateTime, value, currencyId, text);
		}
		public void Withdraw(DateTime dateTime, double value, byte currencyId, string text)
		{
			this.Add(dateTime, -value, currencyId, text);
		}
	}
}
