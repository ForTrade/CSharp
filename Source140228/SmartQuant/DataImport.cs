using System;
using System.Globalization;
using System.IO;
namespace SmartQuant
{
	public class DataImport
	{
		private Framework framework;
		public DataImport(Framework framework)
		{
			this.framework = framework;
		}
		public void Import(string fileName, string symbol, int type)
		{
			Console.WriteLine("Starting export: " + fileName + " " + symbol);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			TextReader textReader = File.OpenText(fileName);
			Instrument instrument = this.framework.InstrumentManager.Get(symbol);
			if (instrument == null)
			{
				instrument = new Instrument(InstrumentType.Stock, symbol, "", 1);
				this.framework.InstrumentManager.Add(instrument, true);
			}
			int num4 = 0;
			double num5 = -1.0;
			int num6 = -1;
			double num7 = -1.0;
			int num8 = -1;
			textReader.ReadLine();
			while (true)
			{
				string text = textReader.ReadLine();
				if (text == null)
				{
					break;
				}
				string[] array = text.Split(new char[]
				{
					','
				});
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				switch (type)
				{
				case 4:
				{
					DateTime dateTime = DateTime.ParseExact(array[0], "yyyy-MM-dd HH:mm:ss.fff", invariantCulture);
					double num9 = double.Parse(array[1], invariantCulture);
					int num10 = int.Parse(array[2]);
					if (num9 > 0.0 && num10 > 0)
					{
						Trade obj = new Trade(dateTime, 1, instrument.Id, num9, num10);
						this.framework.DataManager.Save(instrument, obj);
						num3++;
					}
					break;
				}
				case 5:
				{
					DateTime dateTime2 = DateTime.ParseExact(array[0], "yyyy-MM-dd HH:mm:ss.fff", invariantCulture);
					double num11 = double.Parse(array[1], invariantCulture);
					int num12 = int.Parse(array[2]);
					double num13 = double.Parse(array[3], invariantCulture);
					int num14 = int.Parse(array[4]);
					if (num11 > 0.0 && num12 > 0 && (num5 != num11 || num6 != num12))
					{
						Bid obj2 = new Bid(dateTime2, 1, instrument.Id, num11, num12);
						this.framework.DataManager.Save(instrument, obj2);
						num5 = num11;
						num6 = num12;
						num2++;
					}
					if (num13 > 0.0 && num14 > 0 && (num7 != num13 || num8 != num14))
					{
						Ask obj3 = new Ask(dateTime2, 1, instrument.Id, num13, num14);
						this.framework.DataManager.Save(instrument, obj3);
						num7 = num13;
						num8 = num14;
						num++;
					}
					break;
				}
				}
				if (num4 % 100000 == 0)
				{
					Console.WriteLine(num4);
				}
				num4++;
			}
			Console.WriteLine(string.Concat(new object[]
			{
				"Lines = ",
				num4,
				" bids: ",
				num2,
				" asks: ",
				num,
				" trades: ",
				num3
			}));
			textReader.Close();
		}
	}
}
