using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm.Forms
{
    public partial class FrmAddNewContact : Form
    {
        cls_OutlookManagement outlookManagement = new cls_OutlookManagement();
        public FrmAddNewContact()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            cls_Contact contact = new cls_Contact();
            contact.PatientID = TxtPatientID.Text;
            contact.DiseaseName = TxtDiseaseName.Text;
            contact.FirstName= TxtFName.Text;
            contact.LastName = TxtLName.Text;
            contact.FatherName = TxtFatherName.Text;
            contact.SSID = TxtSSID.Text;
            contact.Phone= TxtPhone.Text;
            contact.Mobile= TxtMobile.Text;
            contact.Notes = TxtNotes.Text;
            if (outlookManagement.AddNewContact(contact))
            {
                MessageBox.Show("بیمار جدید ثبت نام شد...");
                this.Close();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("مشکلی در ثبت نام بیمار جدید رخ داده است...");
                this.Close();
                this.Dispose();
            }
        }
    }
}
