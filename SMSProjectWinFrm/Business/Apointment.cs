﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSProjectWinFrm
{
    public class Appointment
    {
        public  Appointment()
        {
            contact = new Contact();
        }
        public string Subject { get; set; }
        public string Date { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public Contact contact { get; set; }
    }
}
