using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name = PostgreSqlConnection") { }

        public DbSet<ToDoModel> ToDos { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}