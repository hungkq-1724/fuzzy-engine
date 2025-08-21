using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Article> Articles { get; set; }
        public bool IsFollowing { get; set; }
    }

}