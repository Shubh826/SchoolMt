using BAL;
using MDL.Common;
using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Common;
using SchoolMt.Common;

namespace SchoolMt.Controllers
{
    public class SubjectMarkMappingController : Controller
    {

        private List<SubjectMarkMappingMDL> _list;

        BasicPagingMDL objBasicPagingMDL = null;
        private SubjectMarkMappingBAL objBal;
        public SubjectMarkMappingController()
        {
            objBal = new SubjectMarkMappingBAL();
        }
        // GET: SubjectMarkMapping
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetSubjectMarkMappingData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objBal.GetSubjectMarkMappingData(out _list, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
            // TempData["studentlist"] = _list;
            return PartialView("_SubjectMarkMappingnGrid", _list);
        }

        [HttpGet]
        public ActionResult AddEditSubjectMarkMappingData(int id = 0)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass(SessionInfo.User.fk_companyid);
           // ViewData["LookUplist"] = CommonBAL.GetLookUpList(SessionInfo.User.fk_companyid, 0, "Add");
            if (id > 0)
            {
                objBal.GetSubjectMarkMappingData(out _list, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid);
                return View("AddEditSubjectMarkMappingData", _list[0]);
            }
            else
            {
                SubjectMarkMappingMDL obj = new SubjectMarkMappingMDL();
                obj.FK_CompanyId = 1;
                obj.IsActive = true;
                return View("AddEditSubjectMarkMappingData", obj);
            }
        }


        [HttpPost]
        public ActionResult AddEditSubjectMarkMappingData(SubjectMarkMappingMDL obj)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass(SessionInfo.User.fk_companyid);
            //ViewData["LookUplist"] = CommonBAL.GetLookUpList(SessionInfo.User.fk_companyid, 0, "Add");
            obj.userId = SessionInfo.User.userid;
            Messages msg = objBal.InsertSubjectMarkMappingData(obj);
            TempData["Message"] = msg;
            return RedirectToAction("Index");
        }

        public JsonResult GetLookUpDetailList(int companyid, string lookUpTypeName)
        {
            int lookUpId = 0; string actionfrom = ""; 
            return Json(CommonBAL.GetLookUpList(companyid, lookUpId, actionfrom, lookUpTypeName), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubjectWiseMarksList(int companyid, int examtypeId,int PkId)
        {
            return Json(CommonBAL.GetSubjectWiseMarksList(companyid, examtypeId, PkId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BindStudent(int classId=0, int companyId = 0)
        {
            return Json(CommonBAL.GetStudentBySchoolWise(classId, companyId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSchoolConfigurationData(int pkId)
        {
            Messages msg = objBal.DeleteSchoolConfigurationData(pkId);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}