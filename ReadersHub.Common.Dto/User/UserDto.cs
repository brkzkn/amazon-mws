using _21stSolution.Extensions;
using System;

namespace ReadersHub.Common.Dto.User
{
    public class UserDto : ICloneableType
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; } 
        public string EmailAddress { get; set; }
        public System.DateTime? RegisteredDate { get; set; } 
        public int Status { get; set; } 
        public string ImageUrl { get; set; } 
        public int RoleId { get; set; }
        public int? ParentId { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public string EmailConfirmationCode { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
}
