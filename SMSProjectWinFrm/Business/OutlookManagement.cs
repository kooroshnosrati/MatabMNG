﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Outlook = Microsoft.Office.Interop.Outlook;
using System.Windows.Forms;
using System.Globalization;
using SMSProjectWinFrm;
using SMSProjectWinFrm.Business;

namespace SMSProjectWinFrm
{
    public class OutlookManagement
    {
        public bool IsSurfingInAppointment = false;
        Outlook.MAPIFolder defaultContactsFolder = null;
        Outlook.MAPIFolder defaultCalendarFolder = null;
        DAL dal = new DAL();
        ListBox listBox1 = null;
        Logger logger = new Logger();
        TextBox TxtSmsCounter = null;

        public SMSManagement sMSManagement { get; set; }
        private void getfolders(Outlook.Folder folder)
        {
            if (folder.Name.ToLower() == "Contacts".ToLower())
            {
                defaultContactsFolder = folder;
            }
            if (folder.Name.ToLower() == "Calendar".ToLower())
            {
                defaultCalendarFolder = folder;
            }
            //string addrname = folder.AddressBookName;
            //string foldername = folder.Name;
            //string storeID = folder.StoreID;
            //string entryID = folder.EntryID;
            //int kk = 0;
            if (folder.Folders != null)
            {
                foreach (Outlook.Folder item in folder.Folders)
                {
                    getfolders(item);
                }
            }
            else
                return;

        }
        private void InitializeOutlookObjects()
        {
            try
            {
                //initialise an instance of Outlook object
                Outlook._Application outLookApp = new Microsoft.Office.Interop.Outlook.Application();
                ApplicationConfigManagement acm = new ApplicationConfigManagement();
                foreach (Outlook.Folder item in outLookApp.Session.Folders)
                {
                    if (item.Name.ToLower() == acm.ReadSetting("OutlookAccount").ToLower())
                        getfolders(item);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Error instantiating outlook\n {0}", err.Message));
            }
        }
        public OutlookManagement()
        {
            InitializeOutlookObjects();
            //dal.ClearDatabase(); //باید کامنت شود
        }
        public OutlookManagement(ListBox listBox)
        {
            listBox1 = listBox;
            InitializeOutlookObjects();
            //dal.ClearDatabase(); //باید کامنت شود
        }
        public OutlookManagement(TextBox txtbox)
        {
            TxtSmsCounter = txtbox;
            InitializeOutlookObjects();
            //dal.ClearDatabase(); //باید کامنت شود
        }
        public void SendVisitConfirmationSmsToContact(Appointment a, int JobID)
        {
            if (!dal.isSentSMSToMobile(a.contact.Mobile, JobID))
            {
                //.............................................. Send SMS ....................................................
                try
                {
                    if (a.AppointmentDateTime > DateTime.Now)
                    {
                        string TxtBodyTemplate = dal.GetSMSTextBodyTemplateByJobID(JobID);
                        if (TxtBodyTemplate != null)
                        {
                            string bodyStr = string.Format(TxtBodyTemplate, a.contact.FullName, a.AppointmentDateTime.ToLongDateString(), a.AppointmentDateTime.ToShortTimeString());
                            Cls_SMS sms = new Cls_SMS();
                            sms.JobID = JobID;
                            sms.PatientID = int.Parse(a.contact.PatientID);
                            sms.MobileNumber = a.contact.Mobile;
                            sms.TxtBody = bodyStr;
                            sms.TryCount = 0;
                            sms.IsSent = false;
                            sms.ErrorTxt = "";
                            dal.InsertSmsInfoToSentSMSTable(sms);
                            //sMSManagement.SendSMS(bodyStr, a);
                            //dal.AddMobileToSendVisitConfirmationSMS(a);
                            if (listBox1 != null)
                                listBox1.Invoke(new Action(() => listBox1.Items.Add(a.Date + " --- " + a.contact.FullName + " --- " + a.contact.Mobile + " --- " + a.contact.PatientID)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("اتصال با مودم با مشکل مواجه شده است.");
                    logger.ErrorLog(ex.Message);
                    //throw (ex);
                }
                //.............................................. Send SMS ....................................................

            }
        }
        private void CorrectPhoneNumber(Outlook.ContactItem contact)
        {
            if (!string.IsNullOrEmpty(contact.MobileTelephoneNumber))
            {
                contact.MobileTelephoneNumber = contact.MobileTelephoneNumber.StartsWith("0") ? contact.MobileTelephoneNumber.Replace(" ", "") : "0" + contact.MobileTelephoneNumber.Replace(" ", "");
                contact.Save();
            }
        }
        private void FindContactAndSendVisitConfirmationSMS(Appointment a, int JobID)
        {
            bool chk = false;
            string filterStr = "";
            Outlook.ContactItem Contact = null;
            if (a.contact.PatientID.Length > 0)
            {
                filterStr = "[Title]=\"" + a.contact.PatientID + "\"";
                Contact = defaultContactsFolder.Items.Find(filterStr);
            }
            else
                Contact = null;

            if (Contact != null)
            {
                CorrectPhoneNumber(Contact);
                a.contact.PatientID = Contact.Title;
                a.contact.Mobile = string.IsNullOrEmpty(Contact.MobileTelephoneNumber) ? "-1" : Contact.MobileTelephoneNumber;
                chk = true;
            }
            else
            {
                if (a.contact.Mobile.Length > 0)
                {
                    filterStr = "[MobileTelephoneNumber]=\"" + a.contact.Mobile + "\"";
                    Contact = defaultContactsFolder.Items.Find(filterStr);
                }
                else
                    Contact = null;
                if (Contact != null)
                {
                    CorrectPhoneNumber(Contact);
                    a.contact.PatientID = Contact.Title;
                    a.contact.Mobile = Contact.MobileTelephoneNumber;
                    chk = true;
                }
                else
                {
                    if (a.contact.FullName.Length > 0)
                    {
                        filterStr = "[FullName]=\"" + a.contact.FullName + "\"";
                        Contact = defaultContactsFolder.Items.Find(filterStr);
                    }
                    else
                        Contact = null;
                    if (Contact != null)
                    {
                        CorrectPhoneNumber(Contact);
                        a.contact.PatientID = Contact.Title;
                        a.contact.Mobile = Contact.MobileTelephoneNumber;
                        chk = true;
                    }
                    else
                    {
                        if (a.contact.Mobile.Length > 0)
                        {
                            a.contact.PatientID = "-1";
                            chk = true;
                        }
                    }
                }
            }
            if (chk)
                SendVisitConfirmationSmsToContact(a, JobID);
        }
        private void ListOneDayAppointmentsAndSendSMS(DateTime dateTime, int JobID)
        {
            DateTime startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            string AimDate = startDate.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern);

            foreach (Outlook.AppointmentItem item in defaultCalendarFolder.Items)
            {
                string appointmentDate = item.Start.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern);
                if (appointmentDate == AimDate)
                {
                    Appointment a = new Appointment();
                    a.Date = AimDate;
                    a.AppointmentDateTime = item.Start;
                    try
                    {
                        a.contact.PatientID = a.contact.Mobile = new String(item.Subject.Where(Char.IsDigit).ToArray());
                    }
                    catch (Exception err1)
                    {
                        logger.ErrorLog(err1.Message + " ErrorFunction : ListOneDayAppointmentsAndSendSMS");
                        //throw (err1);
                    }
                    a.Subject = item.Subject;
                    if (!string.IsNullOrEmpty(a.contact.PatientID))
                        a.contact.FullName = item.Subject.Replace(a.contact.PatientID, "").Trim();
                    else
                        a.contact.FullName = item.Subject;
                    //a.Start = item.Start;

                    FindContactAndSendVisitConfirmationSMS(a, JobID);
                }
            }
        }
        public void ListForwardAppointmentsAndSendSMS()
        {
            IsSurfingInAppointment = true;
            try
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                {
                    for (short TimeAfterToSendSMS = 0; TimeAfterToSendSMS < 5; TimeAfterToSendSMS++)
                    {
                        DateTime now = DateTime.Now.AddDays(TimeAfterToSendSMS);
                        
                        int jobID = dal.isJobCreated(now, 1, true);
                        if ( jobID == -1)
                            jobID = dal.JobCreat(now, 1, true);
                        ListOneDayAppointmentsAndSendSMS(now, jobID);
                    }
                }
                else
                {
                    for (short TimeAfterToSendSMS = 0; TimeAfterToSendSMS < 3; TimeAfterToSendSMS++)
                    {
                        DateTime now = DateTime.Now.AddDays(TimeAfterToSendSMS);
                        
                        int jobID = dal.isJobCreated(now, 1, true);
                        if (jobID == -1)
                            jobID = dal.JobCreat(now, 1, true);
                        ListOneDayAppointmentsAndSendSMS(now, jobID);
                    }
                }
            }
            catch (Exception err)
            {
                throw (err);
            }
            IsSurfingInAppointment = false;
        }
        private Appointment GetContactInfo(Appointment a)
        {
            string filterStr = "";
            Outlook.ContactItem Contact = null;
            if (a.contact.PatientID.Length > 0)
            {
                filterStr = "[Title]=\"" + a.contact.PatientID + "\"";
                Contact = defaultContactsFolder.Items.Find(filterStr);
            }
            else
                Contact = null;
            if (Contact != null)
            {
                CorrectPhoneNumber(Contact);
                a.contact.PatientID = Contact.Title;
                a.contact.Mobile = string.IsNullOrEmpty(Contact.MobileTelephoneNumber) ? "-1" : Contact.MobileTelephoneNumber;
                return a;
            }
            else
            {
                if (a.contact.Mobile.Length > 0)
                {
                    filterStr = "[MobileTelephoneNumber]=\"" + a.contact.Mobile + "\"";
                    Contact = defaultContactsFolder.Items.Find(filterStr);
                }
                else
                    Contact = null;
                if (Contact != null)
                {
                    CorrectPhoneNumber(Contact);
                    a.contact.PatientID = Contact.Title;
                    a.contact.Mobile = Contact.MobileTelephoneNumber;
                    return a;
                }
                else
                {
                    if (a.contact.FullName.Length > 0)
                    {
                        filterStr = "[FullName]=\"" + a.contact.FullName + "\"";
                        Contact = defaultContactsFolder.Items.Find(filterStr);
                    }
                    else
                        Contact = null;
                    if (Contact != null)
                    {
                        CorrectPhoneNumber(Contact);
                        a.contact.PatientID = Contact.Title;
                        a.contact.Mobile = Contact.MobileTelephoneNumber;
                        return a;
                    }
                    else
                    {
                        if (a.contact.Mobile.Length > 0)
                        {
                            a.contact.PatientID = "-1";
                            return a;
                        }
                    }
                }
            }

            return null;
        }
        public List<Appointment> GetContactsForOneDayAppointments(DateTime dateTime)
        {
            List<Appointment> appointments = new List<Appointment>();
            DateTime startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            string AimDate = startDate.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern);

            foreach (Outlook.AppointmentItem item in defaultCalendarFolder.Items)
            {
                string appointmentDate = item.Start.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern);
                if (appointmentDate == AimDate)
                {
                    Appointment a = new Appointment();
                    a.Date = AimDate;
                    a.AppointmentDateTime = item.Start;
                    try
                    {
                        a.contact.PatientID = a.contact.Mobile = new String(item.Subject.Where(Char.IsDigit).ToArray());
                    }
                    catch (Exception err1)
                    {
                        throw (err1);
                    }
                    a.Subject = item.Subject;
                    if (!string.IsNullOrEmpty(a.contact.PatientID))
                        a.contact.FullName = item.Subject.Replace(a.contact.PatientID, "").Trim();
                    else
                        a.contact.FullName = item.Subject;
                    a = GetContactInfo(a);
                    if (a != null)
                        appointments.Add(a);
                }
            }
            return appointments;
        }
        public void SendAnSMSToAllContacts(int jobID, string StrSmsBody)
        {
            int counter = 0;
            foreach (Outlook.ContactItem contact in defaultContactsFolder.Items)
            {
                try
                {
                    if (!dal.isSentSMSToMobile(contact.MobileTelephoneNumber, jobID))
                    {
                        Cls_SMS sms = new Cls_SMS();
                        sms.JobID = jobID;
                        sms.PatientID = int.Parse(contact.Title);
                        sms.MobileNumber = contact.MobileTelephoneNumber;
                        sms.TxtBody = StrSmsBody;
                        sms.TryCount = 0;
                        sms.IsSent = false;
                        sms.ErrorTxt = "";
                        if (!dal.isSentSMSToMobile(sms.MobileNumber, sms.JobID))
                            dal.InsertSmsInfoToSentSMSTable(sms);
                    }
                    ++counter;
                    TxtSmsCounter.Invoke(new Action(() => TxtSmsCounter.Text = counter.ToString()));
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("اتصال با مودم با مشکل مواجه شده است.");
                    logger.ErrorLog(ex.Message);
                    //throw (ex);
                }
            }
        }
        public long GetContactsCount()
        {
            return defaultContactsFolder.Items.Count;
        }
        public void SendCancelNotificationToContacts(List<Contact> contacts, int JobID, DateTime date)
        {
            DateTime d = new DateTime(date.Year, date.Month, date.Day);
            string TxtBodyTemplate = dal.GetSMSTextBodyTemplateByJobID(JobID);

            if (TxtBodyTemplate != null)
            {
                //listBox1.Invoke(new Action(() => listBox1.Items.Clear()));
                foreach (Contact contact in contacts)
                {
                    string bodyStr = string.Format(TxtBodyTemplate, d.ToLongDateString());
                    Cls_SMS sms = new Cls_SMS();
                    sms.JobID = JobID;
                    sms.PatientID = int.Parse(contact.PatientID);
                    sms.MobileNumber = contact.Mobile;
                    sms.TxtBody = bodyStr;
                    sms.TryCount = 0;
                    sms.IsSent = false;
                    sms.ErrorTxt = "";
                    if (!dal.isSentSMSToMobile(sms.MobileNumber, sms.JobID))
                        dal.InsertSmsInfoToSentSMSTable(sms);
                    //if (listBox1 != null)
                    //    listBox1.Invoke(new Action(() => listBox1.Items.Add(contact.FullName + " --- " + contact.Mobile + " --- " + contact.PatientID)));
                        
                }
            }
        }
    }

    //private void FindContactEmailByName(string firstName, string lastName)
    //{
    //    Microsoft.Office.Interop.Outlook.Application outlook;
    //    outlook = new Microsoft.Office.Interop.Outlook.Application();

    //    Outlook.MAPIFolder contactsFolder =
    //    outlook.GetNamespace("MAPI").GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
    //    Outlook.Items contactItems = contactsFolder.Items;
    //    try
    //    {
    //        Outlook.ContactItem contact = (Outlook.ContactItem)contactItems.Find(String.Format("[FirstName]='{0}' and " + "[LastName]='{1}'", firstName, lastName));
    //        if (contact != null)
    //        {
    //            contact.Display(true);
    //        }
    //        else
    //        {
    //            MessageBox.Show("The contact information was not found.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}