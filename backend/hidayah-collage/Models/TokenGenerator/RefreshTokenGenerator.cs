using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.TokenGenerator
{
    public class RefreshTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(IConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            //DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes);
            DateTime expirationTime = DateTime.Now.AddDays(30);
            return _tokenGenerator.GenerateToken(
                _configuration["JWT:Refresh"],
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"],
                expirationTime);
        }
    }
}
