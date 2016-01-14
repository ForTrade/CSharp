using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class FillSeries : IEnumerable<Fill>, IEnumerable
	{
		private string name;
		private List<Fill> items;
		private Fill min;
		private Fill max;
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}
		public Fill Min
		{
			get
			{
				return this.min;
			}
		}
		public Fill Max
		{
			get
			{
				return this.max;
			}
		}
		public Fill this[int index]
		{
			get
			{
				return this.items[index];
			}
		}
		public FillSeries(string name = "")
		{
			this.name = name;
			this.items = new List<Fill>();
			this.min = null;
			this.max = null;
		}
		public void Clear()
		{
			this.items.Clear();
			this.min = null;
			this.max = null;
		}
		public void Add(Fill fill)
		{
			if (this.min == null)
			{
				this.min = fill;
			}
			else
			{
				if (fill.price < this.min.price)
				{
					this.min = fill;
				}
			}
			if (this.max == null)
			{
				this.max = fill;
			}
			else
			{
				if (fill.price > this.max.price)
				{
					this.max = fill;
				}
			}
			if (this.items.Count != 0 && fill.dateTime < this.items[this.items.Count - 1].dateTime)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"FillSeries::Add (",
					this.name,
					" + incorrect fill order : ",
					fill
				}));
			}
			this.items.Add(fill);
		}
		public IEnumerator<Fill> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		public int GetIndex(DateTime datetime, IndexOption option)
		{
			int num = 0;
			int num2 = 0;
			int num3 = this.items.Count - 1;
			bool flag = true;
			while (flag)
			{
				if (num3 < num2)
				{
					return -1;
				}
				num = (num2 + num3) / 2;
				switch (option)
				{
				case IndexOption.Null:
					if (this.items[num].dateTime == datetime)
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime > datetime)
						{
							num3 = num - 1;
						}
						else
						{
							if (this.items[num].dateTime < datetime)
							{
								num2 = num + 1;
							}
						}
					}
					break;
				case IndexOption.Next:
					if (this.items[num].dateTime >= datetime && (num == 0 || this.items[num - 1].dateTime < datetime))
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime < datetime)
						{
							num2 = num + 1;
						}
						else
						{
							num3 = num - 1;
						}
					}
					break;
				case IndexOption.Prev:
					if (this.items[num].dateTime <= datetime && (num == this.items.Count - 1 || this.items[num + 1].dateTime > datetime))
					{
						flag = false;
					}
					else
					{
						if (this.items[num].dateTime > datetime)
						{
							num3 = num - 1;
						}
						else
						{
							num2 = num + 1;
						}
					}
					break;
				}
			}
			return num;
		}
	}
}
