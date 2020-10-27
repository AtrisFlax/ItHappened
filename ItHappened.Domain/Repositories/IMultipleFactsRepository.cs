using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface IMultipleFactsRepository
    {
        IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId);
        void UpdateUserGeneralFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts);
        bool IsContainFactsForUser(Guid userId);
    }
}