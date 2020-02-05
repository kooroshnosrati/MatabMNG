using SMSProjectWinFrm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class FrmCancelAppointment : Form
{
    public cls_OutlookManagement outlookManagement;
    DateTime LastDateChoosen;
    SerialPort port = new SerialPort();
    Logger logger = new Logger();
    List<cls_Appointment> appointments = new List<cls_Appointment>();
    List<cls_Contact> contacts = new List<cls_Contact>();
    DAL dal = new DAL();
    bool chk = false;
    public FrmCancelAppointment()
    {
        InitializeComponent();
    }
    private void FrmCancelAppointment_Load(object sender, EventArgs e)
    {
        dateTimePicker1.Value = DateTime.Now;
    }
    private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
    {
        if (new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
        {
            MessageBox.Show("لطفا تاریخ مناسب برای اعلام کنسلی ویزیت بیماران انتخاب بفرمایید...");
            return;
        }
        if (!backgroundWorker1.IsBusy)
            backgroundWorker1.RunWorkerAsync();
        else
        {
            dateTimePicker1.Value = LastDateChoosen;
            MessageBox.Show("سیستم در حال پردازش است لطفا تامل بفرمایید...");
        }

    }
    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            LastDateChoosen = dateTimePicker1.Value;

            label4.Invoke(new Action(() => label4.Text = "در حال پردازش .... لطفا تامل بفرمایید..."));
            label4.Invoke(new Action(() => label4.ForeColor = Color.Red));

            button1.Invoke(new Action(() => button1.Enabled = false));
            appointments.Clear();
            appointments = outlookManagement.GetContactsForOneDayAppointments(dateTimePicker1.Value);
            listBox1.Invoke(new Action(() => listBox1.DataSource = null));
            listBox1.Invoke(new Action(() => listBox1.Items.Clear()));
            listBox1.Invoke(new Action(() => listBox1.DataSource = null));
            if (appointments.Count > 0)
            {
                contacts.Clear();
                foreach (cls_Appointment a in appointments)
                    contacts.Add(a.contact);

                listBox1.Invoke(new Action(() => listBox1.DataSource = contacts));
                listBox1.Invoke(new Action(() => listBox1.DisplayMember = "FullName"));
                listBox1.Invoke(new Action(() => listBox1.ValueMember = "Mobile"));
                listBox1.Invoke(new Action(() => listBox1.Refresh()));
                textBox1.Invoke(new Action(() => textBox1.Text = string.Format(dal.GetSMSTextBodyTemplateByCategoryID(2), dateTimePicker1.Value.ToLongDateString())));
                button1.Invoke(new Action(() => button1.Enabled = true));
                label4.Invoke(new Action(() => label4.Text = "آماده ارسال پیغام..."));
                label4.Invoke(new Action(() => label4.ForeColor = Color.Green));
                chk = true;
            }
            else
            {
                label4.Invoke(new Action(() => label4.Text = "در این تاریخ ویزیت صادر نشده است..."));
                label4.Invoke(new Action(() => label4.ForeColor = Color.Red));
                chk = false;
            }
        }
        catch (Exception)
        {
            label4.Invoke(new Action(() => label4.Text = "خطایی رخ داده است..."));
            label4.Invoke(new Action(() => label4.ForeColor = Color.Red));
            chk = false;
        }
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (chk)
        {
            button1.Invoke(new Action(() => button1.Enabled = true));
            label4.Invoke(new Action(() => label4.Text = "آماده ارسال پیغام..."));
            label4.Invoke(new Action(() => label4.ForeColor = Color.Green));
        }
    }
    private void button2_Click(object sender, EventArgs e)
    {
        this.Close();
        this.Dispose();
    }
    private void button1_Click(object sender, EventArgs e)
    {
        int jobID = dal.isJobCreated(LastDateChoosen, 2, true);
        if (jobID == -1)
            jobID = dal.JobCreat(LastDateChoosen, 2, true);
        outlookManagement.SendCancelNotificationToContacts(contacts, jobID, LastDateChoosen);
        MessageBox.Show(string.Format("ارسال پیغام کنسل شدن ویزیت در تاریخ {0} انجام شد...", LastDateChoosen.ToLongDateString()));
    }
}
