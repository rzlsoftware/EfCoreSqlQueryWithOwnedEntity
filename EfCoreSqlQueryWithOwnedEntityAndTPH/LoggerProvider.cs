using Microsoft.Extensions.Logging;
using System;
using static System.Console;

namespace EfCoreSqlQueryWithOwnedEntityAndTPH
{
    public class LoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new ConsoleLogger();

        public void Dispose() { }

        private class ConsoleLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                if (eventId.Id == 20100)    // CommandExecuting
                    WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScope<TState>(TState state) => null;
        }
    }
}
