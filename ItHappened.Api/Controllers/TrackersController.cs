using System;
using ItHappened.Api.Contracts;
using ItHappened.Api.Contracts.Requests.Trackers;
using ItHappened.Api.Contracts.Responses.Trackers;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
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
            var trackerId = _trackerService.CreateTracker(request.UserId, request.TrackerName);
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
        public IActionResult CreateTracker([FromRoute]Guid trackerId, [FromBody]DeleteTrackerRequest request)
        {
            var code = _trackerService.DeleteTracker(trackerId, request.UserId);
            var response = new DeleteTrackerResponse();
            return Ok(response);
        }
    }
}