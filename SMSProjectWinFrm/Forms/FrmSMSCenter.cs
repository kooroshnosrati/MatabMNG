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
using SMSProjectWinFrm.Business;

namespace SMSProjectWinFrm
{
    public partial class FrmSMSCenter : Form
    {
        Cls_SMSToSend sMsToSend = new Cls_SMSToSend();
        //SMSManagement sms = new SMSManagement();
        Logger logger = new Logger();
        SerialPort port = new SerialPort();

        public FrmSMSCenter()
        {
            InitializeComponent();
        }
        private bool LoadGSMModem()
        {
            try
            {
                CmbPortName.Items.Clear();
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                    CmbPortName.Items.Add(port);
                CmbPortName.SelectedIndex = 0;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
                logger.ErrorLog(ex.Message);
                return false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sMsToSend.sMSManagement = new SMSManagement();
            if (LoadGSMModem())
            {
                if (backgroundWorker1.IsBusy != true)
                    backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                if (backgroundWorker1.WorkerSupportsCancellation == true)
                    backgroundWorker1.CancelAsync();
            }
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    button1.Enabled = false;
        //    button2.Enabled = true;
        //    if (backgroundWorker1.IsBusy != true)
        //        backgroundWorker1.RunWorkerAsync();
        //}
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    button1.Enabled = true;
        //    button2.Enabled = false;
        //    if (backgroundWorker1.WorkerSupportsCancellation == true)
        //        backgroundWorker1.CancelAsync();
        //}
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (!worker.CancellationPending)
            {
                try
                {
                    sMsToSend.DoSend();
                    Thread.Sleep(10000);
                }
                catch (Exception err)
                {
                    logger.ErrorLog(err.Message + "---" + err.InnerException);
                    listBox1.Invoke(new Action(() => listBox1.Items.Insert(0, err.Message + "---" + err.InnerException)));
                    //if (backgroundWorker1.WorkerSupportsCancellation == true)
                    //    backgroundWorker1.CancelAsync();
                    //e.Cancel = true;
                    //break;
                }
                
            }
            MessageBox.Show("لطفا یک بار برنامه را ببندید و دوباره اجرا نمایید...");
            e.Cancel = true;
        }
        private void CmbPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string PortName = "";
                CmbPortName.Invoke(new Action(() => PortName = CmbPortName.Text));
                sMsToSend.sMSManagement.Close();
                sMsToSend.sMSManagement.Open(PortName);
                if (backgroundWorker1.IsBusy != true)
                    backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
                logger.ErrorLog(ex.Message);
                if (backgroundWorker1.WorkerSupportsCancellation == true)
                    backgroundWorker1.CancelAsync();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LoadGSMModem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sMsToSend.ResetUnsendSMS();
        }
    }
}
