using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public interface IOrderServer
	{
		List<Order> Load();
		void Save(List<Order> orders);
	}
}
