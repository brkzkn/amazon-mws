using _21stSolution.Extensions;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Core
{
    public class BaseController : Controller
    {
        internal readonly string UserCookieKey = "__YFuser__";
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private readonly Logger _logger;
        private readonly Logger _longRequestLogger;
        //internal int MEDIA_PAGE_SIZE = Convert.ToInt32(ConfigurationManager.AppSettings["MediaPageSize"].ToString());

        public BaseController()
        {
            _logger = LogManager.GetLogger(GetType().FullName);
            _longRequestLogger = LogManager.GetLogger("LongRequestLogger");

        }

        public Logger Logger
        {
            get
            {
                return _logger;
            }
        }

        public Logger LongRequestLogger
        {
            get
            {
                return _longRequestLogger;
            }
        }

        protected string ControllerName
        {
            get { return ControllerContext.RouteData.Values["controller"] + ""; }
        }

        protected string ActionName
        {
            get { return ControllerContext.RouteData.Values["action"] + ""; }
        }

        protected bool IsUserSessionExists()
        {
            return System.Web.HttpContext.Current.Session["__loggedUser"] != null;
        }
        //protected override void OnAuthorization(AuthorizationContext filterContext)
        //{

        //    if (User.Identity.IsAuthenticated && IsUserSessionExists())
        //    {
        //        var uiCulture = (Prizma.Common.Enums.UICulture)Application.CurrentUser.LanguageId;
        //        Logger.Info("Setting UI Culture to [{0}] - User: [{1}]", uiCulture, User.Identity.Name);
        //        PageHelper.InitializeCulture(uiCulture);
        //    }

        //    base.OnAuthorization(filterContext);
        //}

        protected override void OnException(ExceptionContext filterContext)
        {
            Logger.Error("Route Info: " + GetRouteInfo(filterContext));

            HandleEntityValidationException(filterContext.Exception);
            HandleException(filterContext.Exception);

            filterContext.ExceptionHandled = true;

            var statusCode = 500;

            //if (filterContext.Exception is BaseException)
            //{
            //    if ((filterContext.Exception as BaseException).Code == ExceptionConstants.PermissionDenied)
            //    {
            //        statusCode = 401;
            //    }
            //}

            var errorInfo = new ErrorInfo
            {
                IsAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest(),
                IsChildAction = filterContext.IsChildAction,
                StatusCode = statusCode,
                Exception = filterContext.Exception
            };

            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            //--------------------------------------------------

            ActionResult errorResult;

            HttpContext.Response.StatusCode = statusCode;
            if (errorInfo.IsAjaxRequest)
            {
                Logger.Info("Returning Ajax Response for Error");
                errorResult = GetAjaxErrorResult(errorInfo);
            }
            else
            {
                Logger.Info("Returning Redirection Response for Error");
                errorResult = GetErrorResult(errorInfo);
            }

            //-----------------------------------------------------

            filterContext.Result = errorResult;
        }

        protected void HandleEntityValidationException(Exception exception)
        {
            string message = string.Empty;

            var validationException = exception as DbEntityValidationException;
            if (validationException != null)
            {
                foreach (var validationErrors in validationException.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        message += string.Format("|| {0}:{1}",
                            validationErrors.Entry.Entity,
                            validationError.ErrorMessage);
                    }
                }
            }

            Logger.Error("DbEntityValidationException: " + message);
        }

        protected void HandleException(Exception exception)
        {
            Logger.Error(exception, "Exception: " + exception.Message);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ShowNotification = false;
            ViewBag.BaseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            Logger.Info("Begin_Execution Action:" + GetRouteInfo(filterContext));
            _stopWatch.Start();

            foreach (KeyValuePair<string, object> actionParameter in filterContext.ActionParameters)
            {
                //Logger.Info("Parameter[\"{0}\"]: [{1}]", actionParameter.Key, actionParameter.Value.GetAllPropertiesWithValues());
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopWatch.Stop();
            string log = "End_Execution Action:" + GetRouteInfo(filterContext) + " Completed in " + _stopWatch.ElapsedMilliseconds + " ms.";
            Logger.Info(log);
            LogLongRequest(log);
        }

        private void LogLongRequest(string log)
        {
            int longRequestLimitMin = 5;
            if (_stopWatch.ElapsedMilliseconds > longRequestLimitMin * 60 * 1000)
                LongRequestLogger.Trace(log + " LongRequestLimit : " + longRequestLimitMin);
        }

        public JsonResult ReadersHubJson(_21stSolutionAjaxResponse response)
        {
            if (!response.IsSuccess && HttpContext.Response.StatusCode == 200)
            {
                Logger.Trace("StatusCode is 200 but Response not succeeded. Setting StatusCode to 400");
                HttpContext.Response.StatusCode = 400;
            }

            return new JsonResult
            {
                ContentEncoding = Encoding.UTF8,
                Data = response, //Newtonsoft.Json.JsonConvert.SerializeObject(response),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                ContentType = "application/json",
                MaxJsonLength = Int32.MaxValue
            };
        }

        protected new internal JsonResult Json(object data)
        {
            var result = base.Json(data);
            result.MaxJsonLength = Int32.MaxValue;
            return result;
        }

        protected new internal JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            var result = base.Json(data, behavior);
            result.MaxJsonLength = Int32.MaxValue;
            return result;
        }

        public void PutFileIntoCache(string fileKey, byte[] data, string contentType, string fileName)
        {
            var rec = new ErrorFileInfo
            {
                Data = data,
                ContentType = contentType,
                FileName = fileName
            };

            HttpContext.Cache.Insert(fileKey, rec, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        public void PutObjectIntoCache(string fileKey, object data)
        {
            HttpContext.Cache.Insert(fileKey, data, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero, CacheItemPriority.High, null);
        }

        //public List<T> GetListFromCache<T>(string fileKey)
        //{
        //    if (HttpContext.Cache[fileKey] == null)
        //    {
        //        throw new BusinessException(ExceptionConstants.FileNotFound);
        //    }

        //    var record = HttpContext.Cache[fileKey];
        //    HttpContext.Cache.Remove(fileKey);

        //    return record as List<T>;
        //}

        //public FileResult GetFileFromCacheByKey(string fileKey, bool removeCache)
        //{
        //    if (HttpContext.Cache[fileKey] == null)
        //    {
        //        throw new BusinessException(ExceptionConstants.FileNotFound);
        //    }

        //    var record = (ErrorFileInfo)HttpContext.Cache[fileKey];

        //    if (removeCache)
        //    {
        //        HttpContext.Cache.Remove(fileKey);
        //    }

        //    return File(record.Data, record.ContentType, record.FileName);
        //}

        //----------------------

        private string GetRouteInfo(ControllerContext controllerContext)
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

        private ActionResult GetErrorResult(ErrorInfo eInfo)
        {
            TempData["__errorInfo"] = eInfo;
            return Redirect(Url.Action("Index", "Error", new { area = "" }));
        }

        private JsonResult GetAjaxErrorResult(ErrorInfo eInfo)
        {
            var message = eInfo.Exception.ToString();

            if (eInfo.StatusCode == 401)
            {
                HttpContext.Response.StatusCode = 401;
                HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }

            return ReadersHubJson(new _21stSolutionAjaxResponse
            {
                IsSuccess = false,
                Message = message,
                Code = eInfo.StatusCode.ToString()
            });
        }

        public void SetNotificationInfo(NotificationHelper.MessageType messageType, string message)
        {
            ViewBag.ShowNotification = true;
            ViewBag.NotificationMessage = message;
            ViewBag.NotificationType = messageType;
        }
        
    }

}