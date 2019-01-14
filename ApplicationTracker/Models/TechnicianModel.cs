using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationTracker.Models
{
    public class TechnicianModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public char Sex { get; set; }
        public Nullable<DateTime> DateofBirth { get; set; }
        public bool IsProducer { get; set; }
    }
}