using System;
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
    public class cls_OutlookManagement
    {
        public List<cls_Contact> contacts = new List<cls_Contact>();
        public List<cls_Appointment> appointments = new List<cls_Appointment>();
        public bool IsSurfingInAppointment = false;
        Outlook.MAPIFolder defaultContactsFolder = null;
        Outlook.MAPIFolder defaultCalendarFolder = null;
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
            
            defaultContactsFolder.Items.IncludeRecurrences = true;
            Outlook.MAPIFolder ContactFolder = defaultContactsFolder;
            Outlook.Items items1 = ContactFolder.Items;
            items1.IncludeRecurrences = true;
            items1.Sort("[Title]");

            contacts.Clear();
            foreach (Outlook.ContactItem Ocontact in items1)
            {
                try
                {
                    cls_Contact contact = new cls_Contact();
                    contact.PatientID = Ocontact.Title == null ? "" : Ocontact.Title;
                    contact.DiseaseName = Ocontact.JobTitle == null ? "" : Ocontact.JobTitle;
                    contact.FirstName = Ocontact.FirstName == null ? "" : Ocontact.FirstName;
                    contact.LastName = Ocontact.LastName == null ? "" : Ocontact.LastName;
                    contact.FatherName = Ocontact.MiddleName == null ? "" : Ocontact.MiddleName;
                    contact.SSID = Ocontact.Suffix == null ? "" : Ocontact.Suffix;
                    contact.FullName = contact.FirstName + " " + contact.LastName;
                    contact.Phone = Ocontact.HomeTelephoneNumber == null ? "" : Ocontact.HomeTelephoneNumber.Replace(" ", "");
                    contact.Mobile = Ocontact.MobileTelephoneNumber == null ? "" : Ocontact.MobileTelephoneNumber.Replace(" ", "");
                    contact.Notes = Ocontact.Body == null ? "" : Ocontact.Body;
                    contacts.Add(contact);
                    if (counter++ > 500)
                        break;
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(ex.Message);
                }
            }
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

            defaultCalendarFolder.Items.IncludeRecurrences = true;
            Outlook.MAPIFolder AppointmentFolder = defaultCalendarFolder;
            Outlook.Items items1 = AppointmentFolder.Items;
            items1.IncludeRecurrences = true;
            items1.Sort("[Start]");
            //string filterStr = "[Start] >= '" + today.ToString("yyyy-MM-dd HH:mm") + "'"; // AND [End] <= '" + todayOneYearLater.ToString("g") + "'";
            string filterStr = "[Start] >= '" + today.ToString("g") + "'"; // AND [End] <= '" + todayOneYearLater.ToString("g") + "'";
            Outlook.Items items = items1.Restrict(filterStr);
            items.IncludeRecurrences = true;

            foreach (Outlook.AppointmentItem item in items) 
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
                    throw;
                }
            }
        }
        private void getfolders(Outlook.Folder folder)
        {
            if (folder.Name.ToLower() == "Contacts".ToLower())
            {
                defaultContactsFolder = folder;
            }
            if (folder.Name.ToLower() == "Calendar".ToLower())
            {
                defaultCalendarFolder = (Outlook.MAPIFolder)folder;
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
                //Outlook._Application outLookApp = new Microsoft.Office.Interop.Outlook.Application();
                Outlook.Application outLookApp = new Outlook.Application();
                Outlook.NameSpace oNS = outLookApp.GetNamespace("mapi");

                ApplicationConfigManagement acm = new ApplicationConfigManagement();
                string usrName = acm.ReadSetting("OutlookAccount").ToLower();
                string passWord = acm.ReadSetting("OutlookAccountPassword");

                oNS.Logon(usrName, passWord);
                //foreach (Outlook.Folder item in outLookApp.Session.Folders)
                foreach (Outlook.Folder item in oNS.Folders)
                {
                    if (item.Name.ToLower() == acm.ReadSetting("OutlookAccount").ToLower())
                        getfolders(item);
                }
                FillContacts();
                //FillAppointments();
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
            try
            {
                Outlook.ContactItem newContact = defaultContactsFolder.Application.CreateItem(Outlook.OlItemType.olContactItem) as Outlook.ContactItem;

                newContact.Title = contact.PatientID;
                newContact.JobTitle = contact.DiseaseName;
                newContact.FirstName = contact.FirstName;
                newContact.LastName = contact.LastName;
                newContact.MiddleName = contact.FatherName;
                newContact.Suffix = contact.SSID;
                newContact.HomeTelephoneNumber = contact.Phone;
                newContact.MobileTelephoneNumber = contact.Mobile;
                newContact.Body = contact.Notes;
                newContact.Save();
                newContact.Display(true);
                return true;
            }
            catch (Exception)
            {
                return false;
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