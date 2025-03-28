using BCrypt.Net;
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
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserModel> GetAll()
        {
            return _context.Users.ToList();
        }

        public UserModel GetUserByUsername(string username)
        {
            var tempUser = _context.Users.FirstOrDefault(x=>x.UserName == username);
            return tempUser;
        }

        public UserModel GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            return passwordValid;
        }

        
        //public UserModel GetUserByName(string name)
        //{
        //    return _context.Users.FirstOrDefault(n => n.Name);
        //}

        public void CreateUser(UserModel user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = _context.Users.Find(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.Name = user.Name;

                _context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}