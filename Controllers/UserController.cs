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

 
        public ActionResult UserList()
        {
            var currentUser = _userService.GetUserByUsername(User.Identity.Name);

            if (currentUser == null || currentUser.Role != "Admin")
            {
                return RedirectToAction("Index", "ToDo");
            }
            
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "AuthUser");
            }

            var user = _userService.GetAll().FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                ViewBag.UserRole = user.Role;
            }

            if (user?.Role != "Admin")
            {
                return RedirectToAction("Index", "ToDo");
            }

            var users = _userService.GetAll();
            return View(users);
        }

        // GET: User/Create
        public ActionResult CreateUser(int? id)
        {
            if (id.HasValue)
            {
                var userModel = _userService.GetUserById(id.Value);
                if (userModel == null)
                {
                    TempData["ErrorMessage"] = "User not found";
                    return RedirectToAction("UserList");
                }
                return View(userModel);
            }
            return View(new UserModel());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserModel userModel, string password)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userModel);
                }

                if (userModel.Id == 0) // New user
                {
                    var result = _userService.CreateUser(new UserModel
                    {
                        UserName = userModel.UserName,
                        Name = userModel.Name,
                        Role = "User" // Default role
                    }, password);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User created successfully!";
                        return RedirectToAction("UserList");
                    }

                    // Add errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                else
                {
                    var existingUser = _userService.GetUserById(userModel.Id);
                    if (existingUser == null)
                    {
                        TempData["ErrorMessage"] = "User not found";
                        return RedirectToAction("UserList");
                    }

                    existingUser.UserName = userModel.UserName;
                    existingUser.Name = userModel.Name;
                    if (!string.IsNullOrEmpty(password))
                    {
                        existingUser.PasswordHash = password;
                    }

                    var updateResult = _userService.UpdateUser(existingUser);
                    if (updateResult.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User updated successfully";
                        return RedirectToAction("UserList");
                    }
                    return View(updateResult);
                }
                return View(userModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return View(userModel);
            }
        }

        // GET: User/EditUser/5
        public ActionResult EditUser(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("UserList");
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(int id, UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid User Data";
                return RedirectToAction("UserList");
            }

            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("UserList");
            }

            existingUser.UserName = userModel.UserName;
            existingUser.Name = userModel.Name;
            if (!string.IsNullOrEmpty(userModel.PasswordHash))
            {
                existingUser.PasswordHash = userModel.PasswordHash;
            }

            var updateResult = _userService.UpdateUser(existingUser);
            if (updateResult.Succeeded)
            {
                TempData["SuccessMessage"] = "User updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update user: " + string.Join(", ", updateResult.Errors);
            }

            return RedirectToAction("UserList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete user: " + string.Join(", ", result.Errors);
            }
            return RedirectToAction("UserList");
        }
    }
}
