using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSProjectWinFrm.Business
{
    public class Cls_SMSToSend
    {
        public List<Cls_SMS> SMSsToSend { get; set; }

        public Cls_SMSToSend()
        {
            SMSsToSend = new List<Cls_SMS>();
        }
    }
}
