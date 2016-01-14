using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class ExecutionSimulator : Provider, IExecutionSimulator, IExecutionProvider, IProvider
	{
		private bool fillOnQuote;
		private bool fillOnTrade;
		private bool fillOnBar;
		private bool fillOnLevel2;
		private bool fillMarketOnNext;
		private bool fillLimitOnNext;
		private bool fillStopOnNext;
		private bool fillStopLimitOnNext;
		private bool fillAtLimitPrice;
		private ICommissionProvider commissionProvider = new CommissionProvider();
		private ISlippageProvider slippageProvider = new SlippageProvider();
		private IdArray<List<Order>> orders;
		public ICommissionProvider CommissionProvider
		{
			get
			{
				return this.commissionProvider;
			}
			set
			{
				this.commissionProvider = value;
			}
		}
		public ISlippageProvider SlippageProvider
		{
			get
			{
				return this.slippageProvider;
			}
			set
			{
				this.slippageProvider = value;
			}
		}
		public bool FillOnQuote
		{
			get
			{
				return this.fillOnQuote;
			}
			set
			{
				this.fillOnQuote = value;
			}
		}
		public bool FillOnTrade
		{
			get
			{
				return this.fillOnTrade;
			}
			set
			{
				this.fillOnTrade = value;
			}
		}
		public bool FillOnBar
		{
			get
			{
				return this.fillOnBar;
			}
			set
			{
				this.fillOnBar = value;
			}
		}
		public bool FillOnLevel2
		{
			get
			{
				return this.fillOnLevel2;
			}
			set
			{
				this.fillOnLevel2 = value;
			}
		}
		public bool FillMarketOnNext
		{
			get
			{
				return this.fillMarketOnNext;
			}
			set
			{
				this.fillMarketOnNext = value;
			}
		}
		public bool FillLimitOnNext
		{
			get
			{
				return this.fillLimitOnNext;
			}
			set
			{
				this.fillLimitOnNext = value;
			}
		}
		public bool FillStopOnNext
		{
			get
			{
				return this.fillStopOnNext;
			}
			set
			{
				this.fillStopOnNext = value;
			}
		}
		public bool FillStopLimitOnNext
		{
			get
			{
				return this.fillStopLimitOnNext;
			}
			set
			{
				this.fillStopLimitOnNext = value;
			}
		}
		public bool FillAtLimitPrice
		{
			get
			{
				return this.fillAtLimitPrice;
			}
			set
			{
				this.fillAtLimitPrice = value;
			}
		}
		public ExecutionSimulator(Framework framework) : base(framework)
		{
			this.id = 2;
			this.name = "ExecutionSimulator";
			this.description = "Default execution simulator";
			this.url = "www.smartquant.com";
			this.fillOnQuote = true;
			this.fillOnTrade = true;
			this.fillOnBar = false;
			this.fillMarketOnNext = false;
			this.fillLimitOnNext = false;
			this.fillStopOnNext = false;
			this.fillStopLimitOnNext = false;
			this.fillAtLimitPrice = true;
			this.orders = new IdArray<List<Order>>(1000);
		}
		public override void Send(ExecutionCommand command)
		{
			if (base.IsDisconnected)
			{
				this.Connect();
			}
			switch (command.Type)
			{
			case ExecutionCommandType.Send:
				this.Send(command.Order);
				return;
			case ExecutionCommandType.Cancel:
				this.Cancel(command.Order);
				return;
			case ExecutionCommandType.Replace:
				this.Replace(command);
				return;
			default:
				return;
			}
		}
		private void Send(Order order)
		{
			base.EmitExecutionReport(new ExecutionReport
			{
				dateTime = this.framework.Clock.DateTime,
				order = order,
				commandID = order.id,
				instrument = order.instrument,
				ordQty = order.qty,
				timeInForce = order.timeInForce,
				execType = ExecType.ExecNew,
				ordStatus = OrderStatus.New,
				currencyId = order.instrument.currencyId,
				ordType = order.type,
				side = order.side,
				cumQty = 0.0,
				lastQty = 0.0,
				leavesQty = 0.0,
				lastPx = 0.0,
				avgPx = 0.0,
				text = order.text
			});
			int id = order.instrument.Id;
			if (this.orders[id] == null)
			{
				this.orders[id] = new List<Order>();
			}
			this.orders[id].Add(order);
			switch (order.type)
			{
			case OrderType.Market:
				if (!this.fillMarketOnNext)
				{
					Instrument instrument = order.instrument;
					if (this.fillOnQuote)
					{
						switch (order.side)
						{
						case OrderSide.Buy:
						{
							Ask ask = this.framework.DataManager.GetAsk(instrument);
							if (ask != null)
							{
								this.Fill(order, ask.Price, ask.Size);
								return;
							}
							break;
						}
						case OrderSide.Sell:
						{
							Bid bid = this.framework.DataManager.GetBid(instrument);
							if (bid != null)
							{
								this.Fill(order, bid.Price, bid.Size);
								return;
							}
							break;
						}
						}
					}
					if (this.fillOnTrade)
					{
						Trade trade = this.framework.DataManager.GetTrade(instrument);
						if (trade != null)
						{
							this.Fill(order, trade.Price, trade.Size);
						}
					}
				}
				break;
			case OrderType.Stop:
			case OrderType.Limit:
				break;
			default:
				return;
			}
		}
		private void Cancel(Order order)
		{
			this.orders[order.instrument.Id].Remove(order);
			base.EmitExecutionReport(new ExecutionReport
			{
				dateTime = this.framework.Clock.DateTime,
				order = order,
				commandID = order.id,
				instrument = order.instrument,
				ordQty = order.qty,
				timeInForce = order.timeInForce,
				execType = ExecType.ExecCancelled,
				ordStatus = OrderStatus.Cancelled,
				currencyId = order.instrument.currencyId,
				ordType = order.type,
				side = order.side,
				cumQty = order.cumQty,
				lastQty = 0.0,
				leavesQty = order.leavesQty,
				lastPx = 0.0,
				avgPx = 0.0,
				text = order.text
			});
		}
		private void Replace(ExecutionCommand command)
		{
			Order order = command.order;
			base.EmitExecutionReport(new ExecutionReport
			{
				dateTime = this.framework.Clock.DateTime,
				order = order,
				commandID = order.id,
				instrument = order.instrument,
				ordQty = order.Qty,
				execType = ExecType.ExecReplace,
				ordStatus = OrderStatus.Replaced,
				currencyId = order.instrument.currencyId,
				ordType = order.type,
				side = order.side,
				cumQty = order.cumQty,
				lastQty = 0.0,
				leavesQty = order.leavesQty,
				lastPx = 0.0,
				avgPx = 0.0,
				//ordType = order.type,
				price = command.price,
				stopPx = order.stopPx,
				//ordQty = order.qty,
				timeInForce = order.timeInForce,
				text = order.text
			});
		}
		public void Fill(Order order, double price, int size)
		{
			this.orders[order.instrument.Id].Remove(order);
			ExecutionReport executionReport = new ExecutionReport();
			executionReport.dateTime = this.framework.Clock.DateTime;
			executionReport.order = order;
			executionReport.ordType = order.type;
			executionReport.side = order.side;
			executionReport.instrument = order.instrument;
			executionReport.ordQty = order.qty;
			executionReport.commandID = order.id;
			executionReport.timeInForce = order.timeInForce;
			executionReport.execType = ExecType.ExecTrade;
			executionReport.ordStatus = OrderStatus.Filled;
			executionReport.currencyId = order.instrument.currencyId;
			executionReport.cumQty = order.qty;
			executionReport.lastQty = order.qty;
			executionReport.leavesQty = 0.0;
			executionReport.lastPx = price;
			executionReport.avgPx = price;
			executionReport.text = order.Text;
			executionReport.commission = this.commissionProvider.GetCommission(executionReport);
			executionReport.avgPx = this.slippageProvider.GetPrice(executionReport);
			base.EmitExecutionReport(executionReport);
		}
		private void RemoveDoneOrders(int instrument)
		{
			for (int i = this.orders[instrument].Count - 1; i >= 0; i--)
			{
				if (this.orders[instrument][i].IsDone)
				{
					this.orders[instrument].RemoveAt(i);
				}
			}
		}
		public void OnBid(Bid bid)
		{
			if (this.orders[bid.instrumentId] == null)
			{
				return;
			}
			if (this.fillOnQuote)
			{
				for (int i = 0; i < this.orders[bid.instrumentId].Count; i++)
				{
					Order order = this.orders[bid.instrumentId][i];
					if (order.side == OrderSide.Sell)
					{
						while (true)
						{
							switch (order.type)
							{
							case OrderType.Market:
								goto IL_6C;
							case OrderType.Stop:
								if (bid.price <= order.stopPx)
								{
									order.type = OrderType.Market;
									continue;
								}
								break;
							case OrderType.Limit:
								goto IL_81;
							case OrderType.StopLimit:
								if (bid.price <= order.stopPx)
								{
									order.type = OrderType.Limit;
									continue;
								}
								break;
							}
							break;
						}
						goto IL_F5;
						IL_6C:
						this.Fill(order, bid.price, bid.size);
						goto IL_F5;
						IL_81:
						if (bid.price >= order.price)
						{
							if (this.fillAtLimitPrice)
							{
								this.Fill(order, order.price, bid.size);
							}
							else
							{
								this.Fill(order, bid.price, bid.size);
							}
						}
					}
					IL_F5:;
				}
			}
		}
		public void OnAsk(Ask ask)
		{
			if (this.orders[ask.instrumentId] == null)
			{
				return;
			}
			if (this.fillOnQuote)
			{
				for (int i = 0; i < this.orders[ask.instrumentId].Count; i++)
				{
					Order order = this.orders[ask.instrumentId][i];
					if (order.side == OrderSide.Buy)
					{
						while (true)
						{
							switch (order.type)
							{
							case OrderType.Market:
								goto IL_6B;
							case OrderType.Stop:
								if (ask.price >= order.stopPx)
								{
									order.type = OrderType.Market;
									continue;
								}
								break;
							case OrderType.Limit:
								goto IL_80;
							case OrderType.StopLimit:
								if (ask.price >= order.stopPx)
								{
									order.type = OrderType.Limit;
									continue;
								}
								break;
							}
							break;
						}
						goto IL_F4;
						IL_6B:
						this.Fill(order, ask.price, ask.size);
						goto IL_F4;
						IL_80:
						if (ask.price <= order.price)
						{
							if (this.fillAtLimitPrice)
							{
								this.Fill(order, order.price, ask.size);
							}
							else
							{
								this.Fill(order, ask.price, ask.size);
							}
						}
					}
					IL_F4:;
				}
			}
		}
		public void OnTrade(Trade trade)
		{
			if (this.orders[trade.instrumentId] == null)
			{
				return;
			}
			if (this.fillOnTrade)
			{
				int i = 0;
				while (i < this.orders[trade.instrumentId].Count)
				{
					Order order = this.orders[trade.instrumentId][i];
					while (true)
					{
						switch (order.type)
						{
						case OrderType.Market:
							goto IL_60;
						case OrderType.Stop:
							switch (order.side)
							{
							case OrderSide.Buy:
								if (trade.price >= order.stopPx)
								{
									order.type = OrderType.Market;
									continue;
								}
								break;
							case OrderSide.Sell:
								if (trade.price <= order.stopPx)
								{
									order.type = OrderType.Market;
									continue;
								}
								break;
							}
							break;
						case OrderType.Limit:
							goto IL_78;
						case OrderType.StopLimit:
							switch (order.side)
							{
							case OrderSide.Buy:
								if (trade.price >= order.stopPx)
								{
									order.type = OrderType.Limit;
									continue;
								}
								break;
							case OrderSide.Sell:
								if (trade.price <= order.stopPx)
								{
									order.type = OrderType.Limit;
									continue;
								}
								break;
							}
							break;
						}
						break;
					}
					IL_1C1:
					i++;
					continue;
					goto IL_1C1;
					IL_60:
					this.Fill(order, trade.price, trade.size);
					goto IL_1C1;
					IL_78:
					switch (order.side)
					{
					case OrderSide.Buy:
						if (trade.price <= order.price)
						{
							if (this.fillAtLimitPrice)
							{
								this.Fill(order, order.price, trade.size);
							}
							else
							{
								this.Fill(order, trade.price, trade.size);
							}
						}
						break;
					case OrderSide.Sell:
						if (trade.price >= order.price)
						{
							if (this.fillAtLimitPrice)
							{
								this.Fill(order, order.price, trade.size);
							}
							else
							{
								this.Fill(order, trade.price, trade.size);
							}
						}
						break;
					}
					goto IL_1C1;
				}
			}
		}
		public void OnBar(Bar bar)
		{
		}
		public void OnLevel2(Level2Snapshot snapshot)
		{
		}
		public void OnLevel2(Level2Update update)
		{
		}
		public void Clear()
		{
			this.orders.Clear();
		}
	}
}
