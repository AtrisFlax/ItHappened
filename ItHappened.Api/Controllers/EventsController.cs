using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Mapping;
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


        public EventsController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpPost("/trackers/{trackerId}/events")]
        [ProducesResponseType(200)]
        public IActionResult AddEventToTracker([FromRoute] Guid trackerId,
            [FromBody] EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            var eventId = _eventService.CreateEvent(userId, trackerId, request.HappensDate, customParameters);
            return Ok(_mapper.Map<EventPostResponse>(eventId));
        }

        [HttpGet("/trackers/{trackerId}/events/filters")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EventGetResponse>))]
        public IActionResult GetFilteredEvents([FromRoute] Guid trackerId,
            [FromQuery] EventFilterRequest eventFilterRequest)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var filters = CreateFilters(eventFilterRequest);
            var filteredEvents = _eventService.GetAllFilteredEvents(userId, trackerId, filters);
            return Ok(_mapper.Map<EventGetResponse[]>(filteredEvents));
        }

        [HttpGet("/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventGetResponse))]
        public IActionResult GetEvent([FromRoute] Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var @event = _eventService.GetEvent(userId, eventId);
            return Ok(_mapper.Map<EventGetResponse>(@event));
        }

        [HttpGet("/trackers/{trackerId}/events")]
        [ProducesResponseType(200, Type = typeof(EventGetResponse[]))]
        public IActionResult GetAllEvents([FromRoute] Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var events = _eventService.GetAllTrackerEvents(userId, trackerId);
            return Ok(_mapper.Map<EventGetResponse[]>(events));
        }

        [HttpPut("/events/{eventId}")]
        [ProducesResponseType(200)]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody] EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            _eventService.EditEvent(userId, eventId, request.HappensDate, customParameters);
            return Ok();
        }

        [HttpDelete("events/{eventId}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteEvent([FromRoute] Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)); //TODO issue  #169
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