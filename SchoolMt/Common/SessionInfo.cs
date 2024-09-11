using MDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolMt.Common
{
    public class SessionInfo
    {


        public static UserMDL User
        {
            get
            {
                UserMDL _user = null;
                if (HttpContext.Current.Session["User"] != null)
                {
                    _user = (UserMDL)(HttpContext.Current.Session["User"]);
                }
                return _user;
            }
            set
            {
                HttpContext.Current.Session["User"] = value;
            }
        }
        //public static CompanyRightsMDL CompanyRights
        //{
        //    get
        //    {
        //        CompanyRightsMDL _CompanyRightsMDL = null;
        //        if (HttpContext.Current.Session["CompanyRights"] != null)
        //        {
        //            _CompanyRightsMDL = (CompanyRightsMDL)(HttpContext.Current.Session["CompanyRights"]);
        //        }
        //        return _CompanyRightsMDL;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["CompanyRights"] = value;
        //    }
        //}


        //public static List<Menu> Menus
        //{
        //    get
        //    {
        //        List<Menu> _menus = null;
        //        if (HttpContext.Current.Session["Menus"] != null)
        //        {
        //            _menus = (List<Menu>)(HttpContext.Current.Session["Menus"]);
        //        }
        //        return _menus;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["Menus"] = value;
        //    }
        //}
        public static List<FormMDL> formlist
        {
            get
            {
                List<FormMDL> _formlist = null;
                if (HttpContext.Current.Session["formlist"] != null)
                {
                    _formlist = (List<FormMDL>)(HttpContext.Current.Session["formlist"]);
                }
                return _formlist;
            }
            set
            {
                HttpContext.Current.Session["formlist"] = value;
            }
        }
        public static void RemoveSession()
        {
            formlist = null;
            User = null;
        }
    }
}