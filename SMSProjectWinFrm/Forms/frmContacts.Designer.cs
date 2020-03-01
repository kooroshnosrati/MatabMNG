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
            this.cmbDiseaseName = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.TxtEmail = new System.Windows.Forms.TextBox();
            this.TxtAddress = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
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
            this.TxtBirthday = new System.Windows.Forms.TextBox();
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
            this.dataGridView1.Location = new System.Drawing.Point(0, 196);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.Size = new System.Drawing.Size(1289, 412);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TxtBirthday);
            this.groupBox1.Controls.Add(this.cmbDiseaseName);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.TxtEmail);
            this.groupBox1.Controls.Add(this.TxtAddress);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.BtnExit);
            this.groupBox1.Controls.Add(this.button3);
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
            this.groupBox1.Location = new System.Drawing.Point(7, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1274, 190);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "مشخصات بیمار";
            // 
            // cmbDiseaseName
            // 
            this.cmbDiseaseName.FormattingEnabled = true;
            this.cmbDiseaseName.Items.AddRange(new object[] {
            "دیابت",
            "غدد"});
            this.cmbDiseaseName.Location = new System.Drawing.Point(900, 99);
            this.cmbDiseaseName.Name = "cmbDiseaseName";
            this.cmbDiseaseName.Size = new System.Drawing.Size(121, 21);
            this.cmbDiseaseName.TabIndex = 49;
            this.cmbDiseaseName.SelectedIndexChanged += new System.EventHandler(this.cmbDiseaseName_SelectedIndexChanged);
            this.cmbDiseaseName.TextChanged += new System.EventHandler(this.cmbDiseaseName_TextChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(116, 97);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(192, 87);
            this.button4.TabIndex = 48;
            this.button4.Text = "مشاهده پرونده پزشکی";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // TxtEmail
            // 
            this.TxtEmail.Location = new System.Drawing.Point(827, 163);
            this.TxtEmail.Name = "TxtEmail";
            this.TxtEmail.Size = new System.Drawing.Size(207, 21);
            this.TxtEmail.TabIndex = 47;
            // 
            // TxtAddress
            // 
            this.TxtAddress.Location = new System.Drawing.Point(324, 37);
            this.TxtAddress.Multiline = true;
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.Size = new System.Drawing.Size(185, 147);
            this.TxtAddress.TabIndex = 46;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1033, 139);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 44;
            this.label12.Text = "تاریخ تولد :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1043, 166);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 13);
            this.label11.TabIndex = 43;
            this.label11.Text = "ایمیل :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(459, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "آدرس :";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(116, 37);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 41;
            this.button2.Text = "بیمار جدید";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // BtnExit
            // 
            this.BtnExit.Location = new System.Drawing.Point(116, 67);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 40;
            this.BtnExit.Text = "خروج";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(233, 66);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 39;
            this.button3.Text = "ریست";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // TxtNotes
            // 
            this.TxtNotes.Location = new System.Drawing.Point(515, 37);
            this.TxtNotes.Multiline = true;
            this.TxtNotes.Name = "TxtNotes";
            this.TxtNotes.Size = new System.Drawing.Size(191, 147);
            this.TxtNotes.TabIndex = 37;
            this.TxtNotes.TextChanged += new System.EventHandler(this.TxtNotes_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(655, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "توضیحات :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(830, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "تلفن منزل :";
            // 
            // TxtPhone
            // 
            this.TxtPhone.Location = new System.Drawing.Point(715, 76);
            this.TxtPhone.Name = "TxtPhone";
            this.TxtPhone.Size = new System.Drawing.Size(100, 21);
            this.TxtPhone.TabIndex = 34;
            this.TxtPhone.TextChanged += new System.EventHandler(this.TxtPhone_TextChanged);
            // 
            // TxtFatherName
            // 
            this.TxtFatherName.Location = new System.Drawing.Point(715, 106);
            this.TxtFatherName.Name = "TxtFatherName";
            this.TxtFatherName.Size = new System.Drawing.Size(100, 21);
            this.TxtFatherName.TabIndex = 33;
            this.TxtFatherName.TextChanged += new System.EventHandler(this.TxtFatherName_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(847, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "نام پدر :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1027, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "نام بیماری :";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(233, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "ثبت";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TxtMobile
            // 
            this.TxtMobile.Location = new System.Drawing.Point(934, 74);
            this.TxtMobile.Name = "TxtMobile";
            this.TxtMobile.Size = new System.Drawing.Size(100, 21);
            this.TxtMobile.TabIndex = 28;
            this.TxtMobile.TextChanged += new System.EventHandler(this.TxtMobile_TextChanged);
            // 
            // TxtSSID
            // 
            this.TxtSSID.Location = new System.Drawing.Point(715, 23);
            this.TxtSSID.Name = "TxtSSID";
            this.TxtSSID.Size = new System.Drawing.Size(100, 21);
            this.TxtSSID.TabIndex = 27;
            this.TxtSSID.TextChanged += new System.EventHandler(this.TxtSSID_TextChanged);
            // 
            // TxtPatientID
            // 
            this.TxtPatientID.Location = new System.Drawing.Point(908, 24);
            this.TxtPatientID.Name = "TxtPatientID";
            this.TxtPatientID.Size = new System.Drawing.Size(100, 21);
            this.TxtPatientID.TabIndex = 26;
            this.TxtPatientID.TextChanged += new System.EventHandler(this.TxtPatientID_TextChanged);
            // 
            // TxtLName
            // 
            this.TxtLName.Location = new System.Drawing.Point(715, 49);
            this.TxtLName.Name = "TxtLName";
            this.TxtLName.Size = new System.Drawing.Size(100, 21);
            this.TxtLName.TabIndex = 25;
            this.TxtLName.TextChanged += new System.EventHandler(this.TxtLName_TextChanged);
            // 
            // TxtFName
            // 
            this.TxtFName.Location = new System.Drawing.Point(934, 49);
            this.TxtFName.Name = "TxtFName";
            this.TxtFName.Size = new System.Drawing.Size(100, 21);
            this.TxtFName.TabIndex = 24;
            this.TxtFName.TextChanged += new System.EventHandler(this.TxtFName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1046, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "موبایل :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(841, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "کد ملی :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1014, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "شماره پرونده :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(818, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "نام خانوادگی :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1061, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "نام :";
            // 
            // TxtBirthday
            // 
            this.TxtBirthday.Location = new System.Drawing.Point(927, 136);
            this.TxtBirthday.Name = "TxtBirthday";
            this.TxtBirthday.Size = new System.Drawing.Size(100, 21);
            this.TxtBirthday.TabIndex = 50;
            // 
            // frmContacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1289, 608);
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
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtEmail;
        private System.Windows.Forms.TextBox TxtAddress;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cmbDiseaseName;
        private System.Windows.Forms.TextBox TxtBirthday;
    }
}