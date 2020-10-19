using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class MultipleTrackersStatisticsProvider : IMultipleTrackersStatisticsProvider
    {
        private readonly List<IMultipleTrackersStatisticsCalculator> _calculators =
            new List<IMultipleTrackersStatisticsCalculator>();

        public void Add(IMultipleTrackersStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }

        public IReadOnlyCollection<IStatisticsFact> GetFacts(
            IEnumerable<EventTracker> eventTrackers)
        {
            return _calculators
                .Select(calculator => calculator.Calculate(eventTrackers))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .ToList();
        }
    }
}