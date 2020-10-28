using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface IMultipleTrackerRepository
    {
        IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId);
        void UpdateUserGeneralFacts(IReadOnlyCollection<IMultipleTrackersFact> facts,Guid userId);
    }
}