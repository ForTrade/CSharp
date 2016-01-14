using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class StrategyList : IEnumerable<Strategy>, IEnumerable
	{
		private IdArray<Strategy> strategyById;
		private List<Strategy> strategies;
		public int Count
		{
			get
			{
				return this.strategies.Count;
			}
		}
		public Strategy this[int index]
		{
			get
			{
				return this.strategies[index];
			}
			set
			{
				this.strategies[index] = value;
			}
		}
		public StrategyList()
		{
			this.strategies = new List<Strategy>();
			this.strategyById = new IdArray<Strategy>(1000);
		}
		public bool Contains(Strategy strategy)
		{
			return this.strategyById[(int)strategy.id] != null;
		}
		public bool Contains(int id)
		{
			return this.strategyById[id] == null;
		}
		public void Add(Strategy strategy)
		{
			if (this.strategyById[(int)strategy.Id] == null)
			{
				this.strategies.Add(strategy);
				this.strategyById.Add((int)strategy.Id, strategy);
				return;
			}
			Console.WriteLine(string.Concat(new object[]
			{
				"StrategyList::Add strategy ",
				strategy.Name,
				" with Id = ",
				strategy.Id,
				" is already in the list"
			}));
		}
		public void Remove(Strategy strategy)
		{
			this.strategies.Remove(strategy);
			this.strategyById.Remove((int)strategy.Id);
		}
		public Strategy GetByIndex(int index)
		{
			return this.strategies[index];
		}
		public Strategy GetById(int id)
		{
			return this.strategyById[id];
		}
		public void Clear()
		{
			this.strategies.Clear();
			this.strategyById.Clear();
		}
		public IEnumerator<Strategy> GetEnumerator()
		{
			return this.strategies.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.strategies.GetEnumerator();
		}
	}
}
