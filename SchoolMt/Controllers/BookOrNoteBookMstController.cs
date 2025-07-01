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
    public class BookOrNoteBookMstController : Controller
    {
        // GET: BookOrNoteBookMst
        private List<BookOrNoteBookMst> _booklist;


        BasicPagingMDL objBasicPagingMDL = null;
        BookOrNoteBookMstBAL objBookOrNoteBookMstBAL = null;
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
        public PartialViewResult GetBookOrNoteBookData(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objBookOrNoteBookMstBAL = new BookOrNoteBookMstBAL();
            objBookOrNoteBookMstBAL.GetBookOrNoteBookData(out _booklist, out objBasicPagingMDL, SessionInfo.User.userid, Convert.ToInt32(20),CurrentPage, 0, SearchBy, SearchValue, SessionInfo.User.fk_companyid);
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_BookOrNoteBookGrid", _booklist);
        }
        [HttpGet]
        public ActionResult AddEditBookOrNoteBook(int id = 0)
        {
            objBookOrNoteBookMstBAL = new BookOrNoteBookMstBAL();
            BookOrNoteBookMst obj = new BookOrNoteBookMst();
            int CompanyId = SessionInfo.User.fk_companyid;
            ViewData["CompanyList"] = CommonBAL.FillCompany(CompanyId);
            ViewData["ClassList"] = CommonBAL.FillClass();
            if (id != 0)
            {
                objBookOrNoteBookMstBAL.GetBookOrNoteBookData(out _booklist, out objBasicPagingMDL, SessionInfo.User.userid, Convert.ToInt32(20), 1, 0, "", "", SessionInfo.User.fk_companyid);
                if (_booklist[0].IsActive)
                {
                    _booklist[0].Status = "Active";
                }
                else
                {
                    _booklist[0].Status = "InActive";
                }
                return View("AddEditBookOrNoteBook", _booklist[0]);
            }
            else
            {
                obj.IsActive = true;
                obj.CreatedBy = SessionInfo.User.userid;
                obj.CompID = SessionInfo.User.fk_companyid;
                return View("AddEditBookOrNoteBook", obj);
            }

        }
        [HttpPost]
        public ActionResult AddEditBookOrNoteBook(BookOrNoteBookMst ObjBookOrNoteBookMstMDL)
        {
            ObjBookOrNoteBookMstMDL.CreatedBy = SessionInfo.User.userid;
            ObjBookOrNoteBookMstMDL.CompID = SessionInfo.User.fk_companyid;

            objBookOrNoteBookMstBAL = new BookOrNoteBookMstBAL();
            if (ObjBookOrNoteBookMstMDL.Status == "Active")
            {
                ObjBookOrNoteBookMstMDL.IsActive = true;
            }
            else
            {
                ObjBookOrNoteBookMstMDL.IsActive = false;
            }
            if (ModelState.IsValid)
            {
                Messages msg = objBookOrNoteBookMstBAL.AddEditBookOrNoteBook(ObjBookOrNoteBookMstMDL);

                if (msg != null)
                {
                    msg.Message = msg.Message;
                }

                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditBookOrNoteBook", ObjBookOrNoteBookMstMDL);
        }
    }
}