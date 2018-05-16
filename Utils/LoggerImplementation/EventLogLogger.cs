using System.Collections.Generic;
using System.Diagnostics;

namespace Utils
{
	/// <summary>
	/// Used to log a message to Windows Event Log.
	/// </summary>
	/// <remarks>
	/// This logger has 3 optional parameters
	/// <para>&#160;</para>
	/// <para>LogName : String, Event Log Name</para>
	/// <para>Source : String, Source of the event log entry, will be created if it doesn't exists</para>
	/// <para>EntryType : EventLogEntryType, Entry type</para>
	/// </remarks>
	public class EventLogLogger : Logger
	{
		private readonly string _logName;
		private readonly string _source;
		private readonly EventLogEntryType _entryType;

		public EventLogLogger(string name) : this(name,
												  @"Application")
		{
		}

		public EventLogLogger(string name,
							  string logName) : this(name,
													 logName,
													 @"Application")
		{
		}

		public EventLogLogger(string name,
							  string logName,
							  string source) : this(name,
													logName,
													source,
													EventLogEntryType.Information)
		{
		}

		public EventLogLogger(string name,
							  string logName,
							  string source,
							  EventLogEntryType defaultType) : base(name,
																	false,
																	string.Empty)
		{
			_logName = logName;
			_source = source;
			_entryType = defaultType;
		}

		protected override void WriteLog(string message,
										 IDictionary<string, object> optionalParameters)
		{
			var logName = optionalParameters.ContainsKey("LogName")
						  ? (string)optionalParameters["LogName"]
						  : _logName;
			var source = optionalParameters.ContainsKey("Source")
						 ? (string)optionalParameters["Source"]
						 : _source;
			var entryType = optionalParameters.ContainsKey("EntryType")
							? (EventLogEntryType)optionalParameters["EntryType"]
							: _entryType;

			if (!EventLog.SourceExists(source))
			{
				EventLog.CreateEventSource(source,
										   logName);
			}

			using (var eventLog = new EventLog(logName) { Source = source })
			{
				eventLog.WriteEntry(message,
									entryType);
			}
		}
	}
}
