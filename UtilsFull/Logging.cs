using System;
using System.Collections.Generic;
using System.Linq;
using Utils.LoggerImplementation;

namespace Utils
{
    public static class Logging
    {
        private static IList<Logger> _loggers = new List<Logger>();

        #region " Public "
        public static Logger Get(string name)
        {
            if (_loggers == null)
            {
                _loggers = new List<Logger>();
            }
            return _loggers.FirstOrDefault(l => l.Name == name);
        }

        public static void Add(Logger logger)
        {
            Exception validation = Validate(logger);
            if (validation != null)
            {
                throw validation;
            }
            _loggers.Add(logger);
        }

        public static bool Contains(string name)
        {
            return (Get(name) != null);
        }

        public static bool Contains(Logger logger)
        {
            return _loggers.Contains(logger);
        }

        public static void Log(string message)
        {
            Log(message,
                null);
        }

        public static void Log(string message,
                               object optionalParameters)
        {
            _loggers.ToList().ForEach(l => l.Log(message,
                                                 optionalParameters));
        }

        public static void Log(string message,
                               bool timestamp)
        {
            Log(message,
                timestamp,
                null);
        }

        public static void Log(string message,
                               bool timestamp,
                               object optionalParameters)
        {
            _loggers.ToList().ForEach(l => l.Log(message,
                                                 timestamp,
                                                 optionalParameters));
        }

        public static void Log(string message,
                               bool timestamp,
                               string timestampFormat)
        {
            Log(message,
                timestamp,
                timestampFormat,
                null);
        }

        public static void Log(string message,
                               bool timestamp,
                               string timestampFormat,
                               object optionalParameters)
        {
            _loggers.ToList().ForEach(l => l.Log(message,
                                                 timestamp,
                                                 timestampFormat,
                                                 optionalParameters));
        }

        public static void Remove(string name)
        {
            if (!Contains(name))
            {
                throw new ArgumentException(@"Logger doesn't exist",
                                            nameof(name));
            }
            Remove(Get(name));
        }

        public static void Remove(Logger logger)
        {
            if (!Contains(logger))
            {
                throw new ArgumentException(@"Logger doesn't exist",
                                            nameof(logger));
            }
            _loggers.Remove(logger);
        }
        #endregion

        #region " Private "
        private static Exception Validate(Logger logger)
        {
            if (logger == null)
            {
                return new ArgumentNullException(nameof(logger));
            }
            if (string.IsNullOrWhiteSpace(logger.Name))
            {
                return new ArgumentException(@"Logger's name must not be empty",
                                             nameof(logger));
            }
            if (Get(logger.Name) != null)
            {
                return new ArgumentException(@"A logger with the same name already exists.",
                                             nameof(logger));
            }
            return null;
        }
        #endregion
    }
}
