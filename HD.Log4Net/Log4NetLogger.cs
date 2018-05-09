using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;

namespace HD.Log4Net
{
    public partial class Log4NetLogger : ILogger
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _categoryName;
        private readonly bool _includeCategory;
        private readonly ILoggerRepository _loggerRepository;

        public Log4NetLogger(string name, Func<string, LogLevel, bool> filter, bool includeCategory, ILoggerRepository loggerRepository)
        {
            _categoryName = string.IsNullOrEmpty(name) ? nameof(Log4NetLogger) : name;
            _filter = filter;
            _includeCategory = includeCategory;
            _loggerRepository = loggerRepository;
        }


        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || _filter(_categoryName, logLevel);
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message) && exception != null)
            {
                message = exception.ToString();
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }
            if (_includeCategory)
                message = $"{_categoryName} : {message}";

            var log = LogManager.GetLogger(_loggerRepository.Name, _categoryName);
            switch (logLevel)
            {
                case LogLevel.None:
                case LogLevel.Trace:
                case LogLevel.Debug:
                    log.Debug(message);
                    break;
                case LogLevel.Information:
                    log.Info(message);
                    break;
                case LogLevel.Warning:
                    log.Warn(message);
                    break;
                case LogLevel.Error:
                    log.Error(message);
                    break;
                case LogLevel.Critical:
                    log.Fatal(message);
                    break;
            }
        }

        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}
