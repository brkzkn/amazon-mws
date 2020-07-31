using _21stSolution.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using ReadersHub.Common.Constants;

namespace ReadersHub.Common.Dto.User
{
    public class UserExtendedDto : UserDto, ICloneableType
    {
        public UserExtendedDto()
        {
            RoleList = new List<string>();
        }
        public List<string> RoleList { get; set; }
        public string StatusName { get; set; }

        public string Roles
        {
            get
            {
                return string.Join(",", RoleList).Replace(Permissions.Product, "Ürünler").Replace(Permissions.User, "Kullanıcılar").Replace(Permissions.Criterion, "Kriterler");
            }
        }

        public string RegisteredDateString
        {
            get
            {
                return RegisteredDate.Value.ToShortDateString();
            }
        }

        public bool HasPermission(string permissionId)
        {
            return RoleList.Contains(permissionId);
        }

        public bool HasAnyOfPermissions(IEnumerable<string> permissionIds)
        {
            return RoleList.Intersect(permissionIds).Any();
        }

        public void GrantPermissions(IEnumerable<string> permissions)
        {
            RoleList.AddRange(permissions);
        }
    }
}
