namespace SMSProjectWinFrm
{
    partial class frmContacts
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.TxtDiseaseName = new System.Windows.Forms.TextBox();
            this.TxtNotes = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtPhone = new System.Windows.Forms.TextBox();
            this.TxtFatherName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.TxtMobile = new System.Windows.Forms.TextBox();
            this.TxtSSID = new System.Windows.Forms.TextBox();
            this.TxtPatientID = new System.Windows.Forms.TextBox();
            this.TxtLName = new System.Windows.Forms.TextBox();
            this.TxtFName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 171);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.Size = new System.Drawing.Size(1027, 453);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.BtnExit);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.TxtDiseaseName);
            this.groupBox1.Controls.Add(this.TxtNotes);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TxtPhone);
            this.groupBox1.Controls.Add(this.TxtFatherName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.TxtMobile);
            this.groupBox1.Controls.Add(this.TxtSSID);
            this.groupBox1.Controls.Add(this.TxtPatientID);
            this.groupBox1.Controls.Add(this.TxtLName);
            this.groupBox1.Controls.Add(this.TxtFName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(779, 161);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "مشخصات بیمار";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 100);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 41;
            this.button2.Text = "بیمار جدید";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(14, 130);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 40;
            this.BtnExit.Text = "خروج";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 51);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 39;
            this.button3.Text = "ریست";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // TxtDiseaseName
            // 
            this.TxtDiseaseName.Location = new System.Drawing.Point(597, 130);
            this.TxtDiseaseName.Name = "TxtDiseaseName";
            this.TxtDiseaseName.Size = new System.Drawing.Size(100, 21);
            this.TxtDiseaseName.TabIndex = 38;
            this.TxtDiseaseName.TextChanged += new System.EventHandler(this.TxtDiseaseName_TextChanged);
            // 
            // TxtNotes
            // 
            this.TxtNotes.Location = new System.Drawing.Point(95, 20);
            this.TxtNotes.Multiline = true;
            this.TxtNotes.Name = "TxtNotes";
            this.TxtNotes.Size = new System.Drawing.Size(236, 126);
            this.TxtNotes.TabIndex = 37;
            this.TxtNotes.TextChanged += new System.EventHandler(this.TxtNotes_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(337, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "توضیحات :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(515, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "تلفن منزل :";
            // 
            // TxtPhone
            // 
            this.TxtPhone.Location = new System.Drawing.Point(400, 88);
            this.TxtPhone.Name = "TxtPhone";
            this.TxtPhone.Size = new System.Drawing.Size(100, 21);
            this.TxtPhone.TabIndex = 34;
            this.TxtPhone.TextChanged += new System.EventHandler(this.TxtPhone_TextChanged);
            // 
            // TxtFatherName
            // 
            this.TxtFatherName.Location = new System.Drawing.Point(400, 130);
            this.TxtFatherName.Name = "TxtFatherName";
            this.TxtFatherName.Size = new System.Drawing.Size(100, 21);
            this.TxtFatherName.TabIndex = 33;
            this.TxtFatherName.TextChanged += new System.EventHandler(this.TxtFatherName_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(532, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "نام پدر :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(700, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "نام بیماری :";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "ثبت";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TxtMobile
            // 
            this.TxtMobile.Location = new System.Drawing.Point(607, 88);
            this.TxtMobile.Name = "TxtMobile";
            this.TxtMobile.Size = new System.Drawing.Size(100, 21);
            this.TxtMobile.TabIndex = 28;
            this.TxtMobile.TextChanged += new System.EventHandler(this.TxtMobile_TextChanged);
            // 
            // TxtSSID
            // 
            this.TxtSSID.Location = new System.Drawing.Point(400, 20);
            this.TxtSSID.Name = "TxtSSID";
            this.TxtSSID.Size = new System.Drawing.Size(100, 21);
            this.TxtSSID.TabIndex = 27;
            this.TxtSSID.TextChanged += new System.EventHandler(this.TxtSSID_TextChanged);
            // 
            // TxtPatientID
            // 
            this.TxtPatientID.Location = new System.Drawing.Point(581, 20);
            this.TxtPatientID.Name = "TxtPatientID";
            this.TxtPatientID.Size = new System.Drawing.Size(100, 21);
            this.TxtPatientID.TabIndex = 26;
            this.TxtPatientID.TextChanged += new System.EventHandler(this.TxtPatientID_TextChanged);
            // 
            // TxtLName
            // 
            this.TxtLName.Location = new System.Drawing.Point(400, 51);
            this.TxtLName.Name = "TxtLName";
            this.TxtLName.Size = new System.Drawing.Size(100, 21);
            this.TxtLName.TabIndex = 25;
            this.TxtLName.TextChanged += new System.EventHandler(this.TxtLName_TextChanged);
            // 
            // TxtFName
            // 
            this.TxtFName.Location = new System.Drawing.Point(622, 51);
            this.TxtFName.Name = "TxtFName";
            this.TxtFName.Size = new System.Drawing.Size(100, 21);
            this.TxtFName.TabIndex = 24;
            this.TxtFName.TextChanged += new System.EventHandler(this.TxtFName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(719, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "موبایل :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(526, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "کد ملی :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(687, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "شماره پرونده :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(503, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "نام خانوادگی :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(734, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "نام :";
            // 
            // frmContacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1027, 624);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmContacts";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.Text = "مدیریت مشخصات بیماران";
            this.Load += new System.EventHandler(this.frmContacts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TxtMobile;
        private System.Windows.Forms.TextBox TxtSSID;
        private System.Windows.Forms.TextBox TxtPatientID;
        private System.Windows.Forms.TextBox TxtLName;
        private System.Windows.Forms.TextBox TxtFName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtFatherName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtPhone;
        private System.Windows.Forms.TextBox TxtNotes;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TxtDiseaseName;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button BtnExit;
    }
}