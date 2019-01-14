using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationTracker.Models
{
    public class clsJira
    {
        public clsJira() {
            JIRA_Id = string.Empty;
            JIRA_Description = string.Empty;
            Complexity = string.Empty;
            DevComments = string.Empty;
        }
        public string JIRA_Id {get;set;}
        public string JIRA_Description { get; set; }
        public string Complexity { get; set; }
        public string DevComments { get; set; }
        public DateTime AnalysisStartDate { get; set; }
        public DateTime AnalysisEndDate { get; set; }
        public DateTime DevEndDate { get; set; }
    }
}