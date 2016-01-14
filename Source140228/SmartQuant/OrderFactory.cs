using System;
namespace SmartQuant
{
	public class OrderFactory
	{
		private IdArray<Order> orders = new IdArray<Order>(1000000);
		public Order OnExecutionCommand(ExecutionCommand command)
		{
			Order order = this.orders[command.Id];
			if (order == null)
			{
				order = new Order();
				this.orders[command.Id] = order;
				order.dateTime = command.dateTime;
				order.id = command.id;
				order.providerId = command.providerId;
				order.portfolioId = command.portfolioId;
				order.transactTime = command.transactTime;
				order.instrument = command.instrument;
				order.provider = command.provider;
				order.portfolio = command.portfolio;
				order.side = command.side;
				order.type = command.orderType;
				order.timeInForce = command.timeInForce;
				order.price = command.price;
				order.stopPx = command.stopPx;
				order.qty = command.qty;
				order.oCA = command.oCA;
				order.text = command.text;
			}
			command.order = order;
			order.OnExecutionCommand(command);
			return order;
		}
		public Order OnExecutionReport(ExecutionReport report)
		{
			Order order = this.orders[report.CommandId];
			if (order == null)
			{
				return null;
			}
			report.order = order;
			order.OnExecutionReport(report);
			return order;
		}
		public void Reset()
		{
			this.orders.Clear();
		}
	}
}
