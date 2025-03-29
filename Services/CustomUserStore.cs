using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class CustomUserStore : IUserStore<UserModel, int>,
        IUserPasswordStore<UserModel, int>,
        IUserRoleStore<UserModel, int>,
        IQueryableUserStore<UserModel, int>
    {
        private readonly ApplicationDbContext _context;

        public CustomUserStore(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implement all required methods from the interfaces
        public Task CreateAsync(UserModel user)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(UserModel user)
        {
            _context.Users.Remove(user);
            return _context.SaveChangesAsync();
        }

        public Task<UserModel> FindByIdAsync(int userId)
        {
            return Task.FromResult(_context.Users.Find(userId));
        }

        public Task<UserModel> FindByNameAsync(string userName)
        {
            return Task.FromResult(_context.Users.FirstOrDefault(u => u.UserName == userName));
        }

        public Task UpdateAsync(UserModel user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(UserModel user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserModel user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(UserModel user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task AddToRoleAsync(UserModel user, string roleName)
        {
            user.Role = roleName;
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(UserModel user)
        {
            IList<string> roles = new List<string>();
            if (!string.IsNullOrEmpty(user.Role))
            {
                roles.Add(user.Role);
            }
            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(UserModel user, string roleName)
        {
            return Task.FromResult(user.Role == roleName);
        }

        public Task RemoveFromRoleAsync(UserModel user, string roleName)
        {
            if (user.Role == roleName)
            {
                user.Role = null;
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public IQueryable<UserModel> Users => _context.Users;
    }
}