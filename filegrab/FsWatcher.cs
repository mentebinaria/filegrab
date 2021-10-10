using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace filegrab
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
                        SetWatchRecursion((watcherOpts & FsWatcherOpts.WatchSub) == FsWatcherOpts.WatchSub); ;
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
                SetWatchRecursion((watcherOpts & FsWatcherOpts.WatchSub) == FsWatcherOpts.WatchSub);
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

        public void SetWatchFilter(string filter)
        {
            if (filter == string.Empty)
            {
                return;
            }

            foreach (var watcher in fileSystemWatchers)
            {
                watcher.Filter = filter;
            }
        }

        private void SetWatchRecursion(bool opt)
        {
            foreach (var wather in fileSystemWatchers)
            {
                wather.IncludeSubdirectories = opt;
            }
        }

        public void AddWatchEvent(eventDel evnt)
        {
            foreach (var wather in fileSystemWatchers)
            {
                wather.Created += new FileSystemEventHandler(evnt);
            }
        }

        public void WatchStop()
        {
            foreach (var watcher in fileSystemWatchers)
            {
                watcher.EnableRaisingEvents = false;
            }
        }
    }
}
