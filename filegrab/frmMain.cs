﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileGrab
{
    public partial class frmMain : Form
    {
        private readonly FsWatcher fsWatcher = new();
        private FtpUpload ftpUpload;

        public readonly string ProgramName = "FileGrab";
        public bool IsRunning {get; private set;} = true;

        public frmMain()
        {
            InitializeComponent();
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
                MessageBox.Show("Regex rule can't be blank");
                return;
            }

            if (chkHideWindow.Checked)
            {
                ActiveForm.ShowInTaskbar = false;
                ActiveForm.Visible = false;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Log path can't be empty!");
                return;
            }

            Logging.Setup(textBox2.Text);

            if (IsRunning)
            {
                btnStart.Text = "Stop";
                this.Text = $"{ ProgramName } (running)";
                changeControls(false);


				fsWatcher.WatchStart((rbAll.Checked) ? FsWatcherOpts.WatchAll : FsWatcherOpts.WatchDir, txtPath.Text);
                fsWatcher.SetWatchRecursion(chkRecursive.Checked);

                if (radioButton1.Checked)
                {
                    fsWatcher.AddHandler(OnChanged, OnChanged, OnChanged, OnChanged, OnError);
                }
                else if (radioButton2.Checked)
                {
                    fsWatcher.AddHandler(null, OnCreation, null, null, OnError);
                }
                else
                {
                    fsWatcher.AddHandler(null, OnCreation, OnDeleted, OnRenamed, OnError);
                }
                
                IsRunning = false;
            }
            else
            {
                btnStart.Text = "Start";
                this.Text = ProgramName;
                changeControls(true);

                fsWatcher.WatchStop();
                
                statusFileFound.Text = string.Empty;
                
                IsRunning = true;
            }
        }
        
        private void btnPath_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            if (folderDlg.SelectedPath != "")
                txtPath.Text = folderDlg.SelectedPath;
        }

		private void button2_Click(object sender, EventArgs e)
		{
            folderDlg.ShowDialog();
            if (folderDlg.SelectedPath != "")
            {
                textBox2.Text = folderDlg.SelectedPath;
                Logging.Setup(textBox2.Text);
            }

        }

		private void btnCopyToBrowse_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            if (folderDlg.SelectedPath != "")
                txtCopyTo.Text = folderDlg.SelectedPath;
        }

        private void chkFtpAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            txtFtpUser.Enabled = txtFtpPassword.Enabled = !chkFtpAnonymous.Checked;
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
        }

        private void linkWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/p/FileGrab/wiki/Home/");
        }

        // Handlers
        public void OnChanged(object source, FileSystemEventArgs e)
		{
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                Logging.Log($"Changed: { e.FullPath }");
                statusFileFound.Text = $"Changed: { e.FullPath }";
            }
		}

        public void OnCreation(object source, FileSystemEventArgs e)
        {
            // we cannot monitor the copy destination directory
            if (txtCopyTo.Text != "" &&
                e.FullPath.StartsWith(txtCopyTo.Text, StringComparison.CurrentCultureIgnoreCase))
                return;

            Logging.Log($"Created: { e.FullPath }");
            statusFileFound.Text = $"{ e.FullPath }";

            if (txtCopyTo.Text != "")
            {
                try
                {
                    string filename = e.Name[(1 + e.Name.LastIndexOf('\\'))..];
                    string dstFile = Path.Combine(txtCopyTo.Text, filename);

                    Utils.CopyFileTo(e.FullPath, dstFile, expr: txtRule.Text);

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
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            // I'll improve this later
            if (txtFtpHost.Text == "")
                return;

            try
            {
                ftpUpload = FtpUpload.Create(txtFtpHost.Text, (int)txtFtpPort.Value, e.Name);
                ftpUpload.UseCredentials(txtFtpUser.Text, txtFtpPassword.Text, chkFtpAnonymous.Checked);
                ftpUpload.Upload(e.FullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed!\n\nDetails:\n { ex.Message }",
                                "FTP Upload", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnDeleted(object source, FileSystemEventArgs e)
        {
            Logging.Log($"Deleted: { e.FullPath }");
            statusFileFound.Text = $"Deleted: { e.FullPath } { DateTime.Now }";
        }

        public void OnRenamed(object source, RenamedEventArgs e)
        {
            Logging.Log($"Renamed: { e.OldFullPath } -> { e.FullPath }");
            statusFileFound.Text = $"Renamed: { e.OldFullPath } -> { e.FullPath }";
        }

        public void OnError(object source, ErrorEventArgs e)
        {
            MessageBox.Show($"Error :: { e.GetException() }");
        }
	}
}