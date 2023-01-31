using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hidayah_collage.Models.TokenGenerator
{
    public class AccessTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(IConfiguration configuration , TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }
        public LoginResponse GenerateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.NameIdentifier, user.UserName),
            };

            //DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration["JWT:AccessTokenExpirationMinutes"]);
            DateTime expirationTime = DateTime.Now.AddSeconds(15);
            string token = _tokenGenerator.GenerateToken(
                _configuration["JWT:Secret"],
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"],
                expirationTime,
                claims);

            return new LoginResponse()
            {
                Token = token,
                ExpireDate = expirationTime
            };
        }
    }
}
