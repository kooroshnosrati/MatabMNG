using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMSProjectWinFrm.Business;

namespace SMSProjectWinFrm.Forms
{
    public partial class FrmAddNewContact : Form
    {
        public cls_OutlookManagement outlookManagement;
        public FrmAddNewContact()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            cls_Contact contact = new cls_Contact();
            contact.PatientID = TxtPatientID.Text;
            contact.DiseaseName = cmbDiseaseName.Text;
            contact.FirstName= TxtFName.Text;
            contact.LastName = TxtLName.Text;
            contact.FullName = TxtFName.Text + " " + TxtLName.Text;
            contact.FatherName = TxtFatherName.Text;
            contact.SSID = TxtSSID.Text;
            contact.Phone= TxtPhone.Text;
            contact.Mobile= TxtMobile.Text;
            contact.Notes = TxtNotes.Text;
            contact.Birthday = TxtBirthday.Text;
            contact.Email = TxtEmail.Text;
            contact.Address = TxtAddress.Text;
            if (outlookManagement.AddNewContact(contact))
            {
                MessageBox.Show("بیمار جدید ثبت نام شد...");

                string SourceFilesPath = Application.StartupPath + "\\PatientRecordTemplates\\";
                ApplicationConfigManagement acm = new ApplicationConfigManagement();
                try
                {

                    string PatientRecordsPath = acm.ReadSetting("PatientRecords");
                    string fileName = contact.PatientID;
                    string FileFullPath = "";

                    if (contact.DiseaseName == "غدد")
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
                        cls_WordManagement.ChangeHeaderInfo(contact, FileFullPath);
                        System.Diagnostics.Process.Start(FileFullPath);
                    }
                    else
                        System.Diagnostics.Process.Start(FileFullPath);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "پیغام خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, true);
                }


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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void FrmAddNewContact_Load(object sender, EventArgs e)
        {
            //dtpBirthDay.Value = DateTime.Now.ToString(;
            cmbDiseaseName.SelectedIndex = 0;
        }
    }
}
