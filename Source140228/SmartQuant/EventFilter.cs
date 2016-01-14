using System;
namespace SmartQuant
{
	public class EventFilter
	{
		private Framework framework;
		public EventFilter(Framework framework)
		{
			this.framework = framework;
		}
		public virtual Event Filter(Event e)
		{
			return e;
		}
	}
}
