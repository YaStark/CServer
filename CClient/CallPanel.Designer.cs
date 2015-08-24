namespace CClient
{
    partial class CallPanel
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelNone = new System.Windows.Forms.Panel();
            this.panelAnswer = new System.Windows.Forms.Panel();
            this.panelTalk = new System.Windows.Forms.Panel();
            this.buttonAnswerAccept = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonAnswerReject = new System.Windows.Forms.Button();
            this.buttonNoneCall = new System.Windows.Forms.Button();
            this.buttonHide = new System.Windows.Forms.Button();
            this.buttonTalkCancel = new System.Windows.Forms.Button();
            this.panelNone.SuspendLayout();
            this.panelAnswer.SuspendLayout();
            this.panelTalk.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelNone
            // 
            this.panelNone.Controls.Add(this.buttonNoneCall);
            this.panelNone.Location = new System.Drawing.Point(3, 107);
            this.panelNone.Name = "panelNone";
            this.panelNone.Size = new System.Drawing.Size(216, 50);
            this.panelNone.TabIndex = 0;
            // 
            // panelAnswer
            // 
            this.panelAnswer.Controls.Add(this.buttonAnswerReject);
            this.panelAnswer.Controls.Add(this.buttonAnswerAccept);
            this.panelAnswer.Location = new System.Drawing.Point(3, 107);
            this.panelAnswer.Name = "panelAnswer";
            this.panelAnswer.Size = new System.Drawing.Size(217, 50);
            this.panelAnswer.TabIndex = 0;
            // 
            // panelTalk
            // 
            this.panelTalk.Controls.Add(this.buttonTalkCancel);
            this.panelTalk.Location = new System.Drawing.Point(3, 107);
            this.panelTalk.Name = "panelTalk";
            this.panelTalk.Size = new System.Drawing.Size(217, 50);
            this.panelTalk.TabIndex = 1;
            // 
            // buttonAnswerAccept
            // 
            this.buttonAnswerAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAnswerAccept.Location = new System.Drawing.Point(103, 3);
            this.buttonAnswerAccept.Name = "buttonAnswerAccept";
            this.buttonAnswerAccept.Size = new System.Drawing.Size(111, 44);
            this.buttonAnswerAccept.TabIndex = 0;
            this.buttonAnswerAccept.Text = "Accept";
            this.buttonAnswerAccept.UseVisualStyleBackColor = true;
            this.buttonAnswerAccept.Click += new System.EventHandler(this.buttonAnswerAccept_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStatus.Location = new System.Drawing.Point(3, 45);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(306, 55);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Call status";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelName
            // 
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(2, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(307, 45);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "Nickname";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAnswerReject
            // 
            this.buttonAnswerReject.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAnswerReject.Location = new System.Drawing.Point(22, 17);
            this.buttonAnswerReject.Name = "buttonAnswerReject";
            this.buttonAnswerReject.Size = new System.Drawing.Size(75, 30);
            this.buttonAnswerReject.TabIndex = 1;
            this.buttonAnswerReject.Text = "Reject";
            this.buttonAnswerReject.UseVisualStyleBackColor = true;
            this.buttonAnswerReject.Click += new System.EventHandler(this.buttonAnswerDecline_Click);
            // 
            // buttonNoneCall
            // 
            this.buttonNoneCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonNoneCall.Location = new System.Drawing.Point(102, 3);
            this.buttonNoneCall.Name = "buttonNoneCall";
            this.buttonNoneCall.Size = new System.Drawing.Size(111, 44);
            this.buttonNoneCall.TabIndex = 1;
            this.buttonNoneCall.Text = "Call";
            this.buttonNoneCall.UseVisualStyleBackColor = true;
            this.buttonNoneCall.Click += new System.EventHandler(this.buttonNoneCall_Click);
            // 
            // buttonHide
            // 
            this.buttonHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHide.Location = new System.Drawing.Point(222, 124);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(75, 30);
            this.buttonHide.TabIndex = 4;
            this.buttonHide.Text = "Close";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // buttonTalkCancel
            // 
            this.buttonTalkCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTalkCancel.Location = new System.Drawing.Point(103, 4);
            this.buttonTalkCancel.Name = "buttonTalkCancel";
            this.buttonTalkCancel.Size = new System.Drawing.Size(111, 44);
            this.buttonTalkCancel.TabIndex = 1;
            this.buttonTalkCancel.Text = "Cancel";
            this.buttonTalkCancel.UseVisualStyleBackColor = true;
            this.buttonTalkCancel.Click += new System.EventHandler(this.buttonTalkCancel_Click);
            // 
            // CallPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonHide);
            this.Controls.Add(this.panelAnswer);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.panelNone);
            this.Controls.Add(this.panelTalk);
            this.Name = "CallPanel";
            this.Size = new System.Drawing.Size(322, 162);
            this.panelNone.ResumeLayout(false);
            this.panelAnswer.ResumeLayout(false);
            this.panelTalk.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelNone;
        private System.Windows.Forms.Button buttonNoneCall;
        private System.Windows.Forms.Panel panelAnswer;
        private System.Windows.Forms.Button buttonAnswerReject;
        private System.Windows.Forms.Button buttonAnswerAccept;
        private System.Windows.Forms.Panel panelTalk;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.Button buttonTalkCancel;
    }
}
