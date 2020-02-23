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
    public partial class frmTestSendSMS : Form
    {
        Logger logger = new Logger();
        SerialPort port = new SerialPort();

        public frmTestSendSMS()
        {
            InitializeComponent();
        }

        //private void LoadGSMModem()
        //{
        //    try
        //    {
        //        CmbPortName.Items.Clear();
        //        string[] ports = SerialPort.GetPortNames();
        //        foreach (string port in ports)
        //            CmbPortName.Items.Add(port);
        //        CmbPortName.SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.ErrorLog(ex.Message);
        //    }
        //}

        private void frmTestSendSMS_Load(object sender, EventArgs e)
        {
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            textBox1.Text = acm.ReadSetting("TestMessage");
            textBox2.Text = acm.ReadSetting("TestPhone");
            //LoadGSMModem();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //LoadGSMModem();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ResultStr = "";
            com.parsgreen.login.SendSMS.SendSMS sendSMS = new com.parsgreen.login.SendSMS.SendSMS();
            sendSMS.Send("BAD97BEB-291D-40F1-A560-73512A7D3B3C","09195614157", textBox1.Text, ref ResultStr);
            //SMSManagement smsmanagement = new SMSManagement();
            //smsmanagement.Close();
            //smsmanagement.Open(CmbPortName.Text);
            //smsmanagement.SendSMS(textBox1.Text, textBox2.Text);
            //smsmanagement.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

    }
}
