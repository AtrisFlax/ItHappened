using System;
using System.Collections.Generic;
using AutoMapper;
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
        private readonly IEventFilterable _eventFilterable;
        private readonly IMapper _mapper;


        public EventsController(IEventService eventService, IMapper mapper, IEventFilterable eventFilterable)
        {
            _eventService = eventService;
            _mapper = mapper;
            _eventFilterable = eventFilterable;
        }

        [HttpPost("/trackers/{trackerId}/events")]
        [ProducesResponseType(200)]
        public IActionResult AddEventToTracker([FromRoute] Guid trackerId,
            [FromBody] EventRequest request)
        {
            var userId = User.GetUserId();
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            var eventId = _eventService.CreateEvent(userId, trackerId, request.HappensDate, customParameters);
            return Ok(_mapper.Map<EventPostResponse>(eventId));
        }

        [HttpGet("/trackers/{trackerId}/events/filters")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EventGetResponse>))]
        public IActionResult GetFilteredEvents([FromRoute] Guid trackerId,
            [FromQuery] EventFilterDataRequest eventFilterDataRequest)
        {
            var userId = User.GetUserId();
            var eventFilter = _mapper.Map<EventFilterData>(eventFilterDataRequest);
            var filteredEvents = _eventFilterable.GetAllFilteredEvents(userId, trackerId, eventFilter);
            return Ok(_mapper.Map<EventGetResponse[]>(filteredEvents));
        }

        [HttpGet("/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventGetResponse))]
        public IActionResult GetEvent([FromRoute] Guid eventId)
        {
            var userId = User.GetUserId();
            var @event = _eventService.GetEvent(userId, eventId);
            return Ok(_mapper.Map<EventGetResponse>(@event));
        }

        [HttpGet("/trackers/{trackerId}/events")]
        [ProducesResponseType(200, Type = typeof(EventGetResponse[]))]
        public IActionResult GetAllEvents([FromRoute] Guid trackerId)
        {
            var userId = User.GetUserId();
            var events = _eventService.GetAllTrackerEvents(userId, trackerId);
            return Ok(_mapper.Map<EventGetResponse[]>(events));
        }

        [HttpPut("/events/{eventId}")]
        [ProducesResponseType(200)]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody] EventRequest request)
        {
            var userId = User.GetUserId();
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            _eventService.EditEvent(userId, eventId, request.HappensDate, customParameters);
            return Ok();
        }

        [HttpDelete("events/{eventId}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteEvent([FromRoute] Guid eventId)
        {
            var userId = User.GetUserId();
            _eventService.DeleteEvent(userId, eventId);
            return Ok();
        }
    }
}