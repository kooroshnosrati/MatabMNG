using System;
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
    public partial class frmConfiguration : Form
    {
        public frmConfiguration()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            acm.AddUpdateAppSettings("OutlookAccount", TxtOutllokAccount.Text);
            acm.AddUpdateAppSettings("TestPhone", TxtTestPhoneNumber.Text);
            acm.AddUpdateAppSettings("TestMessage", TxtTestMessageBody.Text);
            MessageBox.Show("تنظیمات با موفقیت ذخیره گردید.");
        }

        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            TxtOutllokAccount.Text = acm.ReadSetting("OutlookAccount");
            TxtTestPhoneNumber.Text = acm.ReadSetting("TestPhone");
            TxtTestMessageBody.Text = acm.ReadSetting("TestMessage");
        }
    }
}
