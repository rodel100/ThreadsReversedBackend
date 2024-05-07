using IdentityServer.Models;
using IdentityServer.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace IdentityServer.Services
{
    public class AuthService
    {

        private readonly Regex ValidateUserNameRegex = new Regex("^[a-zA-Z0-9]([._-](?![._-])|[a-zA-Z0-9]){3,18}[a-zA-Z0-9]$");
        private readonly Regex ValidateUserNameEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
        private readonly UserManager<ThreadUser> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ThreadUser> _roleManager;
        private readonly SignInManager<ThreadUser> _signInManager;
        public AuthService(UserManager<ThreadUser> userManager) { 
            _userManager = userManager;
        }
        public async Task<ThreadUser?> ValidateUser(string Username, string password)
        { 
            if (ValidateUserNameEmailRegex.IsMatch(Username)){
                var user = await _userManager.FindByEmailAsync(Username);
                var AuthenticatedUser = await _userManager.CheckPasswordAsync(user, password);
                if (AuthenticatedUser)
                {
                    return user;
                }
            }
            return null;
        }
        public async Task<List<Claim>> AuthClaims(ThreadUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach(var userRole in  userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            return claims;
        }
    }
}
