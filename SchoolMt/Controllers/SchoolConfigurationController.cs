using BAL;
using MDL.Common;
using MDL;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Common;

namespace SchoolMt.Controllers
{
    public class SchoolConfigurationController : Controller
    {

        private List<SchoolConfigurationMDL> _list;

        BasicPagingMDL objBasicPagingMDL = null;
        private SchoolConfigurationBAL objBal;
        public SchoolConfigurationController()
        {
            objBal = new SchoolConfigurationBAL();
        }
        // GET: SchoolConfiguration
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetSchoolConfigurationData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objBal.GetSchoolConfigurationData(out _list, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
           // TempData["studentlist"] = _list;
            return PartialView("_SchoolConfigurationGrid", _list);
        }

        [HttpGet]
        public ActionResult AddEditSchoolConfiguration(int id = 0)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass(SessionInfo.User.fk_companyid);
            ViewData["LookUplist"] = CommonBAL.GetLookUpList(SessionInfo.User.fk_companyid, 0,"Add");
            if (id > 0)
            {
                objBal.GetSchoolConfigurationData(out _list, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid);
                return View("AddEditSchoolConfiguration", _list[0]);
            }
            else
            {
                SchoolConfigurationMDL obj = new SchoolConfigurationMDL();
                obj.FK_CompanyId = 1;    
                obj.IsActive = true;
                return View("AddEditSchoolConfiguration", obj);
            }
        }


        [HttpPost]
        public ActionResult AddEditSchoolConfiguration(SchoolConfigurationMDL obj)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass(SessionInfo.User.fk_companyid);
            ViewData["LookUplist"] = CommonBAL.GetLookUpList(SessionInfo.User.fk_companyid, 0, "Add");
            obj.userId = SessionInfo.User.userid;
            Messages msg = objBal.InsertSchoolConfigurationData(obj);
            TempData["Message"] = msg;
            return RedirectToAction("Index");
        }
        public JsonResult GetClass(int companyID=0)
        {
            return Json(CommonBAL.FillClass(companyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLookUpDetailList(int companyid, int lookUpId,string actionfrom)
        {
            return Json(CommonBAL.GetLookUpList(companyid, lookUpId, actionfrom), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSchoolConfigurationData(string pkIds)
        {
           
            Messages msg = objBal.DeleteSchoolConfigurationData(pkIds);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}