using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                var adminUser = new UserModel
                {
                    UserName = "admin",
                    Name = "System Administrator",
                    PasswordHash = passwordHasher.HashPassword("123"),
                    Role = "Admin"
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
            }

            base.Seed(context);

        }
    }
}