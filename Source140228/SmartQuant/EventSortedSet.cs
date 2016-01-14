using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant
{
	public class EventSortedSet : IEnumerable
	{
		private List<Event> series = new List<Event>();
		public string Name
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public int Count
		{
			get
			{
				return this.series.Count;
			}
		}
		public Event this[int index]
		{
			get
			{
				return this.series[index];
			}
		}
		public void Add(Event e)
		{
			int insertIndex = this.GetInsertIndex(e.dateTime);
			this.series.Insert(insertIndex, e);
		}
		private int GetInsertIndex(DateTime datetime)
		{
			if (this.series.Count == 0)
			{
				return 0;
			}
			if (this.series[0].dateTime > datetime)
			{
				return 0;
			}
			if (this.series[this.series.Count - 1].dateTime <= datetime)
			{
				return this.series.Count;
			}
			int num = 0;
			int num2 = this.series.Count - 1;
			while (num != num2)
			{
				int num3 = (num + num2 + 1) / 2;
				if (this.series[num3].dateTime <= datetime)
				{
					if (this.series[num3 + 1].dateTime > datetime)
					{
						return num3 + 1;
					}
					num = num3;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			return num + 1;
		}
		public IEnumerator GetEnumerator()
		{
			return this.series.GetEnumerator();
		}
		public void Clear()
		{
			this.series.Clear();
		}
		internal void RemoveAt(int index)
		{
			this.series.RemoveAt(0);
		}
	}
}
