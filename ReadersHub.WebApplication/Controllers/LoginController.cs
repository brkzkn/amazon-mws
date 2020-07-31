using ReadersHub.Business.Service.User;
using ReadersHub.WebApplication.Core;
using ReadersHub.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ReadersHub.WebApplication.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DoLogin(LoginViewModel model)
        {
            var response = new _21stSolutionAjaxResponse()
            {
                IsSuccess = false
            };

            if (ModelState.IsValid)
            {
                var result = _userService.Login(model.UsernameOrEmailAddress, model.Password);
                if (result.Result == Common.Enums.LoginResultType.Success)
                {
                    FormsAuthentication.SetAuthCookie(result.User.UserName, false);

                    Application.SetCurrentUserInfo(result.User);

                    //if (model.RememberMe)
                    //{
                    //    var jsonDto = JsonConvert.SerializeObject(result);
                    //    var userCookie = new HttpCookie(UserCookieKey, jsonDto);
                    //    userCookie.Expires.AddDays(15);
                    //    Response.SetCookie(userCookie);
                    //}

                    response.IsSuccess = true;
                    response.RedirectUrl = Url.Action("Index", "Product");
                }

                switch (result.Result)
                {
                    case Common.Enums.LoginResultType.Success:
                        response.Message = "Başarılı";
                        break;
                    case Common.Enums.LoginResultType.InvalidUserNameOrEmailAddress:
                    case Common.Enums.LoginResultType.InvalidPassword:
                    case Common.Enums.LoginResultType.InvalidUserNameOrPassword:
                    case Common.Enums.LoginResultType.UserIsNotActive:
                    case Common.Enums.LoginResultType.UserEmailIsNotConfirmed:
                        response.Message = "Geçersiz kullanıcı adı veya şifre";
                        break;
                    default:
                        response.Message = result.Result.ToString();
                        break;
                }
            }

            return ReadersHubJson(response);
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            if (Request.Cookies[UserCookieKey] != null)
            {
                var user = new HttpCookie(UserCookieKey)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = null
                };
                Response.SetCookie(user);
            }

            return RedirectToAction("Index", "Login");
        }
    }
}