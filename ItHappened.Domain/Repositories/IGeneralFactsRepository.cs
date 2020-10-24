using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface IGeneralFactsRepository
    {
        IReadOnlyCollection<IGeneralFact> LoadUserGeneralFacts(Guid userId);
        void UpdateUserGeneralFacts(IReadOnlyCollection<IGeneralFact> facts,Guid userId);
    }
}