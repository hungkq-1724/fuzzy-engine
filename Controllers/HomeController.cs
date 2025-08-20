using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Users db = new Users();

        public ActionResult Index()
        {
            var articles = db.Articles
                .Include("Author")
                .Include("Comments")
                .OrderByDescending(a => a.CreatedAt)
                .Take(50)
                .ToList();

            return View(articles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArticle(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Json(new { success = false, error = "Tiêu đề không được trống." });

            if (title.Length > 200) // khớp [StringLength(200)]
                title = title.Substring(0, 200);

            if (string.IsNullOrWhiteSpace(content))
                return Json(new { success = false, error = "Nội dung không được trống." });

            var me = db.UserSet.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (me == null)
                return Json(new { success = false, error = "Không tìm thấy người dùng." });

            var article = new Article
            {
                Title = title.Trim(),
                Content = content.Trim(),
                AuthorId = me.Id,
                CreatedAt = DateTime.UtcNow
            };

            db.Articles.Add(article);
            db.SaveChanges();

            return Json(new
            {
                success = true,
                article = new
                {
                    id = article.Id,
                    title = article.Title,
                    content = article.Content,
                    author = string.IsNullOrEmpty(me.Username) ? me.Email : me.Username,
                    avatar = me.Avatar,
                    createdAt = article.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteArticle(long id)
        {
            var me = db.UserSet.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (me == null)
                return new HttpStatusCodeResult(401);

            var article = db.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
                return Json(new { success = false, error = "Bài viết không tồn tại." });

            if (article.AuthorId != me.Id)
                return new HttpStatusCodeResult(403);

            db.Articles.Remove(article);
            db.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ToggleFavorite(long articleId)
        {
            var me = db.UserSet.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (me == null) return new HttpStatusCodeResult(401);

            var fav = db.Favorites.FirstOrDefault(f => f.ArticleId == articleId && f.UserId == me.Id);

            bool liked;
            if (fav == null)
            {
                db.Favorites.Add(new Favorite { ArticleId = articleId, UserId = me.Id });
                liked = true;
            }
            else
            {
                db.Favorites.Remove(fav);
                liked = false;
            }
            db.SaveChanges();

            var count = db.Favorites.Count(f => f.ArticleId == articleId);

            return Json(new { success = true, liked, count });
        }

    }
}