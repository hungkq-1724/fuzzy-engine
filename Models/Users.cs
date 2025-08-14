using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication2.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public partial class Users : DbContext
    {
        public Users()
            : base("name=Users")
        {
        }

        public DbSet<User> UserSet { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Follow> Follows { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
