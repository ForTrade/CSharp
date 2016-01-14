using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
namespace SmartQuant
{
	public class Provider : IProvider
	{
		protected Framework framework;
		protected byte id;
		protected string name;
		protected string description;
		protected string url;
		private ProviderStatus status = ProviderStatus.Disconnected;
		protected EventQueue dataQueue;
		protected EventQueue executionQueue;
		protected EventQueue historicalQueue;
		protected EventQueue instrumentQueue;
		public byte Id
		{
			get
			{
				return this.id;
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}
		public ProviderStatus Status
		{
			get
			{
				return this.status;
			}
			protected set
			{
				if (this.status != value)
				{
					this.status = value;
					if (this.status == ProviderStatus.Connected)
					{
						this.OnConnected();
					}
					if (this.status == ProviderStatus.Disconnected)
					{
						this.OnDisconnected();
					}
					this.framework.eventServer.OnProviderStatusChanged(this);
				}
			}
		}
		public bool IsConnected
		{
			get
			{
				return this.status == ProviderStatus.Connected;
			}
		}
		public bool IsDisconnected
		{
			get
			{
				return this.status == ProviderStatus.Disconnected;
			}
		}
		public Provider(Framework framework)
		{
			this.framework = framework;
			this.status = ProviderStatus.Disconnected;
		}
		public virtual void Process(Event e)
		{
			switch (e.TypeId)
			{
			case 201:
				this.Connect();
				return;
			case 202:
				this.Disconnect();
				return;
			case 203:
				this.Subscribe(((OnSubscribe)e).instrument);
				return;
			case 204:
				this.Unsubscribe(((OnUnsubscribe)e).instrument);
				return;
			default:
				return;
			}
		}
		protected virtual void OnConnected()
		{
			if (this is IDataProvider)
			{
				this.dataQueue = new EventQueue(1, 0, 2, 25000);
				this.dataQueue.Enqueue(new OnQueueOpened());
				this.dataQueue.name = this.name + " data queue";
				this.framework.eventBus.dataPipe.Add(this.dataQueue);
			}
			if (this is IExecutionProvider)
			{
				this.executionQueue = new EventQueue(2, 0, 2, 25000);
				this.executionQueue.Enqueue(new OnQueueOpened());
				this.executionQueue.name = this.name + " execution queue";
				this.framework.eventBus.executionPipe.Add(this.executionQueue);
			}
		}
		protected virtual void OnDisconnected()
		{
			if (this is IDataProvider)
			{
				this.dataQueue.Enqueue(new OnQueueClosed());
				this.dataQueue = null;
			}
			if (this is IExecutionProvider)
			{
				this.executionQueue.Enqueue(new OnQueueClosed());
				this.executionQueue = null;
			}
		}
		public virtual void Connect()
		{
			this.Status = ProviderStatus.Connected;
		}
		public bool Connect(int timeout)
		{
			long ticks = DateTime.Now.Ticks;
			this.Connect();
			while (!this.IsConnected)
			{
				Thread.Sleep(1);
				double totalMilliseconds = TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds;
				if (totalMilliseconds >= (double)timeout)
				{
					Console.WriteLine("Provider::Connect timed out : " + this.name);
					return false;
				}
			}
			return true;
		}
		public virtual void Disconnect()
		{
			this.Status = ProviderStatus.Disconnected;
		}
		public virtual void Subscribe(Instrument instrument)
		{
		}
		public virtual void Unsubscribe(Instrument instrument)
		{
		}
		public virtual void Subscribe(InstrumentList instruments)
		{
			foreach (Instrument current in instruments)
			{
				this.Subscribe(current);
			}
		}
		public virtual void Unsubscribe(InstrumentList instruments)
		{
			foreach (Instrument current in instruments)
			{
				this.Unsubscribe(current);
			}
		}
		public virtual void Send(ExecutionCommand command)
		{
			Console.WriteLine("Provider::Send is not implemented in the base class");
		}
		public virtual void Send(HistoricalDataRequest request)
		{
		}
		public virtual void Send(InstrumentDefinitionRequest request)
		{
		}
		public virtual void RequestHistoricalData(HistoricalDataRequest request)
		{
		}
		public virtual void RequestInstrumentDefinitions(InstrumentDefinitionRequest request)
		{
		}
		protected internal void EmitProviderError(ProviderError error)
		{
			this.framework.eventServer.OnProviderError(error);
		}
		protected internal void EmitError(int id, int code, string text)
		{
			this.EmitProviderError(new ProviderError(this.framework.clock.DateTime, ProviderErrorType.Error, this.id, id, code, text));
		}
		protected internal void EmitError(string text)
		{
			this.EmitError(-1, -1, text);
		}
		protected internal void EmitWarning(int id, int code, string text)
		{
			this.EmitProviderError(new ProviderError(this.framework.clock.DateTime, ProviderErrorType.Warning, this.id, id, code, text));
		}
		protected internal void EmitWarning(string text)
		{
			this.EmitWarning(-1, -1, text);
		}
		protected internal void EmitMessage(int id, int code, string text)
		{
			this.EmitProviderError(new ProviderError(this.framework.clock.DateTime, ProviderErrorType.Message, this.id, id, code, text));
		}
		protected internal void EmitMessage(string text)
		{
			this.EmitMessage(-1, -1, text);
		}
		protected internal void EmitData(DataObject data)
		{
			this.dataQueue.Enqueue(data);
		}
		protected internal void EmitExecutionReport(ExecutionReport report)
		{
			this.executionQueue.Enqueue(report);
		}
		protected internal void EmitAccountData(AccountData data)
		{
			this.executionQueue.Enqueue(data);
		}
		protected internal void EmitHistoricalData(HistoricalData data)
		{
			this.OpenHistoricalDataQueue();
			this.historicalQueue.Enqueue(data);
		}
		protected internal void EmitHistoricalDataEnd(HistoricalDataEnd end)
		{
			this.OpenHistoricalDataQueue();
			this.historicalQueue.Enqueue(end);
			this.CloseHistoricalDataQueue();
		}
		protected internal void EmitHistoricalDataEnd(string requestId, RequestResult result, string text)
		{
			this.EmitHistoricalDataEnd(new HistoricalDataEnd
			{
				RequestId = requestId,
				Result = result,
				Text = text
			});
		}
		private void OpenHistoricalDataQueue()
		{
			if (this.historicalQueue == null)
			{
				this.historicalQueue = new EventQueue(0, 0, 2, 1000);
				this.historicalQueue.name = this.name + " historical queue";
				this.historicalQueue.Enqueue(new OnQueueOpened());
				this.framework.eventBus.servicePipe.Add(this.historicalQueue);
			}
		}
		private void CloseHistoricalDataQueue()
		{
			if (this.historicalQueue != null)
			{
				this.historicalQueue.Enqueue(new OnQueueClosed());
				this.historicalQueue = null;
			}
		}
		protected internal void EmitInstrumentDefinition(InstrumentDefinition definition)
		{
			this.OpenInstrumentQueue();
			this.instrumentQueue.Enqueue(new OnInstrumentDefinition(definition));
		}
		protected internal void EmitInstrumentDefinitionEnd(InstrumentDefinitionEnd end)
		{
			this.OpenInstrumentQueue();
			this.instrumentQueue.Enqueue(new OnInstrumentDefinitionEnd(end));
			this.CloseInstrumentQueue();
		}
		protected internal void EmitInstrumentDefinitionEnd(string requestId, RequestResult result, string text)
		{
			this.EmitInstrumentDefinitionEnd(new InstrumentDefinitionEnd
			{
				RequestId = requestId,
				Result = result,
				Text = text
			});
		}
		private void OpenInstrumentQueue()
		{
			if (this.instrumentQueue == null)
			{
				this.instrumentQueue = new EventQueue(0, 0, 2, 1000);
				this.instrumentQueue.name = this.name + " instrument queue";
				this.instrumentQueue.Enqueue(new OnQueueOpened());
				this.framework.eventBus.servicePipe.Add(this.instrumentQueue);
			}
		}
		private void CloseInstrumentQueue()
		{
			if (this.instrumentQueue != null)
			{
				this.instrumentQueue.Enqueue(new OnQueueClosed());
				this.instrumentQueue = null;
			}
		}
		protected internal virtual ProviderPropertyList GetProperties()
		{
			ProviderPropertyList providerPropertyList = new ProviderPropertyList();
			PropertyInfo[] properties = base.GetType().GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.CanRead && propertyInfo.CanWrite && !(propertyInfo.DeclaringType == typeof(Provider)))
				{
					TypeConverter converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
					if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
					{
						object value = propertyInfo.GetValue(this, null);
						providerPropertyList.SetValue(propertyInfo.Name, converter.ConvertToInvariantString(value));
					}
				}
			}
			return providerPropertyList;
		}
		protected internal virtual void SetProperties(ProviderPropertyList properties)
		{
			PropertyInfo[] properties2 = base.GetType().GetProperties();
			for (int i = 0; i < properties2.Length; i++)
			{
				PropertyInfo propertyInfo = properties2[i];
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
					if (converter != null && converter.CanConvertFrom(typeof(string)))
					{
						string stringValue = properties.GetStringValue(propertyInfo.Name, null);
						if (stringValue != null && converter.IsValid(stringValue))
						{
							propertyInfo.SetValue(this, converter.ConvertFromInvariantString(stringValue), null);
						}
					}
				}
			}
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"provider id = ",
				this.id,
				" ",
				this.name,
				" (",
				this.description,
				") ",
				this.url
			});
		}
	}
}
