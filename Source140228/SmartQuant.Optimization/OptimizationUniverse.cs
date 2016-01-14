using System;
using System.Collections;
using System.Collections.Generic;
namespace SmartQuant.Optimization
{
	public class OptimizationUniverse : IEnumerable<OptimizationParameterSet>, IEnumerable
	{
		internal List<OptimizationParameterSet> parameters = new List<OptimizationParameterSet>();
		public int Count
		{
			get
			{
				return this.parameters.Count;
			}
		}
		public OptimizationParameterSet this[int index]
		{
			get
			{
				return this.parameters[index];
			}
		}
		public void Add(OptimizationParameterSet parameter)
		{
			this.parameters.Add(parameter);
		}
		public void Clear()
		{
			this.parameters.Clear();
		}
		public IEnumerator<OptimizationParameterSet> GetEnumerator()
		{
			return this.parameters.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.parameters.GetEnumerator();
		}
	}
}
