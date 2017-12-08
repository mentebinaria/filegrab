using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileGrab.Common;

namespace FileGrab
{
    public partial class frmMain : Form
    {
        private Monitor monitor;

        public frmMain()
        {
            InitializeComponent();

            this.monitor = new Monitor();
        }

        private void rbSpecific_CheckedChanged(object sender, EventArgs e)
        {
            txtPath.Enabled = btnPath.Enabled = chkRecursive.Enabled = rbSpecific.Checked;
        }

        public void watchStart()
        {
            /* Enable Filter And Regular Expression */
            if (chkRule.Checked)
                this.monitor.Matching(txtRule.Text, chkRuleRegex.Checked);

            /* Enable FTP Service */
            if (this.monitor.Ftp != null)
                setFtpCredentials();

            /* Cleanup */
            this.monitor.Reset();

            /* !!!!!!!!!!! Include Ignore Directories (Avoid Recursion) !!!!!!!!!!! */
            // this.monitor.IgnoreDirectory(@"<Put Your Local FTP Folder Here To Avoid Recursion>");

            /* Enable Shadow Copy */
            if (!string.IsNullOrEmpty(txtCopyTo.Text))
                this.monitor.Capture(
                    txtCopyTo.Text,
                    chkWriteOverwrite.Checked,
                    chkReadIgnoreErrors.Checked,
                    chkWritePreserveTimes.Checked,
                    chkWriteCreateDirTree.Checked);

            /* Enable Some Events */
            this.monitor.OnCreated += Monitor_OnCreated;

            /* Enable Watcher(s) */
            if (rbAll.Checked)
                this.monitor.Watcher(int.Parse(cbReadBufferSize.SelectedItem.ToString()), chkRecursive.Checked);
            else if (rbSpecific.Checked)
                this.monitor.Watcher(txtPath.Text, int.Parse(cbReadBufferSize.SelectedItem.ToString()), chkRecursive.Checked);

            this.monitor.Start();
        }

        private void Monitor_OnCreated(string filename)
        {
            Invoke((MethodInvoker)delegate { statusFileFound.Text = filename; });
        }

        public void watchStop()
        {
            this.monitor.Stop();
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
            if (!validateFtpFields())
                return;

            if (chkFtpAnonymous.Checked)
                this.monitor.FtpService(txtFtpHost.Text, (int)txtFtpPort.Value);
            else
                this.monitor.FtpService(txtFtpHost.Text, txtFtpUser.Text, txtFtpPassword.Text, (int)txtFtpPort.Value);
        }

        private void btnFtpTest_Click(object sender, EventArgs e)
        {
            if (!validateFtpFields())
                return;

            if (chkFtpAnonymous.Checked)
                this.monitor.FtpService(txtFtpHost.Text, (int)txtFtpPort.Value);
            else
                this.monitor.FtpService(txtFtpHost.Text, txtFtpUser.Text, txtFtpPassword.Text, (int)txtFtpPort.Value);

            if (this.monitor.Ftp.Test())
                MessageBox.Show("Connection succeeded!", "FTP test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Connection failed!\n\nDetails:\n", "FTP test", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            this.monitor.Matching(txtRule.Text, chkRuleRegex.Checked);

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
            System.Diagnostics.Process.Start("https://sourceforge.net/p/filegrab/wiki/Home/");
        }
    }
}
