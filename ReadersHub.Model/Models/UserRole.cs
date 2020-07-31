
using Repository.Pattern;

namespace ReadersHub.Model
{

    // User_Role
    
    public class UserRole : Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int UserId { get; set; } // User_Id
        public string RoleName { get; set; } // Role_Name (length: 50)

        // Foreign keys
        public virtual User User { get; set; } // FK_User_Role_Users
    }

}

