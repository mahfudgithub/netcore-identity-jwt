using hidayah_collage.Models.JWT;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.TokenGenerator
{
    public class RefreshTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;
        private readonly JwtConfig _jwtConfig;

        public RefreshTokenGenerator(IConfiguration configuration, TokenGenerator tokenGenerator, JwtConfig jwtConfig)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _jwtConfig = jwtConfig;
        }

        public string GenerateToken()
        {
            //DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes);
            //DateTime expirationTime = DateTime.Now.AddDays(double.Parse(_configuration["JWT:RefreshTokenExpirationDays"], CultureInfo.InvariantCulture));
            DateTime expirationTime = DateTime.Now.AddDays(_jwtConfig.RefreshTokenExpirationDays);
            //DateTime expirationTime = DateTime.Now.AddDays(30);
            return _tokenGenerator.GenerateToken(
                //_configuration["JWT:Refresh"],
                //_configuration["JWT:ValidIssuer"],
                //_configuration["JWT:ValidAudience"],
                _jwtConfig.RefreshTokenSecret,
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                expirationTime);
        }
    }
}
