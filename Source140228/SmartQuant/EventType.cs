using System;
namespace SmartQuant
{
	public class EventType
	{
		public const byte Event = 0;
		public const byte Tick = 1;
		public const byte Bid = 2;
		public const byte Ask = 3;
		public const byte Trade = 4;
		public const byte Quote = 5;
		public const byte Bar = 6;
		public const byte Level2 = 7;
		public const byte Level2Snapshot = 8;
		public const byte Level2Update = 9;
		public const byte Fill = 10;
		public const byte TimeSeriesItem = 11;
		public const byte Order = 12;
		public const byte ExecutionReport = 13;
		public const byte ExecutionCommand = 14;
		public const byte Reminder = 15;
		public const byte StrategyEvent = 16;
		public const byte Text = 17;
		public const byte DataSeries = 18;
		public const byte FieldList = 19;
		public const byte StrategyStatus = 20;
		public const byte ProviderError = 21;
		public const byte Fundamental = 22;
		public const byte News = 23;
		public const byte Group = 50;
		public const byte GroupUpdate = 51;
		public const byte GroupEvent = 52;
		public const byte ResponseEvent = 60;
		public const byte OnFrameworkCleared = 99;
		public const byte OnInstrumentAdded = 100;
		public const byte OnInstrumentDeleted = 101;
		public const byte OnProviderAdded = 102;
		public const byte OnProviderRemoved = 103;
		public const byte OnProviderConnected = 104;
		public const byte OnProviderDisconnected = 105;
		public const byte OnProviderStatusChanged = 106;
		public const byte OnSimulatorStart = 107;
		public const byte OnSimulatorStop = 108;
		public const byte OnSimulatorProgress = 109;
		public const byte OnPositionOpened = 110;
		public const byte OnPositionClosed = 111;
		public const byte OnPositionChanged = 112;
		public const byte OnFill = 113;
		public const byte OnExecutionCommand = 114;
		public const byte OnExecutionReport = 115;
		public const byte OnOrderStatusChanged = 116;
		public const byte OnOrderPartiallyFilled = 117;
		public const byte OnOrderFilled = 118;
		public const byte OnOrderReplaced = 119;
		public const byte OnOrderCancelled = 120;
		public const byte OnOrderDone = 121;
		public const byte OnOrderManagerCleared = 122;
		public const byte OnInstrumentDefinition = 123;
		public const byte OnInstrumentDefintionEnd = 124;
		public const byte OnHistoricalData = 125;
		public const byte OnHistoricalDataEnd = 126;
		public const byte OnPortfolioAdded = 127;
		public const byte OnPortfolioDeleted = 128;
		public const byte OnPortfolioParentChanged = 129;
		public const byte String = 150;
		public const byte Long = 151;
		public const byte Int = 152;
		public const byte DateTime = 153;
		public const byte Char = 154;
		public const byte Boolean = 155;
		public const byte Color = 156;
		public const byte OnConnect = 201;
		public const byte OnDisconnect = 202;
		public const byte OnSubscribe = 203;
		public const byte OnUnsubscribe = 204;
		public const byte OnQueueOpened = 205;
		public const byte OnQueueClosed = 206;
		public const byte OnEventManagerStarted = 207;
		public const byte OnEventManagerStopped = 208;
		public const byte OnEventManagerPaused = 209;
		public const byte OnEventManagerResumed = 210;
		public const byte OnEventManagerStep = 211;
	}
}
