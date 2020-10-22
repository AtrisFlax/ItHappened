using System;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Contracts;
using ItHappened.Api.Contracts.Requests;
using ItHappened.Api.Contracts.Responses;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    public class EventsController : ControllerBase
    {
        public EventsController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Events.Create)]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult AddEventToTracker([FromRoute]Guid trackerId, [FromBody]EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            var newEvent = _eventService.AddEvent(userId, trackerId, request.HappensDate, customParameters);
            return Ok(_mapper.Map<EventResponse>(newEvent));
        }
        
        [HttpGet(ApiRoutes.Events.Get)]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult GetEvent([FromRoute]Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var @event = _eventService.GetEvent(userId, eventId);
            return Ok(_mapper.Map<EventResponse>(@event));
        }
        
        [HttpGet(ApiRoutes.Events.GetAll)]
        [ProducesResponseType(200, Type = typeof(EventResponse[]))]

        public IActionResult GetAllEvents([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var events = _eventService.GetAllEvents(userId, trackerId);
            return Ok(_mapper.Map<EventResponse[]>(events));
        }

        [HttpPut(ApiRoutes.Events.Update)]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody]EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            var editedEvent = _eventService.EditEvent(userId, eventId, request.HappensDate, customParameters);
            return Ok(_mapper.Map<EventResponse>(editedEvent));
        }
        
        [HttpDelete(ApiRoutes.Events.Delete)]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult DeleteEvent([FromRoute]Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _eventService.DeleteEvent(userId, eventId);
            return Ok(_mapper.Map<EventResponse>(deletedTracker));
        }

        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
    }
}