using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StockManager.Models
{

    /*
    Disabled for testing default Auth attribute with new remember me and roles implementation
    */
    
    //public class CheckAuth : AuthorizeAttribute
    //{   
    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        if (!HttpContext.Current.Request.IsAuthenticated || HttpContext.Current.Session["UserID"] == null)
    //        {
    //            if (filterContext.HttpContext.Request.IsAjaxRequest())
    //            {
    //                filterContext.HttpContext.Response.StatusCode = 302;
    //                filterContext.HttpContext.Response.End();
    //            }
    //            else
    //            {
    //                filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl + "?ReturnUrl=" +
    //                     filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
    //            }
    //        }
    //        else
    //        {

    //            if ( !String.IsNullOrEmpty(Roles) )
    //            {
    //                var roleName = HttpContext.Current.Session["RoleName"];

    //                if (!Roles.Equals(roleName))
    //                {
    //                    filterContext.Result = new RedirectResult("NoAccess");
    //                }
    //            }
    //        }
    //    }
    //}
}