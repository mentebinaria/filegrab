using System.IO;

namespace FileGrab.Common
{
    /// <summary>
    /// Shadow Copy Service
    /// </summary>
    public class Copy
    {
        #region +Events

        public delegate void DirectoryCreated(string directory);
        public delegate void FileCopied(string filename);

        public delegate void DirectoryCreatedError(IOException exception);
        public delegate void FileCopiedError(IOException error);

        public event DirectoryCreated OnDirectoryCreated;
        public event FileCopied OnFileCopied;

        public event DirectoryCreatedError OnDirectoryCreatedError;
        public event FileCopiedError OnFileCopiedError;

        #endregion

        #region +Properties

        /* Common Flags */
        public bool CanOverwrite { get; set; }
        public bool CanIgnoreErrors { get; set; }
        public bool CanCreateDirectoryTree { get; set; }
        public bool CanPreserveTimestamps { get; set; }

        /* Files, Path, Working Folders */
        public string Format { get; set; }
        public string Path { get; set; }
        public string FileName { get; private set; }
        public string WorkingPath { get; private set; }
        public string WorkingFileName { get; private set; }

        public FileSystemEventArgs EventArguments { get; private set; }

        #endregion

        #region +Ctor

        public Copy() {}

        public Copy(string path, bool canOverwrite = true, bool canIgnoreErrors = true, bool canPreserveTimestamps = true, bool canCreateDirectoryTree = true)
        {
            this.CanOverwrite = canOverwrite;
            this.CanIgnoreErrors = canIgnoreErrors;
            this.CanPreserveTimestamps = canPreserveTimestamps;
            this.CanCreateDirectoryTree = canCreateDirectoryTree;
            this.Path = path;
        }

        #endregion

        #region +Methods

        /// <summary>
        /// Update Shadow Copy Event
        /// </summary>
        /// <param name="fileSystemEventArgs"></param>
        public void Update(FileSystemEventArgs fileSystemEventArgs)
        {
            this.EventArguments = fileSystemEventArgs;
            this.FileName = System.IO.Path.GetFileName(this.EventArguments.FullPath);
            this.WorkingPath = (this.Path + @"\" + this.EventArguments.Name).Replace(this.FileName, string.Empty);
            this.WorkingFileName = this.WorkingPath + this.FileName;
        }

        /// <summary>
        /// Create Shadow Copy Directory
        /// </summary>
        public void CreateDirectoryIfNotExists()
        {
            try
            {
                if (!Directory.Exists(this.WorkingPath))
                    Directory.CreateDirectory(this.WorkingPath);

                if (this.OnDirectoryCreated != null)
                    this.OnDirectoryCreated.Invoke(this.WorkingPath);
            }
            catch (IOException error)
            {
                if (this.OnDirectoryCreatedError != null)
                    this.OnDirectoryCreatedError.Invoke(error);
            }

        }

        /// <summary>
        /// Make Copy
        /// </summary>
        public void MakeCopy()
        {
            try
            {
                File.Copy(this.EventArguments.FullPath, this.WorkingFileName, this.CanOverwrite);
                File.SetAttributes(this.WorkingFileName, FileAttributes.Normal);

                if (this.CanPreserveTimestamps)
                {
                    File.SetCreationTime(this.WorkingFileName, File.GetCreationTime(this.EventArguments.FullPath));
                    File.SetLastAccessTime(this.WorkingFileName, File.GetLastAccessTime(this.EventArguments.FullPath));
                    File.SetLastWriteTime(this.WorkingFileName, File.GetLastWriteTime(this.EventArguments.FullPath));
                }

                if (this.OnFileCopied != null)
                    this.OnFileCopied.Invoke(this.WorkingFileName);
            }
            catch (IOException error)
            {
                if (this.OnFileCopiedError != null)
                    this.OnFileCopiedError.Invoke(error);
            }
        }

        #endregion
    }
}
