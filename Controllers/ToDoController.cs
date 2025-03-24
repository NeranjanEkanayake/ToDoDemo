using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToDoModel todo)
        {
            if (ModelState.IsValid)
            {
                db.ToDos.Add(todo);
                db.SaveChanges();
                TempData["SuccessMessage"] = "To Do added successfully";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Failed to create the To Do";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ToDo/Edit/{id}")]
        public ActionResult Edit(ToDoModel todo)
        {
            if (ModelState.IsValid)
            {
                var existingTodo = db.ToDos.Find(todo.Id);
                if (existingTodo != null)
                {
                    existingTodo.Title = todo.Title;
                    existingTodo.Description = todo.Description;
                    existingTodo.EditedDate = DateTime.Now;
                    existingTodo.EditedBy = todo.EditedBy;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Edit To Do done";
                }
            }
            TempData["ErrorMessage"] = "Data table error";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ToDo/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var todo = db.ToDos.Find(id);
            if (todo != null)
            {
                db.ToDos.Remove(todo);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Deleted todo Successfully";
                return RedirectToAction("Index");
            }else
            {
                TempData["ErrorMessage"] = "Error deleting todo";
            }
                return RedirectToAction("Index");
        }



        //// GET: ToDo
        //public ActionResult Index()
        //{
        //    var todos = db.ToDos.ToList();
        //    return View(todos);
        //}

        public ActionResult Index(string searchQuery)
        {
            var todos=db.ToDos.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                todos=todos.Where(t => t.Title.Contains(searchQuery));
            }
            ViewBag.SearchQuery = searchQuery;
            return View(todos.ToList());
        }

       
        //// GET: ToDo/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ToDo/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ToDo/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ToDo/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ToDo/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ToDo/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ToDo/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
