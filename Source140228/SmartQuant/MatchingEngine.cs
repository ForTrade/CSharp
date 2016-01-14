using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class MatchingEngine : Provider, IExecutionSimulator, IExecutionProvider, IProvider
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
		private bool addQueueToOrderText;
		private IdArray<List<Order>> orders;
		private ICommissionProvider commissionProvider = new CommissionProvider();
		private ISlippageProvider slippageProvider = new SlippageProvider();
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
		public bool AddQueueToOrderText
		{
			get
			{
				return this.addQueueToOrderText;
			}
			set
			{
				this.addQueueToOrderText = value;
			}
		}
		public MatchingEngine(Framework framework) : base(framework)
		{
			this.id = 101;
			this.name = "MatchingEngine";
			this.fillOnQuote = true;
			this.fillOnTrade = false;
			this.fillOnBar = false;
			this.fillOnLevel2 = false;
			this.fillMarketOnNext = false;
			this.fillLimitOnNext = false;
			this.fillStopOnNext = false;
			this.fillStopLimitOnNext = false;
			this.fillAtLimitPrice = true;
			this.addQueueToOrderText = false;
			this.orders = new IdArray<List<Order>>(1000);
		}
		public override void Connect()
		{
			base.Status = ProviderStatus.Connected;
		}
		public override void Disconnect()
		{
			base.Status = ProviderStatus.Disconnected;
		}
		public override void Send(ExecutionCommand command)
		{
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
				execType = ExecType.ExecNew,
				ordStatus = OrderStatus.New,
				currencyId = order.instrument.currencyId,
				ordType = order.type,
				side = order.side,
				cumQty = 0.0,
				lastQty = 0.0,
				leavesQty = 0.0,
				lastPx = 0.0,
				avgPx = 0.0
			});
			int id = order.instrument.Id;
			if (this.orders[id] == null)
			{
				this.orders[id] = new List<Order>();
			}
			this.orders[id].Add(order);
			if (this.fillOnQuote)
			{
				if (!this.fillMarketOnNext && !this.fillLimitOnNext)
				{
					this.SetQueueSizeByQuote(order);
					if (this.addQueueToOrderText)
					{
						order.Text = order.queueSize.ToString();
					}
					this.Process(order, this.framework.dataManager.GetBid(order.instrument), this.framework.dataManager.GetAsk(order.instrument));
					return;
				}
			}
			else
			{
				if (this.fillOnLevel2 && !this.fillMarketOnNext && !this.fillLimitOnNext)
				{
					this.SetQueueSizeByOrderBook(order);
					if (this.addQueueToOrderText)
					{
						order.Text = order.queueSize.ToString();
					}
					this.Process(order, this.framework.dataManager.GetOrderBook(order.instrument));
				}
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
				execType = ExecType.ExecCancelled,
				ordStatus = OrderStatus.Cancelled,
				currencyId = order.instrument.currencyId,
				ordType = order.type,
				side = order.side,
				cumQty = order.cumQty,
				lastQty = 0.0,
				leavesQty = order.leavesQty,
				lastPx = 0.0,
				avgPx = 0.0
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
				//ordQty = order.qty
			});
		}
		public void Fill(Order order, double price, int size)
		{
			if ((double)size < order.leavesQty)
			{
				base.EmitExecutionReport(new ExecutionReport
				{
					dateTime = this.framework.Clock.DateTime,
					order = order,
					ordType = order.type,
					side = order.side,
					instrument = order.instrument,
					ordQty = order.qty,
					commandID = order.id,
					execType = ExecType.ExecTrade,
					ordStatus = OrderStatus.PartiallyFilled,
					currencyId = order.instrument.currencyId,
					cumQty = order.qty + (double)size,
					lastQty = (double)size,
					leavesQty = order.leavesQty - (double)size,
					lastPx = price,
					avgPx = (order.avgPx * order.cumQty + price * (double)size) / (order.cumQty + (double)size),
					text = order.Text
				});
				return;
			}
			this.orders[order.instrument.Id].Remove(order);
			base.EmitExecutionReport(new ExecutionReport
			{
				dateTime = this.framework.Clock.DateTime,
				order = order,
				ordType = order.type,
				side = order.side,
				instrument = order.instrument,
				ordQty = order.qty,
				commandID = order.id,
				execType = ExecType.ExecTrade,
				ordStatus = OrderStatus.Filled,
				currencyId = order.instrument.currencyId,
				cumQty = order.qty,
				lastQty = order.qty,
				leavesQty = 0.0,
				lastPx = price,
				avgPx = price,
				text = order.Text
			});
		}
		private void Process(Order order, Trade trade)
		{
			if (!order.isQueueCalculated)
			{
				return;
			}
			if (order.type == OrderType.Limit && order.price == trade.price)
			{
				order.queueSize -= (double)trade.size;
				if (order.queueSize < order.leavesQty)
				{
					if (order.queueSize > 0.0)
					{
						double num = order.leavesQty - order.queueSize;
						this.Fill(order, order.price, (int)num);
						return;
					}
					if (order.queueSize < 0.0)
					{
						order.queueSize = 0.0;
						this.Fill(order, order.price, (int)order.leavesQty);
					}
				}
			}
		}
		private void Process(Order order, Bid bid, Ask ask)
		{
			if (ask == null || bid == null)
			{
				return;
			}
			if (!order.isQueueCalculated)
			{
				this.SetQueueSizeByQuote(order);
			}
			if (order.type == OrderType.Limit)
			{
				if (order.isOutPrice)
				{
					double queueSize = order.queueSize;
					this.SetQueueSizeByQuote(order);
					if (order.isOutPrice)
					{
						order.queueSize = queueSize;
					}
				}
				if (order.side == OrderSide.Buy)
				{
					if (bid.price < order.price)
					{
						order.queueSize = 0.0;
						this.Fill(order, order.price, (int)order.leavesQty);
						return;
					}
					if (bid.price == order.price && order.queueSize > (double)bid.size + order.qty)
					{
						order.queueSize = (double)bid.size + order.qty;
						return;
					}
				}
				else
				{
					if (order.side == OrderSide.Sell)
					{
						if (ask.price > order.price)
						{
							order.queueSize = 0.0;
							this.Fill(order, order.price, (int)order.leavesQty);
							return;
						}
						if (ask.price == order.price && order.queueSize > (double)ask.size + order.qty)
						{
							order.queueSize = (double)ask.size + order.qty;
							return;
						}
					}
				}
			}
			else
			{
				if (order.side == OrderSide.Buy)
				{
					if ((double)ask.size >= order.qty)
					{
						this.Fill(order, ask.price, (int)order.leavesQty);
						return;
					}
					this.Fill(order, ask.price, ask.size);
					if (order.leavesQty > 0.0)
					{
						double num = ask.price + order.instrument.tickSize;
						double num2 = Math.Ceiling(order.leavesQty / (double)ask.size);
						if (num2 >= 1.0)
						{
							int num3 = 0;
							while ((double)num3 < num2)
							{
								if (order.leavesQty >= (double)ask.size)
								{
									this.Fill(order, num + (double)num3 * order.instrument.tickSize, ask.size);
								}
								else
								{
									if (order.leavesQty < (double)ask.size)
									{
										this.Fill(order, num + (double)num3 * order.instrument.tickSize, (int)order.leavesQty);
										return;
									}
								}
								num3++;
							}
							return;
						}
						this.Fill(order, num, (int)order.leavesQty);
						return;
					}
				}
				else
				{
					if (order.side == OrderSide.Sell)
					{
						if ((double)bid.size >= order.leavesQty)
						{
							this.Fill(order, bid.price, (int)order.leavesQty);
							return;
						}
						this.Fill(order, bid.price, bid.size);
						if (order.leavesQty > 0.0)
						{
							double num4 = bid.price - order.instrument.tickSize;
							double num5 = Math.Ceiling(order.leavesQty / (double)bid.size);
							if (num5 >= 1.0)
							{
								int num6 = 0;
								while ((double)num6 < num5)
								{
									if (order.leavesQty >= (double)bid.size)
									{
										this.Fill(order, num4 - (double)num6 * order.instrument.tickSize, bid.size);
									}
									else
									{
										if (order.leavesQty < (double)bid.size)
										{
											this.Fill(order, num4 - (double)num6 * order.instrument.tickSize, (int)order.leavesQty);
											return;
										}
									}
									num6++;
								}
								return;
							}
							this.Fill(order, num4, (int)order.leavesQty);
						}
					}
				}
			}
		}
		private void Process(Order order, OrderBook curOrderBook)
		{
			if (!order.isQueueCalculated)
			{
				this.SetQueueSizeByOrderBook(order);
			}
			if (order.type == OrderType.Limit)
			{
				if (curOrderBook.bids.Count == 0 || curOrderBook.asks.Count == 0)
				{
					return;
				}
				if (order.isOutPrice)
				{
					double queueSize = order.queueSize;
					this.SetQueueSizeByOrderBook(order);
					if (order.isOutPrice)
					{
						order.queueSize = queueSize;
					}
				}
				if (order.side == OrderSide.Buy)
				{
					if (curOrderBook.bids[0].price < order.price)
					{
						order.queueSize = 0.0;
						this.Fill(order, curOrderBook.bids[0].price, (int)order.leavesQty);
						return;
					}
					Tick tick = null;
					for (int i = 0; i < curOrderBook.bids.Count; i++)
					{
						tick = curOrderBook.bids[i];
						if (tick.Price == order.price)
						{
							break;
						}
					}
					if (tick != null && order.queueSize > (double)tick.size + order.qty)
					{
						order.queueSize = (double)tick.size + order.qty;
						return;
					}
				}
				else
				{
					if (order.side == OrderSide.Sell)
					{
						if (curOrderBook.asks[0].price > order.price)
						{
							order.queueSize = 0.0;
							this.Fill(order, curOrderBook.asks[0].price, (int)order.leavesQty);
							return;
						}
						Tick tick2 = null;
						for (int j = 0; j < curOrderBook.asks.Count; j++)
						{
							tick2 = curOrderBook.asks[j];
							if (tick2.price == order.price)
							{
								break;
							}
						}
						if (tick2 != null && order.queueSize > (double)tick2.size + order.qty)
						{
							order.queueSize = (double)tick2.size + order.qty;
							return;
						}
					}
				}
			}
			else
			{
				if (order.side == OrderSide.Buy && curOrderBook.asks.Count > 0)
				{
					Tick tick3 = null;
					for (int k = 0; k < curOrderBook.asks.Count; k++)
					{
						tick3 = curOrderBook.asks[k];
						if ((double)tick3.size >= order.leavesQty)
						{
							this.Fill(order, tick3.price, (int)order.leavesQty);
							break;
						}
						this.Fill(order, tick3.price, tick3.size);
					}
					if (tick3 != null && order.leavesQty > 0.0)
					{
						double num = tick3.price + order.instrument.tickSize;
						double num2 = Math.Ceiling(order.leavesQty / (double)tick3.size);
						if (num2 >= 1.0)
						{
							int num3 = 0;
							while ((double)num3 < num2)
							{
								if (order.leavesQty >= (double)tick3.size)
								{
									this.Fill(order, num + (double)num3 * order.instrument.tickSize, tick3.size);
								}
								else
								{
									if (order.leavesQty < (double)tick3.size)
									{
										this.Fill(order, num + (double)num3 * order.instrument.tickSize, (int)order.leavesQty);
										return;
									}
								}
								num3++;
							}
							return;
						}
						this.Fill(order, num, (int)order.leavesQty);
						return;
					}
				}
				else
				{
					if (order.side == OrderSide.Sell && curOrderBook.bids.Count > 0)
					{
						Tick tick4 = null;
						for (int l = 0; l < curOrderBook.bids.Count; l++)
						{
							tick4 = curOrderBook.bids[l];
							if ((double)tick4.Size >= order.leavesQty)
							{
								this.Fill(order, tick4.price, (int)order.leavesQty);
								break;
							}
							this.Fill(order, tick4.price, tick4.size);
						}
						if (tick4 != null && order.leavesQty > 0.0)
						{
							double num4 = tick4.price - order.instrument.tickSize;
							double num5 = Math.Ceiling(order.leavesQty / (double)tick4.size);
							if (num5 >= 1.0)
							{
								int num6 = 0;
								while ((double)num6 < num5)
								{
									if (order.leavesQty >= (double)tick4.size)
									{
										this.Fill(order, num4 - (double)num6 * order.instrument.tickSize, tick4.size);
									}
									else
									{
										if (order.leavesQty < (double)tick4.size)
										{
											this.Fill(order, num4 - (double)num6 * order.instrument.tickSize, (int)order.leavesQty);
											return;
										}
									}
									num6++;
								}
								return;
							}
							this.Fill(order, num4, (int)order.leavesQty);
						}
					}
				}
			}
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
			for (int i = 0; i < this.orders[bid.instrumentId].Count; i++)
			{
				Order order = this.orders[bid.instrumentId][i];
				this.Process(order, bid, this.framework.dataManager.GetAsk(order.instrument));
				if (this.addQueueToOrderText)
				{
					order.Text = order.queueSize.ToString();
				}
			}
		}
		public void OnAsk(Ask ask)
		{
			if (this.orders[ask.instrumentId] == null)
			{
				return;
			}
			for (int i = 0; i < this.orders[ask.instrumentId].Count; i++)
			{
				Order order = this.orders[ask.instrumentId][i];
				this.Process(order, this.framework.dataManager.GetBid(order.instrument), ask);
				if (this.addQueueToOrderText)
				{
					order.Text = order.queueSize.ToString();
				}
			}
		}
		public void OnTrade(Trade trade)
		{
			if (this.orders[trade.instrumentId] == null)
			{
				return;
			}
			for (int i = 0; i < this.orders[trade.instrumentId].Count; i++)
			{
				Order order = this.orders[trade.instrumentId][i];
				if (!order.isOutPrice)
				{
					this.Process(order, trade);
				}
				if (this.addQueueToOrderText)
				{
					order.Text = order.queueSize.ToString();
				}
			}
		}
		public void OnBar(Bar bar)
		{
		}
		public void OnLevel2(Level2Snapshot snapshot)
		{
			if (this.orders[snapshot.instrumentId] == null)
			{
				return;
			}
			for (int i = 0; i < this.orders[snapshot.instrumentId].Count; i++)
			{
				Order order = this.orders[snapshot.instrumentId][i];
				this.Process(order, this.framework.dataManager.GetOrderBook(order.instrument));
				if (this.addQueueToOrderText)
				{
					order.Text = order.queueSize.ToString();
				}
			}
		}
		public void OnLevel2(Level2Update update)
		{
		}
		public void Clear()
		{
			this.orders.Clear();
		}
		private void SetQueueSizeByQuote(Order order)
		{
			Bid bid = this.framework.dataManager.GetBid(order.instrument);
			Ask ask = this.framework.dataManager.GetAsk(order.instrument);
			order.isQueueCalculated = true;
			order.queueSize = 0.0;
			if (order.type == OrderType.Limit)
			{
				if (order.side == OrderSide.Buy && bid != null)
				{
					if (order.price == bid.price)
					{
						order.queueSize = (double)bid.size + order.qty;
						order.isOutPrice = false;
						return;
					}
					order.queueSize = (double)bid.size + order.qty;
					order.isOutPrice = true;
					return;
				}
				else
				{
					if (order.side == OrderSide.Sell && ask != null)
					{
						if (order.price == ask.price)
						{
							order.queueSize = (double)ask.size + order.qty;
							order.isOutPrice = false;
							return;
						}
						order.queueSize = (double)ask.size + order.qty;
						order.isOutPrice = true;
					}
				}
			}
		}
		private void SetQueueSizeByOrderBook(Order order)
		{
			OrderBook orderBook = this.framework.dataManager.GetOrderBook(order.instrument);
			order.isQueueCalculated = true;
			order.queueSize = 0.0;
			if (order.type == OrderType.Limit)
			{
				if (order.side == OrderSide.Buy && orderBook.bids.Count > 0)
				{
					for (int i = 0; i < orderBook.bids.Count; i++)
					{
						Tick tick = orderBook.bids[i];
						order.queueSize = (double)tick.size + order.qty;
						order.isOutPrice = true;
						if (order.price == tick.price)
						{
							order.isOutPrice = false;
							return;
						}
					}
					return;
				}
				if (order.side == OrderSide.Sell && orderBook.asks.Count > 0)
				{
					for (int j = 0; j < orderBook.asks.Count; j++)
					{
						Tick tick2 = orderBook.asks[j];
						order.queueSize = (double)tick2.size + order.qty;
						order.isOutPrice = true;
						if (order.price == tick2.price)
						{
							order.isOutPrice = false;
							return;
						}
					}
				}
			}
		}
	}
}
