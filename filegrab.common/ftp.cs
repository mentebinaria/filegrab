using System;
using System.IO;
using System.Net;

namespace FileGrab.Common
{
    /// <summary>
    /// FTP Service
    /// </summary>
    public class Ftp
    {
        #region +Events

        public delegate void Uploaded(string filename);

        public delegate void UploadedError(WebException exception);

        public delegate void PropertyError(Exception exception);

        public event Uploaded OnUploaded;

        public event UploadedError OnUploadedError;

        public event PropertyError OnPropertyError;

        #endregion

        #region +Properties

        private NetworkCredential Credentials { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public bool Anonymous { get; set; }

        public string CommandMKD { get; set; }
        public string CommandSTO { get; set; }

        #endregion

        #region +Ctor

        /// <summary>
        /// Constructor for Anonymous
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        public Ftp(string hostname, int port)
        {
            this.Credentials = new NetworkCredential("anonymous", "nopass");
            this.Hostname = hostname;
            this.Port = port;
            this.Anonymous = true;
        }

        /// <summary>
        /// Constructor for Authentication
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Ftp(string hostname, int port, string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    throw new Exception("FTP username missing.");

                if (string.IsNullOrEmpty(password))
                    throw new Exception("FTP password missing.");
            }
            catch (Exception error)
            {
                if (this.OnPropertyError != null)
                    this.OnPropertyError.Invoke(error);
            }

            this.Credentials = new NetworkCredential(username, password);
            this.Hostname = hostname;
            this.Port = port;
            this.Anonymous = false;

            this.Username = username;
            this.Password = password;
        }

        #endregion

        #region +Methods

        /// <summary>
        /// Make URI (Remote Folder)
        /// </summary>
        /// <param name="destinationFile"></param>
        /// <returns></returns>
        private string MakeDirectory(string destinationFile)
        {
            return destinationFile.Split(':')[0].Insert(0, "DRIVE_") + "/" + /* Logical Driver */
                   destinationFile.Substring(destinationFile.IndexOf(":\\") + 2).Replace(@"\" + Path.GetFileName(destinationFile), string.Empty).Replace(@"\", "/"); /* Uri Network Format */
        }

        /// <summary>
        /// Make Upload URI
        /// </summary>
        /// <param name="destinationFile"></param>
        /// <returns></returns>
        private string MakeUpload(string destinationFile)
        {
            return string.Format("ftp://{0}:{1}/{2}", this.Hostname, this.Port, destinationFile.Split(':')[0].Insert(0, "DRIVE_") + destinationFile.Split(':')[1]);
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="destinationFile"></param>
        public void Upload(string destinationFile)
        {
            WebRequest webRequestMakeDirectory = null;
            WebRequest webRequestUploadFilename = null;

            try
            {
                if (string.IsNullOrEmpty(this.Hostname))
                    throw new Exception("FTP hostname missing.");

                if (this.Port <= 0)
                    throw new Exception("FTP port invalid or missing.");
            }
            catch (Exception error)
            {
                if (this.OnPropertyError != null)
                    this.OnPropertyError.Invoke(error);
            }

            try
            {
                /* The easy way to MKDIR and STOR without components or write from scratch (.NET v2.0) */
                string[] wkfPts;
                string wkf, wkfPortion = "";

                wkf = this.MakeDirectory(destinationFile);
                wkfPts = wkf.Split('/');

                /* TODO: Make a better solution to do it! */
                for (int i = 0; i < wkfPts.Length; i++)
                {
                    wkfPortion += wkfPts[i] + "/";
                    webRequestMakeDirectory = FtpWebRequest.Create(string.Format("ftp://{0}:{1}/{2}", this.Hostname, this.Port, wkfPortion));
                    webRequestMakeDirectory.Credentials = this.Credentials;
                    webRequestMakeDirectory.Method = WebRequestMethods.Ftp.MakeDirectory;
                    try { webRequestMakeDirectory.GetResponse(); }
                    catch { /* already exists folder. */ }
                }

                /* Upload File */
                webRequestUploadFilename = FtpWebRequest.Create(this.MakeUpload(destinationFile));
                webRequestUploadFilename.Credentials = this.Credentials;
                webRequestUploadFilename.Method = WebRequestMethods.Ftp.UploadFile;
                webRequestUploadFilename.GetResponse();

                if (this.OnUploaded != null)
                    this.OnUploaded.Invoke(destinationFile);
            }
            catch (WebException error)
            {
                if (this.OnUploadedError != null)
                    this.OnUploadedError.Invoke(error);
            }
        }

        /// <summary>
        /// Test FTP Connection
        /// </summary>
        /// <returns></returns>
        public bool Test()
        {
            WebRequest webRequestTest = FtpWebRequest.Create(string.Format("ftp://{0}:{1}", this.Hostname, this.Port));
            webRequestTest.Credentials = this.Credentials;
            webRequestTest.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
            try
            {
                webRequestTest.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
