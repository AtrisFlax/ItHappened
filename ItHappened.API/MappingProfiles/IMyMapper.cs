using System.Collections.Generic;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;

namespace ItHappened.Api.MappingProfiles
{
    public interface IMyMapper
    {
        EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request);
        string EventToJson(Event request);
        string EventsToJson(IReadOnlyCollection<Event> events);
    }
}