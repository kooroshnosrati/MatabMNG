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
    OutlookManagement outlookManagement = new OutlookManagement();
    DateTime LastDateChoosen;
    SerialPort port = new SerialPort();
    Logger logger = new Logger();
    List<Appointment> appointments = new List<Appointment>();
    List<Contact> contacts = new List<Contact>();
    DAL dAL = new DAL();
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

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //MessageBox.Show(listBox1.SelectedValue.ToString());
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
        LastDateChoosen = dateTimePicker1.Value;

        label4.Invoke(new Action(() => label4.Text = "در حال پردازش .... لطفا تامل بفرمایید..."));
        label4.Invoke(new Action(() => label4.ForeColor = Color.Red));

        button1.Invoke(new Action(() => button1.Enabled = false));
        appointments = outlookManagement.GetContactsForOneDayAppointments(dateTimePicker1.Value);

        foreach (Appointment a in appointments)
            contacts.Add(a.contact);

        listBox1.Invoke(new Action(() => listBox1.Items.Clear()));
        listBox1.Invoke(new Action(() => listBox1.DataSource = null));
        listBox1.Invoke(new Action(() => listBox1.DataSource = contacts));
        listBox1.Invoke(new Action(() => listBox1.DisplayMember = "FullName"));
        listBox1.Invoke(new Action(() => listBox1.ValueMember = "Phone"));
        listBox1.Invoke(new Action(() => listBox1.Refresh()));

        textBox1.Invoke(new Action(() => textBox1.Text = string.Format("قابل توجه بیماران گرامی.\n\rمطب خانم دکتر اشرف علی مددی در تاریخ {0} تعطیل میباشد.\n\rبرای هماهنگی ویزیت جدید حتما با شما تماس گرفته خواهد شد.", dateTimePicker1.Value.ToLongDateString())));
        button1.Invoke(new Action(() => button1.Enabled = true));
        label4.Invoke(new Action(() => label4.Text = "آماده ارسال پیغام..."));
        label4.Invoke(new Action(() => label4.ForeColor = Color.Green));
    }

    private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        button1.Invoke(new Action(() => button1.Enabled = true));
        label4.Invoke(new Action(() => label4.Text = "آماده ارسال پیغام..."));
        label4.Invoke(new Action(() => label4.ForeColor = Color.Green));
    }
    private void button2_Click(object sender, EventArgs e)
    {
        this.Close();
        this.Dispose();
    }
    private void button3_Click(object sender, EventArgs e)
    {
        string PortName = "";
        CmbPortName.Invoke(new Action(() => PortName = CmbPortName.Text));
        outlookManagement.sMSManagement.Close();
        outlookManagement.sMSManagement.Open(PortName);
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
    private void CmbPortName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGSMModem();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        SMSManagement sMSManagement = new SMSManagement();
        sMSManagement.Open(CmbPortName.Text);
        foreach (Appointment a in appointments)
        {
            if (!dAL.isSentVisitCancelSMSToMobile(a))
                sMSManagement.SendSMS(textBox1.Text, a);
            else
                MessageBox.Show(string.Format("پیغام کنسل شدن ویزیت بیمار {0} قبلا در تاریخ {1} با شنماره تلفن {2} ارسال شده است...", a.contact.FullName, a.Date, a.contact.Mobile));
        }
    }
}
