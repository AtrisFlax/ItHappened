using System;

namespace ItHappened.Domain.Statistics
{
    public interface IManualStatisticGenerator
    {
        public void UpdateUserGeneralFacts(Guid userId);
        public void UpdateTrackerSpecificFacts(Guid trackerId);
    }
}