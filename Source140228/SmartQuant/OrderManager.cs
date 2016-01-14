using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class OrderManager
	{
		private IOrderServer server;
		private Framework framework;
		private List<Order> orders;
		private Dictionary<string, List<Order>> oCAGroups;
		private int next_id;
		public List<Order> Orders
		{
			get
			{
				return this.orders;
			}
		}
		public IOrderServer Server
		{
			get
			{
				return this.server;
			}
			set
			{
				this.server = value;
			}
		}
		public OrderManager(Framework framework)
		{
			this.framework = framework;
			this.orders = new List<Order>();
			this.oCAGroups = new Dictionary<string, List<Order>>();
			this.next_id = 0;
		}
		public void Register(Order order)
		{
			if (order.id != -1)
			{
				Console.WriteLine("OrderManager::Register Error Order is already registered : id = " + order.id);
				return;
			}
			order.id = this.next_id++;
		}
		public void Send(Order order)
		{
			if (order.id == -1)
			{
				this.Register(order);
			}
			if (order.IsOCA)
			{
				List<Order> list;
				this.oCAGroups.TryGetValue(order.oCA, out list);
				if (list == null)
				{
					list = new List<Order>();
					this.oCAGroups[order.oCA] = list;
				}
				list.Add(order);
			}
			this.orders.Add(order);
			order.dateTime = this.framework.Clock.DateTime;
			ExecutionCommand executionCommand = new ExecutionCommand(ExecutionCommandType.Send, order);
			executionCommand.dateTime = order.dateTime;
			executionCommand.id = order.id;
			executionCommand.providerId = order.providerId;
			executionCommand.portfolioId = order.portfolioId;
			executionCommand.transactTime = order.transactTime;
			executionCommand.instrument = order.instrument;
			executionCommand.provider = order.provider;
			executionCommand.portfolio = order.portfolio;
			executionCommand.side = order.side;
			executionCommand.orderType = order.Type;
			executionCommand.timeInForce = order.timeInForce;
			executionCommand.price = order.price;
			executionCommand.stopPx = order.stopPx;
			executionCommand.qty = order.qty;
			executionCommand.oCA = order.oCA;
			executionCommand.text = order.text;
			order.OnExecutionCommand(executionCommand);
			this.framework.eventServer.OnExecutionCommand(executionCommand);
			order.Provider.Send(executionCommand);
		}
		public void Cancel(Order order)
		{
			ExecutionCommand executionCommand = new ExecutionCommand(ExecutionCommandType.Cancel, order);
			executionCommand.dateTime = this.framework.Clock.DateTime;
			executionCommand.providerId = order.providerId;
			executionCommand.portfolioId = order.portfolioId;
			executionCommand.transactTime = order.transactTime;
			executionCommand.instrument = order.instrument;
			executionCommand.provider = order.provider;
			executionCommand.portfolio = order.portfolio;
			executionCommand.side = order.side;
			executionCommand.orderType = order.Type;
			executionCommand.timeInForce = order.timeInForce;
			executionCommand.price = order.price;
			executionCommand.stopPx = order.stopPx;
			executionCommand.qty = order.qty;
			executionCommand.oCA = order.oCA;
			executionCommand.text = order.text;
			order.OnExecutionCommand(executionCommand);
			this.framework.eventServer.OnExecutionCommand(executionCommand);
			order.Provider.Send(executionCommand);
		}
		internal void Replace(Order order, double price)
		{
			this.Replace(order, price, order.stopPx, order.qty);
		}
		public void Replace(Order order, double price, double stopPx, double qty)
		{
			ExecutionCommand executionCommand = new ExecutionCommand(ExecutionCommandType.Replace, order);
			executionCommand.dateTime = this.framework.Clock.DateTime;
			executionCommand.id = order.id;
			executionCommand.providerId = order.providerId;
			executionCommand.portfolioId = order.portfolioId;
			executionCommand.transactTime = order.transactTime;
			executionCommand.instrument = order.instrument;
			executionCommand.provider = order.provider;
			executionCommand.portfolio = order.portfolio;
			executionCommand.side = order.side;
			executionCommand.orderType = order.Type;
			executionCommand.timeInForce = order.timeInForce;
			executionCommand.price = price;
			executionCommand.stopPx = stopPx;
			executionCommand.qty = qty;
			executionCommand.oCA = order.oCA;
			executionCommand.text = order.text;
			order.OnExecutionCommand(executionCommand);
			this.framework.eventServer.OnExecutionCommand(executionCommand);
			order.Provider.Send(executionCommand);
		}
		internal void OnExecutionReport(ExecutionReport report)
		{
			Order order = report.order;
			OrderStatus status = order.status;
			order.OnExecutionReport(report);
			if (status != order.status)
			{
				this.framework.eventServer.OnOrderStatusChanged(order);
			}
			ExecType execType = report.ExecType;
			switch (execType)
			{
			case ExecType.ExecTrade:
				if (order.status == OrderStatus.PartiallyFilled)
				{
					this.framework.eventServer.OnOrderPartiallyFilled(order);
					return;
				}
				this.framework.eventServer.OnOrderFilled(order);
				this.framework.eventServer.OnOrderDone(order);
				this.ProcessOCA(order);
				return;
			case ExecType.ExecPendingCancel:
				break;
			case ExecType.ExecCancelled:
				this.framework.eventServer.OnOrderCancelled(order);
				this.framework.eventServer.OnOrderDone(order);
				this.ProcessOCA(order);
				return;
			default:
				if (execType != ExecType.ExecReplace)
				{
					return;
				}
				this.framework.eventServer.OnOrderReplaced(order);
				break;
			}
		}
		private void ProcessOCA(Order order)
		{
			if (!order.IsOCA)
			{
				return;
			}
			List<Order> list;
			this.oCAGroups.TryGetValue(order.OCA, out list);
			if (list == null)
			{
				return;
			}
			this.oCAGroups.Remove(order.OCA);
			for (int i = 0; i < list.Count; i++)
			{
				Order order2 = list[i];
				if (order2 != order)
				{
					this.Cancel(order2);
				}
			}
		}
		public void Dump()
		{
			foreach (Order current in this.orders)
			{
				Console.WriteLine(current);
			}
		}
		public void Clear()
		{
			this.orders.Clear();
			this.oCAGroups.Clear();
			this.next_id = 0;
			this.framework.eventServer.OnOrderManagerCleared();
		}
		public void Save()
		{
			this.server.Save(this.orders);
		}
		public void Load()
		{
			this.orders = this.server.Load();
		}
	}
}
