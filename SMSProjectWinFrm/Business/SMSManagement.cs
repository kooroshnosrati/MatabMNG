using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;

namespace SMSProjectWinFrm
{
    public class SMSManagement
    {
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
            SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, Phone);
            comm.SendMessages(pdu);
        }
        public void SendSMS(string bodyStr,Appointment a)
        {
            SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, a.contact.Mobile);
            comm.SendMessages(pdu);
        }

        public void TestSndSMS()
        {
            ApplicationConfigManagement acm = new ApplicationConfigManagement();
            SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(acm.ReadSetting("TestMessage"), true, acm.ReadSetting("TestPhone"));
            comm.SendMessages(pdu);
        }
    }
}