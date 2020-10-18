using System.Collections.Generic;
using System.Linq;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerStatisticsProvider : ISingleTrackerStatisticsProvider
    {
        public void Add(ISingleTrackerStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }
        
        private List<ISingleTrackerStatisticsCalculator> _calculators =
            new List<ISingleTrackerStatisticsCalculator>();

        public IReadOnlyCollection<ISingleTrackerStatisticsFact> GetFacts(EventTracker eventTracker) =>
            _calculators
                .Select(calculator => calculator.Calculate(eventTracker))
                .Where(fact => !fact.IsNone)
                .Select(fact => fact.ValueUnsafe())
                .OrderBy(fact => fact.Priority)
                .ToList();
    }
}