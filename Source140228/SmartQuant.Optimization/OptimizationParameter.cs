using System;
namespace SmartQuant.Optimization
{
	public class OptimizationParameter
	{
		private string name;
		private double value;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public double Value
		{
			get
			{
				return this.value;
			}
		}
		public OptimizationParameter(string name, double value)
		{
			this.name = name;
			this.value = value;
		}
	}
}
