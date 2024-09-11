using BAL;
using BAL.Common;
using MDL;
using MDL.Common;
using SchoolMt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class ExpenseHeadMasterController : Controller
    {
        // GET: ExpenseHeadMaster
        private List<ExpenseHeadMasterMDL> _ExpenseHeadMasterList;
        BasicPagingMDL objBasicPagingMDL = null;
        ExpenseHeadMasterBAL objExpenseHeadMasterBAL = null;
        //public BasicPagingMDL paging = new BasicPagingMDL();

        public ExpenseHeadMasterController()
        {
            objExpenseHeadMasterBAL = new ExpenseHeadMasterBAL();
        }
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetExpenseHead(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            ExpenseHeadMasterMDL objExpenseHeadMasterMDL = new ExpenseHeadMasterMDL();
            objExpenseHeadMasterBAL.GetExpenseHead(out _ExpenseHeadMasterList, out objBasicPagingMDL, 0, SessionInfo.User.userid, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            TempData["_Report"] = _ExpenseHeadMasterList;
            TempData["TotalItem"] = objBasicPagingMDL.TotalItem;
            TempData["SearchBy"] = SearchBy;
            TempData["SearchValue"] = SearchValue;
            TempData["CurrentPage"] = CurrentPage;
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_ExpenseHeadMasterGrid", _ExpenseHeadMasterList);
        }
        [HttpGet]
        public ActionResult AddEditExpenseHead(int id = 0)
        {
            ExpenseHeadMasterMDL obj = new ExpenseHeadMasterMDL();
            ViewData["CompanyList"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            obj.FK_CompanyId = SessionInfo.User.fk_companyid;

            if (id != 0)
            {
                objExpenseHeadMasterBAL.GetExpenseHead(out _ExpenseHeadMasterList, out objBasicPagingMDL, id, SessionInfo.User.userid, SessionInfo.User.fk_companyid);
                return View("AddEditExpenseHead", _ExpenseHeadMasterList[0]);//
            }
            else
            {
                obj.IsActive = true;
                obj.FK_CompanyId = SessionInfo.User.fk_companyid;
                return View("AddEditExpenseHead", obj);
            }

        }
        [HttpPost]
        public ActionResult AddEditExpenseHead(ExpenseHeadMasterMDL objExpenseHeadMasterMDL)
        {
            ViewData["CompanyList"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);

            objExpenseHeadMasterMDL.CreatedBy = SessionInfo.User.userid;
            objExpenseHeadMasterMDL.FK_CompanyId = SessionInfo.User.fk_companyid;

            if (objExpenseHeadMasterMDL.Status == "Active")
            {
                objExpenseHeadMasterMDL.IsActive = true;
            }
            else
            {
                objExpenseHeadMasterMDL.IsActive = false;
            }
            if (ModelState.IsValid)
            {
                Messages msg = objExpenseHeadMasterBAL.AddEditExpenseHead(objExpenseHeadMasterMDL);
                if (msg != null)
                {
                    msg.Message = "Expense Head " + msg.Message;
                }

                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditExpenseHead", objExpenseHeadMasterMDL);
        }
    }
}