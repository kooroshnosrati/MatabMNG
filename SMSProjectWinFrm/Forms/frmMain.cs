using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm
{
    public partial class frmMain : Form
    {
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
            frm.MdiParent = this;
            frm.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            FrmSendSMStoAll frm = new FrmSendSMStoAll();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
