using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace hidayah_collage.Models.TokenGenerator
{
    public class TokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer, string audience, DateTime expirationTime,
            IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
