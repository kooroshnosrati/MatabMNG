﻿using System;
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
        public cls_OutlookManagement outlookManagement;
        Logger logger = new Logger();
        //SerialPort port = new SerialPort();

        public FrmAppointmentSendSMS()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            outlookManagement.listBox1 = listBox1;
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
            try
            {
                button1.Enabled = true;
                button2.Enabled = false;
                if (backgroundWorker1.WorkerSupportsCancellation == true)
                    backgroundWorker1.CancelAsync();
            }
            catch (Exception err)
            {
                int kk = 0;
            }
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
                        if (err.Message.ToLower().IndexOf("Collection was modified".ToLower()) != -1) //; enumeration operation may not execute.")
                            continue;
                        listBox1.Invoke(new Action(() => listBox1.Items.Add(err.Message + "---" + err.InnerException)));
                        e.Cancel = true;
                        break;
                    }
                }
            }
            e.Cancel = true;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button2_Click(null, null);
        }
    }
}
