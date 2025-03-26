using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    public class ToDoController : Controller
    {
        public readonly ToDoService toDoService;

        public ToDoController(ToDoService _toDoService)
        {
            toDoService = _toDoService;
        }


        public ActionResult Search(string searchQuery)
        {
            ViewBag.SearchQuery = searchQuery;

            if (string.IsNullOrEmpty(searchQuery))
            {
                return PartialView("_SearchResults",new List<ToDoModel>());
            }
            var todos = toDoService.GetByName(searchQuery);
            return PartialView("_SearchResults", todos != null ? new List<ToDoModel> { todos } : new List<ToDoModel>());
            //if (!string.IsNullOrEmpty(searchQuery))
            //{
            //    ViewBag.SearchQuery = searchQuery;
            //    return View(todos);
            //}
            //return View(todos);
        }

        public ActionResult Index()
        {
            var todos = toDoService.GetAll();
            if (todos == null)
            {
                Debug.WriteLine("No Todos found");
                todos = new List<ToDoModel>();
            }
            return View(todos);
        }

        //public ActionResult Create()
        //{
        //    return View();
        //}

        

        public ActionResult Create(int? id)
        {
            ToDoModel model = new ToDoModel();

            if (id.HasValue)
            {
                model = toDoService.GetToDoById(id.Value);

                if(model == null)
                {
                    TempData["ErrorMessage"] = "ToDo Not Found";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ToDo/Create")]
        public ActionResult Create(ToDoModel todo)
        {
            if (ModelState.IsValid)
            {
                if(todo.Id == 0)
                {
                    toDoService.AddToDo(todo);

                    TempData["SuccessMessage"] = "To Do added successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    var existingToDo = toDoService.GetToDoById(todo.Id);

                    if (existingToDo == null)
                    {
                        TempData["ErrorMessage"] = "ToDo not found, unable to update.";
                        return RedirectToAction("Index");
                    }

                    existingToDo.Title = todo.Title;
                    existingToDo.Description = todo.Description;
                    existingToDo.EditedDate = DateTime.Now;
                    existingToDo.EditedBy = "Nera";

                    toDoService.UpdateToDo(existingToDo);

                    TempData["SuccessMessage"] = "ToDo updated successfully.";
                }
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Failed to create the To Do";
            return View(todo);
        }

        //modify this to create 
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var existingToDo = toDoService.GetToDoById(id);
            if (existingToDo == null)
            {
                TempData["ErrorMessage"] = "ToDo not found.";
                return RedirectToAction("Index"); // Redirect back to the Index page
            }
            return View(existingToDo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ToDo/Edit/{id}")]
        public ActionResult Edit(int id, ToDoModel toDoModel)
        {
            if (toDoModel == null)
            {
                TempData["ErrorMessage"] = "Invalide ToDo Data";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var existingToDo = toDoService.GetToDoById(id);

                if (existingToDo == null)
                {
                    TempData["ErrorMessage"] = "ToDo not found, unable to update.";
                    return RedirectToAction("Index");
                }

                existingToDo.Title = toDoModel.Title;
                existingToDo.Description = toDoModel.Description;
                existingToDo.EditedDate = DateTime.Now;
                existingToDo.EditedBy = "Nera";

                toDoService.UpdateToDo(existingToDo);

                TempData["SuccessMessage"] = "ToDo updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Data table error";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ToDo/Delete/{id}")]
        public ActionResult Delete(int id)
        {

            toDoService.DeleteToDo(id);
            TempData["SuccessMessage"] = $"Deleted todo Successfully: {id}";
            return RedirectToAction(nameof(Index));
        }
    }
}
