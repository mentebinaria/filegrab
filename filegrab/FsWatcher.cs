using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace FileGrab
{
    public delegate void eventDel(object source, FileSystemEventArgs e);

    [Flags]
    public enum FsWatcherOpts
    {
        // Watch the all devices (C:\, D:\, etc)
        WatchAll = 0,
        // Watch a single directory
        WatchDir = 1,
        // Watch sub directories
        WatchSub = 2,
    }

    public class FsWatcher
    {
        private List<FileSystemWatcher> fileSystemWatchers = new();

        public void WatchStart(FsWatcherOpts watcherOpts = FsWatcherOpts.WatchAll, string? path = null)
        {
            if (watcherOpts == FsWatcherOpts.WatchAll)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.DriveType == DriveType.Fixed)
                    {
                        FileSystemWatcher watch = new();

                        watch.Path = d.RootDirectory.ToString();
						watch.NotifyFilter = NotifyFilters.FileName;
                        watch.EnableRaisingEvents = true;
                        fileSystemWatchers.Add(watch);
                    }
                }
            }
            else if ((watcherOpts & FsWatcherOpts.WatchDir) == FsWatcherOpts.WatchDir)
            {
                FileSystemWatcher watch = new();

                watch.Path = string.IsNullOrEmpty(path) ? throw new Exception("Invalid PATH") : path;
				watch.NotifyFilter = NotifyFilters.FileName;
                watch.EnableRaisingEvents = true;
                fileSystemWatchers.Add(watch);
            }
        }

        public void SetWatchBuffer(int buffer)
        {
            foreach (var watcher in fileSystemWatchers)
            {
                watcher.InternalBufferSize = 1024 * buffer;
            }
        }

        public void SetWatchFilter(string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return;
            }

            foreach (var watcher in fileSystemWatchers)
            {
                watcher.Filter = filter;
            }
        }

		public void SetWatchRecursion(bool opt)
		{
			foreach (var watcher in fileSystemWatchers)
			{
                watcher.IncludeSubdirectories = opt;
			}
		}

		public void AddWatchEvent(eventDel evnt)
        {
            foreach (var watcher in fileSystemWatchers)
            {
                watcher.Created += new FileSystemEventHandler(evnt);
            }
        }

        public void WatchStop()
        {
            for (int i=0; i < fileSystemWatchers.Count; i++)
            {
                fileSystemWatchers[i].EnableRaisingEvents = false;
                fileSystemWatchers.Remove(fileSystemWatchers[i]);
            }
        }
    }
}
