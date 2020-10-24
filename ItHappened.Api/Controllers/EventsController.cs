using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.EventService;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
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
        public IActionResult AddEventToTracker([FromRoute] Guid trackerId, [FromBody]EventsRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var eventsInfoRange = request.Events
                .Select(@event => new EventsInfoRange
                {
                    HappensDate = @event.HappensDate,
                    CustomParameters = GetEventCustomParametersFromRequest(@event)
                });
            _eventService.AddRangeEvent(userId, trackerId, eventsInfoRange);
            return Ok();
        }

        [HttpGet("/trackers/{trackerid}/events/filters")]
        public IActionResult GetFilteredEvents([FromRoute] Guid trackerId,[FromQuery] EventFilterRequest eventFilterRequest)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));

            var filters = new List<IEventsFilter>();
            if (eventFilterRequest.ToDateTime.HasValue && eventFilterRequest.FromDateTime.HasValue)
                filters.Add(new DateTimeFilter(null, eventFilterRequest.FromDateTime.Value, eventFilterRequest.ToDateTime.Value));
            if (!string.IsNullOrEmpty(eventFilterRequest.CommentRegexPattern))
                filters.Add(new CommentFilter(null, eventFilterRequest.CommentRegexPattern));
            if (eventFilterRequest.ScaleLowerLimit.HasValue && eventFilterRequest.ScaleUpperLimit.HasValue)
                filters.Add(new ScaleFilter(null, eventFilterRequest.ScaleLowerLimit.Value, eventFilterRequest.ScaleUpperLimit.Value));
            if (eventFilterRequest.LowerLimitRating.HasValue && eventFilterRequest.UpperLimitRating.HasValue)
                filters.Add(new RatingFilter(null, eventFilterRequest.LowerLimitRating.Value, eventFilterRequest.UpperLimitRating.Value));
            if (eventFilterRequest.ScaleLowerLimit.HasValue && eventFilterRequest.ScaleUpperLimit.HasValue)
                filters.Add(new ScaleFilter(null, eventFilterRequest.ScaleLowerLimit.Value, eventFilterRequest.ScaleUpperLimit.Value));

            _eventService.GetAllFilteredEvents(userId, trackerId, filters);
            return Ok();
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
            var events = _eventService.GetAllEvents(userId, trackerId);
            return Ok(_mapper.Map<EventResponse[]>(events));
        }

        [HttpPut("/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody] EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customParameters = GetEventCustomParametersFromRequest(request);
            var editedEvent = _eventService.EditEvent(userId, eventId, request.HappensDate, customParameters);
            return Ok(_mapper.Map<EventResponse>(editedEvent));
        }

        [HttpDelete("events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult DeleteEvent([FromRoute] Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _eventService.DeleteEvent(userId, eventId);
            return Ok(_mapper.Map<EventResponse>(deletedTracker));
        }

        private EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request)
        {
            var customParameters = new EventCustomParameters(
                null,
                Option<double>.Some(request.Scale),
                Option<double>.Some(request.Rating),
                Option<GeoTag>.Some(new GeoTag(request.GeoTag.GpsLat,
                    request.GeoTag.GpsLng)),
                Option<Comment>.Some(new Comment(request.Comment))
            );
            return customParameters;
        }
    }
}