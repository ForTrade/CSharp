using System;
using System.Collections.Generic;
namespace SmartQuant
{
	public interface IDataSimulator : IDataProvider, IProvider
	{
		DateTime DateTime1
		{
			get;
			set;
		}
		DateTime DateTime2
		{
			get;
			set;
		}
		List<DataSeries> Series
		{
			get;
			set;
		}
		void Clear();
	}
}
