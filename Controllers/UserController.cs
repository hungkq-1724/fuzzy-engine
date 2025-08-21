using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using NLog;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        private Users db = new Users();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Login", "Session");

                var email = User.Identity.Name;
                var user = db.UserSet.FirstOrDefault(u => u.Email == email);

                if (user == null) return HttpNotFound();

                return View(user);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UserController - Index: " + ex.Message);
                return new HttpStatusCodeResult(500, "Đã xảy ra lỗi khi tải trang Index.");
            }
        }

        public ActionResult Edit()
        {
            try
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
            catch (Exception ex)
            {
                logger.Error(ex, "UserController - Edit(GET): " + ex.Message);
                return new HttpStatusCodeResult(500, "Đã xảy ra lỗi khi tải trang Edit.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateUserModel model, HttpPostedFileBase avatarFile)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error(ex, "UserController - Edit(POST): " + ex.Message);
                return new HttpStatusCodeResult(500, "Đã xảy ra lỗi khi cập nhật thông tin.");
            }
        }

        [Authorize]
        public new ActionResult Profile(long id)
        {
            try
            {
                var user = db.UserSet.FirstOrDefault(u => u.Id == id);
                if (user == null) return HttpNotFound();

                var articles = db.Articles
                                 .Include("Author")
                                 .Include("Comments")
                                 .Where(a => a.AuthorId == id)
                                 .OrderByDescending(a => a.CreatedAt)
                                 .ToList();

                var me = db.UserSet.FirstOrDefault(u => u.Email == User.Identity.Name);
                bool isFollowing = false;
                if (me != null)
                {
                    isFollowing = db.Follows.Any(f => f.FollowerId == me.Id && f.FollowingId == id);
                }

                var model = new ProfileViewModel
                {
                    User = user,
                    Articles = articles,
                    IsFollowing = isFollowing
                };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UserController - Profile: " + ex.Message);
                return new HttpStatusCodeResult(500, "Đã xảy ra lỗi khi tải trang Profile.");
            }
        }

        [HttpPost]
        public ActionResult ToggleFollow(long id)
        {
            try
            {
                var me = db.UserSet.FirstOrDefault(u => u.Email == User.Identity.Name);
                if (me == null) return new HttpStatusCodeResult(401);

                var follow = db.Follows.FirstOrDefault(f => f.FollowerId == me.Id && f.FollowingId == id);

                if (follow == null)
                {
                    db.Follows.Add(new Follow { FollowerId = me.Id, FollowingId = id });
                }
                else
                {
                    db.Follows.Remove(follow);
                }
                db.SaveChanges();

                return RedirectToAction("Profile", new { id });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UserController - ToggleFollow: " + ex.Message);
                return new HttpStatusCodeResult(500, "Đã xảy ra lỗi khi thay đổi trạng thái theo dõi.");
            }
        }
    }
}
