using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class PortfolioList : IEnumerable<Portfolio>, IEnumerable
	{
		private IdArray<Portfolio> portfolios = new IdArray<Portfolio>(1000);
		private Dictionary<string, Portfolio> portfolioByName = new Dictionary<string, Portfolio>();
		public Portfolio this[int id]
		{
			get
			{
				return this.portfolios[id];
			}
		}
		public Portfolio this[string name]
		{
			get
			{
				Portfolio result = null;
				this.portfolioByName.TryGetValue(name, out result);
				return result;
			}
		}
		public void Add(Portfolio portfolio)
		{
			this.portfolios[portfolio.id] = portfolio;
			this.portfolioByName[portfolio.Name] = portfolio;
		}
		public void Clear()
		{
			this.portfolios.Clear();
			this.portfolioByName.Clear();
		}
		internal void Remove(Portfolio portfolio)
		{
			this.portfolios.Remove(portfolio.id);
			this.portfolioByName.Remove(portfolio.Name);
		}
		public IEnumerator<Portfolio> GetEnumerator()
		{
			return this.portfolioByName.Values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.portfolioByName.Values.GetEnumerator();
		}
		IEnumerator<Portfolio> IEnumerable<Portfolio>.GetEnumerator()
		{
			return this.portfolioByName.Values.GetEnumerator();
		}
	}
}
