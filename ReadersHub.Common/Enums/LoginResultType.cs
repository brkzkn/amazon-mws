namespace ReadersHub.Common.Enums
{
    public enum LoginResultType
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,

        InvalidPassword,

        UserIsNotActive,

        UserEmailIsNotConfirmed,

        UnknownExternalLogin,

        PasswordResetNeeded,
        InvalidUserNameOrPassword,

    }
}
