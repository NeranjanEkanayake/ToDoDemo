using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    [Table("UserModel", Schema = "public")]
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username {  get; set; }
        public string Password { get; set; }
    }
}