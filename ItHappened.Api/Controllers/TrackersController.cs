using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Contracts;
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
        
        [HttpPost(ApiRoutes.Trackers.Create)]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult CreateTracker([FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customizations = _mapper.Map<TrackerCustomizationsSettings>(request.CustomizationSettings);
            var tracker = new EventTracker(Guid.NewGuid(), userId, request.Name, customizations);
            _trackerRepository.SaveTracker(tracker);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpGet(ApiRoutes.Trackers.GetAll)]
        [ProducesResponseType(200, Type = typeof(List<TrackerResponse>))]
        public IActionResult GetAllTrackers()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackers = _eventTrackerService.GetEventTrackers(userId);
            return Ok(_mapper.Map<List<TrackerResponse>>(trackers));
        }
        
        [HttpGet(ApiRoutes.Trackers.Get)]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult GetTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracker = _eventTrackerService.GetEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }
        
        [HttpPut(ApiRoutes.Trackers.Update)]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult UpdateTracker([FromRoute]Guid trackerId, [FromBody]TrackerRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracker = _eventTrackerService.EditEventTracker(userId, trackerId, request.Name);
            return Ok(_mapper.Map<TrackerResponse>(tracker));
        }

        [HttpDelete(ApiRoutes.Trackers.Delete)]
        [ProducesResponseType(200, Type = typeof(TrackerResponse))]
        public IActionResult DeleteTracker([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _eventTrackerService.DeleteEventTracker(userId, trackerId);
            return Ok(_mapper.Map<TrackerResponse>(deletedTracker));
        }
        
        [HttpGet(ApiRoutes.Trackers.Statistics.GetForSingleTracker)]
        [ProducesResponseType(200, Type = typeof(IFact))]
        public IActionResult GetStatisticsForSingleTracker([FromRoute]Guid trackerId)
        {
            return Ok();
        }
        
        [HttpGet(ApiRoutes.Trackers.Statistics.GetForMultipleTrackers)]
        [ProducesResponseType(200, Type = typeof(List<IFact>))]
        public IActionResult GetStatisticsForAllTrackers()
        {
            return Ok();
        }

        private readonly IStatisticsService _statisticsService;
        private readonly IEventTrackerService _eventTrackerService;
        private readonly IEventTrackerRepository _trackerRepository;
        private readonly IMapper _mapper;
    }
}