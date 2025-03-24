using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDoApp.Models
{
    [Table("TodoModel",Schema ="public")]
    public class ToDoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; } = DateTime.Now;
    }
}