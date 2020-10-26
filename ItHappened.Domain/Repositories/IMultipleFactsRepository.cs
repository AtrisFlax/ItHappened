using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface IMultipleFactsRepository
    {
        IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId);
        void UpdateUserGeneralFacts(IReadOnlyCollection<IMultipleTrackersFact> facts, Guid userId);
    }
}