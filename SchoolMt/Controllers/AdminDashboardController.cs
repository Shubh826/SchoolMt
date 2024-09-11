using BAL;
using SchoolMt.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        private List<AdminDashboardMDL> _AdminDashboardList;
        List<MonthwiseData> _MonthwiseData = null;
        AdminDashboardBAL objAdminDashboardBAL = null;

        public AdminDashboardController()
        {
            objAdminDashboardBAL = new AdminDashboardBAL();
        }
        public ActionResult Index()
        {
            AdminDashboardMDL objmdl = new AdminDashboardMDL();
            objAdminDashboardBAL.GetAdminDashboardData(out objmdl, SessionInfo.User.fk_companyid, SessionInfo.User.userid, SessionInfo.User.ClientId);
            ViewBag.TotalClient = objmdl.TotalClient;
            ViewBag.TotalAgencies = objmdl.TotalAgencies;
            ViewBag.TotalActiveProfiles = objmdl.TotalActiveProfiles;
            ViewBag.TotalClosedJobs = objmdl.TotalClosedJobs;
            ViewBag.CurrentScheduledInterviews = objmdl.CurrentScheduledInterviews;
            ViewBag.TotalShortlistedProfiles = objmdl.TotalShortlistedProfiles;
            ViewBag.OpenJobs = objmdl.OpenJobs;
            ViewBag.ActiveAgenciesorClients = objmdl.ActiveAgenciesorClients;
            return View();
        }
        public JsonResult GetMonthWiseData(int Year)
        {
          
            objAdminDashboardBAL.GetMonthWiseAdminDashboardData(out _MonthwiseData, Year, SessionInfo.User.fk_companyid, SessionInfo.User.userid, SessionInfo.User.ClientId);
            var Monthlist = (from temp in _MonthwiseData select temp.Month).ToList();
            var Clientlist = (from temp in _MonthwiseData select temp.Client).ToList();
            var Agencylist = (from temp in _MonthwiseData select temp.Agency).ToList();
            var Profilelist = (from temp in _MonthwiseData select temp.ActiveProfile).ToList();
            var Joblist = (from temp in _MonthwiseData select temp.Job).ToList();
            //ViewBag.Monthlist = string.Join(",", Monthlist);
            //ViewBag.Clientlist = string.Join(",", Clientlist);
            //ViewBag.Agencylist = string.Join(",", Agencylist);
            //ViewBag.Profilelist = string.Join(",", Profilelist);
            //ViewBag.Joblist = string.Join(",", Joblist);
            dynamic Data = new ExpandoObject();
        
            Data.MasterData = _MonthwiseData;
            Data.Monthlist = Monthlist;
            Data.Clientlist = Clientlist;
            Data.Agencylist = Agencylist;
            Data.Profilelist = Profilelist;
            Data.Joblist = Joblist;
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        }
}