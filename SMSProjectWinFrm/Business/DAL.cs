using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;
using SMSProjectWinFrm.Business;

namespace SMSProjectWinFrm
{
    class DAL
    {
        SqliteConnection m_dbConnection;
        string connectionstringStr = "Data Source=" + Application.StartupPath + "\\db\\my_database.sqlite;";
        public DAL()
        {
            m_dbConnection = new SqliteConnection(connectionstringStr);
            m_dbConnection.Open();
        }
        ~DAL()
        {
            m_dbConnection.Close();
        }
        public void ClearDatabase()
        {
            string sql = "delete from Tbl_SentSMS";
            SqliteCommand command1 = new SqliteCommand(sql, m_dbConnection);
            command1.CommandText = sql;
            command1.CommandType = CommandType.Text;
            command1.ExecuteNonQuery();
        }
        public int isJobCreated(DateTime date, short categoryID, bool AbsoluteDate)
        {
            DateTime d;
            if (AbsoluteDate)
                d = new DateTime(date.Year, date.Month, date.Day);
            else
                d = date;

            string sql = "select * from Tbl_Jobs where JobDate = '" + d.ToString() + "' and CategoryID = '" + categoryID + "'";
            using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
            {
                SqliteDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        return dr.GetInt32(0);
                    }
                }
                command.Dispose();
            }
            return (-1);
        }
        public int JobCreat(DateTime date, short categoryID, bool AbsoluteDate)
        {
            try
            {
                DateTime d;
                if (AbsoluteDate)
                    d = new DateTime(date.Year, date.Month, date.Day);
                else
                    d = date;

                string sql = "INSERT INTO[Tbl_Jobs] ([JobDate],[CategoryID]) VALUES('" + d + "', " + categoryID  + ");";
                using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                return isJobCreated(date, categoryID, AbsoluteDate);
            }
            catch (Exception)
            {
                return (-1);
            }
        }
        public bool isSentSMSToMobile(string Mobile, int JobID)
        {
            string sql = "select * from Tbl_SentSMS where JobID = " + JobID + " and MobileNumber = '" + Mobile + "'";
            SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
            SqliteDataReader dr = command.ExecuteReader();
            command.Dispose();
            return (dr.HasRows);
        }
        public bool isSentVisitCancelSMSToMobile(cls_Appointment a)
        {
            string sql = "select * from Tbl_SentSMS where CategoryID = '2' and SentDate = '" + a.Date + "' and MobileNumber = '" + a.contact.Mobile + "' and PatientID = '" + a.contact.PatientID + "'";
            SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
            SqliteDataReader dr = command.ExecuteReader();
            command.Dispose();
            return (dr.HasRows);
        }
        public void AddMobileToSendVisitConfirmationSMS(cls_Appointment a)
        {
            string sql = "INSERT INTO [Tbl_SentSMS] ([MobileNumber] ,[SentDate] ,[PatientID], [CategoryID]) VALUES ('" + a.contact.Mobile + "', '" + a.Date + "','" + a.contact.PatientID + "', 1);";
            SqliteCommand command1 = new SqliteCommand(sql, m_dbConnection);
            command1.CommandText = sql;
            command1.CommandType = CommandType.Text;
            command1.ExecuteNonQuery();
        }
        public List<Cls_SMS> GetSmsNotSent()
        {
            List<Cls_SMS> SMSs = new List<Cls_SMS>();
            string sql = @"SELECT s.ID, s.JobID,j.JobDate,s.PatientID, s.MobileNumber, s.TxtBody, s.TryCount FROM   Tbl_Jobs as j INNER JOIN Tbl_SentSMS as s ON j.ID = s.JobID WHERE s.IsSent = 0 and s.TryCount < 3 ORDER BY j.JobDate;";

            using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
            {
                SqliteDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    Cls_SMS sms = new Cls_SMS();
                    sms.ID = dr.GetInt32(0);
                    sms.JobID = dr.GetInt32(1);
                    //sms.JobDate = dr.GetDateTime(2);
                    sms.PatientID = dr.GetInt32(3);
                    sms.MobileNumber = dr.GetString(4);
                    sms.TxtBody = dr.GetString(5);
                    sms.TryCount = dr.GetInt16(6);

                    SMSs.Add(sms);
                }
                command.Dispose();
            }
            
            return (SMSs);
        }
        public string GetSMSTextBodyTemplateByCategoryID(int CategoryID)
        {
            string sql = @"SELECT c.TxtBodyTemplate
                            FROM TbL_Categories as c 
                            where c.id = " + CategoryID + ";";
            using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
            {
                SqliteDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        return dr.GetString(0);
                    }
                }
                command.Dispose();
            }
            return null;
        }
        public string GetSMSTextBodyTemplateByJobID(int JobID)
        {
            string sql =  @"SELECT c.TxtBodyTemplate
                            FROM Tbl_Jobs as j
                                   INNER JOIN TbL_Categories as c ON c.ID = j.CategoryID
                            where j.id = " + JobID + ";";
            using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
            {
                SqliteDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        return dr.GetString(0);
                    }
                }
                command.Dispose();
            }
            return null;
        }
        public bool InsertSmsInfoToSentSMSTable(Cls_SMS sms)
        {
            try
            {
                string sql = "INSERT INTO[Tbl_SentSMS] ([JobID],[PatientID], [MobileNumber], [TxtBody], [TryCount], [IsSent], [ErrorTxt]) VALUES (" + sms.JobID + ", " + sms.PatientID + ", '" + sms.MobileNumber + "', '" + sms.TxtBody + "', 0, 0, '');";
                using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return (false);
            }
        }
        public bool SetSuccessSentSMS(Cls_SMS sms)
        {
            try
            {
                string sql = "Update Tbl_SentSMS set IsSent = 1 where ID = " + sms.ID;
                using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return (false);
            }
        }
        public bool SetErrorSentSMS(Cls_SMS sms)
        {
            try
            {
                string sql = "Update Tbl_SentSMS set trycount = " + sms.TryCount + ", ErrorTxt = '" + sms.ErrorTxt + "' where ID = " + sms.ID;
                using (SqliteCommand command = new SqliteCommand(sql, m_dbConnection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                return (false);
            }
        }
    }
}
