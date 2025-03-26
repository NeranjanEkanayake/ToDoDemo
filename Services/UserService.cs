using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Services
{
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

        public UserModel GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                return false;
            }
            return user.Password == password;
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
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
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