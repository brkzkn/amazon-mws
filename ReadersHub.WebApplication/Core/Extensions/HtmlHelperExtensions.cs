using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ReadersHub.WebApplication.Core;

namespace ReadersHub.WebApplication.Core
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString GetDate(this HtmlHelper helper)
        {
            return new MvcHtmlString(DateTime.Now.ToString());
        }

        public static MvcHtmlString GetMessage(this HtmlHelper helper, NotificationHelper.MessageType messageType, string message)
        {
            return new MvcHtmlString(NotificationHelper.Get(messageType, message));
        }

        public static MvcHtmlString GetMessage(this HtmlHelper helper, NotificationHelper.Notification notification)
        {
            return new MvcHtmlString(NotificationHelper.Get(notification));
        }

        public static MvcHtmlString ConvertListToJsArray<T>(this HtmlHelper helper, string jsArrayName, IEnumerable<T> list)
        {
            StringBuilder sb = new StringBuilder();

            if (list != null)
            {
                sb.Append($"var {jsArrayName} = [{string.Join(",", list)}]; ");
            }
            else
            {
                sb.Append($"var {jsArrayName} = []; ");
            }


            return MvcHtmlString.Create(sb.ToString());
        }
    }
}