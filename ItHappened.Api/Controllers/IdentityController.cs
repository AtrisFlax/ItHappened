using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.UserService;
using ItHappened.Infrastructure;
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
            var user = _userService.Register(request.UserName, request.Password);
            var token = _jwtIssuer.GenerateToken(user);
            return Ok(new UserResponse(user.Id, user.Name, token));
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200, Type = typeof(Token))]
        public IActionResult Authenticate([FromBody] UserRequest request)
        {
            var token = _userService.Authenticate(request.UserName, request.Password);
            return Ok(token);
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