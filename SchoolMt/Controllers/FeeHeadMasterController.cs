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
    public class FeeHeadMasterController : Controller
    {
        // GET: FeeHeadMaster
        private List<FeeHeadMasterMDL> _FeeHeadMasterList;
        BasicPagingMDL objBasicPagingMDL = null;
        FeeHeadMasterBAL objFeeHeadMasterBAL = null;
        //public BasicPagingMDL paging = new BasicPagingMDL();

        public FeeHeadMasterController()
        {
            objFeeHeadMasterBAL = new FeeHeadMasterBAL();
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
        public PartialViewResult GetFeeHead(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            FeeHeadMasterMDL objFeeHeadMaster = new FeeHeadMasterMDL();
            objFeeHeadMasterBAL.GetAllFeeHead(out _FeeHeadMasterList, out objBasicPagingMDL, 0, SessionInfo.User.userid, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            TempData["_Report"] = _FeeHeadMasterList;
            TempData["TotalItem"] = objBasicPagingMDL.TotalItem;
            TempData["SearchBy"] = SearchBy;
            TempData["SearchValue"] = SearchValue;
            TempData["CurrentPage"] = CurrentPage;
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_FeeHeadMasterGrid", _FeeHeadMasterList);
        }
        [HttpGet]
        public ActionResult AddEditFeeHead(int id = 0)
        {
            FeeHeadMasterMDL obj = new FeeHeadMasterMDL();
            int FK_CompanyId = SessionInfo.User.fk_companyid;
            ViewData["CompanyList"] = CommonBAL.FillCompany(FK_CompanyId);
            ViewData["FormList"] = CommonBAL.FillForm(FK_CompanyId);
            //ViewData["HomePageList"] = objRoleBal.GetHomePage();
            if (id != 0)
            {
                objFeeHeadMasterBAL.GetAllFeeHead(out _FeeHeadMasterList, out objBasicPagingMDL, id, SessionInfo.User.userid, SessionInfo.User.fk_companyid);
                return View("AddEditFeeHead", _FeeHeadMasterList[0]);//
            }
            else
            {
                obj.IsActive = true;
                obj.FK_CompanyId = SessionInfo.User.fk_companyid;
                return View("AddEditFeeHead", obj);
            }

        }
        [HttpPost]
        public ActionResult AddEditFeeHead(FeeHeadMasterMDL objFeeHeadMaster)
        {
            objFeeHeadMaster.CreatedBy = SessionInfo.User.userid;
            objFeeHeadMaster.FK_CompanyId = SessionInfo.User.fk_companyid;

            if (objFeeHeadMaster.Status == "Active")
            {
                objFeeHeadMaster.IsActive = true;
            }
            else
            {
                objFeeHeadMaster.IsActive = false;
            }
            if (ModelState.IsValid)
            {
                Messages msg = objFeeHeadMasterBAL.AddEditFeeHead(objFeeHeadMaster);
                if (msg != null)
                {
                    msg.Message = "Fee Head " + msg.Message;
                }

                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEditFeeHead", objFeeHeadMaster);
        }
    }
}