using System;
namespace SmartQuant
{
	public class VolumeBarFactoryItem : BarFactoryItem
	{
		public VolumeBarFactoryItem(Instrument instrument, long barSize) : base(instrument, BarType.Volume, barSize)
		{
		}
		protected internal override void OnData(DataObject obj)
		{
			base.OnData(obj);
			this.bar.dateTime = this.GetBarCloseDateTime(obj);
			if (this.bar.volume >= this.barSize)
			{
				base.EmitBar();
			}
		}
	}
}
