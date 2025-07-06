using BAL;
using MDL.Common;
using MDL;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class StudentReportController : Controller
    {
        private List<StudentReportMDL> _list;
      
        BasicPagingMDL objBasicPagingMDL = null;
  
        StudentReportBAL objStudentReportBAL = null;
        public StudentReportController()
        {
            objStudentReportBAL = new StudentReportBAL();
        }
        // GET: StudentReport
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetStudentData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "",string fromDate="",string toDate="")
        {
            objStudentReportBAL.GetStudentReportData(out _list, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue, fromDate, toDate);
            
            ViewBag.paging = objBasicPagingMDL;
            TempData["list"] = _list;
            return PartialView("_StudentReportGrid", _list);
        }

    }
}