using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Outlook = Microsoft.Office.Interop.Outlook;

using System.Windows.Forms;
using System.Globalization;
using SMSProjectWinFrm;
using SMSProjectWinFrm.Business;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SMSProjectWinFrm
{
    public class cls_OutlookManagement
    {
        string CurrentProfileName = "";
        List<string> Accounts = new List<string>();
        List<cls_Store> Stores = new List<cls_Store>();

        Outlook.Application OutlookApp = new Outlook.Application();
        Outlook.NameSpace ns = null;

        Outlook.Recipient CurrentUser = null;
        List<string> CurrentAccounts = new List<string>();

        Outlook.Stores stores = null;
        Outlook.Store store = null;

        Outlook.Accounts accounts = null;
        Outlook.Account account = null;

        Outlook.MAPIFolder rootFolder = null;
        Outlook.Folders folders = null;
        Outlook.MAPIFolder folder = null;
        List<string> Folders = new List<string>();

        class cls_Store
        {
            public string DisplayName;
            public string FilePath;
        }

        public List<cls_Contact> contacts = new List<cls_Contact>();
        public List<cls_Appointment> appointments = new List<cls_Appointment>();
        public bool IsSurfingInAppointment = false;
        //Outlook.MAPIFolder defaultContactsFolder = null;
        //Outlook.MAPIFolder defaultCalendarFolder = null;
        DAL dal = new DAL();
        Logger logger = new Logger();

        public ListBox listBox1 = null;
        public TextBox TxtSmsCounter = null;

        public cls_OutlookManagement()
        {
            InitializeOutlookObjects();
            //dal.ClearDatabase(); //باید کامنت شود
        }
        private void FillContacts()
        {
            int counter = 0;

            Outlook.MAPIFolder mAPIFolder = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
            Outlook.Items items = mAPIFolder.Items;
            items.Sort("[Title]");

            contacts.Clear();
            foreach (Outlook.ContactItem Ocontact in items)
            {
                try
                {
                    bool chk = false;
                    cls_Contact contact = new cls_Contact();
                    contact.PatientID = Ocontact.Title == null ? "" : Ocontact.Title;
                    contact.DiseaseName = Ocontact.JobTitle == null ? "" : Ocontact.JobTitle;
                    contact.FirstName = Ocontact.FirstName == null ? "" : Ocontact.FirstName;
                    contact.LastName = Ocontact.LastName == null ? "" : Ocontact.LastName;
                    contact.FatherName = Ocontact.MiddleName == null ? "" : Ocontact.MiddleName;
                    contact.SSID = Ocontact.Suffix == null ? "" : Ocontact.Suffix;
                    contact.FullName = contact.FirstName + " " + contact.LastName;
                    contact.Birthday = Ocontact.Birthday == null ? DateTime.MinValue : Ocontact.Birthday;
                    contact.Email = Ocontact.Email1Address == null ? "" : Ocontact.Email1Address;
                    contact.Address = Ocontact.HomeAddress == null ? "" : Ocontact.HomeAddress;

                    if (Ocontact.HomeTelephoneNumber == null)
                        contact.Phone = "";
                    else
                    {
                        if (Ocontact.HomeTelephoneNumber.IndexOf(" ") != -1)
                        {
                            contact.Phone = Ocontact.HomeTelephoneNumber.Replace(" ", "");
                            Ocontact.HomeTelephoneNumber = Ocontact.HomeTelephoneNumber.Replace(" ", "");
                            chk = true;
                        }
                        else
                            contact.Phone = Ocontact.HomeTelephoneNumber;
                    }
                    if (Ocontact.MobileTelephoneNumber == null)
                        contact.Mobile = "";
                    else
                    {
                        if (Ocontact.MobileTelephoneNumber.IndexOf(" ") != -1)
                        {
                            contact.Mobile = Ocontact.MobileTelephoneNumber.Replace(" ", "");
                            Ocontact.MobileTelephoneNumber = Ocontact.MobileTelephoneNumber.Replace(" ", "");
                            chk = true;
                        }
                        else
                            contact.Mobile = Ocontact.MobileTelephoneNumber;
                    }

                    contact.Notes = Ocontact.Body == null ? "" : Ocontact.Body;
                    contacts.Add(contact);
                    if (chk)
                        Ocontact.Save();
                    //if (counter++ > 500)
                    //    break;
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(ex.Message);
                }
            }
            if (mAPIFolder != null)
                Marshal.ReleaseComObject(mAPIFolder);
            if (items != null)
                Marshal.ReleaseComObject(items);
        }
        private void FillAppointments()
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime todayOneYearLater = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);

            appointments.Clear();
            for (DateTime SlotDateTime = today; SlotDateTime < todayOneYearLater; SlotDateTime = SlotDateTime.AddMinutes(10))
            {
                cls_Appointment appointment = new cls_Appointment();
                appointment.StartDateTimeStr = SlotDateTime.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern) + " " + SlotDateTime.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortTimePattern);
                appointment.EndDateTimeStr = SlotDateTime.AddMinutes(10).ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern) + " " + SlotDateTime.AddMinutes(10).ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortTimePattern);
                appointment.StartDateTime = SlotDateTime;
                appointment.EndDateTime = SlotDateTime.AddMinutes(10);
                appointment.Subject = " ";
                appointment.Paid = " ";
                appointment.Date = new DateTime(SlotDateTime.Year, SlotDateTime.Month, SlotDateTime.Day);
                appointment.DateStr = appointment.Date.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern) + " " + appointment.Date.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortTimePattern);
                appointments.Add(appointment);
            }

            Outlook.MAPIFolder mAPIFolder = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);
            Outlook.Items items = mAPIFolder.Items;
            items.Sort("[Start]");

            CultureInfo culture = new CultureInfo("EN-en");
            //CultureInfo culture = CultureInfo.CurrentUICulture;

            string filterStr = "[Start] >= '" + today.ToString("d", culture) + "'"; // AND [End] <= '" + todayOneYearLater.ToString("g") + "'";
            Outlook.Items FilteredItems = items.Restrict(filterStr);

            foreach (Outlook.AppointmentItem item in FilteredItems) 
            {
                try
                {
                    if (item.Start < today)
                        continue;
                    string appointmentDate = item.Start.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortDatePattern) + " " + item.Start.ToString(CultureInfo.GetCultureInfo("en-EN").DateTimeFormat.ShortTimePattern);
                    cls_Appointment appointment = appointments.Single(m => m.StartDateTimeStr == appointmentDate);
                    if (item.Subject != null)
                        appointment.Subject = item.Subject;
                    if (item.Location != null)
                        appointment.Paid = item.Location;
                }
                catch (Exception)
                {
                    ;
                }
            }
            if (mAPIFolder != null)
                Marshal.ReleaseComObject(mAPIFolder);
            if (items != null)
                Marshal.ReleaseComObject(items);
            if (FilteredItems != null)
                Marshal.ReleaseComObject(FilteredItems);
        }
        private void InitializeOutlookObjects()
        {
            try
            {
                /*You can Use one of these lines*/
                //ns = OutlookApp.GetNamespace("mapi");
                ns = OutlookApp.Session;
                //CurrentProfileName = ns.CurrentProfileName;
                //CurrentUser = ns.CurrentUser;
                foreach (Outlook.Account account in ns.Accounts)
                {
                    CurrentAccounts.Add(account.SmtpAddress);
                }
                

                ApplicationConfigManagement acm = new ApplicationConfigManagement();
                //string usrName = acm.ReadSetting("OutlookAccount").ToLower();
                //if (acm.ReadSetting("OutlookProfile").ToLower() != CurrentProfileName.ToLower())
                //{
                //    MessageBox.Show("تنظیمات Outlook شما مشکل دارد...", "پیغام خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, true);
                //    System.Environment.Exit(0);
                //}
                
                if (CurrentAccounts.IndexOf(acm.ReadSetting("OutlookAccount").ToLower()) != -1)
                {
                    MessageBox.Show("تنظیمات Outlook شما مشکل دارد...", "پیغام خطا", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign, true);
                    System.Environment.Exit(0);
                }

                FillAppointments();
                FillContacts();
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Error instantiating outlook\n {0}", err.Message));
            }
        }
        public void SendVisitConfirmationSmsToContact(cls_Appointment a, int JobID)
        {
            if (!dal.isSentSMSToMobile(a.contact.Mobile, JobID))
            {
                try
                {
                    if (a.StartDateTime > DateTime.Now)
                    {
                        string TxtBodyTemplate = dal.GetSMSTextBodyTemplateByJobID(JobID);
                        if (TxtBodyTemplate != null)
                        {
                            string bodyStr = string.Format(TxtBodyTemplate, a.contact.FullName, a.StartDateTime.ToLongDateString(), a.StartDateTime.ToShortTimeString());
                            Cls_SMS sms = new Cls_SMS();
                            sms.JobID = JobID;
                            sms.PatientID = int.Parse(a.contact.PatientID);
                            sms.MobileNumber = a.contact.Mobile;
                            sms.TxtBody = bodyStr;
                            sms.TryCount = 0;
                            sms.IsSent = false;
                            sms.ErrorTxt = "";
                            dal.InsertSmsInfoToSentSMSTable(sms);
                            if (listBox1 != null)
                                listBox1.Invoke(new Action(() => listBox1.Items.Add(a.Date + " --- " + a.contact.FullName + " --- " + a.contact.Mobile + " --- " + a.contact.PatientID)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(ex.Message);
                }
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
        private void FindContactAndSendVisitConfirmationSMS(cls_Appointment a, int JobID)
        {
            bool chk = false;
            string filterStr = "";
            Outlook.ContactItem Contact = null;
            if (a.contact.PatientID.Length > 0)
            {
                filterStr = "[Title]=\"" + a.contact.PatientID + "\"";
                Contact = OutlookContactsFind(filterStr);
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
                    Contact = OutlookContactsFind(filterStr);
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
                        Contact = OutlookContactsFind(filterStr);
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
            if (Contact != null)
                Marshal.ReleaseComObject(Contact);

            if (chk)
                SendVisitConfirmationSmsToContact(a, JobID);
        }
        private void ListOneDayAppointmentsAndSendSMS(DateTime dateTime, int JobID) 
        {
            DateTime AbsoluteDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            List<cls_Appointment> oneDayAppointments = appointments.Where(m => m.Date == AbsoluteDate).ToList();
            foreach (cls_Appointment item in oneDayAppointments)
            {
                try
                {
                    item.contact.PatientID = item.contact.Mobile = new String(item.Subject.Where(Char.IsDigit).ToArray());
                }
                catch (Exception err1)
                {
                    logger.ErrorLog(err1.Message + " ErrorFunction : ListOneDayAppointmentsAndSendSMS");
                }
                if (!string.IsNullOrEmpty(item.contact.PatientID))
                    item.contact.FullName = item.Subject.Replace(item.contact.PatientID, "").Trim();
                else
                    item.contact.FullName = item.Subject;

                FindContactAndSendVisitConfirmationSMS(item, JobID);
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
        private cls_Appointment GetContactInfo(cls_Appointment a)
        {
            string filterStr = "";
            Outlook.ContactItem Contact = null;
            if (a.contact.PatientID.Length > 0)
            {
                filterStr = "[Title]=\"" + a.contact.PatientID + "\"";
                Contact = OutlookContactsFind(filterStr);
            }
            else
                Contact = null;
            if (Contact != null)
            {
                CorrectPhoneNumber(Contact);
                a.contact.PatientID = Contact.Title;
                a.contact.Mobile = string.IsNullOrEmpty(Contact.MobileTelephoneNumber) ? "-1" : Contact.MobileTelephoneNumber;

                if (Contact != null)
                    Marshal.ReleaseComObject(Contact);
                return a;
            }
            else
            {
                if (a.contact.Mobile.Length > 0)
                {
                    filterStr = "[MobileTelephoneNumber]=\"" + a.contact.Mobile + "\"";
                    Contact = OutlookContactsFind(filterStr);
                }
                else
                    Contact = null;
                if (Contact != null)
                {
                    CorrectPhoneNumber(Contact);
                    a.contact.PatientID = Contact.Title;
                    a.contact.Mobile = Contact.MobileTelephoneNumber;

                    if (Contact != null)
                        Marshal.ReleaseComObject(Contact);

                    return a;
                }
                else
                {
                    if (a.contact.FullName.Length > 0)
                    {
                        filterStr = "[FullName]=\"" + a.contact.FullName + "\"";
                        Contact = OutlookContactsFind(filterStr);
                    }
                    else
                        Contact = null;
                    if (Contact != null)
                    {
                        CorrectPhoneNumber(Contact);
                        a.contact.PatientID = Contact.Title;
                        a.contact.Mobile = Contact.MobileTelephoneNumber;

                        if (Contact != null)
                            Marshal.ReleaseComObject(Contact);

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

            if (Contact != null)
                Marshal.ReleaseComObject(Contact);

            return null;
        }
        public List<cls_Appointment> GetContactsForOneDayAppointments(DateTime dateTime) // این قسمت باید درست شود. بخش جستجوی ویزیت ها باید از طریق Restrict انجام شود.
        {
            DateTime AbsoluteDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            List<cls_Appointment> oneDayAppointments = appointments.Where(m => m.Date == AbsoluteDate).ToList();
            List<cls_Appointment> CanceloneDayAppointments = new List<cls_Appointment>();
            foreach (cls_Appointment item in oneDayAppointments)
            {
                    try
                    {
                    item.contact.PatientID = item.contact.Mobile = new String(item.Subject.Where(Char.IsDigit).ToArray());
                    }
                    catch (Exception err1)
                    {
                        throw (err1);
                    }
                    if (!string.IsNullOrEmpty(item.contact.PatientID))
                        item.contact.FullName = item.Subject.Replace(item.contact.PatientID, "").Trim();
                    else
                        item.contact.FullName = item.Subject;
                    cls_Appointment aitem = GetContactInfo(item);
                    if (aitem != null)
                    CanceloneDayAppointments.Add(aitem);
             }
            return CanceloneDayAppointments;
        }

        public void SendAnSMSToAllContacts(int jobID, string StrSmsBody)
        {
            int counter = 0;
            foreach (cls_Contact contact in contacts)
            {
                try
                {
                    if (!dal.isSentSMSToMobile(contact.Mobile, jobID))
                    {
                        Cls_SMS sms = new Cls_SMS();
                        sms.JobID = jobID;
                        sms.PatientID = int.Parse(contact.PatientID);
                        sms.MobileNumber = contact.Mobile;
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
            return contacts.Count;
        }
        public void SendCancelNotificationToContacts(List<cls_Contact> contacts, int JobID, DateTime date)
        {
            DateTime d = new DateTime(date.Year, date.Month, date.Day);
            string TxtBodyTemplate = dal.GetSMSTextBodyTemplateByJobID(JobID);

            if (TxtBodyTemplate != null)
            {
                foreach (cls_Contact contact in contacts)
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
                }
            }
        }
        public bool AddNewContact(cls_Contact contact)
        {
            Outlook.MAPIFolder mAPIFolder = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
            Outlook.Items items = mAPIFolder.Items;
            try
            {
                Outlook.ContactItem newContact = items.Add(Outlook.OlItemType.olContactItem) as Outlook.ContactItem;

                newContact.Title = contact.PatientID;
                newContact.JobTitle = contact.DiseaseName;
                newContact.FirstName = contact.FirstName;
                newContact.LastName = contact.LastName;
                newContact.MiddleName = contact.FatherName;
                newContact.Suffix = contact.SSID;
                newContact.HomeTelephoneNumber = contact.Phone;
                newContact.MobileTelephoneNumber = contact.Mobile;
                newContact.Body = contact.Notes;
                newContact.Birthday = contact.Birthday;
                newContact.Email1Address = contact.Email;
                newContact.HomeAddress = contact.Address;

                newContact.Save();
                contacts.Add(contact);

                if (mAPIFolder != null)
                    Marshal.ReleaseComObject(mAPIFolder);
                if (items != null)
                    Marshal.ReleaseComObject(items);
                if (newContact!= null)
                    Marshal.ReleaseComObject(newContact);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateContact(cls_Contact oldContact, cls_Contact newContact)
        {
            try
            {
                string FilterStr = "";
                if (oldContact.PatientID != "")
                    FilterStr += "[Title] = '" + oldContact.PatientID + "'";
                if (oldContact.DiseaseName != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [JobTitle] = '" + oldContact.DiseaseName + "'";
                    else
                        FilterStr += "[JobTitle] = '" + oldContact.DiseaseName + "'";
                }
                if (oldContact.FirstName != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [FirstName] = '" + oldContact.FirstName + "'";
                    else
                        FilterStr += "[FirstName] = '" + oldContact.FirstName + "'";
                }
                if (oldContact.LastName != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [LastName] = '" + oldContact.LastName + "'";
                    else
                        FilterStr += "[LastName] = '" + oldContact.LastName + "'";
                }
                if (oldContact.FatherName != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [MiddleName] = '" + oldContact.FatherName + "'";
                    else
                        FilterStr += "[MiddleName] = '" + oldContact.FatherName + "'";
                }
                if (oldContact.SSID != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [Suffix] = '" + oldContact.SSID + "'";
                    else
                        FilterStr += "[Suffix] = '" + oldContact.SSID + "'";
                }
                if (oldContact.Phone != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [HomeTelephoneNumber] = '" + oldContact.Phone + "'";
                    else
                        FilterStr += "[HomeTelephoneNumber] = '" + oldContact.Phone + "'";
                }
                if (oldContact.Mobile != "")
                {
                    if (FilterStr.Length > 0)
                        FilterStr += " and [MobileTelephoneNumber] = '" + oldContact.Mobile + "'";
                    else
                        FilterStr += "[MobileTelephoneNumber] = '" + oldContact.Mobile + "'";
                }

                Outlook.ContactItem contact = OutlookContactsFind(FilterStr) as Outlook.ContactItem;
                if (contact != null)
                {
                    contact.Title = newContact.PatientID;
                    contact.JobTitle = newContact.DiseaseName;
                    contact.FirstName = newContact.FirstName;
                    contact.LastName = newContact.LastName;
                    contact.MiddleName = newContact.FatherName;
                    contact.Suffix = newContact.SSID;
                    contact.HomeTelephoneNumber = newContact.Phone;
                    contact.MobileTelephoneNumber = newContact.Mobile;
                    contact.Body = newContact.Notes;
                    contact.Birthday = newContact.Birthday;
                    contact.Email1Address = newContact.Email;
                    contact.HomeAddress = newContact.Address;

                    contact.Save();

                    oldContact.DiseaseName = newContact.DiseaseName;
                    oldContact.FatherName = newContact.FatherName;
                    oldContact.FirstName = newContact.FirstName;
                    oldContact.LastName = newContact.LastName;
                    oldContact.Mobile = newContact.Mobile;
                    oldContact.Notes = newContact.Notes;
                    oldContact.PatientID = newContact.PatientID;
                    oldContact.Phone = newContact.Phone;
                    oldContact.SSID = newContact.SSID;
                    oldContact.Birthday = newContact.Birthday;
                    oldContact.Email = newContact.Email;
                    oldContact.Address = newContact.Address;
                    oldContact.FullName = oldContact.FirstName + " " + oldContact.LastName;

                    if (contact != null)
                        Marshal.ReleaseComObject(contact);

                    return true;
                }
            }
            catch (Exception err)
            {
                logger.ErrorLog("Update Conact Info : " + err.Message);
                return false;
            }
            return false;
        }
        public bool UpdateAppointment(cls_Appointment oldAppointment, cls_Appointment newAppointment)
        {
            try
            {
                Outlook.MAPIFolder mAPIFolder = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);
                Outlook.Items items = mAPIFolder.Items;
                items.Sort("[Start]");

                CultureInfo culture = new CultureInfo("EN-en");

                string filterStr = "[Start]='" + oldAppointment.StartDateTime.ToString("g", culture) + "'";// and [End]='" + oldAppointment.EndDateTime.ToString("g") + "'";

                Outlook.AppointmentItem appointment = items.Find(filterStr);

                if (appointment != null)
                {
                    appointment.Subject = newAppointment.Subject;
                    appointment.Location = newAppointment.Paid;
                    appointment.Save();

                    oldAppointment.Subject = newAppointment.Subject;
                    oldAppointment.Paid = newAppointment.Paid;

                    if (mAPIFolder != null)
                        Marshal.ReleaseComObject(mAPIFolder);
                    if (items != null)
                        Marshal.ReleaseComObject(items);
                    if (appointment != null)
                        Marshal.ReleaseComObject(appointment);
                    return true;
                }
                else
                {
                    Outlook.AppointmentItem AddnewAppointment = items.Add(Outlook.OlItemType.olAppointmentItem) as Outlook.AppointmentItem;
                    AddnewAppointment.Start = newAppointment.StartDateTime;
                    AddnewAppointment.End = newAppointment.EndDateTime;
                    AddnewAppointment.Subject = newAppointment.Subject;
                    AddnewAppointment.Location = newAppointment.Paid;
                    AddnewAppointment.Save();
                    oldAppointment.Subject = newAppointment.Subject;
                    oldAppointment.Paid = newAppointment.Paid;

                    if (mAPIFolder != null)
                        Marshal.ReleaseComObject(mAPIFolder);
                    if (items != null)
                        Marshal.ReleaseComObject(items);
                    if (AddnewAppointment != null)
                        Marshal.ReleaseComObject(AddnewAppointment);

                    return true;
                }
            }
            catch (Exception err)
            {
                logger.ErrorLog("Update Conact Info : " + err.Message);
                return false;
            }
            return false;
        }
        public Outlook.ContactItem OutlookContactsFind(string filterStr)
        {
            Outlook.MAPIFolder mAPIFolder = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
            Outlook.Items items = mAPIFolder.Items;
            Outlook.ContactItem ResultItem = null;
            items.Sort("[Title]");
            try
            {
                ResultItem = items.Find(filterStr);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (mAPIFolder != null)
                    Marshal.ReleaseComObject(mAPIFolder);
                if (items != null)
                    Marshal.ReleaseComObject(items);
            }
            return ResultItem;
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