using SMSProjectWinFrm.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm
{

    public partial class frmAppointments : Form
    {
        int rowIndexSeleted = 0;
        DateTime selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        cls_Appointment selectedAppointment;
        bool chk = false;
        public cls_OutlookManagement outlookManagement;
        public frmAppointments()
        {
            InitializeComponent();
        }

        private void frmAppointments_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            RefreshGridView();
        }

        public void RefreshGridView()
        {
            try
            {
                List<cls_Appointment> appointment = (List<cls_Appointment>)outlookManagement.appointments.ToList().Where(a => a.Date == selectedDate).ToList();
                appointment.Sort((u1, u2) => u1.StartDateTime.CompareTo(u2.StartDateTime));

                dataGridView1.DataSource = new BindingList<cls_Appointment>(appointment); ;

                dataGridView1.Columns["StartDateTime"].DisplayIndex = 0;
                dataGridView1.Columns["EndDateTime"].DisplayIndex = 1;
                dataGridView1.Columns["Subject"].DisplayIndex = 2;
                dataGridView1.Columns["Paid"].DisplayIndex = 3;

                dataGridView1.Columns["StartDateTimeStr"].Visible = false;
                dataGridView1.Columns["EndDateTimeStr"].Visible = false;
                dataGridView1.Columns["DateStr"].Visible = false;
                dataGridView1.Columns["Date"].Visible = false;
                dataGridView1.Columns["contact"].Visible = false;

                dataGridView1.Columns["StartDateTime"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns["StartDateTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["EndDateTime"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns["EndDateTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Subject"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Paid"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.Columns["Paid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                //dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Refresh();
                //dataGridView1.Rows[rowIndexSeleted].Selected = true;

                dataGridView1.ClearSelection();
                int nRowIndex = rowIndexSeleted;

                dataGridView1.Rows[nRowIndex].Selected = true;
                dataGridView1.Rows[nRowIndex].Cells[0].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
            catch (Exception)
            {
                MessageBox.Show("لطفا کمی تامل فرمایید تا اطلاعات ویزیت ها تکمیل شود.");
            }
        }

        private void TxtDiseaseName_TextChanged(object sender, EventArgs e)
        {
            RefreshGridView();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            rowIndexSeleted = e.RowIndex;
            selectedAppointment = new cls_Appointment();
            DateTime startDateTime = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["StartDateTime"].Value.ToString());
            DateTime endDateTime = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["EndDateTime"].Value.ToString());
            string subject = dataGridView1.Rows[e.RowIndex].Cells["Subject"].Value.ToString();
            string paid = dataGridView1.Rows[e.RowIndex].Cells["Paid"].Value.ToString();
            selectedAppointment = outlookManagement.appointments.ToList().Single(m => m.StartDateTime == startDateTime & m.EndDateTime == endDateTime);
            TxtStartDateTime.Text = startDateTime.ToString();
            TxtSubject.Text = subject;
            TxtPaid.Text = paid;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //TxtPatientID.Text = "";
            //TxtDiseaseName.Text = "";
            //TxtFName.Text = "";
            //TxtLName.Text = "";
            //TxtFatherName.Text = "";
            //TxtSSID.Text = "";
            //TxtPhone.Text = "";
            //TxtMobile.Text = "";
            //TxtNotes.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cls_Appointment newAppointment = new cls_Appointment();
            try
            {
                DialogResult dr = MessageBox.Show(string.Format("آیا با ثبت ویزیت در تاریخ {0} اطمینان دارید؟", selectedAppointment.StartDateTime), "پیغام اخطار", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    newAppointment.StartDateTime = selectedAppointment.StartDateTime;
                    newAppointment.EndDateTime = selectedAppointment.EndDateTime;
                    newAppointment.Subject = TxtSubject.Text;
                    newAppointment.Paid = TxtPaid.Text;
                    outlookManagement.UpdateAppointment(selectedAppointment, newAppointment);
                }
                RefreshGridView();
            }
            catch (Exception)
            {
                MessageBox.Show("لطفا یکی از سطر ویزیت های لیست شده را با Double Click اینتخاب کنید و بعد از تغییرات کلید ثبت را بزنید...", "پیغام خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmAddNewContact frm = new FrmAddNewContact();
            frm.outlookManagement = outlookManagement;
            //frm.MdiParent = this.ParentForm;
            frm.ShowDialog();
            RefreshGridView();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            selectedDate = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);
            RefreshGridView();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

    }
}
