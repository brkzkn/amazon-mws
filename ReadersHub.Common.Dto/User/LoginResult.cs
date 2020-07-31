using ReadersHub.Common.Enums;

namespace ReadersHub.Common.Dto.User
{
    public class LoginResult<T>
    {
        public LoginResultType Result { get; private set; }

        public T User { get; private set; }


        public LoginResult(LoginResultType result)
        {
            Result = result;
        }

        public LoginResult(T user)
                : this(LoginResultType.Success)
        {
            User = user;
        }
    }
}
