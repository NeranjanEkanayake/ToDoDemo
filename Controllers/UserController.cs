using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService) {
            _userService = userService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = _userService.AuthenticateUser(userModel.Username, userModel.Password);

                if (isAuthenticated)
                {
                    Session["Username"] = userModel.Username;
                    Session["Name"] = userModel.Name;
                    return RedirectToAction("Index", "ToDo");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(userModel);
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Clear session
            return RedirectToAction("Index", "User");
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult CreateUser(int? id)
        {
            UserModel userModel = new UserModel();

            if (id.HasValue)
            {
                userModel = _userService.GetUserById(id.Value);
                if (userModel == null)
                {
                    TempData["ErrorMessage"] = "User not found";
                    return RedirectToAction("CreateUser");
                }
            }
            return View(userModel);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserModel userModel)
        {
                if (ModelState.IsValid)
                {
                    if (userModel.Id == 0)
                    {
                        _userService.CreateUser(userModel);

                        TempData["SuccessMessage"] = "Created user successfully";
                    }
                    else
                    {
                        var existingUser = _userService.GetUserById(userModel.Id);

                        if (existingUser != null)
                        {
                            existingUser.Username = userModel.Username;
                            existingUser.Password = userModel.Password;
                            existingUser.Name = userModel.Name;

                            _userService.UpdateUser(existingUser);
                            TempData["SuccessMessage"] = "Added user successfully";                            
                        }
                        
                    }
                    return RedirectToAction("Index", "ToDo");
                }

            return View(userModel);          
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
