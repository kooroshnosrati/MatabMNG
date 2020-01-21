using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

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

        public bool isSentVisitConfirmationSMSToMobile(Appointment a)
        {
            string sql = "select * from Tbl_SentSMS where CategoryID = '1' and SentDate = '" + a.Date + "' and MobileNumber = '" + a.contact.Mobile + "' and PatientID = '" + a.contact.PatientID + "'";
            SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
            SqliteDataReader dr = command.ExecuteReader();
            command.Dispose();
            return (dr.HasRows);
        }
        public bool isSentVisitCancelSMSToMobile(Appointment a)
        {
            string sql = "select * from Tbl_SentSMS where CategoryID = '2' and SentDate = '" + a.Date + "' and MobileNumber = '" + a.contact.Mobile + "' and PatientID = '" + a.contact.PatientID + "'";
            SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
            SqliteDataReader dr = command.ExecuteReader();
            command.Dispose();
            return (dr.HasRows);
        }

        public void AddMobileToSendVisitConfirmationSMS(Appointment a)
        {
            string sql = "INSERT INTO [Tbl_SentSMS] ([MobileNumber] ,[SentDate] ,[PatientID], [CategoryID]) VALUES ('" + a.contact.Mobile + "', '" + a.Date + "','" + a.contact.PatientID + "', 1);";
            SqliteCommand command1 = new SqliteCommand(sql, m_dbConnection);
            command1.CommandText = sql;
            command1.CommandType = CommandType.Text;
            command1.ExecuteNonQuery();
        }
    }
}
