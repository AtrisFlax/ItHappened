using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Authentication;
using ItHappened.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/user")]
        [ProducesResponseType(200, Type = typeof(UserResponse))]
        public IActionResult Register([FromBody] UserRequest request)
        {
            var userWithToken = _userService.Register(request.UserName, request.Password);
            return Ok(new UserResponse(userWithToken.User.Id, userWithToken.User.Name, userWithToken.Token));
        }

        [HttpGet]
        [Route("/user")]
        [ProducesResponseType(200, Type = typeof(UserWithToken))]
        public IActionResult Authenticate([FromBody] UserRequest request)
        {
            var userWithToken = _userService.Authenticate(request.UserName, request.Password);
            return Ok(new UserResponse(userWithToken.User.Id, userWithToken.User.Name, userWithToken.Token));
        }
    }
}