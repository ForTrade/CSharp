using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class BarFactory
	{
		internal Framework framework;
		private IdArray<List<BarFactoryItem>> itemLists;
		private SortedList<DateTime, SortedList<long, List<BarFactoryItem>>> reminderTable;
		public BarFactory(Framework framework)
		{
			this.framework = framework;
			this.itemLists = new IdArray<List<BarFactoryItem>>(1000);
			this.reminderTable = new SortedList<DateTime, SortedList<long, List<BarFactoryItem>>>();
		}
		public void Add(BarFactoryItem item)
		{
			if (item.factory != null)
			{
				throw new InvalidOperationException("BarFactoryItem is already added to another BarFactory instance.");
			}
			item.factory = this;
			int id = item.instrument.id;
			List<BarFactoryItem> list = this.itemLists[id];
			if (list == null)
			{
				list = new List<BarFactoryItem>();
				this.itemLists[id] = list;
			}
			list.Add(item);
		}
		public void Add(Instrument instrument, BarType barType, long barSize)
		{
			BarFactoryItem item;
			switch (barType)
			{
			case BarType.Time:
				item = new TimeBarFactoryItem(instrument, barSize);
				break;
			case BarType.Tick:
				item = new TickBarFactoryItem(instrument, barSize);
				break;
			case BarType.Volume:
				item = new VolumeBarFactoryItem(instrument, barSize);
				break;
			default:
				throw new ArgumentException(string.Format("Unknown bar type - {0}", barType));
			}
			this.Add(item);
		}
		public void Add(InstrumentList instruments, BarType barType, long barSize)
		{
			foreach (Instrument current in instruments)
			{
				this.Add(current, barType, barSize);
			}
		}
		public void Add(string[] symbols, BarType barType, long barSize)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				string symbol = symbols[i];
				this.Add(this.framework.InstrumentManager.Get(symbol), barType, barSize);
			}
		}
		internal void OnData(DataObject obj)
		{
			if (obj.TypeId != 4)
			{
				return;
			}
			List<BarFactoryItem> list = this.itemLists[((Tick)obj).instrumentId];
			if (list != null)
			{
				foreach (BarFactoryItem current in list)
				{
					current.OnData(obj);
				}
			}
		}
		internal void Clear()
		{
			this.itemLists.Clear();
			this.reminderTable.Clear();
		}
		internal void AddReminder(BarFactoryItem item, DateTime datetime)
		{
			bool flag = false;
			SortedList<long, List<BarFactoryItem>> sortedList;
			if (!this.reminderTable.TryGetValue(datetime, out sortedList))
			{
				sortedList = new SortedList<long, List<BarFactoryItem>>();
				this.reminderTable.Add(datetime, sortedList);
				flag = true;
			}
			List<BarFactoryItem> list;
			if (!sortedList.TryGetValue(item.barSize, out list))
			{
				list = new List<BarFactoryItem>();
				sortedList.Add(item.barSize, list);
			}
			list.Add(item);
			if (flag)
			{
				this.framework.clock.AddReminder(new ReminderCallback(this.OnReminder), datetime, null);
			}
		}
		private void OnReminder(DateTime datetime, object data)
		{
			SortedList<long, List<BarFactoryItem>> sortedList;
			if (this.reminderTable.TryGetValue(datetime, out sortedList))
			{
				this.reminderTable.Remove(datetime);
				foreach (List<BarFactoryItem> current in sortedList.Values)
				{
					foreach (BarFactoryItem current2 in current)
					{
						current2.OnReminder();
					}
				}
			}
		}
	}
}
