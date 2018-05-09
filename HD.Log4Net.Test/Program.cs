using Microsoft.Extensions.Logging;
using System;

namespace HD.Log4Net.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerFac1 = new LoggerFactory();
            var log1 = loggerFac1.AddLog4Net().CreateLogger("log1");
            log1.LogInformation("test1");

            var loggerFac2 = new LoggerFactory();
            var log2 = loggerFac2.AddLog4Net(LogLevel.Trace, null, null, null, true, "log4net.config").CreateLogger("log2");
            log2.LogInformation("test1");

            Console.WriteLine("Hello World!");
        }
    }
}
