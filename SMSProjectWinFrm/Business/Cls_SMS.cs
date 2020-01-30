using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSProjectWinFrm.Business
{
    public class Cls_SMS
    {
        public int ID { get; set; }
        public int JobID { get; set; }
        public DateTime JobDate { get; set; }
        public int PatientID { get; set; }
        public string MobileNumber { get; set; }
        public string TxtBody { get; set; }
        public short TryCount { get; set; }
        public bool IsSent { get; set; }
        public string ErrorTxt { get; set; }
    }
}
