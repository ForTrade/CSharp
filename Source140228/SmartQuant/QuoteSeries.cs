using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class QuoteSeries : IEnumerable<Quote>, IEnumerable
	{
		private string name;
		private List<Quote> items = new List<Quote>();
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
		public Quote this[int index]
		{
			get
			{
				return this.items[index];
			}
		}
		public QuoteSeries(string name = "")
		{
			this.name = name;
		}
		public void Clear()
		{
			this.items.Clear();
		}
		public void Add(Quote quote)
		{
			this.items.Add(quote);
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
		public IEnumerator<Quote> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
	}
}
