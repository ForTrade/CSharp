using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class TickSeries : IEnumerable<Tick>, IEnumerable
	{
		private string name;
		private List<Tick> items;
		private Tick min;
		private Tick max;
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}
		public DateTime FirstDateTime
		{
			get
			{
				if (this.Count <= 0)
				{
					throw new ApplicationException("Array has no elements");
				}
				return this.items[0].DateTime;
			}
		}
		public DateTime LastDateTime
		{
			get
			{
				if (this.Count <= 0)
				{
					throw new ApplicationException("Array has no elements");
				}
				return this.items[this.Count - 1].DateTime;
			}
		}
		public Tick this[int index]
		{
			get
			{
				return this.items[index];
			}
		}
		public TickSeries(string name = "")
		{
			this.name = name;
			this.items = new List<Tick>();
			this.min = null;
			this.max = null;
		}
		public void Clear()
		{
			this.items.Clear();
			this.min = null;
			this.max = null;
		}
		public void Add(Tick tick)
		{
			if (this.min == null)
			{
				this.min = tick;
			}
			else
			{
				if (tick.price < this.min.price)
				{
					this.min = tick;
				}
			}
			if (this.max == null)
			{
				this.max = tick;
			}
			else
			{
				if (tick.price > this.max.price)
				{
					this.max = tick;
				}
			}
			this.items.Add(tick);
		}
		public Tick GetMin()
		{
			return this.min;
		}
		public Tick GetMax()
		{
			return this.max;
		}
		public Tick GetMin(DateTime dateTime1, DateTime dateTime2)
		{
			Tick tick = null;
			for (int i = 0; i < this.items.Count; i++)
			{
				Tick tick2 = this.items[i];
				if (!(tick2.dateTime < dateTime1))
				{
					if (tick2.dateTime > dateTime2)
					{
						return tick;
					}
					if (tick == null)
					{
						tick = tick2;
					}
					else
					{
						if (tick2.price < tick.price)
						{
							tick = tick2;
						}
					}
				}
			}
			return tick;
		}
		public Tick GetMax(DateTime dateTime1, DateTime dateTime2)
		{
			Tick tick = null;
			for (int i = 0; i < this.items.Count; i++)
			{
				Tick tick2 = this.items[i];
				if (!(tick2.dateTime < dateTime1))
				{
					if (tick2.dateTime > dateTime2)
					{
						return tick;
					}
					if (tick == null)
					{
						tick = tick2;
					}
					else
					{
						if (tick2.price > tick.price)
						{
							tick = tick2;
						}
					}
				}
			}
			return tick;
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
		public IEnumerator<Tick> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
	}
}
