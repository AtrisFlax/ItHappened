using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class TrackersController : ControllerBase
    {
        private readonly ITrackerService _trackerService;
        private readonly IMapper _mapper;

        public TrackersController(ITrackerService trackerService, IMapper mapper)
        {
            _trackerService = trackerService;
            _mapper = mapper;
        }

        [HttpPost("/trackers")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult CreateTracker([FromBody] TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var customizationSettings = _mapper.Map<TrackerCustomizationSettings>(request.CustomizationSettings);
            var tracker = _trackerService.CreateEventTracker(userId, request.Name, customizationSettings);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }


        [HttpGet("/trackers")]
        [ProducesResponseType(200, Type = typeof(List<TrackerResponse>))]
        public IActionResult GetAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var trackers = _trackerService.GetEventTrackers(userId);
            return Ok(_mapper.Map<List<TrackerResponse>>(trackers));
        }

        [HttpGet("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult GetTracker([FromRoute] Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));  //TODO issue  #169
            var tracker = _trackerService.GetEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpPut("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult UpdateTracker([FromRoute] Guid trackerId, [FromBody] TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));  //TODO issue  #169
            var customizations = _mapper.Map<TrackerCustomizationSettings>(request.CustomizationSettings);
            _trackerService.EditEventTracker(userId, trackerId, request.Name, customizations);
            return Ok();
        }

        [HttpDelete("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult DeleteTracker([FromRoute] Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));  //TODO issue  #169
            _trackerService.DeleteEventTracker(userId, trackerId);
            return Ok();
        }
    }
}