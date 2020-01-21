using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Threading;
using System.IO.Ports;


namespace SMSProjectWinFrm
{
    public partial class FrmAppointmentSendSMS : Form
    {
        OutlookManagement outlookManagement = null;
        Logger logger = new Logger();
        SerialPort port = new SerialPort();

        public FrmAppointmentSendSMS()
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
        private void Form1_Load(object sender, EventArgs e)
        {
            outlookManagement = new OutlookManagement(listBox1);
            outlookManagement.sMSManagement = new SMSManagement();
            LoadGSMModem();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            if (backgroundWorker1.IsBusy != true)
                backgroundWorker1.RunWorkerAsync();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            if (backgroundWorker1.WorkerSupportsCancellation == true)
                backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (!worker.CancellationPending)
            {
                if (!outlookManagement.IsSurfingInAppointment)
                {
                    try
                    {
                        outlookManagement.ListForwardAppointmentsAndSendSMS();
                    }
                    catch (Exception err)
                    {
                        listBox1.Invoke(new Action(() => listBox1.Items.Add(err.Message + "---" + err.InnerException)));
                        button2_Click(null, null);
                        e.Cancel = true;
                        break;
                    }
                }
            }
            e.Cancel = true;
        }
        private void CmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PortName = "";
            CmbPortName.Invoke(new Action(() => PortName = CmbPortName.Text));
            outlookManagement.sMSManagement.Close();
            outlookManagement.sMSManagement.Open(PortName);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LoadGSMModem();
        }
    }
}
