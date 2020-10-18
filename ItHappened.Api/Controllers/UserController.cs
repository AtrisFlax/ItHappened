using System;
using ItHappened.Api.Requests;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using ItHappened.Domain.User;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventTrackerService _trackerService;

        public UserController(IUserService userService, IEventTrackerService trackerService)
        {
            _userService = userService;
            _trackerService = trackerService;
        }

        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(404)]
        public IActionResult AuthenticateUser([FromBody]LoginRequest loginRequest)
        {
            var userInfo = _userService.AuthenticateUser(loginRequest.Login, loginRequest.Password);
            if (userInfo.status == UserServiceStatusCodes)
            {
                return NotFound();
            }

            var response = GetUserResponse(userInfo);

            return Ok(response);
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UserInfo[]))]
        public IActionResult GetAllUsers([FromQuery] int? take, [FromQuery] int? skip)
        {
            var users = _us.GetAll(take, skip).Select(GetUserResponse).ToArray();
            return Ok(users);
        }

        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.TryGetUserById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var response = GetUserResponse(user);

            return Ok(response);
        }

        [HttpGet]
        [Route("{userId}/posts")]
        [ProducesResponseType(200, Type = typeof(Post[]))]
        public IActionResult GetPostsByUser([FromRoute]int userId)
        {
            var posts = _postRepository.GetPostsByUser(userId);
            return Ok(posts);
        }
        
        [HttpPost]
        [Route("{userId}/posts")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [ProducesResponseType(200, Type = typeof(int))]
        [RandomErrorFilter(ErrorProbability = 0.3)]
        public IActionResult AddPost([FromRoute]int userId, AddPostRequest request)
        {
            var postId = _postRepository.AddPost(userId, request.Title, request.Content);
            return Ok(postId);
        }
        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult Register([FromBody]AddUserRequest request)
        {
            var userId = _userRepository.AddUser(request.Name);
            var user = _userRepository.TryGetUserById(userId);
            return Ok(user);
        }
    }
}