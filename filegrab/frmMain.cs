using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileGrab
{
    public partial class frmMain : Form
    {
        FtpWebRequest ftp;

        private readonly FsWatcher fsWatcher = new();

        public frmMain()
        {
            InitializeComponent();
            fsWatcher.SetWatchBuffer(Convert.ToInt32(cbReadBufferSize.SelectedItem));
            fsWatcher.SetWatchFilter(txtRule.Text);
        }

        private void rbSpecific_CheckedChanged(object sender, EventArgs e)
        {
            txtPath.Enabled = btnPath.Enabled = chkRecursive.Enabled = rbSpecific.Checked;
        }

        private void changeControls(bool state)
        {
            groupFilesystem.Enabled = groupFtp.Enabled = chkHideWindow.Enabled = groupCopy.Enabled = state;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (chkRule.Checked && txtRule.Text == "")
            {
                txtRule.BackColor = System.Drawing.Color.LightPink;
                return;
            }

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
                if (rbAll.Checked)
                {
                    fsWatcher.WatchStart();
                    fsWatcher.AddWatchEvent(OnCreation);
                }
                else
                {
                    fsWatcher.WatchStart(FsWatcherOpts.WatchDir | FsWatcherOpts.WatchSub, txtPath.Text);
                    fsWatcher.AddWatchEvent(OnCreation);
                }
            }
            else
            {
                btnStart.Text = "Start";
                this.Text = "FileGrab";
                changeControls(true);
                fsWatcher.WatchStop();
                statusFileFound.Text = "";
            }
        }

        public void OnCreation(object source, FileSystemEventArgs e)
        {
            // we cannot monitor the copy destination directory
            if (txtCopyTo.Text != "" &&
                e.FullPath.StartsWith(txtCopyTo.Text, StringComparison.CurrentCultureIgnoreCase))
                return;

            if (chkRule.Checked && txtRule.Text != "")
            {
                if (chkRuleRegex.Checked)
                {
                    Regex regex = new(txtRule.Text, RegexOptions.IgnoreCase);

                    if (!(chkRuleNot.Checked ^ regex.IsMatch(Path.GetFileName(e.FullPath))))
                        return;
                }
            }

            statusFileFound.Text = e.FullPath;

            if (txtCopyTo.Text != "")
            {
                try
                {
                    if (!File.Exists(e.FullPath))
                        return;
                    String filename = e.Name.Substring(1 + e.Name.LastIndexOf('\\'));
                    String dstFile = Path.Combine(txtCopyTo.Text, filename);
                    File.Copy(e.FullPath, dstFile, chkWriteOverwrite.Checked);
                    File.SetAttributes(dstFile, FileAttributes.Normal); // remove read-only, hidden, etc

                    if (chkWritePreserveTimes.Checked)
                    {
                        File.SetCreationTime(dstFile, File.GetCreationTime(e.FullPath));
                        File.SetLastAccessTime(dstFile, File.GetLastAccessTime(e.FullPath));
                        File.SetLastWriteTime(dstFile, File.GetLastWriteTime(e.FullPath));
                    }

                }
                catch (IOException ex)
                {
                    if (!chkReadIgnoreErrors.Checked)
                        MessageBox.Show(ex.Message);
                }
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
                    //FIXME: This is a writing error
                    if (!chkReadIgnoreErrors.Checked)
                        MessageBox.Show(ex.ToString());
                }
            }
            return;
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            if (folderDlg.SelectedPath != "")
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
                MessageBox.Show("Connection succeeded!", "FTP test", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (folderDlg.SelectedPath != "")
                txtCopyTo.Text = folderDlg.SelectedPath;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            statusFileFound.Text = "";
            txtPath.Text = Directory.GetCurrentDirectory();
            folderDlg.SelectedPath = txtPath.Text;
            cbReadBufferSize.SelectedItem = cbReadBufferSize.Items[cbReadBufferSize.Items.Count / 2];
        }

        private void chkRule_CheckedChanged(object sender, EventArgs e)
        {
            txtRule.Enabled = chkRuleRegex.Enabled = chkRule.Checked;
            chkRuleNot.Enabled = chkRuleRegex.Checked & chkRule.Checked;
            if (!chkRule.Checked)
            {
                btnStart.Enabled = true;
                txtRule.BackColor = System.Drawing.Color.White;
            }
        }

        private void txtRule_TextChanged(object sender, EventArgs e)
        {
            bool invalid = false;

            if (chkRuleRegex.Checked)
            {
                try
                {
                    Regex regex = new Regex(txtRule.Text);
                }
                catch
                {
                    invalid = true;
                }
            }
            else
            {
                foreach (char p in Path.GetInvalidFileNameChars())
                {
                    if (p == '*' || p == '?' || p == '\\')
                        continue;

                    if (txtRule.Text.IndexOf(p) != -1)
                        invalid = true;
                }
            }

            if (txtRule.Text == "")
                invalid = true;

            txtRule.BackColor = invalid ? System.Drawing.Color.LightPink : System.Drawing.Color.White;
            btnStart.Enabled = !invalid;
        }

        private void chkRuleRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRuleRegex.Checked)
                txtRule_TextChanged(sender, e);
            else
                txtRule.BackColor = System.Drawing.Color.White;
            chkRuleNot.Enabled = chkRuleRegex.Checked;
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show(
        //        "You can set the format using the following variables:\n\n" +
        //        "%{name}\t- the file name without extension\n" +
        //        "%{ext}\t- the file extension\n" +
        //        "%{md5}\t- the file MD5 hash\n" +
        //        "%{sha1}\t- the file SHA1 hash\n" +
        //        "%{sha256}\t- the file SHA-256 hash\n"
        //    );
        //}

        private void linkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/p/FileGrab/wiki/Home/");
        }

        private void ttInfo_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
