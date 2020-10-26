using System;

namespace ItHappened.Domain.Statistics
{
    public interface IManualStatisticGenerator
    {
        public void UpdateOnRequestUserGeneralFacts(Guid userId);
        public void UpdateOnRequestTrackerSpecificFacts(Guid trackerId);
    }
}    