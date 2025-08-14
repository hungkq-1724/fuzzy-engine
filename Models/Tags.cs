using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication2.Models
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)] 
        public string Name { get; set; }
    }
}
