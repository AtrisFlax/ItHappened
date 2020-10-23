using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class TrackersController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ITrackerService _trackerService;
        private readonly ITrackerRepository _trackerRepository;
        private readonly IMapper _mapper;
        
        public TrackersController(ITrackerService trackerService,
            ITrackerRepository trackerRepository,
            IStatisticsService statisticsService,
            IMapper mapper)
        {
            _trackerService = trackerService;
            _trackerRepository = trackerRepository;
            _statisticsService = statisticsService;
            _mapper = mapper;
        }
        
        [HttpPost("/trackers")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult CreateTracker([FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customizations = _mapper.Map<TrackerCustomizationSettings>(request.CustomizationSettings);
            var tracker = _trackerService.CreateEventTracker(userId, request.Name, customizations);
            var map = _mapper.Map<TrackerResponse>(tracker);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpGet("/trackers")]
        [ProducesResponseType(200, Type = typeof(List<TrackerResponse>))]
        public IActionResult GetAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackers = _trackerService.GetEventTrackers(userId);
            return Ok(_mapper.Map<List<TrackerResponse>>(trackers));
        }
        
        [HttpGet("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult GetTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracker = _trackerService.GetEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        
        [HttpPut("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult UpdateTracker([FromRoute]Guid trackerId, [FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customizations = _mapper.Map<TrackerCustomizationSettings>(request.CustomizationSettings);
            var tracker = _trackerService.EditEventTracker(userId, trackerId, request.Name, customizations);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpDelete("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult DeleteTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _trackerService.DeleteEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(deletedTracker));
        }
        
        [HttpGet("/trackers/{trackerId}/statistics")]
        [ProducesResponseType(200, Type = typeof(IFact))]
        public IActionResult GetStatisticsForSingleTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var statistics = _statisticsService.GetStatisticsFactsForTracker(userId, trackerId);
            return Ok(statistics);
        }
        
        [HttpGet("/trackers/statistics")]
        [ProducesResponseType(200, Type = typeof(List<IFact>))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var statistics = _statisticsService.GetStatisticsFactsForAllUserTrackers(userId);
            return Ok(statistics);
        }
    }
}