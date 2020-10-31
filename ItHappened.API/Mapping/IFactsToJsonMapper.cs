using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Api.Mapping
{
    public interface IFactsToJsonMapper
    {
        string SingleFactsToJson(IReadOnlyCollection<ISingleTrackerFact> facts);
        string MultipleFactsToJson(IReadOnlyCollection<IMultipleTrackersFact> facts);
    }
}