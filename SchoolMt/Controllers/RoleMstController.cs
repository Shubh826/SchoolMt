using BAL;
using BAL.Common;
using SchoolMt.Common;
using MDL;
using MDL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolMt.Controllers
{
    public class RoleMstController : Controller
    {
        private List<RoleMstMDL> _RoleList;
        BasicPagingMDL objBasicPagingMDL = null;
        RoleMstBAL objRoleMstBAL = null;
        //public BasicPagingMDL paging = new BasicPagingMDL();

        public RoleMstController()
        {
            objRoleMstBAL = new RoleMstBAL();
        }
        // GET: RoleMst
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
        public PartialViewResult GetRoles(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            RoleMstMDL objRoleMDLL = new RoleMstMDL();
            objRoleMstBAL.GetAllRoles(out _RoleList, out objBasicPagingMDL, 0, SessionInfo.User.userid, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            TempData["_Report"] = _RoleList;
            TempData["TotalItem"] = objBasicPagingMDL.TotalItem;
            TempData["SearchBy"] = SearchBy;
            TempData["SearchValue"] = SearchValue;
            TempData["CurrentPage"] = CurrentPage;
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_RoleGrid", _RoleList);
        }
        [HttpGet]
        public ActionResult AddEditRole(int id = 0)
        {
            RoleMstMDL obj = new RoleMstMDL();
            int FK_CompanyId = SessionInfo.User.fk_companyid;
            ViewData["CompanyList"] = CommonBAL.FillCompany(FK_CompanyId);
            ViewData["FormList"] = CommonBAL.FillForm(FK_CompanyId);
            //ViewData["HomePageList"] = objRoleBal.GetHomePage();
            if (id != 0)
            {
                objRoleMstBAL.GetAllRoles(out _RoleList, out objBasicPagingMDL, id, SessionInfo.User.userid, SessionInfo.User.fk_companyid);
                return View("AddEditRole", _RoleList[0]);//
            }
            else
            {
                obj.IsActive = true;
                obj.IsCompany = true;
                obj.FK_CompanyId = SessionInfo.User.fk_companyid;
                return View("AddEditRole", obj);
            }

        }
        [HttpPost]
        public ActionResult AddEditRole(RoleMstMDL ObjRoleMDL)
        {
            ObjRoleMDL.CreatedBy = SessionInfo.User.userid;
            ObjRoleMDL.FK_CompanyId = SessionInfo.User.fk_companyid;
           
            if (ObjRoleMDL.Status == "Active")
            {
                ObjRoleMDL.IsActive = true;
            }
            else
            {
                ObjRoleMDL.IsActive = false;
            }
            if (ModelState.IsValid && ObjRoleMDL.RoleName != null)
            {
                Messages msg = objRoleMstBAL.AddEditRole(ObjRoleMDL);

                if (msg != null)
                {
                    msg.Message = "Role " + msg.Message;
                }

                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            return View("AddEdit", ObjRoleMDL);
        }
    }
}