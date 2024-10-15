using FRGMBSystem.Filter;
using SchoolMt.Common;
using SchoolMt.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRGMBSystem.Controllers
{
    [CustomAuthenticationAttribute]
    public class BasicController : Controller
    {
        public int CurrentMenuLayout;
        public bool CanView;
        public bool CanAddEdit;
        public int CurrentUser;
        private string ControllerName;
        private string PageTitle;

        public BasicController()
        {
            string Controller = this.GetType().Name;
            ControllerName = Controller.Remove(Controller.Length - 10);
            CurrentUser = getUserId();
            ViewBag.id = GetCurrentMenuLayoutId();
            ViewBag.PageTitle = PageTitle;
        }
        private int GetCurrentMenuLayoutId()
        {
            if (SessionInfo.formlist == null || SessionInfo.formlist.FirstOrDefault(x => x.ControllerName == ControllerName) == null)
            {
                return 0;
            }
            int moduleId = SessionInfo.formlist.FirstOrDefault(x => x.ControllerName == ControllerName).FK_ParentId;
            

            if (moduleId == 0)
            {
                moduleId = SessionInfo.formlist.FirstOrDefault(x => x.ControllerName == ControllerName).PK_FormId;
            }

            PageTitle = SessionInfo.formlist.FirstOrDefault(x => x.PK_FormId == moduleId).FormName;
            return moduleId;
        }

        private dynamic getUserId()
        {
            if (SessionInfo.User == null)
            {
                return 0;
            }
            return SessionInfo.User.userid;
        }


    }
}