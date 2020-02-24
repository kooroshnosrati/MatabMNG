using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm
{
    public partial class FrmSendSMStoAll : Form
    {
        public cls_OutlookManagement outlookManagement;
        //DateTime LastDateChoosen;
        //SerialPort port = new SerialPort();
        Logger logger = new Logger();
        List<cls_Appointment> appointments = new List<cls_Appointment>();
        List<cls_Contact> contacts = new List<cls_Contact>();
        DAL dAL = new DAL();

        public FrmSendSMStoAll()
        {
            InitializeComponent();
        }
        private void FrmSendSMStoAll_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            label2.ForeColor = Color.Green;
            outlookManagement.TxtSmsCounter = textBox2;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = outlookManagement.GetContactsCount().ToString();
            if (backgroundWorker1.IsBusy != true)
                backgroundWorker1.RunWorkerAsync();

            //SMSManagement smsmanagement = new SMSManagement();
            //smsmanagement.Close();
            //smsmanagement.Open(CmbPortName.Text);
            ////smsmanagement.SendSMS(textBox1.Text, textBox2.Text);
            //smsmanagement.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                label2.Invoke(new Action(() => label2.Text = "لطفا منتظر باشید تا تمام پیغام ها ارسال گردد..."));
                label2.Invoke(new Action(() => label2.ForeColor = Color.Green));

                string StrSmsBody = "";
                button1.Invoke(new Action(() => button1.Enabled = false));
                button4.Invoke(new Action(() => button4.Enabled = false));
                textBox1.Invoke(new Action(() => StrSmsBody = textBox1.Text));
                textBox1.Invoke(new Action(() => textBox1.Enabled = false));
                
                int jobID = dAL.isJobCreated(DateTime.Now, 3, false);
                if (jobID == -1)
                    jobID = dAL.JobCreat(DateTime.Now, 3, false);
                outlookManagement.SendAnSMSToAllContacts(jobID, StrSmsBody);
            }
            catch (Exception)
            {
                label2.Invoke(new Action(() => label2.Text = "خطایی رخ داده است..."));
                label2.Invoke(new Action(() => label2.ForeColor = Color.Red));
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Invoke(new Action(() => button1.Enabled = true));
            button4.Invoke(new Action(() => button4.Enabled = true));
            textBox1.Invoke(new Action(() => textBox1.Enabled = true));
            label2.Invoke(new Action(() => label2.Text = "ارسال پیامک ها به پایان رسید..."));
            label2.Invoke(new Action(() => label2.ForeColor = Color.Green));
        }
    }
}
