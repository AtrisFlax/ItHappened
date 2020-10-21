using System;
using System.Security.Claims;
using ItHappened.Api.Authentication;
using ItHappened.Api.Contracts;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
            var user = _userService.TryFindByLogin(request.Name);
            if (user != null)
                return Unauthorized("Username is already in use");

            user = _userService.Register(request.Name, request.Password);
            var token = _jwtIssuer.GenerateToken(user);
            var response = new LoginResponse(token);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("login")]
        public IActionResult Authenticate([FromBody]LoginRequest request)
        {
            var user = _userService.TryFindByLogin(request.Name);
            if (user == null)
                return Unauthorized("User with provided credentials not found");
            
            var passwordHashedWithSalt = _passwordHasher.HashWithSalt(request.Password, user.Password.Salt);
            if (passwordHashedWithSalt != user.Password.Hash)
                return Unauthorized("User with provided credentials not found");
                
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