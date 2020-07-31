using _21stSolution.Application.Services.Dto;
using _21stSolution.Dto.DataTable;
using _21stSolution.Extensions;
using ReadersHub.Business.Service.User;
using ReadersHub.Common.Constants;
using ReadersHub.Common.Dto.User;
using ReadersHub.WebApplication.Core;
using ReadersHub.WebApplication.Core.Attribute;
using ReadersHub.WebApplication.Models;
using System.Web.Mvc;

namespace ReadersHub.WebApplication.Controllers
{
    public class UserController : AuthorizedController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: User
        [Permission(Permissions.User)]
        public ActionResult Index()
        {
            return View();
        }

        [Permission(Permissions.User)]
        public PartialViewResult AddUser(int userId = 0)
        {
            UserViewModel model = new UserViewModel();
            if (userId > 0)
            {
              var dto =  _userService.GetUser(userId);
                model = GetModel(dto);
            }
            return PartialView("_Create", model);
        }

        [HttpPost]
        [Permission(Permissions.User)]
        public JsonResult Create(UserViewModel model)
        {
            _21stSolutionAjaxResponse response = new _21stSolutionAjaxResponse();

            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Lütfen zorunlu alanları giriniz";
                return ReadersHubJson(response);
            }
            
            if (model.Password != model.PasswordAgain)
            {
                response.IsSuccess = false;
                response.Message = "Girdiğiniz şifreler aynı değil";
                return ReadersHubJson(response);
            }

            var dto = GetDto(model);
            _userService.AddUser(dto);

            response.IsSuccess = true;
            response.Message = "Kullanıcı başarıyla eklendi";

            return ReadersHubJson(response);
        }

        [Permission(Permissions.User)]
        public JsonResult GetList(DTParametersDto<UserFilterDto> param)
        {
            var request = param.Convert();

            PagedResult<UserExtendedDto> response = _userService.GetUsers(request);

            var result = new DTResult<UserExtendedDto>
            {
                draw = param.Draw,
                data = response.Items,
                recordsFiltered = response.TotalCount,
                recordsTotal = response.TotalCount
            };

            return Json(result);
        }

        private UserExtendedDto GetDto(UserViewModel model)
        {
            var dto = new UserExtendedDto()
            {
                EmailAddress = model.Email,
                Id = model.Id,
                FullName = model.FullName,
                Password = model.Password,
                UserName = model.UserName,
                RoleList = model.Roles
            };

            return dto;
        }

        private UserViewModel GetModel(UserExtendedDto dto)
        {
            var model = new UserViewModel()
            {
                Email = dto.EmailAddress,
                FullName = dto.FullName,
                Id = dto.Id,
                Password = dto.Password,
                UserName = dto.UserName,
                Roles = dto.RoleList
            };

            return model;
        }

    }
}