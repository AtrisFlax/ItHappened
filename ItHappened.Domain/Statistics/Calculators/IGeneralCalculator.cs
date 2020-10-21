using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IGeneralCalculator
    {
        Option<IGeneralFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}