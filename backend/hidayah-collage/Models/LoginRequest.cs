using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models
{
    public class LoginRequest
    {
        [Required, EmailAddress, StringLength(50)]
        public string Email { get; set; }
        [Required, StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
    }
}
