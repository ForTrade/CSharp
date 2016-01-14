using System;
namespace SmartQuant
{
	public class PortfolioStatistics
	{
		private Portfolio portfolio;
		private TradeDetector detector;
		public int TotalTrades
		{
			get;
			private set;
		}
		public int LongTrades
		{
			get;
			private set;
		}
		public int ShortTrades
		{
			get;
			private set;
		}
		public int WinningTrades
		{
			get;
			private set;
		}
		public int LoosingTrades
		{
			get;
			private set;
		}
		public int WinningLongTrades
		{
			get;
			private set;
		}
		public int LoosingLongTrades
		{
			get;
			private set;
		}
		public int WinningShortTrades
		{
			get;
			private set;
		}
		public int LoosingShortTrades
		{
			get;
			private set;
		}
		internal PortfolioStatistics(Portfolio portfolio)
		{
			this.portfolio = portfolio;
			this.detector = new TradeDetector(TradeDetectionType.FIFO);
			this.detector.TradeDetected += new TradeInfoEventHandler(this.detector_TradeDetected);
		}
		private void detector_TradeDetected(object sender, TradeInfoEventArgs args)
		{
			TradeInfo tradeInfo = args.TradeInfo;
			if (tradeInfo.PnL > 0.0)
			{
				this.WinningTrades++;
				if (tradeInfo.IsLong)
				{
					this.WinningLongTrades++;
				}
				else
				{
					this.WinningShortTrades++;
				}
			}
			else
			{
				if (tradeInfo.PnL < 0.0)
				{
					this.LoosingTrades++;
					if (tradeInfo.IsLong)
					{
						this.LoosingLongTrades++;
					}
					else
					{
						this.LoosingShortTrades++;
					}
				}
			}
			this.TotalTrades++;
			if (tradeInfo.IsLong)
			{
				this.LongTrades++;
				return;
			}
			this.ShortTrades++;
		}
		internal void Add(Fill fill)
		{
			this.detector.Add(fill);
		}
	}
}
