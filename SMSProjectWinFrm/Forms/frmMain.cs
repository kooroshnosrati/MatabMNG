using SMSProjectWinFrm.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm
{
    public partial class frmMain : Form
    {
        FrmWarmUp frmwarmup;
        bool chk = false, chk1 = false;
        cls_OutlookManagement outlookManagement;
        public frmMain()
        {
            InitializeComponent();
        }
        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            Application.Exit();
        }

        private void اطلاعرسانیاتوماتیکویزیتبیمارانToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAppointmentSendSMS frm = new FrmAppointmentSendSMS();
            frm.outlookManagement = outlookManagement;
            //outlookManagement.listBox1 = frm.listBox1;
            frm.MdiParent = this;
            frm.Show();
        }

        private void تستارسالپیامکToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestSendSMS frm = new frmTestSendSMS();
            frm.MdiParent = this;
            frm.Show();
        }

        private void تنظیماتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfiguration frm = new frmConfiguration();
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmGroupNotificationForAppointments frm = new FrmGroupNotificationForAppointments();
            frm.outlookManagement = outlookManagement;
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FrmSendSMStoAll frm = new FrmSendSMStoAll();
            frm.outlookManagement = outlookManagement;
            //frm.tx = frm.te
            frm.MdiParent = this;
            frm.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
                backgroundWorker1.RunWorkerAsync();
            this.Hide();
            frmwarmup = new FrmWarmUp();
            frmwarmup.ShowDialog();

            this.Show();


        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            outlookManagement = new cls_OutlookManagement();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            frmwarmup.Hide();
            frmwarmup.Close();
            frmwarmup.Dispose();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmAppointments frm = new frmAppointments();
            frm.outlookManagement = outlookManagement;
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            FrmSMSCenter frm = new FrmSMSCenter();
            frm.MdiParent = this;
            frm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (outlookManagement != null)
                {
                    toolStripStatusLabel1.Text = "آماده سازی مشخصات بیماران " + outlookManagement.contacts.Count + ". تعداد کل: " + outlookManagement.ContactTotalCount;
                    toolStripStatusLabel3.Text = "آماده سازی ویزیت ها . بروز رسانی برای روز " + outlookManagement.AppointmentInitialDate;
                }
            }
            catch (Exception)
            {
                ;
            }

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            frmContacts frm = new frmContacts();
            frm.outlookManagement = outlookManagement;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
