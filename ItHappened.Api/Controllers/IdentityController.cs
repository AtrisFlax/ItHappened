using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using ItHappened.Api.Authentication;
using ItHappened.Api.Contracts;
using ItHappened.Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public IdentityController(IUserService userService, IJwtIssuer jwtIssuer, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _jwtIssuer = jwtIssuer;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        [Route("registration")]
        public IActionResult Register([FromBody]LoginRequest request)
        {
            var user = _userService.TryFindByLogin(request.Login);
            if (user != null)
            {
                return Unauthorized("Username is already in use");
            }

            var hashedPassword = _passwordHasher.Hash(request.Password);
            var token = _jwtIssuer.GenerateToken(user);
            var response = new LoginResponse(token);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("authentication")]
        public IActionResult Authenticate([FromBody]LoginRequest request)
        {
            var user = _userService.TryFindByLogin(request.Login);
            var hashedPassword = _passwordHasher.Hash(request.Password);
            if (user == null || hashedPassword != request.Password)
            {
                return Unauthorized("User with provided credentials not found");
            }

            var token = _jwtIssuer.GenerateToken(user);
            var response = new LoginResponse(token);
            return Ok(response);
        }

        [HttpGet]
        [Route("self")]
        [Authorize]
        public IActionResult GetSelf()
        {
            var result = new
            {
                Id = User.FindFirstValue(JwtClaimTypes.Id),
                Login = User.FindFirstValue(JwtClaimTypes.Login)
            };
            
            return Ok(result);
        }
        
        private readonly IUserService _userService;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IPasswordHasher _passwordHasher;
    }
}