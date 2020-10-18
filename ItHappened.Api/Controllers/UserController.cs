using System;
using ItHappened.Api.Requests;
using ItHappened.Api.Responses;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
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
        public IActionResult GetUser(string userId)
        {
            var user = _userService.GetUser(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var response = new GetUserResponse(user.Name);
            return Ok(user);
        }

        [HttpGet]
        [Route("{userId}/trackers")]
        [ProducesResponseType(200, Type = typeof(EventTracker[]))]
        public IActionResult GetUserTrackers([FromRoute]GetUserTrackerRequest trackerRequest)
        {
            var trackers = _trackerService.GetAllTrackers(trackerRequest.Id);
            return Ok(trackers);
        }
        
        [HttpPost]
        [Route("{userId}/tracker")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult AddTracker([FromRoute]AddTrackingRequest request)
        {
            var postId = _trackerService.CreateTracker(request.UserId, request.TrackingName);
            return Ok(postId);
        }
    }
}