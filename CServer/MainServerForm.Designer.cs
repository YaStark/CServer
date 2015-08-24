namespace CServer
{
    partial class MainServerForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainServerForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttonTSStart = new System.Windows.Forms.ToolStripButton();
            this.buttonTSStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonTSSettings = new System.Windows.Forms.ToolStripButton();
            this.buttonTSUInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.listBoxUsers = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonUsersKick = new System.Windows.Forms.Button();
            this.groupBoxUInfo = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxUdp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNick = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLastResp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSocket = new System.Windows.Forms.TextBox();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.buttonSettingsDefault = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSettingsApply = new System.Windows.Forms.Button();
            this.textBoxMsgPort = new System.Windows.Forms.NumericUpDown();
            this.textBoxVoicePort = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelServerIP = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBoxUInfo.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMsgPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxVoicePort)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonTSStart,
            this.buttonTSStop,
            this.toolStripSeparator1,
            this.buttonTSSettings,
            this.buttonTSUInfo,
            this.toolStripDropDownButton1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(662, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // buttonTSStart
            // 
            this.buttonTSStart.Image = ((System.Drawing.Image)(resources.GetObject("buttonTSStart.Image")));
            this.buttonTSStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonTSStart.Name = "buttonTSStart";
            this.buttonTSStart.Size = new System.Drawing.Size(51, 22);
            this.buttonTSStart.Text = "Start";
            this.buttonTSStart.Click += new System.EventHandler(this.buttonTSStart_Click);
            // 
            // buttonTSStop
            // 
            this.buttonTSStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonTSStop.Image")));
            this.buttonTSStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonTSStop.Name = "buttonTSStop";
            this.buttonTSStop.Size = new System.Drawing.Size(51, 22);
            this.buttonTSStop.Text = "Stop";
            this.buttonTSStop.Click += new System.EventHandler(this.buttonTSStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonTSSettings
            // 
            this.buttonTSSettings.Checked = true;
            this.buttonTSSettings.CheckOnClick = true;
            this.buttonTSSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buttonTSSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonTSSettings.Image = ((System.Drawing.Image)(resources.GetObject("buttonTSSettings.Image")));
            this.buttonTSSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonTSSettings.Name = "buttonTSSettings";
            this.buttonTSSettings.Size = new System.Drawing.Size(91, 22);
            this.buttonTSSettings.Text = "Socket Settings";
            this.buttonTSSettings.Click += new System.EventHandler(this.buttonTSSettings_Click);
            // 
            // buttonTSUInfo
            // 
            this.buttonTSUInfo.Checked = true;
            this.buttonTSUInfo.CheckOnClick = true;
            this.buttonTSUInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buttonTSUInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buttonTSUInfo.Image = ((System.Drawing.Image)(resources.GetObject("buttonTSUInfo.Image")));
            this.buttonTSUInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonTSUInfo.Name = "buttonTSUInfo";
            this.buttonTSUInfo.Size = new System.Drawing.Size(58, 22);
            this.buttonTSUInfo.Text = "User Info";
            this.buttonTSUInfo.Click += new System.EventHandler(this.buttonTSUInfo_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(56, 22);
            this.toolStripDropDownButton1.Text = "Log";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton1.Text = "Options";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBoxUInfo);
            this.groupBox2.Controls.Add(this.groupBoxSettings);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 418);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listBoxUsers);
            this.groupBox5.Controls.Add(this.panel1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 118);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(247, 180);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Users";
            // 
            // listBoxUsers
            // 
            this.listBoxUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUsers.FormattingEnabled = true;
            this.listBoxUsers.Location = new System.Drawing.Point(3, 16);
            this.listBoxUsers.Name = "listBoxUsers";
            this.listBoxUsers.ScrollAlwaysVisible = true;
            this.listBoxUsers.Size = new System.Drawing.Size(241, 133);
            this.listBoxUsers.TabIndex = 6;
            this.listBoxUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxUsers_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonUsersKick);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 149);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(241, 28);
            this.panel1.TabIndex = 7;
            // 
            // buttonUsersKick
            // 
            this.buttonUsersKick.Location = new System.Drawing.Point(162, 3);
            this.buttonUsersKick.Name = "buttonUsersKick";
            this.buttonUsersKick.Size = new System.Drawing.Size(75, 23);
            this.buttonUsersKick.TabIndex = 0;
            this.buttonUsersKick.Text = "Kick";
            this.buttonUsersKick.UseVisualStyleBackColor = true;
            this.buttonUsersKick.Click += new System.EventHandler(this.buttonUsersKick_Click);
            // 
            // groupBoxUInfo
            // 
            this.groupBoxUInfo.Controls.Add(this.label5);
            this.groupBoxUInfo.Controls.Add(this.textBoxUdp);
            this.groupBoxUInfo.Controls.Add(this.label1);
            this.groupBoxUInfo.Controls.Add(this.textBoxNick);
            this.groupBoxUInfo.Controls.Add(this.label4);
            this.groupBoxUInfo.Controls.Add(this.textBoxLastResp);
            this.groupBoxUInfo.Controls.Add(this.label2);
            this.groupBoxUInfo.Controls.Add(this.textBoxSocket);
            this.groupBoxUInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxUInfo.Location = new System.Drawing.Point(3, 298);
            this.groupBoxUInfo.Name = "groupBoxUInfo";
            this.groupBoxUInfo.Size = new System.Drawing.Size(247, 117);
            this.groupBoxUInfo.TabIndex = 4;
            this.groupBoxUInfo.TabStop = false;
            this.groupBoxUInfo.Text = "User Info";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "UDP Socket";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxUdp
            // 
            this.textBoxUdp.BackColor = System.Drawing.Color.White;
            this.textBoxUdp.Location = new System.Drawing.Point(88, 70);
            this.textBoxUdp.Name = "textBoxUdp";
            this.textBoxUdp.ReadOnly = true;
            this.textBoxUdp.Size = new System.Drawing.Size(152, 20);
            this.textBoxUdp.TabIndex = 17;
            this.textBoxUdp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "Nickname";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxNick
            // 
            this.textBoxNick.BackColor = System.Drawing.Color.White;
            this.textBoxNick.Location = new System.Drawing.Point(88, 18);
            this.textBoxNick.Name = "textBoxNick";
            this.textBoxNick.ReadOnly = true;
            this.textBoxNick.Size = new System.Drawing.Size(152, 20);
            this.textBoxNick.TabIndex = 1;
            this.textBoxNick.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Last response";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxLastResp
            // 
            this.textBoxLastResp.BackColor = System.Drawing.Color.White;
            this.textBoxLastResp.Location = new System.Drawing.Point(88, 96);
            this.textBoxLastResp.Name = "textBoxLastResp";
            this.textBoxLastResp.ReadOnly = true;
            this.textBoxLastResp.Size = new System.Drawing.Size(152, 20);
            this.textBoxLastResp.TabIndex = 7;
            this.textBoxLastResp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "TCP Socket";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxSocket
            // 
            this.textBoxSocket.BackColor = System.Drawing.Color.White;
            this.textBoxSocket.Location = new System.Drawing.Point(88, 44);
            this.textBoxSocket.Name = "textBoxSocket";
            this.textBoxSocket.ReadOnly = true;
            this.textBoxSocket.Size = new System.Drawing.Size(152, 20);
            this.textBoxSocket.TabIndex = 2;
            this.textBoxSocket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Controls.Add(this.labelServerIP);
            this.groupBoxSettings.Controls.Add(this.buttonSettingsDefault);
            this.groupBoxSettings.Controls.Add(this.buttonReset);
            this.groupBoxSettings.Controls.Add(this.buttonSettingsApply);
            this.groupBoxSettings.Controls.Add(this.textBoxMsgPort);
            this.groupBoxSettings.Controls.Add(this.textBoxVoicePort);
            this.groupBoxSettings.Controls.Add(this.label7);
            this.groupBoxSettings.Controls.Add(this.label6);
            this.groupBoxSettings.Controls.Add(this.label3);
            this.groupBoxSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSettings.Location = new System.Drawing.Point(3, 16);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(247, 102);
            this.groupBoxSettings.TabIndex = 15;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Socket Settings";
            // 
            // buttonSettingsDefault
            // 
            this.buttonSettingsDefault.Location = new System.Drawing.Point(9, 72);
            this.buttonSettingsDefault.Name = "buttonSettingsDefault";
            this.buttonSettingsDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonSettingsDefault.TabIndex = 29;
            this.buttonSettingsDefault.Text = "Default";
            this.buttonSettingsDefault.UseVisualStyleBackColor = true;
            this.buttonSettingsDefault.Click += new System.EventHandler(this.buttonSettingsDefault_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(88, 72);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 28;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSettingsApply
            // 
            this.buttonSettingsApply.Location = new System.Drawing.Point(169, 72);
            this.buttonSettingsApply.Name = "buttonSettingsApply";
            this.buttonSettingsApply.Size = new System.Drawing.Size(71, 23);
            this.buttonSettingsApply.TabIndex = 27;
            this.buttonSettingsApply.Text = "Apply";
            this.buttonSettingsApply.UseVisualStyleBackColor = true;
            this.buttonSettingsApply.Click += new System.EventHandler(this.buttonSettingsApply_Click);
            // 
            // textBoxMsgPort
            // 
            this.textBoxMsgPort.Location = new System.Drawing.Point(67, 46);
            this.textBoxMsgPort.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this.textBoxMsgPort.Minimum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.textBoxMsgPort.Name = "textBoxMsgPort";
            this.textBoxMsgPort.Size = new System.Drawing.Size(51, 20);
            this.textBoxMsgPort.TabIndex = 26;
            this.textBoxMsgPort.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.textBoxMsgPort.ValueChanged += new System.EventHandler(this.settingsTextChanged);
            // 
            // textBoxVoicePort
            // 
            this.textBoxVoicePort.Location = new System.Drawing.Point(189, 44);
            this.textBoxVoicePort.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this.textBoxVoicePort.Minimum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.textBoxVoicePort.Name = "textBoxVoicePort";
            this.textBoxVoicePort.Size = new System.Drawing.Size(51, 20);
            this.textBoxVoicePort.TabIndex = 25;
            this.textBoxVoicePort.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.textBoxVoicePort.ValueChanged += new System.EventHandler(this.settingsTextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(124, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.TabIndex = 22;
            this.label7.Text = "Voice port";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "Msg port";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "IP";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(253, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 418);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // textBoxLog
            // 
            this.textBoxLog.BackColor = System.Drawing.Color.White;
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Location = new System.Drawing.Point(3, 16);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(403, 399);
            this.textBoxLog.TabIndex = 0;
            // 
            // labelServerIP
            // 
            this.labelServerIP.Location = new System.Drawing.Point(67, 16);
            this.labelServerIP.Name = "labelServerIP";
            this.labelServerIP.Size = new System.Drawing.Size(173, 20);
            this.labelServerIP.TabIndex = 30;
            this.labelServerIP.Text = "IP";
            this.labelServerIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 443);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainServerForm";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainServerForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBoxUInfo.ResumeLayout(false);
            this.groupBoxUInfo.PerformLayout();
            this.groupBoxSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxMsgPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxVoicePort)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonTSStart;
        private System.Windows.Forms.ToolStripButton buttonTSStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBoxUInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLastResp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSocket;
        private System.Windows.Forms.TextBox textBoxNick;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton buttonTSSettings;
        private System.Windows.Forms.ToolStripButton buttonTSUInfo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox listBoxUsers;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSettingsApply;
        private System.Windows.Forms.NumericUpDown textBoxMsgPort;
        private System.Windows.Forms.NumericUpDown textBoxVoicePort;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSettingsDefault;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxUdp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonUsersKick;
        private System.Windows.Forms.Label labelServerIP;
    }
}

