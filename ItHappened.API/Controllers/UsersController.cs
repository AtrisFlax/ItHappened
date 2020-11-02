using System.Security.Claims;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Authentication;
using ItHappened.Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/users")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        public IActionResult Register([FromBody] UserRequest request)
        {
            var userWithToken = _userService.Register(request.UserName, request.Password);
            return Ok(new UserResponse(userWithToken.User.Id, userWithToken.User.Name, userWithToken.Token));
        }

        [HttpGet]
        [Route("/users")]
        [ProducesResponseType(200, Type = typeof(UserWithToken))]
        public IActionResult Authenticate([FromBody] UserRequest request)
        {
            var userWithToken = _userService.Authenticate(request.UserName, request.Password);
            return Ok(new UserResponse(userWithToken.User.Id, userWithToken.User.Name, userWithToken.Token));
        }

        [HttpGet]
        [Route("/self")]
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