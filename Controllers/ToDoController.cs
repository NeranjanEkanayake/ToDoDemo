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
        public JsonResult Create(ToDoModel todo)
        {
            if (ModelState.IsValid)
            {
                db.ToDos.Add(todo);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [Route("ToDo/Edit/{id}")]
        public JsonResult Edit(int id, ToDoModel updatedToDo)
        {
            var todo = db.ToDos.Find(id);
            if (todo != null)
            {
                todo.Title = updatedToDo.Title;
                todo.Description = updatedToDo.Description;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [Route("ToDo/Delete/{id}")]
        public JsonResult Delete(int id)
        {
            var todo = db.ToDos.Find(id);
            if (todo != null)
            {
                db.ToDos.Remove(todo);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }




        //// GET: ToDo
        public ActionResult Index()
        {
            var todos = db.ToDos.ToList();
            return View(todos);
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
