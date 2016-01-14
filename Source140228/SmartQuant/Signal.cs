using System;
namespace SmartQuant
{
	public class Signal
	{
		private double value;
		public double Value
		{
			get
			{
				return this.value;
			}
		}
		public Signal(double value)
		{
			this.value = value;
		}
	}
}
