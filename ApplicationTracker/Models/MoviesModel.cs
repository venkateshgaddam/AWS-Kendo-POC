using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationTracker.Models
{
    public class MoviesModel
    {
        public int Movie_Id { get; set; }
        public string MovieName { get; set; }
        public Nullable<System.DateTime> DateOfRelease { get; set; }
        public string Producer { get; set; }
        public string Color { get; set; }
    }
}