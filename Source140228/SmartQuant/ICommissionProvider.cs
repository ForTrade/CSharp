using System;
namespace SmartQuant
{
	public interface ICommissionProvider
	{
		CommissionType Type
		{
			get;
			set;
		}
		double Commission
		{
			get;
			set;
		}
		double MinCommission
		{
			get;
			set;
		}
		double GetCommission(ExecutionReport report);
	}
}
