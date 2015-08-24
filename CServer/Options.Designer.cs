namespace CServer
{
    partial class Options
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numAfkTimeoutSec = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numShutdownTimeoutSec = new System.Windows.Forms.NumericUpDown();
            this.cBoxAfkUserState = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numRequestTimeoutMs = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAfkTimeoutSec)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numShutdownTimeoutSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequestTimeoutMs)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cBoxAfkUserState);
            this.groupBox1.Controls.Add(this.numAfkTimeoutSec);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(283, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 77);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Afk state";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Timeout, sec";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numAfkTimeoutSec
            // 
            this.numAfkTimeoutSec.DecimalPlaces = 1;
            this.numAfkTimeoutSec.Location = new System.Drawing.Point(141, 44);
            this.numAfkTimeoutSec.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numAfkTimeoutSec.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAfkTimeoutSec.Name = "numAfkTimeoutSec";
            this.numAfkTimeoutSec.Size = new System.Drawing.Size(73, 20);
            this.numAfkTimeoutSec.TabIndex = 4;
            this.numAfkTimeoutSec.ThousandsSeparator = true;
            this.numAfkTimeoutSec.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numShutdownTimeoutSec);
            this.groupBox2.Controls.Add(this.numRequestTimeoutMs);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(265, 77);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Shutdown server timeout, sec";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numShutdownTimeoutSec
            // 
            this.numShutdownTimeoutSec.DecimalPlaces = 1;
            this.numShutdownTimeoutSec.Location = new System.Drawing.Point(165, 16);
            this.numShutdownTimeoutSec.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numShutdownTimeoutSec.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numShutdownTimeoutSec.Name = "numShutdownTimeoutSec";
            this.numShutdownTimeoutSec.Size = new System.Drawing.Size(84, 20);
            this.numShutdownTimeoutSec.TabIndex = 5;
            this.numShutdownTimeoutSec.ThousandsSeparator = true;
            this.numShutdownTimeoutSec.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // cBoxAfkUserState
            // 
            this.cBoxAfkUserState.AutoSize = true;
            this.cBoxAfkUserState.Location = new System.Drawing.Point(169, 18);
            this.cBoxAfkUserState.Name = "cBoxAfkUserState";
            this.cBoxAfkUserState.Size = new System.Drawing.Size(15, 14);
            this.cBoxAfkUserState.TabIndex = 5;
            this.cBoxAfkUserState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cBoxAfkUserState.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Enable afk timeout kick";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Request timeout, ms";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRequestTimeoutMs
            // 
            this.numRequestTimeoutMs.DecimalPlaces = 1;
            this.numRequestTimeoutMs.Location = new System.Drawing.Point(165, 42);
            this.numRequestTimeoutMs.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.numRequestTimeoutMs.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numRequestTimeoutMs.Name = "numRequestTimeoutMs";
            this.numRequestTimeoutMs.Size = new System.Drawing.Size(84, 20);
            this.numRequestTimeoutMs.TabIndex = 6;
            this.numRequestTimeoutMs.ThousandsSeparator = true;
            this.numRequestTimeoutMs.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(350, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(431, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 30);
            this.panel1.TabIndex = 6;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 127);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAfkTimeoutSec)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numShutdownTimeoutSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRequestTimeoutMs)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numAfkTimeoutSec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numShutdownTimeoutSec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cBoxAfkUserState;
        private System.Windows.Forms.NumericUpDown numRequestTimeoutMs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
    }
}