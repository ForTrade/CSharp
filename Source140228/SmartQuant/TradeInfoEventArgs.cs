using System;
namespace SmartQuant
{
	internal class TradeInfoEventArgs : EventArgs
	{
		public TradeInfo TradeInfo
		{
			get;
			private set;
		}
		public TradeInfoEventArgs(TradeInfo tradeInfo)
		{
			this.TradeInfo = tradeInfo;
		}
	}
}
