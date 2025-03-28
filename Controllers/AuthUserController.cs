using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Web.Security;
using ToDoApp.Services;
using System.Web.UI.WebControls;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class AuthUserController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Login(string returnUrl)
        {
            if (!_context.Users.Any())
            {
                return RedirectToAction("CreateFirstUser");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                var result = passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

                if (result == Microsoft.AspNet.Identity.PasswordVerificationResult.Success)
                {
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        username,
                        DateTime.Now, 
                        DateTime.Now.AddMinutes(30),
                        false,
                        user.Role,
                        FormsAuthentication.FormsCookiePath
                        );

                    // Encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    // Create the authentication cookie
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);
                    FormsAuthentication.SetAuthCookie(username, true);

                    return RedirectToAction("Index", "ToDo");
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","");
        }

        public ActionResult CreateFirstUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFirstUser(UserModel model, string password)
        {
            if (!_context.Users.Any()) // Ensure only first user gets Admin role
            {
                var passwordHasher = new Microsoft.AspNet.Identity.PasswordHasher();
                model.PasswordHash = passwordHasher.HashPassword("123");
                model.Role = "Admin"; // First user is Admin
                model.UserName = "Admin";
                _context.Users.Add(model);
                _context.SaveChanges();

                FormsAuthentication.SetAuthCookie(model.UserName, false);
                return RedirectToAction("Index", "ToDo");
            }
            return RedirectToAction("Login");
        }
    }
}