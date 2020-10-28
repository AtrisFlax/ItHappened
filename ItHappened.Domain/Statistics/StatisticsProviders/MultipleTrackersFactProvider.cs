using System;
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

        public IReadOnlyCollection<IMultipleTrackersFact> GetFacts(IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents)
        {
            var now = DateTimeOffset.UtcNow;
            return _calculators
                .Select(calculator => calculator.Calculate(trackerWithItsEvents, now))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .ToList().AsReadOnly();
        }
    }
}