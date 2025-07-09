using BAL;
using BAL.Common;
using FRGMBSystem.Controllers;
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
    public class ExpenseMasterController : BasicController
    {
        // GET: ExpenseMaster
        BasicPagingMDL objBasicPagingMDL = null;
        private ExpenseMasterBAL objExpenseMasterBAL;
        List<ExpenseMasterMDL> _ExpenseMasterList;
        private ExpenseMasterMDL objExpenseMasterMDL;

        public ExpenseMasterController()
        {
            objExpenseMasterBAL = new ExpenseMasterBAL();
            objExpenseMasterMDL = new ExpenseMasterMDL();

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
        public PartialViewResult GetExpenseList(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            BasicPagingMDL objBasicPagingMDL;

            objExpenseMasterBAL.GetExpenseList(
                out _ExpenseMasterList,
                out objBasicPagingMDL,
                0,
                SessionInfo.User.fk_companyid,
                20,
                CurrentPage,
                SearchBy,
                SearchValue
            );

            ViewBag.paging = objBasicPagingMDL;
            TempData["expenselist"] = _ExpenseMasterList;

            return PartialView("_ExpenseGrid", _ExpenseMasterList);
        }

        [HttpGet]
        public ActionResult AddEditExpense(int id = 0)
        {
            objExpenseMasterMDL = new ExpenseMasterMDL();
            ViewData["ExpenseHeadList"] = CommonBAL.FillExpenseHead(SessionInfo.User.fk_companyid);
            if (id != 0)
            {
                objExpenseMasterBAL.GetExpenseList(
                out _ExpenseMasterList,
                out objBasicPagingMDL,
                id,
                SessionInfo.User.fk_companyid,
                20,
                1,
                "",
                ""
            );
                return View("AddEditExpense", _ExpenseMasterList[0]);
            }
            else
            {
                return View("AddEditExpense", objExpenseMasterMDL);
            }

        }
        [HttpPost]
        public ActionResult AddEditExpense(ExpenseMasterMDL expenseMasterMDL)
        {
            expenseMasterMDL.CreatedBy = SessionInfo.User.userid;
            expenseMasterMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
            ViewData["ExpenseHeadList"] = CommonBAL.FillExpenseHead(SessionInfo.User.fk_companyid);
            if (ModelState.IsValid)
            {
                Messages msg = objExpenseMasterBAL.AddEditExpense(expenseMasterMDL);
                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditFeeBill", expenseMasterMDL);


        }
    }
}