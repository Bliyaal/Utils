using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
	/// <summary>
	/// Base class that must be inherited. Provide standard functionalities for all derived classes.
	/// </summary>
	public abstract class Logger
	{
		/// <summary>
		/// Unique name identifying the logger
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Indicates wheter or not the logger adds a timestamp before the message
		/// </summary>
		private readonly bool _timestamp;

		/// <summary>
		/// DateTime format used for the timestamp
		/// </summary>
		private readonly string _timestampFormat;

		/// <summary>
		/// Base constructor. Does the standard initialization.
		/// </summary>
		/// <param name="name">Unique name identifying the logger</param>
		protected Logger(string name) : this(name,
											 false)
		{
		}

		/// <summary>
		/// Base constructor. Does the standard initialization.
		/// </summary>
		/// <param name="name">Unique name identifying the logger</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		protected Logger(string name,
						 bool timestamp) : this(name,
												timestamp,
												string.Empty)
		{
		}

		/// <summary>
		/// Base constructor. Does the standard initialization.
		/// </summary>
		/// <param name="name">Unique name identifying the logger</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		/// <param name="timestampFormat">DateTime format used for the timestamp</param>
		protected Logger(string name,
						 bool timestamp,
						 string timestampFormat)
		{
			Name = name;
			_timestamp = timestamp;
			_timestampFormat = timestampFormat;
		}

		/// <summary>
		/// Invoke the specialized function WriteLog.
		/// </summary>
		/// <param name="message">Message to log</param>
		public void Log(string message)
		{
			Log(message,
				_timestamp);
		}

		/// <summary>
		/// Invoke the specialized function WriteLog.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="optionalParameters">Anonymous type/value collection</param>
		public void Log(string message,
						object optionalParameters)
		{
			Log(message,
				_timestamp,
				optionalParameters);
		}

		/// <summary>
		/// Invoke the specialized function WriteLog.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		public void Log(string message,
						bool timestamp)
		{
			Log(message,
				timestamp,
				_timestampFormat);
		}

		/// <summary>
		/// Invoke the specialized function WriteLog.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		/// <param name="optionalParameters">Anonymous type/value collection</param>
		public void Log(string message,
						bool timestamp,
						object optionalParameters)
		{
			Log(message,
				timestamp,
				_timestampFormat,
				optionalParameters);
		}

		/// <summary>
		/// Invoke the specialized function WriteLog.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		/// <param name="timestampFormat">DateTime format used for the timestamp</param>
		public void Log(string message,
						bool timestamp,
						string timestampFormat)
		{
			Log(message,
				timestamp,
				timestampFormat,
				null);
		}

		/// <summary>
		/// Invoke the specialized function WriteLog.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		/// <param name="timestampFormat">DateTime format used for the timestamp</param>
		/// <param name="optionalParameters">Anonymous type/value collection</param>
		public void Log(string message,
						bool timestamp,
						string timestampFormat,
						object optionalParameters)
		{
			WriteLog(FormatMessage(message,
								   timestamp,
								   timestampFormat),
					 ConvertToDictionary(optionalParameters));
		}

		/// <summary>
		/// Converts optional parameters to a more usable dictionary.
		/// </summary>
		/// <param name="optionalParameters">Anonymous type/value collection</param>
		/// <returns></returns>
		protected static IDictionary<string, object> ConvertToDictionary(object optionalParameters)
		{
			IDictionary<string, object> parameters;

			if (optionalParameters != null)
			{
				var properties = optionalParameters.GetType().GetProperties();

				parameters = properties.ToDictionary(prop => prop.Name,
													 prop => prop.GetValue(optionalParameters,
																		   null));
			}
			else
			{
				parameters = new Dictionary<string, object>();
			}
			return parameters;
		}

		/// <summary>
		/// Formats the final message with timestamp if needed.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="timestamp">Indicates wheter or not the logger adds a timestamp before the message</param>
		/// <param name="timestampFormat">DateTime format used for the timestamp</param>
		/// <returns></returns>
		protected static string FormatMessage(string message,
											  bool timestamp,
											  string timestampFormat)
		{
			var log = new StringBuilder();

			if (timestamp)
			{
				log.Append($"{DateTime.Now.ToString(timestampFormat)} : ");
			}

			log.Append(message);
			return log.ToString();
		}

		/// <summary>
		/// Writes the message to the log.
		/// </summary>
		/// <param name="message">Message to log</param>
		/// <param name="optionalParameters">Anonymous type/value collection</param>
		protected abstract void WriteLog(string message,
										 IDictionary<string, object> optionalParameters);
	}
}
