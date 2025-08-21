using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly Users db = new Users();

        public ActionResult Show(long id)
        {
            var article = db.Articles
                            .Include("Author")
                            .FirstOrDefault(a => a.Id == id);

            if (article == null) return HttpNotFound();

            var comments = db.Comments
                             .Where(c => c.ArticleId == id)
                             .OrderByDescending(c => c.CreatedAt)
                             .ToList();

            ViewBag.Comments = comments;
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(long articleId, string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return RedirectToAction("Show", new { id = articleId });

            var me = db.UserSet.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (me == null) return new HttpStatusCodeResult(401);

            var comment = new Comment
            {
                ArticleId = articleId,
                AuthorId = me.Id,
                Body = body.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Show", new { id = articleId });
        }
    }
}
