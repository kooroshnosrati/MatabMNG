namespace SMSProjectWinFrm
{
    partial class frmConfiguration
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtOutllokAccount = new System.Windows.Forms.TextBox();
            this.TxtTestPhoneNumber = new System.Windows.Forms.TextBox();
            this.TxtTestMessageBody = new System.Windows.Forms.TextBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "پست الکترونیکی مدیریت مطب :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "شماره تلفن برای تست پیامک :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "متن پیامک تستی :";
            // 
            // TxtOutllokAccount
            // 
            this.TxtOutllokAccount.Location = new System.Drawing.Point(158, 6);
            this.TxtOutllokAccount.Name = "TxtOutllokAccount";
            this.TxtOutllokAccount.Size = new System.Drawing.Size(151, 21);
            this.TxtOutllokAccount.TabIndex = 3;
            // 
            // TxtTestPhoneNumber
            // 
            this.TxtTestPhoneNumber.Location = new System.Drawing.Point(151, 35);
            this.TxtTestPhoneNumber.Name = "TxtTestPhoneNumber";
            this.TxtTestPhoneNumber.Size = new System.Drawing.Size(158, 21);
            this.TxtTestPhoneNumber.TabIndex = 4;
            //this.TxtTestPhoneNumber.Text = "09195614157";
            // 
            // TxtTestMessageBody
            // 
            this.TxtTestMessageBody.Location = new System.Drawing.Point(97, 67);
            this.TxtTestMessageBody.Name = "TxtTestMessageBody";
            this.TxtTestMessageBody.Size = new System.Drawing.Size(212, 21);
            this.TxtTestMessageBody.TabIndex = 5;
            //this.TxtTestMessageBody.Text = "این یک پیامک تستی میباشد.";
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(333, 12);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 6;
            this.BtnSave.Text = "ذخیره";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(333, 64);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 7;
            this.BtnExit.Text = "خروج";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // frmConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 261);
            this.ControlBox = false;
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.TxtTestMessageBody);
            this.Controls.Add(this.TxtTestPhoneNumber);
            this.Controls.Add(this.TxtOutllokAccount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConfiguration";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.Text = "تنظیمات";
            this.Load += new System.EventHandler(this.frmConfiguration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtOutllokAccount;
        private System.Windows.Forms.TextBox TxtTestPhoneNumber;
        private System.Windows.Forms.TextBox TxtTestMessageBody;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnExit;
    }
}