using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerFactProvider : ISingleTrackerFactProvider
    {
        private readonly List<ISingleTrackerStatisticsCalculator> _calculators =
            new List<ISingleTrackerStatisticsCalculator>();

        public void Add(ISingleTrackerStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }

        public IReadOnlyCollection<ISingleTrackerTrackerFact> GetFacts(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            return _calculators
                .Select(calculator => calculator.Calculate(events, tracker))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .ToList();
        }
    }
}