using BAL.Common;
using BAL;
using MDL;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDL.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Ajax.Utilities;
using System.Windows.Interop;

namespace SchoolMt.Controllers
{
    public class LookUpController : Controller
    {
        private List<LookUpMDL> _list;


        BasicPagingMDL objBasicPagingMDL = null;
        LookUpMasterBAL objBal = null;
        List<LookUpDetailMDL> _List = null;

        public LookUpController()
        {
            objBal = new LookUpMasterBAL();
        }
        // GET: LookUp
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }

            CompanyMDL objCompanyMDL = new CompanyMDL();
            return View();
        }

        [HttpGet]
        public PartialViewResult GetLookUpDetailData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {


            objBal.GetLookUpDetailData(out _List, out objBasicPagingMDL, 0, CurrentPage,20 , SearchBy, SearchValue);

            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_LookUpGridDetail", _List);
        }




        [HttpGet]
        public ActionResult AddEditLookupDetail(int id = 0)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            LookUpDetailMDL obj=new LookUpDetailMDL();
            if (id != 0)
            {
                objBal.GetLookUpDetailData(out _List,out objBasicPagingMDL, id, 1,10, "", "");
                
                return View("AddEditLookupDetail", _List[0]);
            }
            else
            {
               
                obj.IsActive = true;
                obj.SortId = objBal.GetMaxId(0);
                obj.Fk_CompanyId = SessionInfo.User.fk_companyid;
                return View("AddEditLookupDetail", obj);
            }

        }
        [HttpPost]
        public ActionResult AddEditLookupDetail(LookUpDetailMDL Obj)
        {
           
            Messages msg = new Messages();
            Obj.UserId = SessionInfo.User.userid;
           // Obj.Fk_CompanyId = SessionInfo.User.fk_companyid;

            if (Obj.Status == "Active")
            {
                Obj.IsActive = true;
            }
            else
            {
                Obj.IsActive = false;
            }
               msg = objBal.AddEditLookUpDetail(Obj);
                if (msg != null)
                {
                    msg.Message = msg.Message;
                }

                TempData["Message"] = msg;
                return RedirectToAction("Index");
 
        }

        [HttpPost]
        public JsonResult AddEditLookUp(FormCollection formdata)
        {
            LookUpMDL obj= new LookUpMDL();
            Messages Msg = new Messages();
            string LookupName = formdata["LookupName"];
            obj.PkId = 0;
            obj.LookupName= LookupName;
            obj.UserId = SessionInfo.User.userid;
            obj.IsActive= true;
            Msg = objBal.AddEditLookUp(obj);
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLookUpList()
        {
            List<LookUpMDL> _LookUpList = new List<LookUpMDL>();
            bool result = objBal.GetLookUpData(out _LookUpList, 0, "", "");
            ViewData["LookUpList"] = _LookUpList;
            return Json(_LookUpList, JsonRequestBehavior.AllowGet);
        }
    }
}