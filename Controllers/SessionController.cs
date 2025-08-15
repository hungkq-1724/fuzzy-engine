using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class SessionController : Controller
    {
        private Users db = new Users();

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.UserSet.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(model);
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = hashedPassword
                };

                db.UserSet.Add(user);
                db.SaveChanges();

                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string identifier, string password)
        {
            var user = db.UserSet
                .FirstOrDefault(u => u.Email == identifier || u.Username == identifier);

            System.Diagnostics.Debug.WriteLine(user);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
