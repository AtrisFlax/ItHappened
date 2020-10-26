using System.Collections.Generic;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Api.MappingProfiles
{
    public interface IMyMapper
    {
        EventCustomParameters GetEventCustomParametersFromRequest(EventRequest request);
        string EventToJson(Event request);
        string EventsToJson(IReadOnlyCollection<Event> events);
        string SingleFactsToJson(IReadOnlyCollection<ISingleTrackerFact> facts);
        string MultipleFactsToJson(IReadOnlyCollection<IMultipleTrackersFact> facts);
    }
}