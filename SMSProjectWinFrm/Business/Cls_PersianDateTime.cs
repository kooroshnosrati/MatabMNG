using System;
using System.Data;
using System.Configuration;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Collections.Generic;

/// <summary>
/// Summary description for Cls_PersianDateTime
/// </summary>
public class Cls_PersianDateTime
{
    PersianCalendar m_PersianCalendar = new PersianCalendar();
    private DateTime m_date = new DateTime();
    public int m_Year, m_Month, m_Day;
    string [] wDays = {"شنبه", "یک شنبه", "دو شنبه", "سه شنبه", "چهار شنبه", "پنج شنبه", "جمعه"};
    string[] wMonths = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
    
    public string PMonth
    {
        get
        {
            return wMonths[m_PersianCalendar.GetMonth(m_date) - 1];
        }
    }

    public string PDayOfWeek
    {
        get
        {
            string result = "";
            switch (m_PersianCalendar.GetDayOfWeek(m_date))
            {
                case DayOfWeek.Saturday:
                    result = wDays[0];
                    break;
                case DayOfWeek.Sunday:
                    result = wDays[1];
                    break;
                case DayOfWeek.Monday:
                    result = wDays[2];
                    break;
                case DayOfWeek.Tuesday:
                    result = wDays[3];
                    break;
                case DayOfWeek.Wednesday:
                    result = wDays[4];
                    break;
                case DayOfWeek.Thursday:
                    result = wDays[5];
                    break;
                case DayOfWeek.Friday:
                    result = wDays[6];
                    break;
            }
            return result;
        }
    }

    public int Year
    {
        get
        {
            return m_Year;
        }
        set
        {
            m_Year = value;
            InitializeDateTime();
        }
    }

    public int Month
    {
        get
        {
            return m_Month;
        }
        set
        {
            m_Month = value;
            InitializeDateTime();
        }
    }

    public int Day
    {
        get
        {
            return m_Day;
        }
        set
        {
            m_Day = value;
            InitializeDateTime();
        }
    }

    private void InitializeDateTime()
    {
        try
        {
            m_date = m_PersianCalendar.ToDateTime(m_Year, m_Month, m_Day, 0, 0, 0, 0, PersianCalendar.PersianEra);
        }
        catch (Exception e)
        {
            throw (e);
        }
    }

    public Cls_PersianDateTime()
    {
        m_date = DateTime.Now;
        m_Year = m_PersianCalendar.GetYear(this.m_date);
        m_Month = m_PersianCalendar.GetMonth(this.m_date);
        m_Day = m_PersianCalendar.GetDayOfMonth(this.m_date);
    }

    public Cls_PersianDateTime(DateTime m_date)
    {
        this.m_date = m_date;
        m_Year = m_PersianCalendar.GetYear(this.m_date);
        m_Month = m_PersianCalendar.GetMonth(this.m_date);
        m_Day = m_PersianCalendar.GetDayOfMonth(this.m_date);
    }

    public DateTime ToDateTime()
    {

        return m_date;
    }

    public String ToLongPersianDate()
    {
        string result = "";

        result = PDayOfWeek + " " + m_PersianCalendar.GetDayOfMonth(m_date).ToString()+ " " + PMonth + " " + m_PersianCalendar.GetYear(m_date).ToString();
        return result;
    }

    public String ToPersianDate()
    {
        string result = "";
        result = m_PersianCalendar.GetYear(m_date).ToString().PadLeft(4, '0') + "/" + m_PersianCalendar.GetMonth(m_date).ToString().PadLeft(2, '0') + "/" + m_PersianCalendar.GetDayOfMonth(m_date).ToString().PadLeft(2, '0');
        return result;
    }

    public String ToPersianDateTime()
    {
        string result = "";
        result = m_PersianCalendar.GetYear(m_date).ToString().PadLeft(4, '0') + "/" + m_PersianCalendar.GetMonth(m_date).ToString().PadLeft(2, '0') + "/" + m_PersianCalendar.GetDayOfMonth(m_date).ToString().PadLeft(2, '0') + " " + m_PersianCalendar.GetHour(m_date).ToString().PadLeft(2, '0') + ":" + m_PersianCalendar.GetMinute(m_date).ToString().PadLeft(2, '0') + ":" + m_PersianCalendar.GetSecond(m_date).ToString().PadLeft(2, '0');
        return result;
    }

    public DateTime AddMonths(int Months)
    {
        return m_date.AddMonths(Months);
    }
    public DateTime AddYears(int Years)
    {
        return m_date.AddYears(Years);
    }

    public DateTime AddDays(int Days)
    {
        return m_date.AddDays(Days);
    }

    public void SetDate(DateTime m_date)
    {
        this.m_date = m_date;
        m_Year = m_PersianCalendar.GetYear(this.m_date);
        m_Month = m_PersianCalendar.GetMonth(this.m_date);
        m_Day = m_PersianCalendar.GetDayOfMonth(this.m_date);
    }
    public void SetDate(int year, string MonthName, int day)
    {
        MonthName = MonthName.Replace('ي', 'ی');
        PersianCalendar p = new PersianCalendar();
        List<string> wMonthsList = new List<string>(wMonths);
        int month = wMonthsList.IndexOf(MonthName) + 1;

        DateTime dt = new DateTime(year, month, day, p);
        this.m_date = dt;
        m_Year = m_PersianCalendar.GetYear(this.m_date);
        m_Month = m_PersianCalendar.GetMonth(this.m_date);
        m_Day = m_PersianCalendar.GetDayOfMonth(this.m_date);

    }

    public void SetDate(int year, int month, int day)
    {
        PersianCalendar p = new PersianCalendar();
        DateTime dt = new DateTime(year, month, day, p);
        this.m_date = dt;
        m_Year = m_PersianCalendar.GetYear(this.m_date);
        m_Month = m_PersianCalendar.GetMonth(this.m_date);
        m_Day = m_PersianCalendar.GetDayOfMonth(this.m_date);

    }

}
