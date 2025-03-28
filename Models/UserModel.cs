using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class UserModel : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {        
        public string Name { get; set; } 
        public string Role { get; set; }

        public UserModel() 
        {
            Role = "User";
        }
    }

    public class UserLogin : IdentityUserLogin<int> { }
    public class UserRole : IdentityUserRole<int> { }
    public class UserClaim : IdentityUserClaim<int> { }
}