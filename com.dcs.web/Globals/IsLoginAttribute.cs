using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.dcs.web.Globals
{
    public class IsLoginAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session["CurrentUser"] == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}