using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class MultipleTrackersFactProvider : IMultipleTrackersFactProvider
    {
        private readonly List<IMultipleTrackersStatisticsCalculator> _calculators = new List<IMultipleTrackersStatisticsCalculator>();

        public void Add(IMultipleTrackersStatisticsCalculator statisticsCalculator)
        {
            _calculators.Add(statisticsCalculator);
        }

        public IReadOnlyCollection<IMultipleTrackersFact> GetFacts(IEnumerable<EventTracker> eventTrackers)
        {
            return _calculators
                .Select(calculator => calculator.Calculate(eventTrackers))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .ToList();
        }
    }
}