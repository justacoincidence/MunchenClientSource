using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Runtime.CompilerServices;
using MelonLoader;
using MunchenClient.ModuleSystem.Modules;

namespace MunchenClient.Utils
{
	internal class ConsoleUtils
	{
		private static readonly ConcurrentQueue<ConsoleEntry> writeQueue = new ConcurrentQueue<ConsoleEntry>();

		private static readonly MelonLogger.Instance loggerInstance = new MelonLogger.Instance("MunchenClient");

		internal static void ProcessWriteQueue()
		{
			for (int i = 0; i < writeQueue.Count; i++)
			{
				if (!writeQueue.TryDequeue(out var result))
				{
					continue;
				}
				string text = GeneralHandler.RemoveRichStyleFromText(result.text);
				if (result.identifier != string.Empty)
				{
					if (result.callerLine != -1)
					{
						loggerInstance.Msg(result.textColor, $"[{result.callerName}:{result.callerLine}] [{result.identifier}]: {text}");
					}
					else
					{
						loggerInstance.Msg(result.textColor, "[" + result.identifier + "]: " + text);
					}
				}
				else if (result.callerLine != -1)
				{
					loggerInstance.Msg(result.textColor, $"[{result.callerName}:{result.callerLine}]: {text}");
				}
				else
				{
					loggerInstance.Msg(result.textColor, text);
				}
			}
		}

		internal static void Info(string identifier, string text, ConsoleColor textColor = ConsoleColor.Gray, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = -1)
		{
			QueueToConsole(identifier, text, textColor, callerName, callerLine);
		}

		internal static void Exception(string identifier, string exceptionIdentifier, Exception e, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = -1)
		{
			QueueToConsole(identifier, "EXCEPTION: " + exceptionIdentifier + " - Report to our staff", ConsoleColor.Red, callerName, callerLine);
			QueueToConsole(string.Empty, "============= STACK TRACE ====================", ConsoleColor.Red, string.Empty, -1);
			QueueToConsole(string.Empty, e.StackTrace, ConsoleColor.Red, string.Empty, -1);
			QueueToConsole(string.Empty, "===============================================", ConsoleColor.Red, string.Empty, -1);
			QueueToConsole(string.Empty, "============== MESSAGE ========================", ConsoleColor.Red, string.Empty, -1);
			QueueToConsole(string.Empty, e.Message, ConsoleColor.Red, string.Empty, -1);
			QueueToConsole(string.Empty, "===============================================", ConsoleColor.Red, string.Empty, -1);
		}

		internal static void QueueToConsole(string identifier, string text, ConsoleColor textColor, string callerName, int callerLine)
		{
			writeQueue.Enqueue(new ConsoleEntry
			{
				identifier = identifier,
				text = text,
				textColor = textColor,
				callerName = callerName,
				callerLine = callerLine
			});
		}

		internal static void FlushToConsole(string identifier, string text, ConsoleColor textColor = ConsoleColor.Gray, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLine = -1)
		{
			QueueToConsole(identifier, text, textColor, callerName, callerLine);
			ProcessWriteQueue();
		}

		internal static void SetTitle(string title)
		{
			Console.Title = title;
		}

		internal static string GetTitle()
		{
			return Console.Title;
		}

		internal static ConsoleColor ClosestConsoleColor(byte r, byte g, byte b)
		{
			ConsoleColor result = ConsoleColor.White;
			double num = (int)r;
			double num2 = (int)g;
			double num3 = (int)b;
			double num4 = double.MaxValue;
			foreach (ConsoleColor value in Enum.GetValues(typeof(ConsoleColor)))
			{
				string name = Enum.GetName(typeof(ConsoleColor), value);
				Color color = Color.FromName((name == "DarkYellow") ? "Orange" : name);
				double num5 = Math.Pow((double)(int)color.R - num, 2.0) + Math.Pow((double)(int)color.G - num2, 2.0) + Math.Pow((double)(int)color.B - num3, 2.0);
				if (num5 == 0.0)
				{
					return value;
				}
				if (num5 < num4)
				{
					num4 = num5;
					result = value;
				}
			}
			return result;
		}
	}
}
