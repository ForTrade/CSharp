using System;
namespace SmartQuant
{
	public class TickBarFactoryItem : BarFactoryItem
	{
		public TickBarFactoryItem(Instrument instrument, long barSize) : base(instrument, BarType.Tick, barSize)
		{
		}
		protected internal override void OnData(DataObject obj)
		{
			base.OnData(obj);
			this.bar.dateTime = this.GetBarCloseDateTime(obj);
			if (this.bar.n == this.barSize)
			{
				base.EmitBar();
			}
		}
	}
}
