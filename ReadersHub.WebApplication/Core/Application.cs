using ReadersHub.Common.Dto.User;
using System.Web;

namespace ReadersHub.WebApplication.Core
{
    public static class Application
    {
        private const string _prefrences = "_loginPreferences";

        public static UserExtendedDto CurrentUser
        {
            get
            {
                var loggedUser = (HttpContext.Current.Session["__loggedUser"] as UserExtendedDto);

                if (HttpContext.Current.User != null && !HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    loggedUser = null;
                }

                return loggedUser;
            }
        }

        public static void SetCurrentUserInfo(UserExtendedDto user)
        {
            HttpContext.Current.Session["__loggedUser"] = user;
        }
    }
}