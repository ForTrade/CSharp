using System;
using System.Net;
using System.Net.Sockets;
namespace SmartQuant
{
	public class DataFileServer
	{
		private int port = 1000;
		private IPAddress address = IPAddress.Any;
		private TcpListener server;
		private FileManager fileManager;
		public DataFileServer(string path)
		{
			Console.WriteLine(DateTime.Now + " Creating DataFileServer on " + path);
			this.fileManager = new FileManager(path);
		}
		public void Start()
		{
			this.server = new TcpListener(this.address, this.port);
			this.server.Start();
			while (true)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					DateTime.Now,
					" Listening for client connection on port ",
					this.port,
					" ..."
				}));
				TcpClient client = this.server.AcceptTcpClient();
				Console.WriteLine(DateTime.Now + " Client connected");
				new DataFileServerClient(client, this.fileManager);
			}
		}
		public void Stop()
		{
			this.server.Stop();
			this.fileManager.Close();
		}
	}
}
