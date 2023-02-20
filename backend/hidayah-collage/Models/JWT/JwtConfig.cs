using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.JWT
{
    public class JwtConfig
    {
        public string AccessTokenSecret { get; set; }
        public double AccessTokenExpirationSeconds { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double RefreshTokenExpirationDays { get; set; }
    }
}
