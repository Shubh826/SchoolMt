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
    public class AgencyDashboardController : Controller
    {
         private List<AgencyDashboardMDL> _AvailableBenchList;
        private List<JoiningTotalJobs> objtotal;
        private List<TotalOpenJobs> objopenjobs;
        private List<LocationWiseOpen> objLocationWise;
        AgencyDashboardBAL objAgencyDashboardBAL = null;

        public AgencyDashboardController()
        {
            objAgencyDashboardBAL = new AgencyDashboardBAL();
        }
        public ActionResult Index()
        {
            AgencyDashboardMDL objmdl = new AgencyDashboardMDL();
            objAgencyDashboardBAL.getDashboardData(out objmdl, out objtotal, out objopenjobs, out objLocationWise, SessionInfo.User.fk_companyid, SessionInfo.User.userid, SessionInfo.User.ClientId);
            var countbtime = (from temp in objtotal select temp.CountOnbtime).ToList();
            var onbtime = (from temp in objtotal select temp.Onbtime).ToList();
            var OpenJobs = (from temp in objopenjobs select temp.CountOpenJob).ToList();
            var countLocation = (from temp in objLocationWise select temp.CountLocation).ToList();
            var Location = (from temp in objLocationWise select temp.Location).ToList();
            ViewBag.CountTime = string.Join(",", countbtime);
            ViewBag.Time_List = string.Join(",", onbtime);
            ViewBag.OpenJobs = string.Join(",", OpenJobs);
            ViewBag.countLocation = string.Join(",", countLocation);
            ViewBag.Location = string.Join(",", Location);
            ViewBag.TotalJobs = objmdl.TotalJobs;
            ViewBag.TotalProfile = objmdl.TotalProfile;
            ViewBag.TotalOnboard = objmdl.TotalOnboard;
            ViewBag.TotalShedInterview = objmdl.TotalShedInterview;

            return View();
        }

        public ActionResult GetDashboardDetail()
        {
            return View();
        }
    }
}