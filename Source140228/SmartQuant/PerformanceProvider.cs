using System;
namespace SmartQuant
{
	public class PerformanceProvider : Provider, IDataProvider, IExecutionProvider, IProvider
	{
		public PerformanceProvider(Framework framework) : base(framework)
		{
		}
	}
}
