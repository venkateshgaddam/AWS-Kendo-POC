using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationTracker.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
    }
}