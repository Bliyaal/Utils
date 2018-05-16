using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Used to log a message to standard console window.
	/// </summary>
	/// <remarks>
	/// This logger has no optional parameters.
	/// </remarks>
	public class ConsoleLogger : Logger
	{
		public ConsoleLogger(string name) : this(name, false)
		{
		}

		public ConsoleLogger(string name,
							 bool timestamp) : this(name, timestamp, string.Empty)
		{
		}

		public ConsoleLogger(string name,
							 bool timestamp,
							 string timestampFormat)
			: base(name,
					timestamp,
					timestampFormat)
		{
		}

		protected override void WriteLog(string message,
										 IDictionary<string, object> optionalParameters)
		{
			Console.WriteLine(message);
		}
	}
}
