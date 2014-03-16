using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Permissions;
using System.Net;

namespace filegrab
{
    public partial class Form1 : Form
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        FtpWebRequest ftp;

        public Form1()
        {
            InitializeComponent();
        }

        private void rbSpecific_CheckedChanged(object sender, EventArgs e)
        {
                txtPath.Enabled = btnPath.Enabled = rbSpecific.Checked;
        }

        public void watchStart()
        {
            watcher.Path = (rbAll.Checked) ? "C:\\" : txtPath.Text;
            watcher.IncludeSubdirectories = true;
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
            groupBox1.Enabled = groupBox2.Enabled = chkHideWindow.Enabled = state;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (chkHideWindow.Checked)
                Form1.ActiveForm.Visible = false;

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
            }
        }

        public void OnCreation(object source, FileSystemEventArgs e)
        {
            if (!validateFtpFields())
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
            folderBrowserDialog1.ShowDialog();
            txtPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chkFtpAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            txtFtpUser.Enabled = txtFtpPassword.Enabled = !chkFtpAnonymous.Checked;
        }

        private void txtFtpHost_TextChanged(object sender, EventArgs e)
        {
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
            ftp = (FtpWebRequest)WebRequest.Create("ftp://" + txtFtpHost.Text + ":" + txtFtpPort.Value);
            ftp.Method = WebRequestMethods.Ftp.ListDirectory;

            if (!validateFtpFields())
                return;

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




    }
}
