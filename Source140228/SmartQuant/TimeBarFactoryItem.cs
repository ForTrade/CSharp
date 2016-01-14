using System;
namespace SmartQuant
{
	public class TimeBarFactoryItem : BarFactoryItem
	{
		public TimeBarFactoryItem(Instrument instrument, long barSize) : base(instrument, BarType.Time, barSize)
		{
		}
		protected internal override void OnData(DataObject obj)
		{
			bool flag = this.bar == null;
			base.OnData(obj);
			if (flag)
			{
				base.AddReminder(this.bar.dateTime);
			}
		}
		protected override DateTime GetBarOpenDateTime(DataObject obj)
		{
			long num = (long)obj.dateTime.TimeOfDay.TotalSeconds / this.barSize * this.barSize;
			return obj.dateTime.Date.AddSeconds((double)num);
		}
		protected override DateTime GetBarCloseDateTime(DataObject obj)
		{
			return this.GetBarOpenDateTime(obj).AddSeconds((double)this.barSize);
		}
		protected internal override void OnReminder()
		{
			base.EmitBar();
		}
	}
}
