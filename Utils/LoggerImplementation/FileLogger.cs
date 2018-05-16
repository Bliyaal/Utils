using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils
{
    /// <summary>
    /// Used to log a message to a file.
    /// </summary>
    /// <remarks>
    /// This logger has no optional parameters.
    /// </remarks>
    public class FileLogger : Logger
    {
        private readonly StreamWriter _stream;
        private readonly string _filePath;

        public string FilePath => _stream != null ? ((FileStream)_stream.BaseStream).Name : _filePath;

        public FileLogger(string name) : this(name, string.Empty) { }

        public FileLogger(string name,
                          string filepath) : this(name, filepath, false) { }

        public FileLogger(string name,
                          string filepath,
                          bool timestamp) : this(name, filepath, timestamp, string.Empty) { }

        public FileLogger(string name,
                          string filepath,
                          bool timestamp,
                          string timestampFormat) : base(name,
                                                         timestamp,
                                                         timestampFormat) => _filePath = filepath;

        public FileLogger(string name,
                          StreamWriter stream) : base(name) => _stream = stream ?? throw new ArgumentNullException(nameof(stream));

        protected override void WriteLog(string message,
                                         IDictionary<string, object> optionalParameters)
        {
            if (_stream != null)
            {
                _stream.WriteLine(message);
            }
            else
            {
                using (var stream = new StreamWriter(_filePath,
                                                     true,
                                                     Encoding.UTF8))
                {
                    stream.WriteLine(message);
                }
            }
        }
    }
}
