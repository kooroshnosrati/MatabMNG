using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;

namespace SMSProjectWinFrm
{
    public class SMSManagement
    {
        Logger logger = new Logger();
        ApplicationConfigManagement acm = new ApplicationConfigManagement();
        com.parsgreen.login.SendSMS.SendSMS sendSMS = new com.parsgreen.login.SendSMS.SendSMS();

        public GsmCommMain comm;
        bool IsPortOpen = false;
        public SMSManagement()
        {
            IsPortOpen = Open();
        }
        ~SMSManagement()
        {
            Close();
        }
        public bool Open()
        {
            bool chk = false;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    try
                    {
                        comm = new GsmCommMain(port, 19200, 500);
                        comm.Open();
                        chk = true;
                        break;
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("هیچ پورتی وجود ندارد...");
            }
            
            if (!chk)
                MessageBox.Show("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
            
            return chk;
        }
        public void Close()
        {
            while(comm != null && (comm.IsConnected() || comm.IsOpen()))
            {
                comm.ResetToDefaultConfig();
                //comm.ReleaseProtocol();
                comm.Close();
            }
            comm = null;
        }

        public void SendSMS(string bodyStr, string Phone)
        {
            Thread.Sleep(10000);
            string ResultStr = "";
            string Signeture = acm.ReadSetting("Signeture");
            int Result = sendSMS.Send(Signeture, Phone, bodyStr, ref ResultStr);
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
                        logger.ErrorLog(string.Format("ParsGreen Send SMS . Phone:{0} BodyText:{1} Error:{2}", Phone, bodyStr, Result));
                        break;
                }
                if (IsPortOpen)
                {
                    SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, Phone);
                    comm.SendMessages(pdu);
                }
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