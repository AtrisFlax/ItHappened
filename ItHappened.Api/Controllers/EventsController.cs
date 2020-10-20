using System;
using ItHappened.Api.Contracts;
using ItHappened.Api.Contracts.Requests;
using ItHappened.Api.Contracts.Requests.Events;
using ItHappened.Api.Contracts.Responses;
using ItHappened.Api.Contracts.Responses.Events;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.Api.Controllers
{
    public class EventsController : ControllerBase
    {
        [HttpPost(ApiRoutes.Events.Create)]
        [ProducesResponseType(200, Type = typeof(CreateEventResponse))]
        public IActionResult CreateEvent([FromRoute]Guid trackerId, [FromBody]CreateEventRequest request)
        {
            return Ok(new CreateEventResponse());
        }
        
        [HttpGet(ApiRoutes.Events.Get)]
        [ProducesResponseType(200, Type = typeof(GetEventResponse))]
        public IActionResult GetEvent([FromRoute]Guid eventId, [FromBody]GetEventRequest request)
        {
            return Ok(new GetEventResponse());
        }
        
        [HttpPut(ApiRoutes.Events.Update)]
        [ProducesResponseType(200, Type = typeof(UpdateEventResponse))]
        public IActionResult UpdateEvent([FromRoute]Guid eventId, [FromBody]UpdateEventRequest request)
        {
            return Ok(new CreateEventResponse());
        }
        
        [HttpDelete(ApiRoutes.Events.Delete)]
        [ProducesResponseType(200, Type = typeof(CreateEventResponse))]
        public IActionResult DeleteEvent([FromRoute]Guid eventId, [FromBody]DeleteEventRequest request)
        {
            return Ok(new CreateEventResponse());
        }
    }
}