using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm.Business
{
    public class Cls_SMSToSend
    {
        public SMSManagement sMSManagement = new SMSManagement();
        DAL dal = new DAL();
        public List<Cls_SMS> SMSsToSend { get; set; }
        public ListBox SentList;
        public TextBox TxtCount, TxtTimeToEnd;
        public Cls_SMSToSend()
        {
            
            SMSsToSend = new List<Cls_SMS>();
        }
        public void DoSend()
        {
            string ErrorMessage = "";
            bool SmsCountOverFlow = false;
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            int MaxSmsCountPerDay = int.Parse(acm.ReadSetting("MaxSmsCountPerDay"));
            int smsCountOnDay = dal.GetSmsCountSentOnDay(DateTime.Now);

            SMSsToSend = dal.GetSmsNotSent();
            int counter = 0;

            foreach (Cls_SMS sms in SMSsToSend)
            {
                SmsCountOverFlow = false;
                DateTime dtStart = DateTime.Now;
                try
                {
                    if (smsCountOnDay < MaxSmsCountPerDay)
                    {
                        ++counter;
                        sMSManagement.SendSMS(sms.TxtBody, sms.MobileNumber);
                        //sMSManagement.SendSMS(sms.TxtBody, "09195614157");
                        Thread.Sleep(10000);
                        dal.IncrementSmsCountSentOnDay(DateTime.Now);
                        dal.SetSuccessSentSMS(sms);
                        TimeSpan ts = DateTime.Now - dtStart;
                        SentList.Invoke(new Action(() => SentList.Items.Insert(0, string.Format("ID={0} JobID={1} PatientID={2} Mobile={3} Time={4}", sms.ID, sms.JobID, sms.PatientID, sms.MobileNumber, ts.TotalSeconds))));
                        TxtCount.Invoke(new Action(() => TxtCount.Text = (SMSsToSend.Count - counter).ToString()));

                        int totalSeconds = (int)(((double)(SMSsToSend.Count - counter) * ts.TotalSeconds));
                        TimeSpan tsTimeToEnd = new TimeSpan((long)totalSeconds * (long)10000000);
                        TxtTimeToEnd.Invoke(new Action(() => TxtTimeToEnd.Text = string.Format("{0}:{1}:{2}", tsTimeToEnd.Hours, tsTimeToEnd.Minutes, tsTimeToEnd.Seconds)));
                    }
                    else
                    {
                        TimeSpan ts = DateTime.Now - dtStart;
                        ErrorMessage = "تعداد پیامک های ارسالی از حد مجاز بیشتر شده است...";
                        
                        TxtCount.Invoke(new Action(() => TxtCount.Text = (SMSsToSend.Count - counter).ToString()));
                        int totalSeconds = (int)(((double)(SMSsToSend.Count - counter) * ts.TotalSeconds));
                        TimeSpan tsTimeToEnd = new TimeSpan((long)totalSeconds * (long)10000000);
                        TxtTimeToEnd.Invoke(new Action(() => TxtTimeToEnd.Text = string.Format("{0}:{1}:{2}", tsTimeToEnd.Hours, tsTimeToEnd.Minutes, tsTimeToEnd.Seconds)));
                        SmsCountOverFlow = true;
                        throw new Exception(ErrorMessage);
                    }
                        
                }
                catch (Exception err)
                {
                    if (!SmsCountOverFlow)
                    {
                        sms.ErrorTxt = err.Message;
                        if (sms.TryCount < 3)
                            sms.TryCount++;
                        dal.SetErrorSentSMS(sms);
                        TimeSpan ts = DateTime.Now - dtStart;
                        ErrorMessage = string.Format("Error-{2} PatientID={0} Mobile={1} TryCount={3} Time={4}", sms.PatientID, sms.MobileNumber, err.Message, sms.TryCount, ts.TotalSeconds);
                        TxtCount.Invoke(new Action(() => TxtCount.Text = (SMSsToSend.Count - counter).ToString()));

                        int totalSeconds = (int)(((double)(SMSsToSend.Count - counter) * ts.TotalSeconds));
                        TimeSpan tsTimeToEnd = new TimeSpan((long)totalSeconds * (long)10000000);
                        TxtTimeToEnd.Invoke(new Action(() => TxtTimeToEnd.Text = string.Format("{0}:{1}:{2}", tsTimeToEnd.Hours, tsTimeToEnd.Minutes, tsTimeToEnd.Seconds)));
                        throw new Exception(ErrorMessage);
                    }
                    else
                        throw new Exception(ErrorMessage);
                }
            }
        }
        public void ResetUnsendSMS()
        {
            dal.ResetUnsendSMS();
    }
    }
}
