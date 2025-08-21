using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class HomeViewModel
    {
        public List<Article> AllArticles { get; set; }
        public List<Article> FollowingArticles { get; set; }
    }
}