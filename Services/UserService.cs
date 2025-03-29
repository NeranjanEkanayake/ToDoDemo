using BCrypt.Net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    [Authorize]
    public class UserService
    {
        private readonly UserManager<UserModel, int> _userManager;
        private readonly ApplicationDbContext _context;
        public UserService(UserManager<UserModel, int> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<UserModel> GetAll()
        {
            return _context.Users.ToList();
        }

        public UserModel GetUserByUsername(string username)
        {
            return _userManager.FindByName(username);
        }

        public UserModel GetUserById(int id)
        {
            return _userManager.FindById(id);
        }

        public bool AuthenticateUser(string username, string password)
        {
            var user = _userManager.FindByName(username);

            if (user == null)
            {
                return false;
            }

           return _userManager.CheckPassword(user, password);
        }

        
        //public UserModel GetUserByName(string name)
        //{
        //    return _context.Users.FirstOrDefault(n => n.Name);
        //}

        public IdentityResult CreateUser(UserModel user, string password)
        {
            try
            {
                // Verify the user object
                if (user == null)
                {
                    return IdentityResult.Failed("User object is null");
                }

                // Verify password
                if (string.IsNullOrWhiteSpace(password))
                {
                    return IdentityResult.Failed("Password cannot be empty");
                }

                // Hash the password before saving
                var hasher = new PasswordHasher();
                user.PasswordHash = hasher.HashPassword(password);

                // Add to context
                _context.Users.Add(user);

                // Save changes
                var changes = _context.SaveChanges();

                // Verify save
                if (changes > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed("No changes saved to database");
            }
            catch (Exception ex)
            {
                // Log the full error
                System.Diagnostics.Debug.WriteLine($"User creation failed: {ex}");
                return IdentityResult.Failed($"Error: {ex.Message}");
            }
        }

        public IdentityResult UpdateUser(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = _userManager.FindById(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Name = user.Name;
                existingUser.Role = user.Role;

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    _userManager.RemovePassword(existingUser.Id);
                    _userManager.AddPassword(existingUser.Id, user.PasswordHash);
                }
                return _userManager.Update(existingUser);
            }
            return IdentityResult.Failed("User not found");
        }

        public IdentityResult DeleteUser(int id)
        {
            var user = _userManager.FindById(id);
            if (user != null)
            {
                return _userManager.Delete(user);
            }
            return IdentityResult.Failed("User not found.");
        }
    }
}