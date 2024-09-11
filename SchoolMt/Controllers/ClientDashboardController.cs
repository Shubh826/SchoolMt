using BAL;
using SchoolMt.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class ClientDashboardController : Controller
    {
        // GET: ClientDashboard
        private List<ClientDashboardMDL> _AvailableBenchList;
        private List<JoiningTotalProfile> objtotalProfile;
        private List<TotalSkillsWiseProfile> objtotalSkillsProfile;
        private List<LocationWiseProfile> objLocationWise;
        ClientDashboardBAL objClientDashboardBAL = null;

        public ClientDashboardController()
        {
            objClientDashboardBAL = new ClientDashboardBAL();
        }
        public ActionResult Index()
        {

            ClientDashboardMDL objmdl = new ClientDashboardMDL();
            objClientDashboardBAL.getClientDashboardData(out objmdl,out objtotalSkillsProfile, out objtotalProfile, out objLocationWise, SessionInfo.User.fk_companyid, SessionInfo.User.userid, SessionInfo.User.ClientId);
            var CountAvailability = (from temp in objtotalProfile select temp.CountAvailability).ToList();
            var AvailabilityDuration = (from temp in objtotalProfile select temp.AvailabilityDuration).ToList();
            var CountSkills = (from temp in objtotalSkillsProfile select temp.CountSkills).ToList();
            var Skills = (from temp in objtotalSkillsProfile select temp.Skills).ToList();
            var countLocation = (from temp in objLocationWise select temp.CountLocation).ToList();
            var Location = (from temp in objLocationWise select temp.Location).ToList();
            ViewBag.CountAvailability = string.Join(",", CountAvailability);
            ViewBag.AvailabilityDuration = string.Join(",", AvailabilityDuration);
            ViewBag.CountSkills = string.Join(",", CountSkills);
            ViewBag.countLocation = string.Join(",", countLocation);
            ViewBag.Location = string.Join(",", Location);
            ViewBag.Skills = string.Join(",", Skills);
            ViewBag.TotalAvbProfile = objmdl.TotalAvbProfile;
            ViewBag.TotalShortlisted = objmdl.TotalShortlisted;
            ViewBag.ShortlistedProfile = objmdl.ShortlistedProfile;
            ViewBag.TotalShedInterview = objmdl.TotalShedInterview;
            ViewBag.selectedProfile = objmdl.selectedProfile;
            return View();
        }
    }
}