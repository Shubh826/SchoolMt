using FRGMBMDLClassLibrary;
using FRGMBSystem.Common;
using MDL;
using SchoolMt.Common;
using SchoolMt.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FRGMBSystem.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public class CustomAuthenticationAttribute : ActionFilterAttribute, IActionFilter
    {
           
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipCustomAuthenticationAttribute), false).Any())
            {
               
                return;
            }
            if (filterContext.HttpContext.Session["User"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.ClearContent();
                    filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                    //filterContext.Result = new RedirectToRouteResult(new
                    //           RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
                    //filterContext.Result = new RedirectToRouteResult(new
                    //         RouteValueDictionary(new { controller = "Account", action = "LogOff", area = "" }));

                    //         new RedirectToRouteResult(
                    //new RouteValueDictionary
                    // {
                    //     { "controller", "Account" },
                    //     { "action", "Timeout" }
                    // });
                }

                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));

                    //         new RedirectToRouteResult(
                    //new RouteValueDictionary
                    // {
                    //     { "controller", "Account" },
                    //     { "action", "Login" }
                    // });
                }
            }
            else
            {
                var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                List<FormMDL> menus = SessionInfo.formlist;

                FormMDL MenuItem = menus.SingleOrDefault(m => m.ControllerName == ControllerName);

                if (MenuItem == null || MenuItem.CanView == false)
                {
                    if (MenuItem == null && ControllerName == "Dashboard")
                    {

                        filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Dashboard", action = "Dashboard", area = "" }));
                        //    new RedirectToRouteResult(
                        //    new RouteValueDictionary {
                        //{"Controller","Home"},
                        //{"Action","Dashboard"}
                        //});
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Account", action = "LogOff", area = "" }));

                        //    new RedirectToRouteResult(
                        //    new RouteValueDictionary {
                        //{"Controller","Account"},
                        //{"Action","LogOff"}
                        //});
                    }

                }
                else
                {
                    // user = SessionInfo.User;
                    filterContext.HttpContext.Items["UserRoleAndRights"] = new UserRoleAndRightsMDL() {CanView = MenuItem.CanView, CanAdd = MenuItem.CanAdd, CanEdit = MenuItem.CanEdit, CanDelete= MenuItem.CanDelete };


                }

            }
        }
     }
}