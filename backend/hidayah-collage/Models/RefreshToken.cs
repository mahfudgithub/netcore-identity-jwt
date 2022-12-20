using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
