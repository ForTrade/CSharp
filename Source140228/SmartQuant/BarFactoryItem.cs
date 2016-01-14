using System;
namespace SmartQuant
{
	public class BarFactoryItem
	{
		protected internal BarFactory factory;
		protected internal Instrument instrument;
		protected BarType barType;
		protected internal long barSize;
		protected Bar bar;
		protected BarFactoryItem(Instrument instrument, BarType barType, long barSize)
		{
			this.factory = null;
			this.instrument = instrument;
			this.barType = barType;
			this.barSize = barSize;
		}
		protected internal virtual void OnData(DataObject obj)
		{
			Tick tick = (Tick)obj;
			if (this.bar == null)
			{
				this.bar = new Bar();
				this.bar.instrumentId = tick.instrumentId;
				this.bar.type = this.barType;
				this.bar.size = this.barSize;
				this.bar.openDateTime = this.GetBarOpenDateTime(obj);
				this.bar.dateTime = this.GetBarCloseDateTime(obj);
				this.bar.open = tick.price;
				this.bar.high = tick.price;
				this.bar.low = tick.price;
				this.bar.close = tick.price;
				this.bar.volume = (long)tick.size;
			}
			else
			{
				if (tick.price > this.bar.high)
				{
					this.bar.high = tick.price;
				}
				if (tick.price < this.bar.low)
				{
					this.bar.low = tick.price;
				}
				this.bar.close = tick.price;
				this.bar.volume += (long)tick.size;
			}
			this.bar.n += 1L;
		}
		protected internal virtual void OnReminder()
		{
		}
		protected virtual DateTime GetBarOpenDateTime(DataObject obj)
		{
			return obj.dateTime;
		}
		protected virtual DateTime GetBarCloseDateTime(DataObject obj)
		{
			return obj.dateTime;
		}
		protected void AddReminder(DateTime datetime)
		{
			this.factory.AddReminder(this, datetime);
		}
		protected void EmitBar()
		{
			this.factory.framework.eventServer.OnEvent(this.bar);
			this.bar = null;
		}
	}
}
