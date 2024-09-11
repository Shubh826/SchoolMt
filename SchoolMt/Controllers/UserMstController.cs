using System;
using BAL;
using BAL.Common;
using SchoolMt.Common;
using MDL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cryptography;
using MDL.Common;

namespace SchoolMt.Controllers
{
    public class UserMstController : Controller
    {
        private List<UserMDL> _UserList;
        BasicPagingMDL objBasicPagingMDL = null;
        UserMstBAL objUserMstBAL = null;
        //public BasicPagingMDL paging = new BasicPagingMDL();

        public UserMstController()
        {
            objUserMstBAL = new UserMstBAL();
        }
        // GET: UserMst
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
        public PartialViewResult GetUsers(int CurrentPage = 1, string SearchBy = "", string SearchValue = "")
        {
            RoleMstMDL objRoleMDLL = new RoleMstMDL();
            objUserMstBAL.GetUserData(out _UserList, out objBasicPagingMDL, 0, SessionInfo.User.userid, SessionInfo.User.fk_companyid, Convert.ToInt32(20), CurrentPage, SearchBy, SearchValue);
            TempData["_Report"] = _UserList;
            TempData["TotalItem"] = objBasicPagingMDL.TotalItem;
            TempData["SearchBy"] = SearchBy;
            TempData["SearchValue"] = SearchValue;
            TempData["CurrentPage"] = CurrentPage;
            ViewBag.paging = objBasicPagingMDL;
            return PartialView("_UserGrid", _UserList);
        }
        [HttpGet]
        public ActionResult AddEditUser(int id = 0)
        {
           
            int FK_CompanyId = SessionInfo.User.fk_companyid;
            ViewData["Rolelist"] = CommonBAL.FillRole(FK_CompanyId);

            if (id != 0)
            {
                objUserMstBAL.GetUserData(out _UserList, out objBasicPagingMDL, id, SessionInfo.User.userid, SessionInfo.User.fk_companyid);
                _UserList[0].Password = ClsCrypto.Decrypt(_UserList[0].Password);

                return View("AddEditUser", _UserList[0]);
            }
            else
            {
                UserMDL obj = new UserMDL();
                obj.IsActive = true;
                obj.fk_companyid = SessionInfo.User.fk_companyid;
                return View("AddEditUser", obj);
            }
        }
        [HttpPost]
        public ActionResult AddEditUser(UserMDL userMDL)

        {
            if(userMDL.Status == "Active")
            {
                userMDL.IsActive = true; 
            }
            else
            {
                userMDL.IsActive = false;
            }
            userMDL.fk_companyid = SessionInfo.User.fk_companyid;
            ViewData["Rolelist"] = CommonBAL.FillRole(SessionInfo.User.fk_companyid);
            userMDL.Password = ClsCrypto.Encrypt(userMDL.Password);
            userMDL.CreatedBy = SessionInfo.User.userid;
            userMDL.UpdatedBy = SessionInfo.User.userid;
            ModelState.Remove("Address");
            ModelState.Remove("ClientId");
            ModelState.Remove("Fk_ClientId");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("countryname");
            ModelState.Remove("CompanyName");
            ModelState.Remove("RoleName");
            ModelState.Remove("MiddleName");
            ModelState.Remove("LastName");
            ModelState.Remove("stateid");
            ModelState.Remove("statename");
            ModelState.Remove("userid");
            //ModelState.Remove("FirstName");
            ModelState.Remove("cityid");
            if (ModelState.IsValid)
            {
                Messages msg = objUserMstBAL.AddEditUser(userMDL);
                TempData["Message"] = msg;
                return RedirectToAction("Index");
            }
            else
            {
                UserMDL objUserMDL = new UserMDL();
                objUserMDL.IsActive = true;
                return View("AddEditUser", objUserMDL);
            }

        }
        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            Messages msg = new Messages();
            int CreatedBy = SessionInfo.User.userid;
            msg = objUserMstBAL.DeleteUser(id, CreatedBy);
            TempData["Message"] = msg;
            return RedirectToAction("Index");
        }
    }
}