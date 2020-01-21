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
        OutlookManagement outlookManagement = new OutlookManagement();
        DateTime LastDateChoosen;
        SerialPort port = new SerialPort();
        Logger logger = new Logger();
        List<Appointment> appointments = new List<Appointment>();
        List<Contact> contacts = new List<Contact>();
        DAL dAL = new DAL();

        public FrmSendSMStoAll()
        {
            InitializeComponent();
        }

        private void LoadGSMModem()
        {
            try
            {
                CmbPortName.Items.Clear();
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                    CmbPortName.Items.Add(port);
                CmbPortName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(ex.Message);
            }
        }

        private void FrmSendSMStoAll_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            label2.ForeColor = Color.Green;

            outlookManagement = new OutlookManagement(textBox2);
            outlookManagement.sMSManagement = new SMSManagement();
            LoadGSMModem();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadGSMModem();
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
                button3.Invoke(new Action(() => button3.Enabled = false));
                CmbPortName.Invoke(new Action(() => CmbPortName.Enabled = false));
                label2.Invoke(new Action(() => label2.Text = "لطفا منتظر باشید تا تمام پیغام ها ارسال گردد..."));
                label2.Invoke(new Action(() => label2.ForeColor = Color.Green));

                string StrSmsBody = "";
                button1.Invoke(new Action(() => button1.Enabled = false));
                button4.Invoke(new Action(() => button4.Enabled = false));
                textBox1.Invoke(new Action(() => StrSmsBody = textBox1.Text));
                textBox1.Invoke(new Action(() => textBox1.Enabled = false));

                outlookManagement.SendAnSMSToAllContacts(StrSmsBody);
            }
            catch (Exception)
            {
                label2.Invoke(new Action(() => label2.Text = "خطایی رخ داده است..."));
                label2.Invoke(new Action(() => label2.ForeColor = Color.Red));
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CmbPortName.Invoke(new Action(() => CmbPortName.Enabled = true));
            button1.Invoke(new Action(() => button1.Enabled = true));
            button4.Invoke(new Action(() => button4.Enabled = true));
            textBox1.Invoke(new Action(() => textBox1.Enabled = true));
            label2.Invoke(new Action(() => label2.Text = "ارسال پیامک ها به پایان رسید..."));
            label2.Invoke(new Action(() => label2.ForeColor = Color.Green));
        }

        private void CmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PortName = "";
            CmbPortName.Invoke(new Action(() => PortName = CmbPortName.Text));
            outlookManagement.sMSManagement.Close();
            outlookManagement.sMSManagement.Open(PortName);
        }
    }
}
