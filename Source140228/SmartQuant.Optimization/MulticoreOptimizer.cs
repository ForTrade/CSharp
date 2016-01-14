using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
namespace SmartQuant.Optimization
{
	public class MulticoreOptimizer
	{
		private Stopwatch watch = new Stopwatch();
		private long event_count;
		public long Elapsed
		{
			get
			{
				return this.watch.ElapsedMilliseconds;
			}
		}
		public long EventCount
		{
			get
			{
				return this.event_count;
			}
		}
		public OptimizationParameterSet Optimize(Strategy strategy, InstrumentList instruments, OptimizationUniverse universe, int bunch = -1)
		{
			this.event_count = 0L;
			this.watch.Start();
			int num;
			if (bunch == -1)
			{
				num = universe.Count;
			}
			else
			{
				num = bunch;
			}
			int num2 = 0;
			while (num2 + num < universe.Count)
			{
				this.Optimize(strategy, instruments, universe, num2, num);
				num2 += num;
			}
			this.Optimize(strategy, instruments, universe, num2, universe.Count - num2);
			int index = 0;
			for (int i = 1; i < universe.Count; i++)
			{
				if (universe[i].Objective > universe[index].Objective)
				{
					index = i;
				}
			}
			Console.WriteLine(string.Concat(new object[]
			{
				"Best Objective ",
				universe[index],
				" Objective = ",
				universe[index].Objective
			}));
			Console.WriteLine("Optimization done");
			this.watch.Stop();
			Console.WriteLine(string.Concat(new object[]
			{
				"Processed ",
				this.event_count,
				" events in ",
				this.watch.ElapsedMilliseconds,
				" msec - ",
				(double)this.event_count / (double)this.watch.ElapsedMilliseconds * 1000.0,
				" event/sec"
			}));
			return universe[index];
		}
		private void Optimize(Strategy strategy, InstrumentList instruments, OptimizationUniverse universe, int index, int n)
		{
			Framework[] array = new Framework[n];
			Strategy[] array2 = new Strategy[n];
			for (int i = 0; i < n; i++)
			{
				if (i == 0)
				{
					array[i] = strategy.framework;
				}
				else
				{
					array[i] = new Framework("framework " + i, array[i - 1].EventBus, strategy.framework.InstrumentServer, null);
				}
				array2[i] = (Strategy)Activator.CreateInstance(strategy.GetType(), new object[]
				{
					array[i],
					"strategy " + i
				});
			}
			for (int j = 0; j < n; j++)
			{
				OptimizationParameterSet optimizationParameterSet = universe[index + j];
				foreach (OptimizationParameter current in optimizationParameterSet)
				{
					if (current.Name == "Bar")
					{
						using (IEnumerator<Instrument> enumerator2 = instruments.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Instrument current2 = enumerator2.Current;
								array[j].EventManager.BarFactory.Add(current2, BarType.Time, (long)current.Value);
							}
							continue;
						}
					}
					FieldInfo field = array2[j].GetType().GetField(current.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.SetField);
					if (field != null)
					{
						if (field.FieldType == typeof(int))
						{
							field.SetValue(array2[j], (int)current.Value);
						}
						else
						{
							field.SetValue(array2[j], current.Value);
						}
					}
					else
					{
						Console.WriteLine("Optimizer::Optimize Can not set field with name " + current.Name);
					}
				}
			}
			for (int k = 0; k < n; k++)
			{
				foreach (Instrument current3 in instruments)
				{
					array2[k].AddInstrument(current3);
				}
			}
			for (int l = n - 1; l >= 0; l--)
			{
				array[l].StrategyManager.StartStrategy(array2[l], StrategyMode.Backtest);
			}
			bool flag;
			do
			{
				flag = true;
				for (int m = 0; m < n; m++)
				{
					if (array2[m].Status != StrategyStatus.Stopped)
					{
						flag = false;
					}
				}
				Thread.Sleep(10);
			}
			while (!flag);
			for (int num = 0; num < n; num++)
			{
				universe[num].Objective = array2[num].Objective();
				Console.WriteLine(universe[num] + " Objective = " + universe[num].Objective);
			}
			for (int num2 = 0; num2 < n; num2++)
			{
				this.event_count += array[num2].eventManager.EventCount;
			}
			for (int num3 = 0; num3 < n; num3++)
			{
				array[num3] = null;
				array2[num3] = null;
			}
			strategy.framework.Clear();
		}
	}
}
