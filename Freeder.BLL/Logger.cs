using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Freeder.BLL
{

    public class Logger : ILogger
    {
        private string filePath;
        static object locker = new object();
        public Logger(string path)
        {
            filePath = path;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                try
                {
                    Monitor.Enter(locker);
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
                finally
                {
                    Monitor.Exit(locker);
                }
            }
        }

    }
}
