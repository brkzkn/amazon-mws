
namespace ReadersHub.Common.Dto.User
{
    public class UserPasswordDto
    {
        public long UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PasswordResetCode { get; set; }
    }
}
