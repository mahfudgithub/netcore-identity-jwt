using hidayah_collage.Models.JWT;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace hidayah_collage.Models.TokenGenerator
{
    public class AccessTokenGenerator
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly JwtConfig _jwtConfig;

        public AccessTokenGenerator(TokenGenerator tokenGenerator, JwtConfig jwtConfig)
        {
            _tokenGenerator = tokenGenerator;
            _jwtConfig = jwtConfig;
        }
        public LoginResponse GenerateToken(ApplicationUser user, Claim[] roles)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("name", user.FirstName +" "+ user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
            };
            foreach(Claim claim in roles)
            {
                claims.Add(claim);
            }            

            //DateTime expirationTime = DateTime.Now.AddSeconds(double.Parse(_configuration["JWT:AccessTokenExpirationSeconds"], CultureInfo.InvariantCulture));
            DateTime expirationTime = DateTime.Now.AddSeconds(_jwtConfig.AccessTokenExpirationSeconds);
            //DateTime expirationTime = DateTime.Now.AddSeconds(5);
            //DateTime expirationTime = DateTime.Now.AddHours(15);
            string token = _tokenGenerator.GenerateToken(
                //_configuration["JWT:Secret"],
                //_configuration["JWT:ValidIssuer"],
                //_configuration["JWT:ValidAudience"],
                _jwtConfig.AccessTokenSecret,
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
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
