using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: User/UserList/5
        public ActionResult UserList()
        {
            // Get logged-in user username from identity
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "AuthUser"); // If not authenticated, redirect to login
            }

            // Fetch the user by username and get their role
            var user = _userService.GetAll().FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                ViewBag.UserRole = user.Role;  // Assuming 'Role' is your custom column in the User table
            }

            if (user?.Role != "Admin")  // Check if the user is not an Admin
            {
                return RedirectToAction("Index", "ToDo");
            }

            var users = _userService.GetAll();
            return View(users);
        }

        //    if (Session["Username"] == null || Session["Username"].ToString() != "admin")
        //    {
        //        return RedirectToAction("Index", "ToDo");
        //    }
        //    var users = _userService.GetAll();
        //    return View(users);
        //}

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
                    userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash);
                    _userService.CreateUser(userModel);

                    TempData["SuccessMessage"] = "Created user successfully";
                }
                else
                {
                    var existingUser = _userService.GetUserById(userModel.Id);

                    if (existingUser != null)
                    {
                        existingUser.UserName = userModel.UserName;
                        existingUser.Name = userModel.Name;

                        if (!string.IsNullOrEmpty(userModel.PasswordHash))
                        {
                            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.PasswordHash);
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

                existingUser.UserName = userModel.UserName;
                existingUser.Name = userModel.Name;
                existingUser.PasswordHash = userModel.PasswordHash;

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
