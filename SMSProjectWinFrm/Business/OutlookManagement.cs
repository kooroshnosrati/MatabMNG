using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Outlook = Microsoft.Office.Interop.Outlook;
using System.Windows.Forms;
using System.Globalization;

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
        public void SendVisitConfirmationSmsToContact(Appointment a)
        {
            if (!dal.isSentVisitConfirmationSMSToMobile(a))
            {
                //.............................................. Send SMS ....................................................
                try
                {
                    if (a.AppointmentDateTime > DateTime.Now)
                    {
                        string bodyStr = string.Format("آقای/خانم\n{0} نوبت ويزيت شما براي خانم دكتر علي مددي تاريخ {1} ساعت {2} میباشد.", a.contact.FullName, a.AppointmentDateTime.ToLongDateString(), a.AppointmentDateTime.ToShortTimeString());
                        sMSManagement.SendSMS(bodyStr, a);
                        dal.AddMobileToSendVisitConfirmationSMS(a);
                        if (listBox1 != null)
                            listBox1.Invoke(new Action(() => listBox1.Items.Add(a.Date + " --- " + a.contact.FullName + " --- " + a.contact.Mobile + " --- " + a.contact.PatientID)));
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
        private void FindContactAndSendVisitConfirmationSMS(Appointment a)
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
                SendVisitConfirmationSmsToContact(a);
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
                    SendVisitConfirmationSmsToContact(a);
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
                        SendVisitConfirmationSmsToContact(a);
                    }
                    else
                    {
                        if (a.contact.Mobile.Length > 0)
                        {
                            a.contact.PatientID = "-1";
                            SendVisitConfirmationSmsToContact(a);
                        }
                    }
                }
            }
        }
        private void ListOneDayAppointmentsAndSendSMS(DateTime dateTime)
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

                    FindContactAndSendVisitConfirmationSMS(a);
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
                    for (short TimeAfterToSendSMS = 0; TimeAfterToSendSMS < 4; TimeAfterToSendSMS++)
                    {
                        DateTime now = DateTime.Now.AddDays(TimeAfterToSendSMS);
                        ListOneDayAppointmentsAndSendSMS(now);
                    }
                }
                else
                {
                    for (short TimeAfterToSendSMS = 0; TimeAfterToSendSMS < 3; TimeAfterToSendSMS++)
                    {
                        DateTime now = DateTime.Now.AddDays(TimeAfterToSendSMS);
                        ListOneDayAppointmentsAndSendSMS(now);
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
            List<Appointment> contacts = new List<Appointment>();
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
                    contacts.Add(a);
                }
            }
            return contacts;
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