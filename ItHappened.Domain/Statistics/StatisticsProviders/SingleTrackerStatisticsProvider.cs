using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerStatisticsProvider : ISingleTrackerStatisticsProvider
    {
        private readonly List<ISingleTrackerStatisticsCalculator> _calculators =
            new List<ISingleTrackerStatisticsCalculator>();

        public void Add(ISingleTrackerStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }

        public IReadOnlyCollection<IStatisticsFact> GetFacts(EventTracker eventTracker)
        {
            return _calculators
                .Select(calculator => calculator.Calculate(eventTracker))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .ToList();
        }
    }
}