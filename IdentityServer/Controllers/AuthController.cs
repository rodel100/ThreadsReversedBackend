
using IdentityServer.Models;
using IdentityServer.Models.DTO;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.DotNet.MSIdentity.Shared;
using System.Net;
using System.Security.Claims;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ThreadUser> _userManager;
        private readonly SignInManager<ThreadUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;
        private readonly JWTservice _jwtService;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<ThreadUser> userManager, JWTservice jwtService, AuthService authService,IConfiguration configuration, SignInManager<ThreadUser> signInManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _authService = authService;
            _configuration = configuration;
            _signInManager = signInManager;
            _logger = logger;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registermodel)
        {
            try
            {
               String message = "";
                var userExist = await _userManager.FindByNameAsync(registermodel.Username);
                if (userExist != null)
                {
                    message = "User exist, choose another username";
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                var emailExist = await _userManager.FindByEmailAsync(registermodel.Email);
                if (emailExist != null)
                {
                    message = "Email exist, use another email";
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                var newUser = new ThreadUser
                {
                    UserName = registermodel.Username,
                    Email = registermodel.Email,
                };
                var createUser = _userManager.CreateAsync(newUser, registermodel.Password);
                if (!createUser.IsCompletedSuccessfully)
                {
                    message = "Error with user";
                    return (StatusCode(StatusCodes.Status500InternalServerError, message));
                }
                return (StatusCode(StatusCodes.Status200OK, "User created"));
            }
            catch (Exception e)
            {
                return (StatusCode(StatusCodes.Status500InternalServerError, e.Message));
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest();
            }
            var validUser = await _authService.ValidateUser(loginDTO.UserName, loginDTO.Password);
            var AuthClaims = await _authService.AuthClaims(validUser);
            var token = _jwtService.GenerateTokens(AuthClaims);
            return Ok(token);
        }
        [HttpGet]
        [Route("/Sign-in-with-facebook")]
        public async Task<IActionResult> FacebookLogin()
        {
            var redirectUrl = Url.Action("FacebookRedirect", "Auth", null, Request.Scheme);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            var facebookChallenge = new ChallengeResult("Facebook", properties);
            return facebookChallenge;
        }
        [HttpGet]
        [Route("/facebookRedirect")]
        public async Task<IActionResult> FacebookRedirect()
        {
            try
            {
                var getinfo = await _signInManager.GetExternalLoginInfoAsync();
                var name = getinfo.Principal.FindFirstValue(ClaimTypes.Name);
                var email = getinfo.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(email);
                if(user == null)
                {
                    var newUser = new ThreadUser
                    {
                        UserName = name,
                        Email = email,
                    };
                    var createUser = await _userManager.CreateAsync(newUser);
                    if (!createUser.Succeeded)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error with user");
                    }
                    user = newUser;
                }
                if (user != null)
                {
                    var AuthClaims = await _authService.AuthClaims(user);
                    var token = _jwtService.GenerateTokens(AuthClaims);
                    return Ok(token);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "There is no User");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            };
        }
    }
}
