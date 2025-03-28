using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class AuthService
    {
        private readonly UserManager<UserModel, int> _userManager;

        public AuthService()
        {
            var store = new UserStore<UserModel, Role, int, UserLogin, UserRole, UserClaim>(new ApplicationDbContext());

            _userManager = new UserManager<UserModel, int>(store);
            _userManager.PasswordHasher = new PasswordHasher();
        }

        public bool RegisterUser(string username, string password,string name)
        {
            var user = new UserModel { Name = name, UserName = username, PasswordHash = password };
            var result = _userManager.Create(user);
            return result.Succeeded;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _userManager.Find(username, password);
            return user != null;
        }
    }
}