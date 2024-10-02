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
using DocumentFormat.OpenXml.Drawing.Diagrams;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace SchoolMt.Controllers
{
    public class ClassController : Controller
    {

        private List<ClassMdl> _Classlist;


        BasicPagingMDL objBasicPagingMDL = null;
        ClassBAL objClassBal = null;
        // GET: Class
        public ActionResult Index()
        {

            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
            return View();
        }
        [HttpGet]
        public PartialViewResult getClassList(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objClassBal = new ClassBAL();
            objClassBal.GetClassDetails(out _Classlist, 0, SearchBy, SearchValue, SessionInfo.User.fk_companyid);
            //ViewBag.paging = objBasicPagingMDL;
            return PartialView("_classlist", _Classlist);
        }
        [HttpGet]
        public ActionResult AddEditClass(int id = 0)
        {
            objClassBal = new ClassBAL();
            ClassMdl obj = new ClassMdl();
            int CompanyId = SessionInfo.User.fk_companyid;
            ViewData["CompanyList"] = CommonBAL.FillCompany(CompanyId);
            if (id != 0)
            {
                objClassBal.GetClassDetails(out _Classlist, 0, "", "", CompanyId);
                if(_Classlist[0].IsActive)
                {
                    _Classlist[0].Status = "Active";
                }
                else
                {
                    _Classlist[0].Status = "InActive";
                }
                return View("AddEditClass", _Classlist[0]);
            }
            else
            {
                obj.IsActive = true;
              
                obj.CompID = SessionInfo.User.fk_companyid;
                return View("AddEditClass", obj);
            }

        }
        [HttpPost]
        public ActionResult AddEditClass(ClassMdl ObjClassMDL)
        {
            ObjClassMDL.CreatedBy = SessionInfo.User.userid;
            ObjClassMDL.CompID = SessionInfo.User.fk_companyid;

            objClassBal = new ClassBAL();
            if (ObjClassMDL.Status == "Active")
            {
                ObjClassMDL.IsActive = true;
            }
            else
            {
                ObjClassMDL.IsActive = false;
            }
            if (ModelState.IsValid && ObjClassMDL.ClassName != null)
            {
                Messages msg = objClassBal.AddEditClass(ObjClassMDL);

                if (msg != null)
                {
                    msg.Message = "Class " + msg.Message;
                }

                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEdit", ObjClassMDL);
        }
    }
}