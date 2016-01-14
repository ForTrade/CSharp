using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
namespace SmartQuant
{
	public class FileManager
	{
		internal string path;
		private Dictionary<string, FileStream> files = new Dictionary<string, FileStream>();
		public FileManager(string path)
		{
			this.path = path;
		}
		public FileStream GetFile(string name, FileMode mode = FileMode.OpenOrCreate)
		{
			bool flag = false;
			FileStream result;
			try
			{
				Monitor.Enter(this, ref flag);
				FileStream fileStream;
				this.files.TryGetValue(name, out fileStream);
				if (fileStream == null)
				{
					Console.WriteLine(string.Concat(new object[]
					{
						DateTime.Now,
						" Opening file : ",
						this.path,
						"\\",
						name
					}));
					fileStream = new FileStream(this.path + "\\" + name, mode);
					this.files.Add(name, fileStream);
				}
				result = fileStream;
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
		public void Close()
		{
			foreach (FileStream current in this.files.Values)
			{
				current.Close();
			}
		}
	}
}
