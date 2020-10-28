using System;

namespace ItHappened.Domain.Statistics
{
    public interface IBackgroundStatisticGenerator
    {
        void UpdateUserFacts(Guid userId);
    }
}