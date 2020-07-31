using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadersHub.WebApplication.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Roles = new List<string>();
        }

        public int Id { get; set; }

        public bool IsEdit { get; set; }

        [Required]
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        
        [Required]
        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}