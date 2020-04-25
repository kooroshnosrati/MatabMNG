﻿using System;
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
    public class SMSManagement : IDisposable
    {
        Logger logger = new Logger();
        ApplicationConfigManagement acm = new ApplicationConfigManagement();
        com.parsgreen.login.SendSMS.SendSMS sendSMS = new com.parsgreen.login.SendSMS.SendSMS();
        //public GsmCommMain comm;
        bool IsPortOpen = false;
        string GSMport = "";

        public SMSManagement()
        {
            IsPortOpen = Open();
        }
        ~SMSManagement()
        {
            //Close();
        }
        public GsmCommMain Open(string port)
        {
            GsmCommMain comm;
            try
            {
                //Close();
                comm = new GsmCommMain(port, 19200, 500);
                comm.Open();
                return comm;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Open()
        {
            //ApplicationConfigManagement acm = new ApplicationConfigManagement();
            //GSMport = acm.ReadSetting("GSMport");
            //if (GSMport.Length > 0 )
            //{
            //    return true;
            //}

            GsmCommMain comm = null;
            //Close();
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
                        SmsSubmitPdu[] pdu1 = SmartMessageFactory.CreateConcatTextMessage("آقای/خانم نورمحمدی.", true, "09195614157");
                        comm.SendMessages(pdu1);
                        SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage("آقای/خانم نورمحمدی \r\nنوبت ويزيت شما براي خانم دكتر علي مددي تاريخ يكشنبه, 7 اردیبهشت 1399 ساعت 09:20 ق.ظ میباشد.", true, "09195614157");
                        comm.SendMessages(pdu);
                        chk = true;
                        GSMport = port;
                        logger.ErrorLog("Valid Port of GSM is : " + GSMport);
                        Close(ref comm);
                        break;
                    }
                    catch (Exception err)
                    {
                        Close(ref comm);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorLog("هیچ پورتی وجود ندارد...");
            }

            if (!chk)
                logger.ErrorLog("خطای ارتباط با مودم .... \n\r لطفا از ارتباط مودم با سیستم اطمینان حاصل نمایید....");
            return chk;
        }
        public void Close(ref GsmCommMain comm)
        {
            try
            {
                while (comm != null && (comm.IsConnected() || comm.IsOpen()))
                {
                    //comm.ResetToDefaultConfig();
                    //comm.ReleaseProtocol();
                    comm.Close();
                }
            }
            catch (Exception)
            {
                ;
            }
            comm = null;
        }
        public string SendSMSWithGSM(string bodyStr, string Phone, ref bool SendStatus)
        {
            string ReturnStr = "";
            //GsmCommMain comm = new GsmCommMain(GSMport, 19200, 500);
            //comm.Open();
            //SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, "09195614157");
            //comm.SendMessages(pdu);

            GsmCommMain comm = Open(GSMport);
            try
            {
                if (comm != null)
                {
                    //SmsSubmitPdu[] pdu = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, Phone);
                    SmsSubmitPdu[] pdu1 = SmartMessageFactory.CreateConcatTextMessage(bodyStr, true, "09195614157");
                    comm.SendMessages(pdu1);
                    Close(ref comm);
                }
                ReturnStr += " --- از طریق گوشی محلی ارسال شد";
                SendStatus = true;
            }
            catch (Exception err)
            {
                Close(ref comm);
                ReturnStr += " --- خطای ارسال از طریق گوشی محلی --- " + err.Message;
            }
            return ReturnStr;
        }
        public string SendSMS(string bodyStr, string Phone, ref bool SendStatus)
        {
            bool sendStatus = false;
            Thread.Sleep(10000);
            string ReturnStr = "";
            string ResultStr = "";
            string Signeture = acm.ReadSetting("Signeture");
            //int Result = sendSMS.Send(Signeture, Phone, bodyStr, ref ResultStr);
            int Result = 0;
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
                        ReturnStr += "بلک لیست ";
                        break;
                }
                ReturnStr += " --- ";
            }
            catch (Exception)
            {
                ReturnStr = "";
            }
            ReturnStr += "مقداربرگشتی:";
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



                //bool isSentByGSM = false;
                //string returnStr = SendSMSWithGSM(bodyStr, Phone, ref isSentByGSM);
                //if (isSentByGSM)
                //{
                //    ReturnStr += " --- از طریق گوشی محلی ارسال شد";
                //    sendStatus = true;
                //}
                //else
                //    ReturnStr += " --- خطای ارسال از طریق گوشی محلی --- " + returnStr;
                if (IsPortOpen)
                {
                    ReturnStr += SendSMSWithGSM(bodyStr, Phone, ref sendStatus);
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
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    //Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SMSManagement()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}