using System;
using System.Collections.Generic;
namespace SmartQuant
{
	internal class FundamentalFieldByName : Dictionary<string, byte>
	{
		internal FundamentalFieldByName()
		{
			base.Add("CashFlow", 1);
			base.Add("PE", 2);
			base.Add("Beta", 3);
			base.Add("ProfitMargin", 4);
			base.Add("ReturnOnEquity", 5);
			base.Add("PriceBook", 6);
			base.Add("DebtEquity", 7);
			base.Add("InterestCoverage", 8);
			base.Add("BookValue", 9);
			base.Add("PriceSales", 10);
			base.Add("DividendPayout", 11);
		}
	}
}
