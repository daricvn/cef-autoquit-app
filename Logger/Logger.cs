using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService
{
    public static class Logger
    {
        private static bool _loggerInit = false;
        public static string FileName = "logs/error.txt";
        private static void Initializer()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("ErrorLog") { FileName = FileName };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
            _loggerInit = true;
        }

        public static NLog.Logger Instance
        {
            get
            {
                if (!_loggerInit)
                    Initializer();
                return NLog.LogManager.GetCurrentClassLogger();
            }
        }

        public static void Log(string message)
        {
            Instance.Info(message);
        }
        public static void Warn(string message)
        {
            Instance.Warn(message);
        }
        public static void Warn(string message, Exception ex)
        {
            Instance.Warn(ex, message);
        }
        public static void Error(string message)
        {
            Instance.Error(message);
        }
        public static void Error(string message, Exception ex)
        {
            Instance.Error(ex, message);
        }
    }
}
