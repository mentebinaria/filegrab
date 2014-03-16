using System;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace filegrab
{
    public partial class frmMain : Form
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        FtpWebRequest ftp;

        public frmMain()
        {
            InitializeComponent();
        }

        private void rbSpecific_CheckedChanged(object sender, EventArgs e)
        {
                txtPath.Enabled = btnPath.Enabled = chkRecursive.Enabled = rbSpecific.Checked;
        }

        public void watchStart()
        {
            watcher.Path = (rbAll.Checked) ? "C:\\" : txtPath.Text;
            watcher.IncludeSubdirectories = rbAll.Checked | chkRecursive.Enabled;
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Created += new FileSystemEventHandler(OnCreation);
            watcher.EnableRaisingEvents = true;
        }

        public void watchStop()
        {
            watcher.EnableRaisingEvents = false;
        }

        private void changeControls(bool state)
        {
           groupFilesystem.Enabled = groupFtp.Enabled = chkHideWindow.Enabled = groupCopy.Enabled = state;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (chkHideWindow.Checked)
            {
                frmMain.ActiveForm.ShowInTaskbar = false;
                frmMain.ActiveForm.Visible = false;
            }

            if (btnStart.Text.Equals("Start"))
            {
                btnStart.Text = "Stop";
                this.Text += " (running)";
                changeControls(false);
                watchStart();
            }
            else
            {
                btnStart.Text = "Start";
                this.Text = "FileGrab";
                changeControls(true);
                watchStop();
                statusFileFound.Text = "";
            }
        }

        public void OnCreation(object source, FileSystemEventArgs e)
        {
            statusFileFound.Text = e.FullPath;

            if (txtCopyTo.Text != "")
            {
                try
                {
                    File.Copy(e.FullPath, Path.Combine(txtCopyTo.Text, e.Name));
                }
                catch (IOException ex) { MessageBox.Show(ex.Message); }
            }

            if (txtFtpHost.Text == "")
                return;

            ftp = (FtpWebRequest)WebRequest.Create("ftp://" + txtFtpHost.Text + ":" + txtFtpPort.Value + "/" + e.Name);
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            setFtpCredentials();
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            using (FileStream fs = File.OpenRead(e.FullPath))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                try
                {
                    Stream requestStream = ftp.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    requestStream.Flush();
                    statusFileFound.Text = e.FullPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return;
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            txtPath.Text = folderDlg.SelectedPath;
        }

        private void chkFtpAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            txtFtpUser.Enabled = txtFtpPassword.Enabled = !chkFtpAnonymous.Checked;
        }

        private bool validateFtpFields()
        {
            if (txtFtpHost.Text.Length < 1)
            {
                MessageBox.Show("FTP host missing", "FileGrab", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFtpHost.Focus();
                return false;
            }

            if (chkFtpAnonymous.Checked)
                return true;

            if (txtFtpUser.Text.Length < 1)
            {
                MessageBox.Show("FTP user missing", "FileGrab", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFtpUser.Focus();
                return false;
            }
            else if (txtFtpPassword.Text.Length < 1)
            {
                MessageBox.Show("FTP password missing", "FileGrab", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFtpPassword.Focus();
                return false;
            }
            return true;
        }

        private void setFtpCredentials()
        {
            if (chkFtpAnonymous.Checked)
                ftp.Credentials = new NetworkCredential("anonymous", "anonymous@anonymous.net");
            else
                ftp.Credentials = new NetworkCredential(txtFtpUser.Text, txtFtpPassword.Text);
        }

        private void btnFtpTest_Click(object sender, EventArgs e)
        {
            if (!validateFtpFields())
                return;

            ftp = (FtpWebRequest)WebRequest.Create("ftp://" + txtFtpHost.Text + ":" + txtFtpPort.Value);
            ftp.Method = WebRequestMethods.Ftp.ListDirectory;

            setFtpCredentials();

            try
            {
                ftp.GetResponse();
                MessageBox.Show("Connection success!", "FTP test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                MessageBox.Show("Connection failed!\n\nDetails:\n" + ex.ToString(),
                    "FTP test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopyToBrowse_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            txtCopyTo.Text = folderDlg.SelectedPath;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            statusFileFound.Text = "";
        }
    }
}
