using System;
using System.Web.Mvc;
using System.Web.Security;
using ReadersHub.WebApplication.Core.Attribute;

namespace ReadersHub.WebApplication.Core
{
    public class AuthorizedController : BaseController
    {
        private ActionResult GetUserNotAuthenticatedResult()
        {
            var loginUrl = Url.Action("Index", "Login");

            ActionResult userNotAuthenticatedResult;


            userNotAuthenticatedResult = Redirect(loginUrl);

            return userNotAuthenticatedResult;
        }

        private ActionResult GetUserSessionExpiredResult()
        {
            ActionResult sessionExpiredResult;

            var loginUrl = Url.Action("Index", "Login");

            sessionExpiredResult = Redirect(loginUrl);

            return sessionExpiredResult;
        }


        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Do not check for authorization
                //if (!User.Identity.IsAuthenticated || !IsUserSessionExists())
                //{
                //    FormsAuthentication.SignOut();

                //    Server.ClearError();
                //    Response.Clear();

                //    filterContext.Result = GetUserSessionExpiredResult();

                //    return;
                //}
            }
            else
            {
                if (!User.Identity.IsAuthenticated && HttpContext.Request.IsAjaxRequest())
                {
                    Server.ClearError();
                    Response.Clear();

                    filterContext.Result = GetUserNotAuthenticatedResult();
                    return;
                }

                if (User.Identity.IsAuthenticated && !IsUserSessionExists())
                {
                    FormsAuthentication.SignOut();

                    Server.ClearError();
                    Response.Clear();

                    filterContext.Result = GetUserSessionExpiredResult();

                    return;
                }

                // If action method is not defined as AllowAnonymous & doesn't have permission attribute:
                if (!filterContext.ActionDescriptor.IsDefined(typeof(PermissionAttribute), true))
                {
                    throw new Exception(string.Format("PermissionNotDefined /{0}/{1}", ControllerName, ActionName));
                }
            }
            base.OnAuthorization(filterContext);
        }

    }
}