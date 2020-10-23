using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Contracts.Requests;
using ItHappened.Api.Contracts.Responses;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.StatisticService;
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
        public TrackersController(IEventTrackerService eventTrackerService,
            IEventTrackerRepository trackerRepository,
            IStatisticsService statisticsService,
            IMapper mapper)
        {
            _eventTrackerService = eventTrackerService;
            _trackerRepository = trackerRepository;
            _statisticsService = statisticsService;
            _mapper = mapper;
        }
        
        [HttpPost("/trackers")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult CreateTracker([FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            //var customizations = _mapper.Map<TrackerCustomizationsSettings>(request.CustomizationSettings);
            var customizations = new TrackerCustomizationSettings(
                request.CustomizationSettings.ScaleMeasurementUnit,
                request.CustomizationSettings.PhotoIsOptional,
                request.CustomizationSettings.RatingIsOptional,
                request.CustomizationSettings.GeoTagIsOptional,
                request.CustomizationSettings.CommentIsOptional);
            
            var tracker = new EventTracker(Guid.NewGuid(), userId, request.Name, customizations);
            _trackerRepository.SaveTracker(tracker);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpGet("/trackers")]
        [ProducesResponseType(200, Type = typeof(List<TrackerResponse>))]
        public IActionResult GetAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackers = _eventTrackerService.GetEventTrackers(userId);
            return Ok(_mapper.Map<List<TrackerResponse>>(trackers));
        }
        
        [HttpGet("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult GetTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracker = _eventTrackerService.GetEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        
        [HttpPut("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult UpdateTracker([FromRoute]Guid trackerId, [FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            //TODO REFACTOR MAPPING
            var customizations = new TrackerCustomizationSettings(
                request.CustomizationSettings.ScaleMeasurementUnit,
                request.CustomizationSettings.PhotoIsOptional,
                request.CustomizationSettings.RatingIsOptional,
                request.CustomizationSettings.GeoTagIsOptional,
                request.CustomizationSettings.CommentIsOptional);
            
            var tracker = _eventTrackerService.EditEventTracker(userId, trackerId, request.Name, customizations);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpDelete("/trackers/{trackerId}")]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult DeleteTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _eventTrackerService.DeleteEventTracker(userId, trackerId);
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

        private readonly IStatisticsService _statisticsService;
        private readonly IEventTrackerService _eventTrackerService;
        private readonly IEventTrackerRepository _trackerRepository;
        private readonly IMapper _mapper;
    }
}