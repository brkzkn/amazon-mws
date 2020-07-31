using _21stSolution.Application.Services.Dto;
using _21stSolution.Dto;
using ReadersHub.Common.Dto.User;
using ReadersHub.Common.Enums;
using System.Collections.Generic;

namespace ReadersHub.Business.Service.User
{
    public interface IUserService
    {
        LoginResult<UserExtendedDto> Login(string usernameOrEmail, string password);
        long AddUser(UserExtendedDto userDto);

        ICollection<SelectItemDto> GetActiveUsers();

        PagedResult<UserExtendedDto> GetUsers(PagedRequest<UserFilterDto> request);

        UserExtendedDto GetUser(long id);

        void UpdateUser(UserExtendedDto dto);
        void Delete(long id);
        void ChangePassword(UserPasswordDto dto);

        void ForgotPassword(string userName);

        void EmailConfirmation(string emailConfirmationCode);

        void ResetPassword(UserPasswordDto dto);

        bool HasPasswordResetCode(string resetCode);

        string ProcessLoginAttempt(string username, LoginResultType resultType);
    }
}
