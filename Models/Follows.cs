using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication2.Models
{
    public class Follow
    {
        [Key]
        public long Id { get; set; }

        public long FollowerId { get; set; }
        public virtual User Follower { get; set; }

        public long FollowingId { get; set; }
        public virtual User Following { get; set; }
    }
}
