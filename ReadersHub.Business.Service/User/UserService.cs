using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _21stSolution.Application.Services.Dto;
using _21stSolution.Dto;
using ReadersHub.Common.Dto.User;
using ReadersHub.Common.Enums;
using ReadersHub.Common.Dto.Mapper;
using _21stSolution.Helper;
using Microsoft.AspNet.Identity;
using _21stSolution.Extensions;
using System.Linq.Expressions;
using System.Data.Entity;
using _21stSolution;

namespace ReadersHub.Business.Service.User
{
    public class UserService : Service<Model.User>, IUserService
    {
        private readonly IRepository<Model.User> _repository;
        private readonly IRepository<Model.UserRole> _userRolerepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryable<Model.User> _table;

        public UserService(IRepository<Model.User> repository, IRepository<Model.UserRole> userRolerepository, IUnitOfWork unitOfWork)
            : base(repository)
        {
            _repository = repository;
            _userRolerepository = userRolerepository;
            _unitOfWork = unitOfWork;
            _table = base.Queryable();
        }

        public LoginResult<UserExtendedDto> Login(string usernameOrEmail, string password)
        {
            var entity = _table.SingleOrDefault(x => x.Username == usernameOrEmail);

            if (entity == null)
            {
                return new LoginResult<UserExtendedDto>(LoginResultType.InvalidUserNameOrPassword);
            }

            var passwordRes = new PasswordHasher().VerifyHashedPassword(entity.Password, password);
            if (passwordRes != PasswordVerificationResult.Success)
            {
                return new LoginResult<UserExtendedDto>(LoginResultType.InvalidPassword);
            }

            var dto = entity.ConvertToDto();
            var extendedDto = dto.CloneTo<UserExtendedDto>();

            /// TODO: Set RoleIdList
            /// extendedDto.RoleIdList = new List<int>();
            /// 
            var list = _userRolerepository.Queryable().Where(x => x.UserId == dto.Id).Select(x => x.RoleName).ToList();
            if (list.Count > 0)
            {
                extendedDto.RoleList.AddRange(list);
            }

            return new LoginResult<UserExtendedDto>(extendedDto);
        }

        public long AddUser(UserExtendedDto userDto)
        {
            var _userRoleRepository = _repository.GetRepository<Model.UserRole>();

            var user = userDto.ConvertToEntity();
            user.RegisteredDate = DateTime.Now;


            var passwordHasher = new PasswordHasher();
            user.Password = passwordHasher.HashPassword(userDto.Password);

            try
            {
                _unitOfWork.BeginTransaction();
                _repository.Insert(user);
                _unitOfWork.SaveChanges();
                var userId = user.Id;

                userDto.RoleList?.ForEach(f => _userRoleRepository.Insert(
                    new Model.UserRole()
                    {
                        RoleName = f,
                        UserId = userId
                    }));
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return userId;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public ICollection<SelectItemDto> GetActiveUsers()
        {
            throw new NotImplementedException();
        }

        public PagedResult<UserExtendedDto> GetUsers(PagedRequest<UserFilterDto> request)
        {

            /// Set default orders
            /// 
            if (request.Orders.Count == 0)
            {
                request.Orders.Add(new Order()
                {
                    IsDescending = true,
                    OrderBy = "Id",
                    Priority = 1
                });
            }

            var query = _table.Sort(request.Orders);

            var condition = GetFilterExpression(request.Filters);
            if (condition != null)
            {
                query = query.Where(condition);
            }

            var count = query.Count();
            var items = query.Skip(request.SkipCount).Take(request.MaxResultCount).Include(x => x.UserRoles).Select(x => new UserExtendedDto()
            {
                Id = x.Id,
                EmailAddress = x.Email,
                UserName = x.Username,
                RegisteredDate = x.RegisteredDate,
                FullName = x.FullName,
                RoleList = x.UserRoles.Select(y => y.RoleName).ToList(),
            }).ToList();
            
            return new PagedResult<UserExtendedDto> { Items = items, TotalCount = count };
        }

        public UserExtendedDto GetUser(long id)
        {
            var entity = _table.Include(x => x.UserRoles).SingleOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new _21stSolutionException("Kullanıcı bulunamadı");
            }
            var dto = entity.ConvertToDto();
            var extendedDto = dto.CloneTo<UserExtendedDto>();
            if (entity.UserRoles.Count > 0)
            {
                extendedDto.RoleList = entity.UserRoles.Select(x => x.RoleName).ToList();
            }

            return extendedDto;
        }

        public void UpdateUser(UserExtendedDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(UserPasswordDto dto)
        {
            throw new NotImplementedException();
        }

        public void ForgotPassword(string userName)
        {
            throw new NotImplementedException();
        }

        public void EmailConfirmation(string emailConfirmationCode)
        {
            throw new NotImplementedException();
        }

        public void ResetPassword(UserPasswordDto dto)
        {
            throw new NotImplementedException();
        }

        public bool HasPasswordResetCode(string resetCode)
        {
            throw new NotImplementedException();
        }

        public string ProcessLoginAttempt(string username, LoginResultType resultType)
        {
            throw new NotImplementedException();
        }


        private Expression<Func<Model.User, bool>> GetFilterExpression(UserFilterDto filter)
        {
            Expression<Func<Model.User, bool>> condition = null;

            if (filter != null)
            {
                if (!filter.EmailAddress.IsNullOrEmpty()) { condition = condition.And(x => x.Email.Contains(filter.EmailAddress)); }
                if (!filter.FullName.IsNullOrEmpty()) { condition = condition.And(x => x.FullName.Contains(filter.FullName)); }
                if (!filter.UserName.IsNullOrEmpty()) { condition = condition.And(x => x.Username.Contains(filter.UserName)); }
            }

            return condition;
        }

    }
}
