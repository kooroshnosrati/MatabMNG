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
                logger.ErrorLog("هیچ پورتی وجود ندارد...");
                //MessageBox.Show("هیچ پورتی وجود ندارد...");
            }
            
            if (!chk)
                logger.ErrorLog("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
            //MessageBox.Show("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
            
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

        public string SendSMS(string bodyStr, string Phone, ref bool SendStatus)
        {
            bool sendStatus = false;
            Thread.Sleep(10000);
            string ReturnStr = "";
            string ResultStr = "";
            string Signeture = acm.ReadSetting("Signeture");
            int Result = sendSMS.Send(Signeture, Phone, bodyStr, ref ResultStr);
            try
            {
                string[] ResultStrNodes = ResultStr.Split(';');
                ReturnStr = "وضعیت ارسال با پنل:";
                switch (ResultStrNodes[1])
                {
                    case "0":
                        ReturnStr += "موفق ";
                        break;
                    case "1":
                        ReturnStr += "ناموفق ";
                        break;
                    case "2":
                        ReturnStr += "خطا ";
                        break;
                    case "3":
                        ReturnStr += "BlackList ";
                        break;
                }
                ReturnStr += " --- ";
            }
            catch (Exception)
            {
                ReturnStr = "";
            }
            ReturnStr += "ReturnValue:";
            if (Result != 1)
            {
                switch (Result)
                {
                    case -1:
                        ReturnStr += "امضاء معتبر نیست";
                        break;
                    case 0:
                        ReturnStr += "ارسال نشد";
                        break;
                    case 2:
                        ReturnStr += "پیامک معتبر نیست";
                        break;
                    case 3:
                        ReturnStr += "هیچ شماره موبایلی موجود نیست";
                        break;
                    case 4:
                        ReturnStr += "فیلتر میباشد";
                        break;
                    case 5:
                        ReturnStr += "اوپراتور قطع است";
                        //MessageBox.Show("سرویس ارسال پیامک برای این اپراتوع قطع است . لطفا بعدا تلاش فرمایید...");
                        break;
                    case 6:
                        ReturnStr += "ارسال مجاز نیست";
                        break;
                    case 7:
                        ReturnStr += "حساب کاربری غیرفعال است";
                        break;
                    case 8:
                        ReturnStr += "اعتبار تمام شده است";
                        MessageBox.Show("اعتبار سرویس شما تمام شده است لطفا آن را شارژ کنید...");
                        break;
                    case 9:
                        ReturnStr += "محدودیت در تعداد درخواست";
                        break;
                    case 10:
                        ReturnStr += "محدودیت ارسال روزانه";
                        break;
                    case 11:
                        ReturnStr += "شماره پیانک معتبر نیست";
                        break;
                    case 12:
                        ReturnStr += "خطا";
                        break;
                    case 13:
                        ReturnStr += "حساب کاربری منقضی شده است";
                        break;
                    case 14:
                        ReturnStr += "باید به پنل لاگین کرد";
                        break;
                    default:
                        logger.ErrorLog(string.Format("ParsGreen Send SMS . Phone:{0} BodyText:{1} Error:{2}", Phone, bodyStr, Result));
                        break;
                }
                if (IsPortOpen)
                {
                    SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, Phone);
                    comm.SendMessages(pdu);
                    ReturnStr += " --- از طریق گوشی محلی ارسال شد";
                    sendStatus = true;
                }
            }
            else
            {
                ReturnStr += "از طریق پنل ارسال شد";
                sendStatus = true;
            }
            
            SendStatus = sendStatus;
            return ReturnStr;
        }
        public void SendSMS(string bodyStr, cls_Appointment a)
        {
            bool SendStatus = false;
            SendSMS(bodyStr, a.contact.Mobile, ref SendStatus);
        }

        public void TestSndSMS()
        {
            bool SendStatus = false;
            SendSMS(acm.ReadSetting("TestMessage"), acm.ReadSetting("TestPhone"), ref SendStatus);
        }
    }
}