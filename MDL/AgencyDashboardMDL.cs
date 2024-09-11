using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
   public class AgencyDashboardMDL
    {
        public int TotalJobs { get; set; }
        public int TotalProfile { get; set; }
        public int TotalShedInterview { get; set; }
        public int TotalOnboard { get; set; }
        public string TotalOpenJobs { get; set; }
        public string LocationTotalJobs { get; set; }
        public string JoiningTotalJobs { get; set; }
    }

    public class JoiningTotalJobs
    {
        public string Onbtime { get; set; }
        public int CountOnbtime { get; set; }
    }
    public class TotalOpenJobs
    {
        public string OpenJob { get; set; }
        public int CountOpenJob { get; set; }
    }
    public class LocationWiseOpen
    {
        public string Location { get; set; }
        public int CountLocation { get; set; }
    }
}
