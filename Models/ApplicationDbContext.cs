using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<UserModel,Role,int,UserLogin,UserRole,UserClaim>
    {
        public ApplicationDbContext() : base("name = PostgreSqlConnection") { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ToDoModel> ToDos { get; set; }
    }
    public class Role : IdentityRole<int, UserRole> { }
}