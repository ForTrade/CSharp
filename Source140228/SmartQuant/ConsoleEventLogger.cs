using System;
namespace SmartQuant
{
	public class ConsoleEventLogger : EventLogger
	{
		public ConsoleEventLogger(Framework framework) : base(framework, "Console")
		{
		}
		public override void OnEvent(Event e)
		{
			if (e != null && e.TypeId != 2 && e.TypeId != 3 && e.TypeId != 4 && e.TypeId != 6)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"Event ",
					e.TypeId,
					" ",
					e.GetType()
				}));
			}
		}
	}
}
