using System;
namespace SmartQuant
{
	public class EventLogger
	{
		protected internal Framework framework;
		public string Name
		{
			get;
			private set;
		}
		public EventLogger(Framework framework, string name)
		{
			this.framework = framework;
			this.Name = name;
			framework.EventLoggerManager.Add(this);
		}
		public virtual void OnEvent(Event e)
		{
		}
	}
}
