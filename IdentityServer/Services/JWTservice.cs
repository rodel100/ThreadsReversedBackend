using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Services
{
    public class JWTservice
    {
        private readonly IConfiguration _configuration;

        public JWTservice(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateTokens(List<Claim> tokenClaims) {
            var key = _configuration["JWT:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
                tokenClaims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var TokenString = new JwtSecurityTokenHandler().WriteToken(Token);

            return TokenString;
        }
        public string validateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["JWT:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                IssuerSigningKey = securityKey
            };
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return HttpStatusCode.OK.ToString();
        }
    }
}
