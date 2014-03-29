namespace filegrab
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnStart = new System.Windows.Forms.Button();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.chkHideWindow = new System.Windows.Forms.CheckBox();
            this.txtFtpHost = new System.Windows.Forms.TextBox();
            this.txtFtpPort = new System.Windows.Forms.NumericUpDown();
            this.groupFtp = new System.Windows.Forms.GroupBox();
            this.chkFtpAnonymous = new System.Windows.Forms.CheckBox();
            this.btnFtpTest = new System.Windows.Forms.Button();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.lblFtpPassword = new System.Windows.Forms.Label();
            this.txtFtpUser = new System.Windows.Forms.TextBox();
            this.lblFtpUser = new System.Windows.Forms.Label();
            this.lblFtpPort = new System.Windows.Forms.Label();
            this.lblFtpHost = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMonitor = new System.Windows.Forms.TabPage();
            this.groupFilesystem = new System.Windows.Forms.GroupBox();
            this.chkRecursive = new System.Windows.Forms.CheckBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.btnPath = new System.Windows.Forms.Button();
            this.rbSpecific = new System.Windows.Forms.RadioButton();
            this.tabCapture = new System.Windows.Forms.TabPage();
            this.groupCopy = new System.Windows.Forms.GroupBox();
            this.txtCopyTo = new System.Windows.Forms.TextBox();
            this.btnCopyToBrowse = new System.Windows.Forms.Button();
            this.lblLocation = new System.Windows.Forms.Label();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusFileFound = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.txtFtpPort)).BeginInit();
            this.groupFtp.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabMonitor.SuspendLayout();
            this.groupFilesystem.SuspendLayout();
            this.tabCapture.SuspendLayout();
            this.groupCopy.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(399, 263);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(96, 44);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkHideWindow
            // 
            this.chkHideWindow.AutoSize = true;
            this.chkHideWindow.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHideWindow.Location = new System.Drawing.Point(16, 280);
            this.chkHideWindow.Name = "chkHideWindow";
            this.chkHideWindow.Size = new System.Drawing.Size(113, 21);
            this.chkHideWindow.TabIndex = 10;
            this.chkHideWindow.Text = "Hide window";
            this.chkHideWindow.UseVisualStyleBackColor = true;
            // 
            // txtFtpHost
            // 
            this.txtFtpHost.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpHost.Location = new System.Drawing.Point(89, 28);
            this.txtFtpHost.Name = "txtFtpHost";
            this.txtFtpHost.Size = new System.Drawing.Size(261, 23);
            this.txtFtpHost.TabIndex = 4;
            // 
            // txtFtpPort
            // 
            this.txtFtpPort.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpPort.Location = new System.Drawing.Point(400, 28);
            this.txtFtpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtFtpPort.Name = "txtFtpPort";
            this.txtFtpPort.Size = new System.Drawing.Size(62, 23);
            this.txtFtpPort.TabIndex = 5;
            this.txtFtpPort.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // groupFtp
            // 
            this.groupFtp.Controls.Add(this.chkFtpAnonymous);
            this.groupFtp.Controls.Add(this.btnFtpTest);
            this.groupFtp.Controls.Add(this.txtFtpPassword);
            this.groupFtp.Controls.Add(this.lblFtpPassword);
            this.groupFtp.Controls.Add(this.txtFtpUser);
            this.groupFtp.Controls.Add(this.lblFtpUser);
            this.groupFtp.Controls.Add(this.lblFtpPort);
            this.groupFtp.Controls.Add(this.lblFtpHost);
            this.groupFtp.Controls.Add(this.txtFtpPort);
            this.groupFtp.Controls.Add(this.txtFtpHost);
            this.groupFtp.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupFtp.Location = new System.Drawing.Point(6, 69);
            this.groupFtp.Name = "groupFtp";
            this.groupFtp.Size = new System.Drawing.Size(477, 141);
            this.groupFtp.TabIndex = 8;
            this.groupFtp.TabStop = false;
            this.groupFtp.Text = "FTP upload";
            // 
            // chkFtpAnonymous
            // 
            this.chkFtpAnonymous.AutoSize = true;
            this.chkFtpAnonymous.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFtpAnonymous.Location = new System.Drawing.Point(248, 67);
            this.chkFtpAnonymous.Name = "chkFtpAnonymous";
            this.chkFtpAnonymous.Size = new System.Drawing.Size(102, 21);
            this.chkFtpAnonymous.TabIndex = 7;
            this.chkFtpAnonymous.Text = "Anonymous";
            this.chkFtpAnonymous.UseVisualStyleBackColor = true;
            this.chkFtpAnonymous.CheckedChanged += new System.EventHandler(this.chkFtpAnonymous_CheckedChanged);
            // 
            // btnFtpTest
            // 
            this.btnFtpTest.Location = new System.Drawing.Point(248, 103);
            this.btnFtpTest.Name = "btnFtpTest";
            this.btnFtpTest.Size = new System.Drawing.Size(102, 23);
            this.btnFtpTest.TabIndex = 9;
            this.btnFtpTest.Text = "Test";
            this.btnFtpTest.UseVisualStyleBackColor = true;
            this.btnFtpTest.Click += new System.EventHandler(this.btnFtpTest_Click);
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpPassword.Location = new System.Drawing.Point(89, 103);
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.PasswordChar = '*';
            this.txtFtpPassword.Size = new System.Drawing.Size(148, 23);
            this.txtFtpPassword.TabIndex = 8;
            // 
            // lblFtpPassword
            // 
            this.lblFtpPassword.AutoSize = true;
            this.lblFtpPassword.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFtpPassword.Location = new System.Drawing.Point(10, 106);
            this.lblFtpPassword.Name = "lblFtpPassword";
            this.lblFtpPassword.Size = new System.Drawing.Size(69, 17);
            this.lblFtpPassword.TabIndex = 10;
            this.lblFtpPassword.Text = "Password";
            // 
            // txtFtpUser
            // 
            this.txtFtpUser.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpUser.Location = new System.Drawing.Point(89, 65);
            this.txtFtpUser.Name = "txtFtpUser";
            this.txtFtpUser.Size = new System.Drawing.Size(148, 23);
            this.txtFtpUser.TabIndex = 6;
            // 
            // lblFtpUser
            // 
            this.lblFtpUser.AutoSize = true;
            this.lblFtpUser.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFtpUser.Location = new System.Drawing.Point(10, 68);
            this.lblFtpUser.Name = "lblFtpUser";
            this.lblFtpUser.Size = new System.Drawing.Size(33, 17);
            this.lblFtpUser.TabIndex = 9;
            this.lblFtpUser.Text = "User";
            // 
            // lblFtpPort
            // 
            this.lblFtpPort.AutoSize = true;
            this.lblFtpPort.Location = new System.Drawing.Point(360, 31);
            this.lblFtpPort.Name = "lblFtpPort";
            this.lblFtpPort.Size = new System.Drawing.Size(34, 17);
            this.lblFtpPort.TabIndex = 8;
            this.lblFtpPort.Text = "Port";
            // 
            // lblFtpHost
            // 
            this.lblFtpHost.AutoSize = true;
            this.lblFtpHost.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFtpHost.Location = new System.Drawing.Point(10, 31);
            this.lblFtpHost.Name = "lblFtpHost";
            this.lblFtpHost.Size = new System.Drawing.Size(36, 17);
            this.lblFtpHost.TabIndex = 7;
            this.lblFtpHost.Text = "Host";
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabMonitor);
            this.tabControl1.Controls.Add(this.tabCapture);
            this.tabControl1.Controls.Add(this.tabAdvanced);
            this.tabControl1.Controls.Add(this.tabAbout);
            this.tabControl1.Font = new System.Drawing.Font("Century Gothic", 9.75F);
            this.tabControl1.Location = new System.Drawing.Point(8, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(498, 249);
            this.tabControl1.TabIndex = 12;
            // 
            // tabMonitor
            // 
            this.tabMonitor.Controls.Add(this.groupFilesystem);
            this.tabMonitor.Location = new System.Drawing.Point(4, 29);
            this.tabMonitor.Name = "tabMonitor";
            this.tabMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabMonitor.Size = new System.Drawing.Size(490, 216);
            this.tabMonitor.TabIndex = 0;
            this.tabMonitor.Text = "Monitor";
            this.tabMonitor.UseVisualStyleBackColor = true;
            // 
            // groupFilesystem
            // 
            this.groupFilesystem.Controls.Add(this.chkRecursive);
            this.groupFilesystem.Controls.Add(this.txtPath);
            this.groupFilesystem.Controls.Add(this.rbAll);
            this.groupFilesystem.Controls.Add(this.btnPath);
            this.groupFilesystem.Controls.Add(this.rbSpecific);
            this.groupFilesystem.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupFilesystem.Location = new System.Drawing.Point(6, 6);
            this.groupFilesystem.Name = "groupFilesystem";
            this.groupFilesystem.Size = new System.Drawing.Size(477, 204);
            this.groupFilesystem.TabIndex = 0;
            this.groupFilesystem.TabStop = false;
            this.groupFilesystem.Text = "Filesystem";
            // 
            // chkRecursive
            // 
            this.chkRecursive.AutoSize = true;
            this.chkRecursive.Checked = true;
            this.chkRecursive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecursive.Enabled = false;
            this.chkRecursive.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRecursive.Location = new System.Drawing.Point(13, 104);
            this.chkRecursive.Name = "chkRecursive";
            this.chkRecursive.Size = new System.Drawing.Size(166, 21);
            this.chkRecursive.TabIndex = 13;
            this.chkRecursive.Text = "Include subdirectories";
            this.chkRecursive.UseVisualStyleBackColor = true;
            // 
            // txtPath
            // 
            this.txtPath.Enabled = false;
            this.txtPath.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(13, 75);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(413, 23);
            this.txtPath.TabIndex = 2;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAll.Location = new System.Drawing.Point(13, 21);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(113, 21);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "Whole system";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // btnPath
            // 
            this.btnPath.Enabled = false;
            this.btnPath.Location = new System.Drawing.Point(432, 75);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(29, 23);
            this.btnPath.TabIndex = 3;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // rbSpecific
            // 
            this.rbSpecific.AutoSize = true;
            this.rbSpecific.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSpecific.Location = new System.Drawing.Point(13, 48);
            this.rbSpecific.Name = "rbSpecific";
            this.rbSpecific.Size = new System.Drawing.Size(110, 21);
            this.rbSpecific.TabIndex = 1;
            this.rbSpecific.Text = "Specific path";
            this.rbSpecific.UseVisualStyleBackColor = true;
            this.rbSpecific.CheckedChanged += new System.EventHandler(this.rbSpecific_CheckedChanged);
            // 
            // tabCapture
            // 
            this.tabCapture.Controls.Add(this.groupCopy);
            this.tabCapture.Controls.Add(this.groupFtp);
            this.tabCapture.Location = new System.Drawing.Point(4, 29);
            this.tabCapture.Name = "tabCapture";
            this.tabCapture.Padding = new System.Windows.Forms.Padding(3);
            this.tabCapture.Size = new System.Drawing.Size(490, 216);
            this.tabCapture.TabIndex = 1;
            this.tabCapture.Text = "Capture";
            this.tabCapture.UseVisualStyleBackColor = true;
            // 
            // groupCopy
            // 
            this.groupCopy.Controls.Add(this.txtCopyTo);
            this.groupCopy.Controls.Add(this.btnCopyToBrowse);
            this.groupCopy.Controls.Add(this.lblLocation);
            this.groupCopy.Location = new System.Drawing.Point(6, 6);
            this.groupCopy.Name = "groupCopy";
            this.groupCopy.Size = new System.Drawing.Size(477, 57);
            this.groupCopy.TabIndex = 9;
            this.groupCopy.TabStop = false;
            this.groupCopy.Text = "Copy to";
            // 
            // txtCopyTo
            // 
            this.txtCopyTo.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCopyTo.Location = new System.Drawing.Point(89, 22);
            this.txtCopyTo.Name = "txtCopyTo";
            this.txtCopyTo.Size = new System.Drawing.Size(338, 23);
            this.txtCopyTo.TabIndex = 13;
            // 
            // btnCopyToBrowse
            // 
            this.btnCopyToBrowse.Location = new System.Drawing.Point(433, 22);
            this.btnCopyToBrowse.Name = "btnCopyToBrowse";
            this.btnCopyToBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnCopyToBrowse.TabIndex = 14;
            this.btnCopyToBrowse.Text = "...";
            this.btnCopyToBrowse.UseVisualStyleBackColor = true;
            this.btnCopyToBrowse.Click += new System.EventHandler(this.btnCopyToBrowse_Click);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(10, 25);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(65, 17);
            this.lblLocation.TabIndex = 12;
            this.lblLocation.Text = "Location";
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Location = new System.Drawing.Point(4, 29);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(490, 216);
            this.tabAdvanced.TabIndex = 3;
            this.tabAdvanced.Text = "Advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.label1);
            this.tabAbout.Location = new System.Drawing.Point(4, 29);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(490, 216);
            this.tabAbout.TabIndex = 2;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(421, 153);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusFileFound,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 314);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(508, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusFileFound
            // 
            this.statusFileFound.Name = "statusFileFound";
            this.statusFileFound.Size = new System.Drawing.Size(90, 17);
            this.statusFileFound.Text = "statusFileFound";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 336);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.chkHideWindow);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileGrab";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtFtpPort)).EndInit();
            this.groupFtp.ResumeLayout(false);
            this.groupFtp.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabMonitor.ResumeLayout(false);
            this.groupFilesystem.ResumeLayout(false);
            this.groupFilesystem.PerformLayout();
            this.tabCapture.ResumeLayout(false);
            this.groupCopy.ResumeLayout(false);
            this.groupCopy.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.CheckBox chkHideWindow;
        private System.Windows.Forms.TextBox txtFtpHost;
        private System.Windows.Forms.NumericUpDown txtFtpPort;
        private System.Windows.Forms.GroupBox groupFtp;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label lblFtpPassword;
        private System.Windows.Forms.TextBox txtFtpUser;
        private System.Windows.Forms.Label lblFtpUser;
        private System.Windows.Forms.Label lblFtpPort;
        private System.Windows.Forms.Label lblFtpHost;
        private System.Windows.Forms.Button btnFtpTest;
        private System.Windows.Forms.CheckBox chkFtpAnonymous;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabMonitor;
        private System.Windows.Forms.TabPage tabCapture;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbSpecific;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.GroupBox groupFilesystem;
        private System.Windows.Forms.GroupBox groupCopy;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.CheckBox chkRecursive;
        private System.Windows.Forms.TextBox txtCopyTo;
        private System.Windows.Forms.Button btnCopyToBrowse;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusFileFound;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabAdvanced;
    }
}

