using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SMSProjectWinFrm.Business
{
    public class Cls_SMSToSend
    {
        public SMSManagement sMSManagement = new SMSManagement();
        DAL dal = new DAL();
        public List<Cls_SMS> SMSsToSend { get; set; }

        public Cls_SMSToSend()
        {
            
            SMSsToSend = new List<Cls_SMS>();
        }
        public void DoSend()
        {
            SMSsToSend = dal.GetSmsNotSent();
            foreach (Cls_SMS sms in SMSsToSend)
            {
                try
                {
                    sMSManagement.SendSMS(sms.TxtBody, sms.MobileNumber);
                    //sMSManagement.SendSMS(sms.TxtBody, "09195614157");
                    dal.SetSuccessSentSMS(sms);
                }
                catch (Exception err)
                {
                    sms.ErrorTxt = err.Message;
                    if (sms.TryCount < 3)
                        sms.TryCount++;
                    dal.SetErrorSentSMS(sms);
                    string ErrorMessage = string.Format("Error-" + err.Message + " PatientID={0} Mobile={1}", sms.PatientID, sms.MobileNumber);
                    throw new Exception(ErrorMessage);
                }
            }
        }
    }
}
