using System;
using System.Security.Claims;
using ItHappened.Api.Authentication;
using ItHappened.Api.Contracts;
using ItHappened.Api.Contracts.Requests.Trackers;
using ItHappened.Api.Contracts.Responses.Trackers;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class TrackersController : ControllerBase
    {
        public TrackersController(IUserService userService, IEventTrackerService trackerService)
        {
            _userService = userService;
            _trackerService = trackerService;
        }
        
        private readonly IUserService _userService;
        private readonly IEventTrackerService _trackerService;
        
        [HttpPost(ApiRoutes.Trackers.Create)]
        [ProducesResponseType(200, Type = typeof(CreateTrackerResponse))]
        public IActionResult CreateTracker([FromBody]CreateTrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackerId = _trackerService.CreateTracker(Guid.NewGuid(), request.TrackerName);
            var response = new CreateTrackerResponse(trackerId);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.Trackers.GetAll)]
        [ProducesResponseType(200, Type = typeof(EventTracker[]))]
        public IActionResult GetAllTrackers([FromBody]GetAllTrackersRequest request)
        {
            var trackers = _trackerService.GetAllUserTrackers(request.UserId);
            return Ok(trackers);
        }
        
        [HttpGet(ApiRoutes.Trackers.Get)]
        [ProducesResponseType(200, Type = typeof(GetTrackerResponse))]
        public IActionResult GetTracker([FromRoute]Guid trackerId, [FromBody]GetTrackerRequest request)
        {
            var trackers = _trackerService.GetAllUserTrackers(trackerId);
            return Ok(trackers);
        }
        
        [HttpPut(ApiRoutes.Trackers.Update)]
        [ProducesResponseType(200, Type = typeof(UpdateTrackerResponse))]
        public IActionResult UpdateTracker([FromRoute]Guid trackerId, [FromBody]UpdateTrackerRequest request)
        {
            //var trackers = _trackerService.GetTracker;
            return Ok();
        }

        [HttpDelete(ApiRoutes.Trackers.Delete)]
        [ProducesResponseType(200, Type = typeof(CreateTrackerResponse))]
        public IActionResult DeleteTracker([FromRoute]Guid trackerId, [FromBody]DeleteTrackerRequest request)
        {
            var code = _trackerService.DeleteTracker(trackerId, request.UserId);
            var response = new DeleteTrackerResponse();
            return Ok(response);
        }
        
        [HttpGet(ApiRoutes.Trackers.Statistics.GetForSingleTracker)]
        [ProducesResponseType(200, Type = typeof(CreateTrackerResponse))]
        public IActionResult GetStatisticsForSingleTracker([FromRoute]Guid trackerId)
        {
            return Ok();
        }
        
        [HttpGet(ApiRoutes.Trackers.Statistics.GetForMultipleTrackers)]
        [ProducesResponseType(200, Type = typeof(CreateTrackerResponse))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            return Ok();
        }
    }
}