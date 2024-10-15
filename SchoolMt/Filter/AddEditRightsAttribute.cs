using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRGMBSystem.Filter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AddEditRightsAttribute : ActionFilterAttribute, IActionFilter
    {
    }
}