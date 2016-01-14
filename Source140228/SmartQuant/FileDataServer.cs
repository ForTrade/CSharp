using System;
using System.IO;
namespace SmartQuant
{
	public class FileDataServer : DataServer
	{
		private DataFile file;
		private DataSeries series;
		private IdArray<DataSeries>[] seriesList;
		private bool isOpen;
		public FileDataServer(Framework framework, string fileName, string host = null) : base(framework)
		{
			if (host == null)
			{
				this.file = new DataFile(fileName, framework.streamerManager);
				return;
			}
			this.file = new NetDataFile(fileName, host, framework.streamerManager);
		}
		public override void Open()
		{
			if (!this.isOpen)
			{
				this.file.Open(FileMode.OpenOrCreate);
				this.seriesList = new IdArray<DataSeries>[128];
				for (int i = 0; i < this.seriesList.Length; i++)
				{
					this.seriesList[i] = new IdArray<DataSeries>(1000);
				}
				this.isOpen = true;
			}
		}
		public override void Close()
		{
			if (this.isOpen)
			{
				this.file.Close();
				this.isOpen = false;
			}
		}
		public override void Flush()
		{
			this.file.Flush();
		}
		private string GetSuffix(byte type)
		{
			switch (type)
			{
			case 1:
				return "Tick";
			case 2:
				return "Bid";
			case 3:
				return "Ask";
			case 4:
				return "Trade";
			case 5:
				return "Quote";
			case 6:
				return "Bar";
			case 7:
			case 8:
			case 9:
				return "Level2";
			default:
				switch (type)
				{
				case 22:
					return "Fundamental";
				case 23:
					return "News";
				default:
					return "";
				}
				break;
			}
		}
		private string GetName(Instrument instrument, byte type)
		{
			return string.Concat(new object[]
			{
				instrument.symbol,
				".",
				instrument.id,
				".",
				this.GetSuffix(type)
			});
		}
		public override void Save(Instrument instrument, DataObject obj)
		{
			byte b;
			if (this.tapeMode)
			{
				b = 1;
			}
			else
			{
				b = obj.TypeId;
			}
			this.series = this.seriesList[(int)b][instrument.Id];
			if (this.series == null)
			{
				string name = this.GetName(instrument, b);
				this.series = (DataSeries)this.file.Get(name);
				if (this.series == null)
				{
					this.series = new DataSeries(name);
					this.file.Write(name, this.series);
				}
				this.seriesList[(int)b][instrument.Id] = this.series;
			}
			this.series.Add(obj);
		}
		public override DataSeries GetDataSeries(Instrument instrument, byte type)
		{
			this.series = this.seriesList[(int)type][instrument.Id];
			if (this.series == null)
			{
				this.series = (this.file.Get(this.GetName(instrument, type)) as DataSeries);
				this.seriesList[(int)type][instrument.Id] = this.series;
			}
			return this.series;
		}
		public override DataSeries GetDataSeries(string name)
		{
			return (DataSeries)this.file.Get(name);
		}
		public override void DeleteDataSeries(Instrument instrument, byte type)
		{
			this.series = this.seriesList[(int)type][instrument.Id];
			if (this.series != null)
			{
				this.seriesList[(int)type].Remove(instrument.id);
			}
			this.file.Delete(this.GetName(instrument, type));
		}
		public override void DeleteDataSeries(string name)
		{
			this.series = (DataSeries)this.file.Get(name);
			if (this.series != null)
			{
				for (int i = 0; i < this.seriesList.Length; i++)
				{
					for (int j = 0; j < this.seriesList[i].Size; j++)
					{
						if (this.seriesList[i][j] == this.series)
						{
							this.seriesList[i].Remove(j);
						}
					}
				}
				this.file.Delete(name);
			}
		}
		public override DataSeries AddDataSeries(Instrument instrument, byte type)
		{
			DataSeries dataSeries = this.seriesList[(int)type][instrument.Id];
			if (dataSeries == null)
			{
				string name = this.GetName(instrument, type);
				dataSeries = (DataSeries)this.file.Get(name);
				if (dataSeries == null)
				{
					dataSeries = new DataSeries(name);
					this.file.Write(name, dataSeries);
				}
				this.seriesList[(int)type][instrument.Id] = dataSeries;
			}
			return dataSeries;
		}
		public override DataSeries AddDataSeries(string name)
		{
			return base.AddDataSeries(name);
		}
	}
}
