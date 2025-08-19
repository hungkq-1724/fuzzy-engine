using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class UpdateUserModel
    {
        [StringLength(50)]
        public string Username { get; set; }

        public string Avatar { get; set; }
    }
}