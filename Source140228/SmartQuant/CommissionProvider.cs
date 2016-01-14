using System;
namespace SmartQuant
{
	public class CommissionProvider : ICommissionProvider
	{
		private CommissionType type;
		private double commission;
		private double minCommission;
		public double Commission
		{
			get
			{
				return this.commission;
			}
			set
			{
				this.commission = value;
			}
		}
		public CommissionType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		public double MinCommission
		{
			get
			{
				return this.minCommission;
			}
			set
			{
				this.minCommission = value;
			}
		}
		public virtual double GetCommission(ExecutionReport report)
		{
			double num;
			switch (this.type)
			{
			case CommissionType.PerShare:
				num = this.commission * report.cumQty;
				break;
			case CommissionType.Percent:
				num = this.commission * report.cumQty * report.avgPx;
				break;
			case CommissionType.Absolute:
				num = this.commission;
				break;
			default:
				throw new NotSupportedException("Unknown commission type " + this.type);
			}
			if (num < this.minCommission)
			{
				return this.minCommission;
			}
			return num;
		}
	}
}
