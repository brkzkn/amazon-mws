using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReadersHub.WebApplication.Core.Attribute
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissions;

        public PermissionAttribute(string permission)
        {
            _permissions = new[] { permission };
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (!isAuthorized)
            {
                //Logger.Warn("Not authorized for request." + GetRouteInfo(httpContext.Request.RequestContext));
                return false;
            }

            bool hasPermission = Application.CurrentUser.HasAnyOfPermissions(_permissions);
            if (hasPermission)
            {
                return true;
            }

            //Logger.Warn("No permission [{0}] for request." + GetRouteInfo(httpContext.Request.RequestContext),
            //    string.Join("|", _permissions));

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            throw new NotImplementedException();
        }

        private string GetRouteInfo(RequestContext controllerContext)
        {
            string action = controllerContext.RouteData.Values["action"].ToString();
            string controller = controllerContext.RouteData.Values["controller"].ToString();

            var info = string.Format("/{0}/{1}", controller, action);

            var user = controllerContext.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                info += string.Format(" - User: [{0}]", user.Identity.Name);
            }

            return info;
        }

    }
}