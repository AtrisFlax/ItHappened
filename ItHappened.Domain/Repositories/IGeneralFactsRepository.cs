using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface IGeneralFactsRepository
    {
        IReadOnlyCollection<IGeneralFact> LoadUserGeneralFacts(Guid userId);
        void UpdateUserGeneralFacts(Guid userId);
    }
}