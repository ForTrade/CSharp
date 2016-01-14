using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
namespace SmartQuant
{
	public class DataFileManager
	{
		internal string path;
		private Dictionary<string, DataFile> files = new Dictionary<string, DataFile>();
		private StreamerManager streamerManager = new StreamerManager();
		public DataFileManager(string path)
		{
			this.path = path;
			this.streamerManager.Add(new DataObjectStreamer());
			this.streamerManager.Add(new BidStreamer());
			this.streamerManager.Add(new AskStreamer());
			this.streamerManager.Add(new QuoteStreamer());
			this.streamerManager.Add(new TradeStreamer());
			this.streamerManager.Add(new BarStreamer());
			this.streamerManager.Add(new Level2SnapshotStreamer());
			this.streamerManager.Add(new Level2UpdateStreamer());
			this.streamerManager.Add(new NewsStreamer());
			this.streamerManager.Add(new FundamentalStreamer());
			this.streamerManager.Add(new DataSeriesStreamer());
		}
		public DataFile GetFile(string name, FileMode mode = FileMode.OpenOrCreate)
		{
			bool flag = false;
			DataFile result;
			try
			{
				Monitor.Enter(this, ref flag);
				DataFile dataFile;
				this.files.TryGetValue(name, out dataFile);
				if (dataFile == null)
				{
					Console.WriteLine(DateTime.Now + " Opening file : " + name);
					dataFile = new DataFile(this.path + "\\" + name, this.streamerManager);
					dataFile.Open(mode);
					this.files.Add(name, dataFile);
				}
				result = dataFile;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
		public void Close(string name)
		{
			DataFile dataFile;
			this.files.TryGetValue(name, out dataFile);
			if (dataFile != null)
			{
				dataFile.Close();
				this.files.Remove(name);
			}
		}
		public DataSeries GetSeries(string fileName, string seriesName)
		{
			DataFile file = this.GetFile(fileName, FileMode.OpenOrCreate);
			DataSeries dataSeries = (DataSeries)file.Get(seriesName);
			if (dataSeries == null)
			{
				dataSeries = new DataSeries(seriesName);
				file.Write(seriesName, dataSeries);
			}
			return dataSeries;
		}
		public void Delete(string fileName, string objectName)
		{
			DataFile file = this.GetFile(fileName, FileMode.OpenOrCreate);
			file.Delete(objectName);
		}
		public void Close()
		{
			foreach (DataFile current in this.files.Values)
			{
				current.Close();
			}
		}
	}
}
