using System;
namespace SmartQuant
{
	public class DataFilter
	{
		private Framework framework;
		private Instrument instrument;
		private Bid bid;
		private Ask ask;
		private Trade trade;
		public DataFilter(Framework framework)
		{
			this.framework = framework;
		}
		public virtual DataObject Filter(DataObject obj)
		{
			return obj;
		}
	}
}
