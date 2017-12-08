using System;
using System.Collections.Generic;
using System.IO;

namespace FileGrab.Common
{
    /// <summary>
    /// Monitor Service
    /// </summary>
    public class Monitor
    {
        #region +Events

        public delegate void Created(string filename);

        public event Created OnCreated;

        #endregion

        #region +Properties

        private IDictionary<String, FileSystemWatcher> Watchers { get; set; }
        private IList<String> IgnoreDirectories { get; set; }

        public Copy Copy { get; set; }
        public Rule Rule { get; set; }
        public Ftp Ftp { get; set; }

        #endregion

        #region +Ctor

        public Monitor()
        {
            this.Watchers = new Dictionary<String, FileSystemWatcher>();
            this.IgnoreDirectories = new List<String>();
        }

        #endregion

        #region +Methods

        public void Watcher(int bufferSize, bool isRecursive = true)
        {
            foreach (var logicalDriver in DriveInfo.GetDrives())
                if (logicalDriver.DriveType == DriveType.Fixed || logicalDriver.DriveType == DriveType.Removable)
                    this.Watcher(logicalDriver.RootDirectory.FullName, bufferSize, isRecursive);

        }

        public void Watcher(string path, int bufferSize, bool isRecursive = true)
        {
            FileSystemWatcher instance = null;
            
            if (this.Watchers.ContainsKey(path))
                return;

            instance = new FileSystemWatcher(path);
            instance.InternalBufferSize = bufferSize * 1024;
            instance.IncludeSubdirectories = isRecursive;
            instance.NotifyFilter = NotifyFilters.FileName;

            if (this.Rule != null && !this.Rule.IsRegularExpression)
                instance.Filter = this.Rule.Expression;

            if (this.Copy != null && this.IgnoreDirectories.IndexOf(this.Copy.Path) < 0)
                this.IgnoreDirectories.Add(this.Copy.Path);

            // instance.EnableRaisingEvents = true;

            instance.Created += delegate (object source, FileSystemEventArgs e)
            {
                bool isWatchFolder = false;

                foreach (var ignoreFolder in this.IgnoreDirectories)
                    if ((isWatchFolder = e.FullPath.Contains(ignoreFolder)))
                        break;

                if (isWatchFolder)
                    return;

                if (this.Rule != null && this.Rule.IsRegularExpression && this.Rule.IsMatching(Path.GetFileName(e.FullPath)))
                    return;

                /* On Created Notification */
                if (this.OnCreated != null)
                    this.OnCreated.Invoke(e.FullPath);

                /* Shadow Copy */
                if (this.Copy != null)
                {
                    this.Copy.Update(e);
                    this.Copy.CreateDirectoryIfNotExists();
                    this.Copy.MakeCopy();

                    /* FTP Services */
                    if (this.Ftp != null)
                        this.Ftp.Upload(this.Copy.WorkingFileName);
                }

            };

            this.Watchers.Add(path, instance);
        }

        /// <summary>
        /// Enable Shadow Copy
        /// </summary>
        /// <param name="path"></param>
        /// <param name="canOverwrite"></param>
        /// <param name="canIgnoreErrors"></param>
        /// <param name="canPreserveTimestamps"></param>
        /// <param name="canCreateDirectoryTree"></param>
        public void Capture(string path, bool canOverwrite = true, bool canIgnoreErrors = true, bool canPreserveTimestamps = true, bool canCreateDirectoryTree = true)
        {
            this.Copy = new Copy(path, canOverwrite, canIgnoreErrors, canPreserveTimestamps, canCreateDirectoryTree);
        }

        /// <summary>
        /// Enable Filter Or Regular Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="isRegularExpression"></param>
        public void Matching(string expression, bool isRegularExpression = false)
        {
            this.Rule = new Rule(expression, isRegularExpression);
        }

        /// <summary>
        /// Enable FTP Services (Anonymous)
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        public void FtpService(string hostname, int port = 21)
        {
            this.Ftp = new Ftp(hostname, port);
        }

        /// <summary>
        /// Enable FTP Services (Authentication)
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="port"></param>
        public void FtpService(string hostname, string username, string password, int port = 21)
        {
            this.Ftp = new Ftp(hostname, port, username, password);
        }

        /// <summary>
        /// Ignore Directories (Generally used to avoid recursion)
        /// </summary>
        /// <param name="directory"></param>
        public void IgnoreDirectory(string directory)
        {
            this.IgnoreDirectories.Add(directory);
        }

        /// <summary>
        /// Start Watcher(s)
        /// </summary>
        public void Start()
        {
            foreach (var watcher in this.Watchers)
                watcher.Value.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stop Watcher(s)
        /// </summary>
        public void Stop()
        {
            foreach (var watcher in this.Watchers)
                watcher.Value.EnableRaisingEvents = false;
        }

        /// <summary>
        /// Clear Ignore Directories And Watcher(s)
        /// </summary>
        public void Reset()
        {
            this.IgnoreDirectories.Clear();
            this.Watchers.Clear();
        }

        #endregion
    }
}
