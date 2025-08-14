using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication2.Models
{
    public class Favorite
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; }

        public long ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
