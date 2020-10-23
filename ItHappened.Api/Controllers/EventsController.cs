using System;
using System.Security.Claims;
using AutoMapper;
using ItHappened.Api.Authentication;
using ItHappened.Api.Models.Requests;
using ItHappened.Api.Models.Responses;
using ItHappened.Application.Services.EventService;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
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
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult AddEventToTracker([FromBody]EventRequest request, [FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            //var customParameters = _mapper.Map<EventCustomParameters>(request.CustomParameters);
            var customParameters = GetEventCustomParametersFromRequest(request);
            var newEvent = _eventService.AddEvent(userId, trackerId, request.HappensDate, customParameters);
            var map = _mapper.Map<EventResponse>(newEvent);
            return Ok(_mapper.Map<EventResponse>(newEvent));
        }
        
        [HttpGet("/trackers/{trackerId}/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult GetEvent([FromRoute]Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var @event = _eventService.GetEvent(userId, eventId);
            return Ok(_mapper.Map<EventResponse>(@event));
        }
        
        [HttpGet("/trackers/{trackerId}/events")]
        [ProducesResponseType(200, Type = typeof(EventResponse[]))]
        public IActionResult GetAllEvents([FromRoute]Guid trackerId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var events = _eventService.GetAllEvents(userId, trackerId);
            return Ok(_mapper.Map<EventResponse[]>(events));
        }

        [HttpPut("/trackers/{trackerId}/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromBody]EventRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var customParameters = _mapper.Map<EventCustomParameters>(request);
            var editedEvent = _eventService.EditEvent(userId, eventId, request.HappensDate, customParameters);
            return Ok(_mapper.Map<EventResponse>(editedEvent));
        }
        
        [HttpDelete("/trackers/{trackerId}/events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventResponse))]
        public IActionResult DeleteEvent([FromRoute]Guid eventId)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var deletedTracker = _eventService.DeleteEvent(userId, eventId);
            return Ok(_mapper.Map<EventResponse>(deletedTracker));
        }

        private EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request)
        {
            var customParameters = new EventCustomParameters(
                Option<Photo>.Some(new Photo(request.CustomParameters.Photo.PhotoBytes)),
                Option<double>.Some(request.CustomParameters.Scale),
                Option<double>.Some(request.CustomParameters.Rating),
                Option<GeoTag>.Some(new GeoTag(request.CustomParameters.GeoTag.GpsLat,
                    request.CustomParameters.GeoTag.GpsLng)),
                Option<Comment>.Some(new Comment(request.CustomParameters.Comment))
            );
            return customParameters;
        }
    }
}