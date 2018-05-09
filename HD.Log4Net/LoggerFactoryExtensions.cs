using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HD.Log4Net
{
    public static class LoggerFactoryExtensions
    {
        #region LoggerFactoryExtension
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            return AddLog4Net(factory, LogLevel.Trace);
        }
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, LogLevel minLevel, string logDir = null, string layoutPattern = null, string datePattern = null, bool includeCategory = true, string configPath = null)
        {
            return AddLog4Net(
               factory,
               (_, logLevel) => logLevel >= minLevel,
               logDir,
               layoutPattern,
               datePattern,
               includeCategory,
               configPath);
        }
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, string logDir = null, string layoutPattern = null, string datePattern = null, bool includeCategory = true, string configPath = null)
        {
            factory.AddProvider(new Log4NetLoggerProvider(filter, logDir, layoutPattern, datePattern, includeCategory, configPath));
            return factory;
        }
        #endregion

        #region LoggerBuilderExtension
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
        {
            return AddLog4Net(builder, LogLevel.Trace);
        }
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, LogLevel minLevel, string logDir = null, string layoutPattern = null, string datePattern = null, bool includeCategory = true, string configPath = null)
        {
            return AddLog4Net(
               builder,
               (_, logLevel) => logLevel >= minLevel,
               logDir,
               layoutPattern,
               datePattern,
               includeCategory,
               configPath);
        }
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, Func<string, LogLevel, bool> filter, string logDir = null, string layoutPattern = null, string datePattern = null, bool includeCategory = true, string configPath = null)
        {
            builder.Services.AddSingleton(typeof(ILoggerProvider), new Log4NetLoggerProvider(filter, logDir, layoutPattern, datePattern, includeCategory, configPath));
            return builder;
        }
        #endregion
    }
}