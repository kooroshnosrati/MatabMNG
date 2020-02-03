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
        public string Date { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public cls_Contact contact { get; set; }
    }
}
