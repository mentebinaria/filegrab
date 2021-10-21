using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileGrab
{
	public class Logging
	{
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Setup(string path)
        {
            try
            {
                var config = new NLog.Config.LoggingConfiguration();

                var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"{path}/logs.txt" };

                config.AddRule(LogLevel.Info, LogLevel.Info, logfile);

                NLog.LogManager.Configuration = config;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Could not setup the log configuration!");
            }
        }

        public static void Log(string message)
        {
            try
            {
                Logger.Info(message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Some unexpected error occurred");
            }
        }
    }
}
