using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSProjectWinFrm
{
    public class cls_Appointment
    {
        public cls_Appointment()
        {
            contact = new cls_Contact();
        }
        public string Subject { get; set; }
        public string Paid { get; set; }
        public string StartDateTimeStr { get; set; }
        public string EndDateTimeStr { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string DateStr { get; set; }
        public DateTime Date { get; set; }
        public cls_Contact contact { get; set; }
    }
}
