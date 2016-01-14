using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant.Optimization
{
	public class OptimizationParameterSet : IEnumerable<OptimizationParameter>, IEnumerable
	{
		internal List<OptimizationParameter> parameters = new List<OptimizationParameter>();
		private double objective;
		public double Objective
		{
			get
			{
				return this.objective;
			}
			set
			{
				this.objective = value;
			}
		}
		public OptimizationParameter this[int index]
		{
			get
			{
				return this.parameters[index];
			}
		}
		public void Add(OptimizationParameter parameter)
		{
			this.parameters.Add(parameter);
		}
		public void Add(string name, double value)
		{
			this.parameters.Add(new OptimizationParameter(name, value));
		}
		public IEnumerator<OptimizationParameter> GetEnumerator()
		{
			return this.parameters.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.parameters.GetEnumerator();
		}
		public override string ToString()
		{
			string text = "";
			foreach (OptimizationParameter current in this.parameters)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					current.Name,
					" = ",
					current.Value,
					" "
				});
			}
			return text;
		}
	}
}
