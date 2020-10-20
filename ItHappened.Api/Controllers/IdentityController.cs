using System;
using System.Reflection.Metadata.Ecma335;
using ItHappened.Api.Contracts;
using ItHappened.Api.Contracts.Requests;
using ItHappened.Api.Contracts.Requests.User;
using ItHappened.Api.Contracts.Responses;
using ItHappened.Api.Contracts.Responses.User;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventTrackerService _trackerService;

        public IdentityController(IUserService userService, IEventTrackerService trackerService)
        {
            _userService = userService;
            _trackerService = trackerService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        [ProducesResponseType(404)]
        public IActionResult CreateUser([FromBody]CreateUserRequest request)
        {
            var userId = _userService.CreateUser(request.Name);
            var response = new CreateUserResponse(userId);
            return Ok(response);
        }
    }
}