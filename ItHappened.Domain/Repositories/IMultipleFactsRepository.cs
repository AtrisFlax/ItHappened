using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface IMultipleFactsRepository
    {
        IReadOnlyCollection<IMultipleTrackersFact> ReadUserGeneralFacts(Guid userId);
        void CreateUserGeneralFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts);
        bool IsContainFactsForUser(Guid userId);
    }
}