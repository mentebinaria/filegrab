/*
 * SPDX-License-Identifier: GPL-3.0-or-later
 *
 * Copyright (c) 2014, 2017, 2021 Fernando Mercês
 * Copyright (c) 2021 FallAngel1337
 *
 * This program is free software: you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * this program. If not, see <https://www.gnu.org/licenses/>.
 */

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
