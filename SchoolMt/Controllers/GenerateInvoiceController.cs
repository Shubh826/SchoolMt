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
    public class GenerateInvoiceController : Controller
    {
        // GET: GenerateInvoice
        private List<GenerateInvoiceMDL> _GenerateInvoicelist;
        BasicPagingMDL objBasicPagingMDL = null;
        GenerateInvoiceBAL objGenerateInvoiceBAL = null;
        public GenerateInvoiceController()
        {
            objGenerateInvoiceBAL = new GenerateInvoiceBAL();
        }
        public ActionResult Index()
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            if (TempData["Message"] != null)
            {
                ViewBag.Msg = (Messages)TempData["Message"];
                TempData["Message"] = null;
            }
            return View();
        }
        public PartialViewResult GetGenerateInvoice(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            objGenerateInvoiceBAL.GetGenerateInvoice(out _GenerateInvoicelist, out objBasicPagingMDL, 0, SessionInfo.User.fk_companyid, SessionInfo.User.userid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            ViewBag.paging = objBasicPagingMDL;
            
            return PartialView("_GenerateInvoiceGrid", _GenerateInvoicelist);
        }
        [HttpGet]
        public ActionResult AddEditGenerateInvoice(int id = 0)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["AcYearlist"] = CommonBAL.FillAcYear();
            ViewData["MonthList"] = CommonBAL.FillMonth();

            GenerateInvoiceMDL obj = new GenerateInvoiceMDL();
            List<feehead> _feeHeadlist = new List<feehead>();

            obj.FK_CompanyId = SessionInfo.User.fk_companyid;
            objGenerateInvoiceBAL.GetfeeHead(out _feeHeadlist, SessionInfo.User.fk_companyid, SessionInfo.User.userid);
            obj._feeHeadlist = _feeHeadlist;
            @ViewBag.FeeHeadList = _feeHeadlist;
            if (id != 0)
            {
                objGenerateInvoiceBAL.GetGenerateInvoice(out _GenerateInvoicelist, out objBasicPagingMDL, id, SessionInfo.User.fk_companyid, SessionInfo.User.userid, Convert.ToInt32(20));
                return View("AddEditGenerateInvoice", _GenerateInvoicelist[0]);
            }
            else
            {
                return View("AddEditGenerateInvoice", obj);
            }
        }


        [HttpPost]
        public ActionResult AddEditGenerateInvoice(GenerateInvoiceMDL objGenerateInvoiceMDL)
        {
            ViewData["companylist"] = CommonBAL.FillCompany(SessionInfo.User.fk_companyid);
            ViewData["Classlist"] = CommonBAL.FillClass();
            ViewData["AcYearlist"] = CommonBAL.FillAcYear();
            //ViewData["MonthList"] = CommonBAL.FillMonth();

            objGenerateInvoiceMDL.CreatedBy = SessionInfo.User.userid;
            objGenerateInvoiceMDL.IsActive = true;
            if (ModelState.IsValid)
            {
                Messages msg = objGenerateInvoiceBAL.AddEditGenerateInvoice(objGenerateInvoiceMDL);
                msg.Message = "Invoice " + msg.Message;
                TempData["Message"] = msg;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
            //if (ModelState.IsValid)
            //{
            //    Messages msg = objGenerateInvoiceBAL.AddEditGenerateInvoice(objGenerateInvoiceMDL);
            //    msg.Message = "Invoice " + msg.Message;
            //    TempData["Message"] = msg;
            //    return RedirectToAction("Index");
            //}
            //return View("AddEditGenerateInvoice", objGenerateInvoiceMDL);
        }
        public JsonResult BindStudent(string ClassName)
        {
            return Json(CommonBAL.BindStudent(ClassName), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckStudentMonth(int Id = 0)
        {
            return Json(CommonBAL.CheckStudentMonth(Id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindMonth()
        {
            return Json(CommonBAL.FillMonth(), JsonRequestBehavior.AllowGet);
        }

    }
}