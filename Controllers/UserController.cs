using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        private Users db = new Users();

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Session");

            var email = User.Identity.Name;
            var user = db.UserSet.FirstOrDefault(u => u.Email == email);

            if (user == null) return HttpNotFound();

            return View(user);
        }

        public ActionResult Edit()
        {
            var email = User.Identity.Name;
            var user = db.UserSet.FirstOrDefault(u => u.Email == email);

            if (user == null) return HttpNotFound();

            var model = new UpdateUserModel
            {
                Username = user.Username,
                Avatar = user.Avatar
            };

            ViewBag.email = user.Email;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateUserModel model, HttpPostedFileBase avatarFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = User.Identity.Name;
            var user = db.UserSet.FirstOrDefault(u => u.Email == email);

            if (user == null) return HttpNotFound();

            if (user.Username != model.Username)
                user.Username = model.Username;

            if (avatarFile != null && avatarFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(avatarFile.FileName);

                string userFolder = Path.Combine(Server.MapPath("~/Uploads/Avatars"), user.Id.ToString());
                Directory.CreateDirectory(userFolder);

                string path = Path.Combine(userFolder, fileName);
                avatarFile.SaveAs(path);

                user.Avatar = $"/Uploads/Avatars/{user.Id}/{fileName}";
            }

            db.SaveChanges();

            TempData["Success"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }

    }
}