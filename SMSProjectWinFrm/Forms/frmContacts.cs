﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


        private void frmContacts_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            RefreshGridView();
        }

        private void TxtPatientID_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        public void RefreshGridView()
        {
            List<cls_Contact> contacts = (List<cls_Contact>)outlookManagement.contacts
                .Where(a => a.PatientID != null && a.PatientID.IndexOf(TxtPatientID.Text) != -1)
                .Where(b => b.SSID != null && b.SSID.IndexOf(TxtSSID.Text) != -1)
                .Where(c => c.FirstName != null && c.FirstName.IndexOf(TxtFName.Text) != -1)
                .Where(d => d.LastName != null && d.LastName.IndexOf(TxtLName.Text) != -1)
                .Where(e => e.Mobile != null && e.Mobile.IndexOf(TxtMobile.Text) != -1)
                .Where(f => f.DiseaseName != null && f.DiseaseName.IndexOf(TxtDiseaseName.Text) != -1)
                .Where(g => g.FatherName != null && g.FatherName.IndexOf(TxtFatherName.Text) != -1)
                .Where(h => h.Phone != null && h.Phone.IndexOf(TxtPhone.Text) != -1)
                .Where(i => i.Notes != null && i.Notes.IndexOf(TxtNotes.Text) != -1).ToList();


            BindingList<cls_Contact> list = new BindingList<cls_Contact>(contacts);
            dataGridView1.DataSource = list;
            
            dataGridView1.Columns[9].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Refresh();
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
            selectedContact = outlookManagement.contacts.Single(m =>
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
            TxtDiseaseName.Text = selectedContact.DiseaseName;
            TxtFName.Text = selectedContact.FirstName;
            TxtLName.Text = selectedContact.LastName;
            TxtFatherName.Text = selectedContact.FatherName;
            TxtSSID.Text = selectedContact.SSID;
            TxtPhone.Text = selectedContact.Phone;
            TxtMobile.Text = selectedContact.Mobile;
            TxtNotes.Text = selectedContact.Notes;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TxtPatientID.Text = "";
            TxtDiseaseName.Text = "";
            TxtFName.Text = "";
            TxtLName.Text = "";
            TxtFatherName.Text = "";
            TxtSSID.Text = "";
            TxtPhone.Text = "";
            TxtMobile.Text = "";
            TxtNotes.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(string.Format("آیا از تغییر اطلاعات بیمار به نام {0} با شماره پرونده {1} اطمینان دارید؟", selectedContact.FullName, selectedContact.PatientID), "پیغام اخطار", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                selectedContact.PatientID = TxtPatientID.Text;
                selectedContact.DiseaseName = TxtDiseaseName.Text;
                selectedContact.FirstName = TxtFName.Text;
                selectedContact.LastName = TxtLName.Text;
                selectedContact.FatherName = TxtFatherName.Text;
                selectedContact.SSID = TxtSSID.Text;
                selectedContact.Phone = TxtPhone.Text;
                selectedContact.Mobile = TxtMobile.Text;
                selectedContact.Notes = TxtNotes.Text;
            }
            RefreshGridView();
        }
    }
}
