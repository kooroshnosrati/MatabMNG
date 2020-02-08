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
            //Thread t = new Thread(new ThreadStart(StartSlpashForm));
            //t.Start();
            InitializeComponent();
            
            //try
            //{
            //    //t.Suspend();
            //    //t.Interrupt();
            //    //t.Join();
            //    t.Abort();
            //}
            //catch (Exception)
            //{
            //    ;
            //}
        }
        public void StartSlpashForm()
        {
            try
            {
                //Application.Run(new FrmWarmUp());
            }
            catch (Exception)
            {
                ;
            }
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
            FrmCancelAppointment frm = new FrmCancelAppointment();
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

            frmwarmup = new FrmWarmUp();
            frmwarmup.ShowDialog();

            FrmSMSCenter frm = new FrmSMSCenter();
            frm.MdiParent = this;
            frm.Show();
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            frmContacts frm = new frmContacts();
            frm.outlookManagement = outlookManagement;
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FrmAddNewContact frm = new FrmAddNewContact();
            frm.outlookManagement = outlookManagement;
            frm.MdiParent = this;
            frm.Show();
        }

        //private void frmMain_Shown(object sender, EventArgs e)
        //{
        //    //FrmWarmUp frmwarmup = new FrmWarmUp();
        //    //frmwarmup.MdiParent = this;
        //    //frmwarmup.StartPosition = FormStartPosition.CenterParent;
        //    //frmwarmup.Show();

        //    //if (backgroundWorker1.IsBusy != true)
        //    //    backgroundWorker1.RunWorkerAsync();

        //    //while (!chk)
        //    //{
        //    //    frmwarmup.Show();
        //    //}
        //    ////Thread.Sleep(5000);
        //    ////while (!chk1)
        //    ////    Thread.Sleep(5000);

        //    //frmwarmup.Hide();

        //}

    }
}
