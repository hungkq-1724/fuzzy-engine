using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication2.Models
{

    [Table("Comments")]
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public long AuthorId { get; set; }

        public long ArticleId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
