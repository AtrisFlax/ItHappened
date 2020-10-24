using System;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public IdentityController(IUserService userService, IJwtIssuer jwtIssuer, IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _userService = userService;
            _jwtIssuer = jwtIssuer;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        public IActionResult Register([FromBody] UserRequest request)
        {
            var user = _userService.TryFindByLogin(request.Name);
            if (user != null)
                return Unauthorized("Username is already in use");

            
            user = _userService.Register(request.Name, request.Password);
            var token = _jwtIssuer.GenerateToken(user);
            return Ok(new UserResponse(user.Id, user.Name, token));
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        public IActionResult Authenticate([FromBody] UserRequest request)
        {
            var user = _userService.TryFindByLogin(request.Name);
            if (user == null)
                return Unauthorized("User with provided credentials not found");

            var passwordHashedWithSalt = _passwordHasher.HashWithSalt(request.Password, user.Password.Salt);
            if (passwordHashedWithSalt != user.Password.Hash)
                return Unauthorized("User with provided credentials not found");

            var token = _jwtIssuer.GenerateToken(user);
            return Ok(new UserResponse(user.Id, user.Name, token));
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
    }
}