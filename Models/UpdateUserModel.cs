using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class UpdateUserModel
    {
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(255), EmailAddress]
        public string Email { get; set; }
    }
}