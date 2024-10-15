using SchoolMt.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FRGMBSystem.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipCustomAuthenticationAttribute), false).Any())
            {

                return;
            }
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
                        filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Account", action = "Login", area = "" }));
            }
        }
    }
}