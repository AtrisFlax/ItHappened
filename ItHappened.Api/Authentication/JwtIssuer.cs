using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ItHappened.Api.Options;
using ItHappened.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ItHappened.Api.Authentication
{
    public class JwtIssuer : IJwtIssuer
    {
        private readonly JwtOptions _jwtOptions;

        public JwtIssuer(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Login, user.Name)
            };
            var secret = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var jwtToken = new JwtSecurityToken(
                claims: claims, 
                expires: DateTime.Now.Add(_jwtOptions.ExpiresAfter),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secret), 
                    SecurityAlgorithms.HmacSha256Signature));
            
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return tokenString;
        }
    }
}