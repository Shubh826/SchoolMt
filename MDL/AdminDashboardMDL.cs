using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class AdminDashboardMDL
    {
        public int TotalClient { get; set; }
        public int TotalAgencies { get; set; }
        public int TotalActiveProfiles { get; set; }
        public int TotalClosedJobs { get; set; }
        public int CurrentScheduledInterviews { get; set; }
        public int TotalShortlistedProfiles { get; set; }
        public int OpenJobs { get; set; }
        public int ActiveAgenciesorClients { get; set; }

    }

    public class MonthwiseData
    {
        public string Month { get; set; }
        public int Client { get; set; }
        public int Agency { get; set; }
        public int ActiveProfile { get; set; }
        public int Job { get; set; }

    }
    public class Monthlist
    {
        public string Month { get; set; }
    }
    public class Clientlist
    {
        public int Client { get; set; }
    }
    public class Agencylist
    {
        public int Agency { get; set; }
    }
    public class Profilelist
    {
        public int ActiveProfile { get; set; }
    }
    public class Joblist
    {
        public int Job { get; set; }
    }
    }

