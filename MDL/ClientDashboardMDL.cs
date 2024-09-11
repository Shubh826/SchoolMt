using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class ClientDashboardMDL
    {
        public int TotalAvbProfile { get; set; }
        public int ShortlistedProfile { get; set; }
        public int TotalShedInterview { get; set; }
        public int selectedProfile { get; set; }
        public int TotalShortlisted { get; set; }
        public string SkillsWiseProfiles { get; set; }
        public string LocationWiseProfiles { get; set; }
        public string ProfilesJoiningTime { get; set; }
    }
    public class JoiningTotalProfile
    {
        public string AvailabilityDuration { get; set; }
        public int CountAvailability { get; set; }
    }
    public class TotalSkillsWiseProfile
    {
        public string Skills { get; set; }
        public int CountSkills { get; set; }
    }
    public class LocationWiseProfile
    {
        public string Location { get; set; }
        public int CountLocation { get; set; }
    }
}
