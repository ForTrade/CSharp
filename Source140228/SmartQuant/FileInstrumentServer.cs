using System;
using System.IO;
namespace SmartQuant
{
	public class FileInstrumentServer : InstrumentServer
	{
		private DataFile file;
		private bool isOpen;
		public FileInstrumentServer(Framework framework, string fileName, string host = null) : base(framework)
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
		public override InstrumentList Load()
		{
			this.instruments.Clear();
			foreach (ObjectKey current in this.file.Keys.Values)
			{
				if (current.TypeId == 100)
				{
					Instrument instrument = current.GetObject() as Instrument;
					this.instruments.Add(instrument);
				}
			}
			return this.instruments;
		}
		public override void Save(Instrument instrument)
		{
			this.file.Write(instrument.symbol, instrument);
		}
		public override void Delete(Instrument instrument)
		{
			this.file.Delete(instrument.Symbol);
		}
	}
}
