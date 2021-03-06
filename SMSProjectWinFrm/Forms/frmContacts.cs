﻿using SMSProjectWinFrm.Business;
using SMSProjectWinFrm.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm
{

    public partial class frmContacts : Form
    {
        cls_Contact selectedContact;
        bool chk = false;
        public cls_OutlookManagement outlookManagement;
        public frmContacts()
        {
            InitializeComponent();
        }

        private void frmContacts_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            //dtpBirthDay.Value = DateTime.Now;
            RefreshGridView();
        }

        private void TxtPatientID_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        public void RefreshGridView()
        {
            try
            {
                List<cls_Contact> contacts = (List<cls_Contact>)outlookManagement.contacts.ToList()
                    .Where(a => a.PatientID != null && a.PatientID.IndexOf(TxtPatientID.Text) != -1)
                    .Where(b => b.SSID != null && b.SSID.IndexOf(TxtSSID.Text) != -1)
                    .Where(c => c.FirstName != null && c.FirstName.IndexOf(TxtFName.Text) != -1)
                    .Where(d => d.LastName != null && d.LastName.IndexOf(TxtLName.Text) != -1)
                    .Where(e => e.Mobile != null && e.Mobile.IndexOf(TxtMobile.Text) != -1)
                    .Where(f => f.DiseaseName != null && f.DiseaseName.IndexOf(cmbDiseaseName.Text) != -1)
                    .Where(g => g.FatherName != null && g.FatherName.IndexOf(TxtFatherName.Text) != -1)
                    .Where(h => h.Phone != null && h.Phone.IndexOf(TxtPhone.Text) != -1)
                    .Where(i => i.Notes != null && i.Notes.IndexOf(TxtNotes.Text) != -1).ToList();


                BindingList<cls_Contact> list = new BindingList<cls_Contact>(contacts);
                dataGridView1.DataSource = list;

                dataGridView1.Columns[9].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[11].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[12].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                dataGridView1.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("لطفا کمی تامل فرمایید تا اطلاعات بیماران تکمیل شود.");
            }
        }

        private void TxtSSID_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtFName_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtLName_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtMobile_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtFatherName_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtPhone_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtNotes_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void TxtDiseaseName_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedContact = new cls_Contact();
            string PatientID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string DiseaseName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string FirstName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string LastName = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            string FatherName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            string SSID = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            string FullName = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            string Phone = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            string Mobile = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            string Notes = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            string Birthday = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            string Email = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
            string Address = dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString();

            selectedContact = outlookManagement.contacts.ToList().Single(m =>
            m.PatientID == PatientID
            & m.DiseaseName == DiseaseName
            & m.FirstName == FirstName
            & m.LastName == LastName
            & m.FatherName == FatherName
            & m.SSID == SSID
            & m.FullName == FullName
            & m.Phone == Phone
            & m.Mobile == Mobile
            & m.Notes == Notes);

            TxtPatientID.Text = selectedContact.PatientID;
            cmbDiseaseName.Text = selectedContact.DiseaseName;
            TxtFName.Text = selectedContact.FirstName;
            TxtLName.Text = selectedContact.LastName;
            TxtFatherName.Text = selectedContact.FatherName;
            TxtSSID.Text = selectedContact.SSID;
            TxtPhone.Text = selectedContact.Phone;
            TxtMobile.Text = selectedContact.Mobile;
            TxtNotes.Text = selectedContact.Notes;
            TxtBirthday.Text = selectedContact.Birthday;
            TxtEmail.Text = selectedContact.Email;
            TxtAddress.Text = selectedContact.Address;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TxtPatientID.Text = "";
            cmbDiseaseName.Text = "";
            TxtFName.Text = "";
            TxtLName.Text = "";
            TxtFatherName.Text = "";
            TxtSSID.Text = "";
            TxtPhone.Text = "";
            TxtMobile.Text = "";
            TxtNotes.Text = "";
            TxtBirthday.Text = "";
            TxtEmail.Text = "";
            TxtAddress.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cls_Contact newContact = new cls_Contact();
            try
            {
                DialogResult dr = MessageBox.Show(string.Format("آیا از تغییر اطلاعات بیمار به نام {0} با شماره پرونده {1} اطمینان دارید؟", selectedContact.FullName, selectedContact.PatientID), "پیغام اخطار", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    newContact.PatientID = TxtPatientID.Text;
                    newContact.DiseaseName = cmbDiseaseName.Text;
                    newContact.FirstName = TxtFName.Text;
                    newContact.LastName = TxtLName.Text;
                    newContact.FatherName = TxtFatherName.Text;
                    newContact.SSID = TxtSSID.Text;
                    newContact.Phone = TxtPhone.Text;
                    newContact.Mobile = TxtMobile.Text;
                    newContact.Notes = TxtNotes.Text;
                    newContact.Birthday = TxtBirthday.Text;
                    newContact.Email = TxtEmail.Text;
                    newContact.Address = TxtAddress.Text;
                    outlookManagement.UpdateContact(selectedContact, newContact);

                }
                RefreshGridView();
            }
            catch (Exception)
            {
                MessageBox.Show("لطفا یکی از بیماران لیست شده را با Double Click اینتخاب کنید و بعد از تغییرات کلید ثبت را بزنید...", "پیغام خطا",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign,true);
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            FrmAddNewContact frm = new FrmAddNewContact();
            frm.outlookManagement = outlookManagement;
            //frm.MdiParent = this.ParentForm;
            frm.ShowDialog();
            RefreshGridView();
        }

        private void BtnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string SourceFilesPath = Application.StartupPath + "\\PatientRecordTemplates\\";
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            try
            {

                string PatientRecordsPath =  acm.ReadSetting("PatientRecords");
                string fileName = selectedContact.PatientID;
                string FileFullPath = "";
                     
                if (selectedContact.DiseaseName == "غدد")
                {
                    SourceFilesPath += "E.Docx";
                     fileName += "E.Docx";
                }
                else
                {
                    SourceFilesPath += "D.Docx";
                    fileName += "D.Docx";
                }
                    
                FileFullPath = PatientRecordsPath + "\\" + fileName;
                if (!File.Exists(FileFullPath))
                {
                    File.Copy(SourceFilesPath, FileFullPath);

                    cls_WordManagement cls_WordManagement = new cls_WordManagement();
                    cls_WordManagement.ChangeHeaderInfo(selectedContact, FileFullPath);
                    System.Diagnostics.Process.Start(FileFullPath);
                }
                else
                    System.Diagnostics.Process.Start(FileFullPath);
            }
            catch (Exception)
            {
                MessageBox.Show("لطفا یکی از بیماران لیست شده را با Double Click انتخاب کنید تا امکان مشاهده پرونده پزشکی بیمار انتخاب شده را دشاته باشید...", "پیغام خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, true);
            }
        }

        private void cmbDiseaseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void cmbDiseaseName_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }
    }
}
