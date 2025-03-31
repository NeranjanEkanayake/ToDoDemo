using BCrypt.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<UserModel> _passwordHasher;
        public UserService(
            UserManager<UserModel, int> userManager,
            PasswordHasher<UserModel> passwordHasher,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
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

        public Microsoft.AspNet.Identity.IdentityResult CreateUser(UserModel user, string password)
        {
            try
            {
                // Verify the user object
                if (user == null)
                {
                    return Microsoft.AspNet.Identity.IdentityResult.Failed("User object is null");
                }

                // Verify password
                if (string.IsNullOrWhiteSpace(password))
                {
                    return Microsoft.AspNet.Identity.IdentityResult.Failed("Password cannot be empty");
                }

                // Hash the password before saving               
                user.PasswordHash = _passwordHasher.HashPassword(user, password);

                // Add to context
                _context.Users.Add(user);

                // Save changes
                var changes = _context.SaveChanges();

                // Verify save
                if (changes > 0)
                {
                    return Microsoft.AspNet.Identity.IdentityResult.Success;
                }
                return Microsoft.AspNet.Identity.IdentityResult.Failed("No changes saved to database");
            }
            catch (Exception ex)
            {
                // Log the full error
                System.Diagnostics.Debug.WriteLine($"User creation failed: {ex}");
                return Microsoft.AspNet.Identity.IdentityResult.Failed($"Error: {ex.Message}");
            }
        }

        public Microsoft.AspNet.Identity.IdentityResult UpdateUser(UserModel user, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            existingUser.UserName = user.UserName;
            existingUser.Name = user.Name;
            existingUser.Role = user.Role;

            if (!string.IsNullOrWhiteSpace(password))
            {
                //var hasher = new PasswordHasher();
                //existingUser.PasswordHash = hasher.HashPassword(password);
                existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, password);
            }
            try
            {
                _context.SaveChanges(); // Commit changes to the database
                return Microsoft.AspNet.Identity.IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return Microsoft.AspNet.Identity.IdentityResult.Failed($"Error updating user: {ex.Message}");
            }
        }

        public Microsoft.AspNet.Identity.IdentityResult DeleteUser(int id)
        {
            var user = _userManager.FindById(id);
            if (user != null)
            {
                return _userManager.Delete(user);
            }
            return Microsoft.AspNet.Identity.IdentityResult.Failed("User not found.");
        }
    }
}