using System;
using ItHappened.Api.Requests;
using ItHappened.Api.Responses;
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

        [HttpPost]
        [Route("{userId}")]
        [ProducesResponseType(404)]
        public IActionResult CreateUser([FromBody]CreateUserRequest createUserRequest)
        {
            var userId = _userService.CreateUser(createUserRequest.Name);
            var response = new CreateUserResponse(userId);
            return Ok(response);
        }
        
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult GetUser([FromBody]GetUserRequest userRequest)
        {
            var user = _userService.GetUser(userRequest.Id);
            if (user == null)
            {
                return NotFound();
            }

            //var response = new GetUserResponse(user.Guid);

            return Ok(user);
        }

        [HttpGet]
        [Route("{userId}/trackers")]
        [ProducesResponseType(200, Type = typeof(EventTracker[]))]
        public IActionResult GetPostsByUser([FromRoute]GetUserTrackingsRequest trackingsRequest)
        {
            var trackers = _trackerService.GetAllTrackers(trackingsRequest.Id);
            return Ok(trackers);
        }
        
        [HttpPost]
        [Route("{userId}/trackers")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult AddPost([FromRoute]AddTrackingRequest request)
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