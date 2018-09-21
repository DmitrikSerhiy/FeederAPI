using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freeder.BLL
{
    public class LoggerProvider : ILoggerProvider
    {
        private string path;
        public LoggerProvider(string Path)
        {
            path = Path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(path);
        }

        public void Dispose()
        { 
        }
    }
}
