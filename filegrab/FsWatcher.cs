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
    public delegate void renameDel(object source, RenamedEventArgs e);
    public delegate void errorDel(object source, ErrorEventArgs e);

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

        public eventDel? OnCreated { get; set; }
        public eventDel? OnChanged { get; set; }
        public eventDel? OnDeleted { get; set; }
        public eventDel? OnRenamed { get; set; }
        public errorDel? OnError { get; set; }

        public void WatchStart(FsWatcherOpts watcherOpts = FsWatcherOpts.WatchAll, string? path = null)
        {
            if (watcherOpts == FsWatcherOpts.WatchAll)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.DriveType == DriveType.Fixed)
                    {
                        FileSystemWatcher watcher = new();

                        watcher.Path = d.RootDirectory.ToString();
                        watcher.NotifyFilter = NotifyFilters.Attributes
                                           | NotifyFilters.CreationTime
                                           | NotifyFilters.DirectoryName
                                           | NotifyFilters.FileName
                                           | NotifyFilters.LastAccess
                                           | NotifyFilters.LastWrite
                                           | NotifyFilters.Security
                                           | NotifyFilters.Size;
                        watcher.EnableRaisingEvents = true;
                        fileSystemWatchers.Add(watcher);
                    }
                }
            }
            else if ((watcherOpts & FsWatcherOpts.WatchDir) == FsWatcherOpts.WatchDir)
            {
                FileSystemWatcher watch = new();

                watch.Path = string.IsNullOrEmpty(path) ? throw new Exception("Invalid PATH") : path;
                watch.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;
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

        public void WatchStop()
        {
            for (int i=0; i < fileSystemWatchers.Count; i++)
            {
                fileSystemWatchers[i].EnableRaisingEvents = false;
                fileSystemWatchers.Remove(fileSystemWatchers[i]);
            }
        }

        public void AddHandler(eventDel? onchanged = null, eventDel? oncreated = null,
                               eventDel? ondeleted = null, renameDel? onrenamed = null, errorDel? onerror = null)
        {
            foreach (var watcher in fileSystemWatchers)
            {
                watcher.Changed += new FileSystemEventHandler(onchanged ?? onChanged);
                watcher.Created += new FileSystemEventHandler(oncreated ?? onCreated);
                watcher.Deleted += new FileSystemEventHandler(ondeleted ?? onDeleted);
                watcher.Renamed += new RenamedEventHandler(onrenamed ?? onRenamed);
                watcher.Error += new ErrorEventHandler(onerror ?? onError);
            }
        }


        private static void onChanged(object sender, FileSystemEventArgs e)
        {
            // Empty Handler
        }

        private static void onCreated(object sender, FileSystemEventArgs e)
        {
            // Empty Handler
        }

        private static void onDeleted(object sender, FileSystemEventArgs e) 
        {
            // Empty Handler
        }

        private static void onRenamed(object sender, RenamedEventArgs e)
        {
            // Empty Handler
        }

        private static void onError(object sender, ErrorEventArgs e)
        {
            // Empty Handler
        }

    } 
}
