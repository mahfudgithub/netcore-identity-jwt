using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
