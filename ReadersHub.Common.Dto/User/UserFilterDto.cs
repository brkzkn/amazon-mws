using System;

namespace ReadersHub.Common.Dto.User
{

    public class UserFilterDto
    {
        public long? Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public bool? IsEmailConfirmed { get; set; }

        public long? ParentId { get; set; }

        public int? RoleId { get; set; }

        public DateTime? LastPasswordSetTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool? IsStatic { get; set; }

        public bool? IsActive { get; set; }

        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public string RoleName { get; set; }
    }
}
