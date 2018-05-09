using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;
using System.Reflection;

namespace HD.Log4Net
{
    public class Log4NetHelper
    {
        public static ILoggerRepository Init(string logDir = null, string layoutPattern = null, string datePattern = null, string configPath = null)
        {
            var repositoryName = Guid.NewGuid().ToString();
            LogManager.CreateRepository(repositoryName);
            var repository = LogManager.GetRepository(repositoryName);
            if (!string.IsNullOrWhiteSpace(configPath) && File.Exists(configPath))
            {
                XmlConfigurator.Configure(repository, new FileInfo(configPath));
            }
            else
            {
                XmlConfigurator.Configure(repository, GetConfigStream("log4net.config", logDir, layoutPattern, datePattern));
            }
            return repository;
        }

        /// <summary>
        /// 读取嵌入的资源
        /// </summary>
        private static Stream GetConfigStream(string fileName, string logDir = null, string layoutPattern = null, string datePattern = null)
        {

            var asm = Assembly.GetExecutingAssembly();
            var projectName = asm.GetName().Name.ToString();
            var stream = asm.GetManifestResourceStream(projectName + "." + fileName);
            if (string.IsNullOrWhiteSpace(logDir) && 
                string.IsNullOrWhiteSpace(layoutPattern) && 
                string.IsNullOrWhiteSpace(datePattern))
            {
                return stream;
            }
            logDir = logDir?.Trim();
            layoutPattern = layoutPattern?.Trim();
            datePattern = datePattern?.Trim();

            using (stream)
            using (var sr = new StreamReader(stream))
            {
                var config = sr.ReadToEnd();

                config = !string.IsNullOrWhiteSpace(logDir) ? config.Replace("logs\\", logDir.EndsWith("\\") ? logDir : logDir + "\\") : config;
                config = !string.IsNullOrWhiteSpace(layoutPattern) ? config.Replace(@"%newline%date [%thread] %-5level %logger [%property{NDC}] - %message%newline", layoutPattern) : config;
                config = !string.IsNullOrWhiteSpace(datePattern) ? config.Replace(@"yyyy-MM-dd\\'error.log'", string.Format(datePattern, "error")) : config;
                config = !string.IsNullOrWhiteSpace(datePattern) ? config.Replace(@"yyyy-MM-dd\\'warn.log'", string.Format(datePattern, "warn")) : config;
                config = !string.IsNullOrWhiteSpace(datePattern) ? config.Replace(@"yyyy-MM-dd\\'trace.log'", string.Format(datePattern, "trace")) : config;

                return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config));
            }
        }
    }
}
