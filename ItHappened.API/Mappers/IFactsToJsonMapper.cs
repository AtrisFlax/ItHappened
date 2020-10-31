using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Api.Mappers
{
    public interface IFactsToJsonMapper
    {
        string SingleFactsToJson(IReadOnlyCollection<ISingleTrackerFact> facts);
        string MultipleFactsToJson(IReadOnlyCollection<IMultipleTrackersFact> facts);
    }
}