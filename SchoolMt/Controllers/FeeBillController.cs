using BAL;
using BAL.Common;
using MDL;
using MDL.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class FeeBillController : Controller
    {
        BasicPagingMDL objBasicPagingMDL = null;
        private FeeBillBAL objFeeBillBal;
        List<FeeBillMDL> _FeeBillList;
        private FeeBillMDL objFeeBillMDL;


        public FeeBillController()
        {
            objFeeBillBal = new FeeBillBAL();
            objFeeBillMDL = new FeeBillMDL();

        }
        // GET: FeeBill
        public ActionResult Index()
        {
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["ClassCodelist"] = CommonBAL.FillClassCode();
            return View();
        }
        public PartialViewResult GetFeeBillData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objFeeBillBal.GetFeeBillData(out _FeeBillList, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            
            ViewBag.paging = objBasicPagingMDL;
            TempData["FeeBillList"] = _FeeBillList;
            return PartialView("_FeeBillGrid", _FeeBillList);
        }
        [HttpGet]
        public ActionResult AddEditFeeBill(int id = 0)
        {
            objFeeBillMDL = new FeeBillMDL();
            if (id != 0)
            {
                objFeeBillBal.GetFeeBillData(out _FeeBillList, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid, Convert.ToInt32(20)); return View("AddEditFeeBill", _FeeBillList[0]);
            }
            else
            {
                objFeeBillMDL = new FeeBillMDL();

                return View("AddEditFeeBill", objFeeBillMDL);
            }

        }
        [HttpPost]
        public ActionResult AddEditFeeBill(FeeBillMDL objFeeBillMDL)
        {
            objFeeBillMDL.CreatedBy = SessionInfo.User.userid;
            objFeeBillMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
            if (ModelState.IsValid)
            {
                Messages msg = objFeeBillBal.AddEditFeeBill(objFeeBillMDL);
                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditFeeBill");


        }
    }
}