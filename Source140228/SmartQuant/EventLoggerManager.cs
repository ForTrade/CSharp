using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public class EventLoggerManager
	{
		private Dictionary<string, EventLogger> loggers;
		public EventLoggerManager()
		{
			this.loggers = new Dictionary<string, EventLogger>();
		}
		public void Add(EventLogger logger)
		{
			this.loggers[logger.Name] = logger;
		}
		public EventLogger GetLogger(string name)
		{
			return this.loggers[name];
		}
	}
}
