using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
    }
}
