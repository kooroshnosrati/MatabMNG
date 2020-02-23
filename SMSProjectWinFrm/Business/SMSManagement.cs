using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;

namespace SMSProjectWinFrm
{
    public class SMSManagement
    {
        ApplicationConfigManagement acm = new ApplicationConfigManagement();
        com.parsgreen.login.SendSMS.SendSMS sendSMS = new com.parsgreen.login.SendSMS.SendSMS();

        public GsmCommMain comm;
        public void Open(string PortName)
        {
            comm = new GsmCommMain(PortName, 19200, 500);
            comm.Open();
        }
        public void Close()
        {
            if (comm != null && comm.IsOpen())
                comm.Close();
            comm = null;
        }

        public void SendSMS(string bodyStr, string Phone)
        {
            string ResultStr = "";
            int Result = sendSMS.Send(acm.ReadSetting("Signeture"), Phone, bodyStr, ref ResultStr);
            if (Result != 1)
            {
                switch (Result)
                {
                    case 5:
                        MessageBox.Show("سرویس ارسال پیامک برای این اپراتوع قطع است . لطفا بعدا تلاش فرمایید...");
                        break;
                    case 8:
                        MessageBox.Show("اعتبار سرویس شما تمام شده است لطفا آن را شارژ کنید...");
                        break;
                    default:
                        break;
                }
                SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, Phone);
                comm.SendMessages(pdu);
            }
        }
        public void SendSMS(string bodyStr, cls_Appointment a)
        {
            SendSMS(bodyStr, a.contact.Mobile);
        }

        public void TestSndSMS()
        {
            SendSMS(acm.ReadSetting("TestMessage"), acm.ReadSetting("TestPhone"));
        }
    }
}