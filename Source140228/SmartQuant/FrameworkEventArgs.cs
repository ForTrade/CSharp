using System;
namespace SmartQuant
{
	public class FrameworkEventArgs : EventArgs
	{
		public Framework Framework
		{
			get;
			private set;
		}
		public FrameworkEventArgs(Framework framework)
		{
			this.Framework = framework;
		}
	}
}
