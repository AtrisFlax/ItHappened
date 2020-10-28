using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.MappingProfiles;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.EventService;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IMyMapper _myMapper;


        public EventsController(IEventService eventService, IMapper mapper, IMyMapper myMapper)
        {
            _eventService = eventService;
            _mapper = mapper;
            _myMapper = myMapper;
        }

        [HttpPost("/trackers/{trackerId}/events")]
        [ProducesResponseType(200)]
        public IActionResult AddEventToTracker([FromRoute] Guid trackerId,
            [FromBody]EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customParameters = _myMapper.GetEventCustomParametersFromRequest(request);
            var eventId = _eventService.CreateEvent(userId, trackerId, request.HappensDate, customParameters);
            return Ok(eventId);
        }

        [HttpGet("/trackers/{trackerId}/events/filters")]
        public IActionResult GetFilteredEvents([FromRoute] Guid trackerId,
            [FromQuery] EventFilterRequest eventFilterRequest)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var filters = CreateFilters(eventFilterRequest);
            var filteredEvents = _eventService.GetAllFilteredEvents(userId, trackerId, filters);
            return Ok(_mapper.Map<EventResponse[]>(filteredEvents));
        }

        [HttpGet("/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult GetEvent([FromRoute] Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var @event = _eventService.GetEvent(userId, eventId);
            return Ok(_mapper.Map<EventResponse>(@event));
        }

        [HttpGet("/trackers/{trackerId}/events")]
        [ProducesResponseType(200, Type = typeof(EventResponse[]))]
        public IActionResult GetAllEvents([FromRoute] Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var events = _eventService.GetAllTrackerEvents(userId, trackerId);
            return Ok(_myMapper.EventsToJson(events));
        }

        [HttpPut("/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody] EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customParameters = _myMapper.GetEventCustomParametersFromRequest(request);
            _eventService.EditEvent(userId, eventId, request.HappensDate, customParameters);
            return Ok();
        }

        [HttpDelete("events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult DeleteEvent([FromRoute] Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            _eventService.DeleteEvent(userId, eventId);
            return Ok();
        }

        private static IEnumerable<IEventsFilter> CreateFilters(EventFilterRequest eventFilterRequest)
        {
            var filters = new List<IEventsFilter>();
            if (eventFilterRequest.ToDateTime.HasValue && eventFilterRequest.FromDateTime.HasValue)
                filters.Add(
                    new DateTimeFilter(null, eventFilterRequest.FromDateTime.Value,
                        eventFilterRequest.ToDateTime.Value));
            if (!string.IsNullOrEmpty(eventFilterRequest.CommentRegexPattern))
                filters.Add(new CommentFilter(null, eventFilterRequest.CommentRegexPattern));
            if (eventFilterRequest.ScaleLowerLimit.HasValue && eventFilterRequest.ScaleUpperLimit.HasValue)
                filters.Add(new ScaleFilter(null, eventFilterRequest.ScaleLowerLimit.Value,
                    eventFilterRequest.ScaleUpperLimit.Value));
            if (eventFilterRequest.LowerLimitRating.HasValue && eventFilterRequest.UpperLimitRating.HasValue)
                filters.Add(new RatingFilter(null, eventFilterRequest.LowerLimitRating.Value,
                    eventFilterRequest.UpperLimitRating.Value));
            if (eventFilterRequest.ScaleLowerLimit.HasValue && eventFilterRequest.ScaleUpperLimit.HasValue)
                filters.Add(new ScaleFilter(null, eventFilterRequest.ScaleLowerLimit.Value,
                    eventFilterRequest.ScaleUpperLimit.Value));
            return filters;
        }
    }
}