using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace App
{
    public class FileLogger : ILogger
    {
        private string file;
        private static object _lock = new object();

        public FileLogger (string file)
        {
            this.file = file;
        }

        public bool IsEnabled (LogLevel level)
        {
            return true;
        }

        public IDisposable BeginScope<TState> (TState state)
        {
            return null;
        }

        public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState,Exception,string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    string record = String.Format ("[{0}][{1}] -> {2}{3}", DateTime.Now, Thread.CurrentThread.ManagedThreadId, formatter(state, exception), Environment.NewLine);
                    File.AppendAllText(file, record);
                }
            }
        }
    }
}