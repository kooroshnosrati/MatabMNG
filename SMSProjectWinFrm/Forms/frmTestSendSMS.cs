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

using SMSProjectWinFrm.Business;

namespace SMSProjectWinFrm
{
    public partial class frmTestSendSMS : Form
    {
        Logger logger = new Logger();
        SerialPort port = new SerialPort();
        SMSManagement sMSManagement = new SMSManagement();

        public frmTestSendSMS()
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

        private void frmTestSendSMS_Load(object sender, EventArgs e)
        {
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            textBox1.Text = acm.ReadSetting("TestMessage");
            textBox2.Text = acm.ReadSetting("TestPhone");
            LoadGSMModem();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadGSMModem();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ResultStr = "";
            sMSManagement.Close();
            sMSManagement.Open(CmbPortName.Text);
            sMSManagement.SendSMS(textBox1.Text, textBox2.Text);
            sMSManagement.Close();

            com.parsgreen.login.SendSMS.SendSMS sendSMS = new com.parsgreen.login.SendSMS.SendSMS();
            sendSMS.Send("BAD97BEB-291D-40F1-A560-73512A7D3B3C", "09195614157", textBox1.Text, ref ResultStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void CmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string PortName = "";
                CmbPortName.Invoke(new Action(() => PortName = CmbPortName.Text));
                sMSManagement.Close();
                sMSManagement.Open(PortName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
                logger.ErrorLog(ex.Message);
            }
        }
    }
}