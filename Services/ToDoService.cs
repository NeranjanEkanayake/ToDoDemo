using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ToDoService
    {
        private readonly ApplicationDbContext context;

        public ToDoService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public List<ToDoModel> GetAll()
        {
            return context.ToDos.ToList();
        }

        public ToDoModel GetToDoById(int id)
        {
            return context.ToDos.Find(id);
        }

        public ToDoModel GetByName(string toDo)
        {
            return context.ToDos.FirstOrDefault(t=> t.Title.Equals(toDo,StringComparison.OrdinalIgnoreCase));
        }

        public void AddToDo(ToDoModel toDoModel)
        {
            context.ToDos.Add(toDoModel);
            context.SaveChanges();
        }

        public void UpdateToDo(ToDoModel toDoModel)
        {   
            if (toDoModel == null) { 
                throw new ArgumentNullException(nameof(toDoModel)); 
            }
          
            var existingToDo = context.ToDos.Find(toDoModel.Id);
            if (existingToDo != null)
            {
                existingToDo.Title = toDoModel.Title;
                existingToDo.Description = toDoModel.Description;
                existingToDo.EditedDate = toDoModel.EditedDate;
                existingToDo.EditedBy = toDoModel.EditedBy;
                context.SaveChanges();
            }                        
        }

        public void DeleteToDo(int id)
        {
            var toDo = context.ToDos.Find( id);
            if (toDo != null)
            {
                context.ToDos.Remove(toDo);
                context.SaveChanges();
            }
        }
    }
}