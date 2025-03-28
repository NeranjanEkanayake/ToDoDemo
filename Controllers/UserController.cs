﻿using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
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
                    var tempUser = _userService.GetUserByUsername(userModel.Username);

                    if (tempUser != null)
                    {
                        var user = _userService.GetUserById(tempUser.Id);
                        Session["Username"] = user.Username;
                        Session["Name"] = user.Name;
                        return RedirectToAction("Index", "ToDo");
                    }
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
            Session.Clear();
            Session.Abandon();

            FormsAuthentication.SignOut();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();

            return RedirectToAction("Index", "User");
        }

        // GET: User/UserList/5
        public ActionResult UserList()
        {
            if (Session["Username"] == null || Session["Username"].ToString() != "admin")
            {
                return RedirectToAction("Index", "ToDo");
            }
            var users = _userService.GetAll();
            return View(users);
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
                    userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                    _userService.CreateUser(userModel);

                    TempData["SuccessMessage"] = "Created user successfully";
                }
                else
                {
                    var existingUser = _userService.GetUserById(userModel.Id);

                    if (existingUser != null)
                    {
                        existingUser.Username = userModel.Username;
                        existingUser.Name = userModel.Name;

                        if (!string.IsNullOrEmpty(userModel.Password))
                        {
                            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                        }
                        _userService.UpdateUser(existingUser);
                        TempData["SuccessMessage"] = "Added user successfully";
                    }

                }
                return RedirectToAction("Index", "ToDo");
            }

            return View(userModel);
        }

        // GET: User/EditUser/5
        public ActionResult EditUser(int id)
        {
            var existingUser = _userService.GetUserById(id);

            if(existingUser == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("UserList");
            }
            return View(existingUser);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("User/EditUser/{id}")]
        public ActionResult EditUser(int id, UserModel userModel)
        {
            if (userModel == null)
            {
                TempData["ErrorMessage"] = "Invalid User";

                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var existingUser = _userService.GetUserById(id);
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "Error getting user, unable to update";
                    return RedirectToAction("UserList");
                }

                existingUser.Username = userModel.Username;
                existingUser.Name = userModel.Name;
                existingUser.Password = userModel.Password;

                _userService.UpdateUser(existingUser);
                TempData["SuccessMessage"] = "Updated user successfully";
                return RedirectToAction("UserList");
            }
            TempData["ErrorMessage"] = "Data table unavailable";
            return RedirectToAction("UserList");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("User/DeleteUser/{id}")]
        public ActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            TempData["SuccessMessage"] = "User Deleted successfully";
            return RedirectToAction(nameof(UserList));
        }
    }
}
