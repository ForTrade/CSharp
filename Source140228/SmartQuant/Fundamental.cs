using System;
namespace SmartQuant
{
	public class Fundamental : DataObject
	{
		internal int providerId;
		internal int instrumentId;
		internal IdArray<double> fields = new IdArray<double>(10);
		internal static FundamentalFieldByName fieldByName = new FundamentalFieldByName();
		public override byte TypeId
		{
			get
			{
				return 22;
			}
		}
		public double this[byte index]
		{
			get
			{
				return this.fields[(int)index];
			}
			set
			{
				if (this.fields == null)
				{
					this.fields = new IdArray<double>(10);
				}
				this.fields[(int)index] = value;
			}
		}
		public double this[string name]
		{
			get
			{
				return this.fields[(int)Fundamental.fieldByName[name]];
			}
			set
			{
				this[Fundamental.fieldByName[name]] = value;
			}
		}
		public Fundamental()
		{
		}
		public Fundamental(DateTime dateTime, int providerId, int instrumentId) : base(dateTime)
		{
			this.providerId = providerId;
			this.instrumentId = instrumentId;
		}
		public static void AddField(string name, byte index)
		{
			Fundamental.fieldByName.Add(name, index);
		}
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < this.fields.Size; i++)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					i.ToString(),
					"=",
					this.fields[i].ToString(),
					";"
				});
			}
			return text;
		}
	}
}
