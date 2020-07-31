
using Repository.Pattern;

namespace ReadersHub.Model
{

    // Users
    
    public class User : Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public string Username { get; set; } // Username (length: 50)
        public string Password { get; set; } // Password (length: 255)
        public string FullName { get; set; } // Full_Name (length: 100)
        public string Email { get; set; } // Email (length: 150)
        public System.DateTime? RegisteredDate { get; set; } // Registered_Date

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<UserRole> UserRoles { get; set; } // User_Role.FK_User_Role_Users

        public User()
        {
            UserRoles = new System.Collections.Generic.List<UserRole>();
        }
    }

}

