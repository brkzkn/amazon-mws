using ReadersHub.Common.Dto.User;
using System;

namespace ReadersHub.Common.Dto.Mapper
{
    public static class UserMapper
    {
        public static UserDto ConvertToDto(this Model.User entity)
        {
            return new UserDto()
            {
                Id = entity.Id,
                EmailAddress = entity.Email,
                FullName = entity.FullName,
                Password = entity.Password,
                RegisteredDate = entity.RegisteredDate ?? DateTime.Now,
                UserName = entity.Username,
            };
        }

        public static Model.User ConvertToEntity(this UserDto dto)
        {
            return new Model.User()
            {
                Id = dto.Id,
                Email = dto.EmailAddress,
                FullName = dto.FullName,
                Password = dto.Password,
                RegisteredDate = dto.RegisteredDate,
                Username = dto.UserName,
            };
        }
    }
}
