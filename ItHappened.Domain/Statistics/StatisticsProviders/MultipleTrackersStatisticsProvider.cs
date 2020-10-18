using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class MultipleTrackersStatisticsProvider : IMultipleTrackersStatisticsProvider
    {
        public void Add(IMultipleTrackersStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }
        
        private readonly List<IMultipleTrackersStatisticsCalculator> _calculators =
            new List<IMultipleTrackersStatisticsCalculator>();

        public IReadOnlyCollection<IMultipleTrackersStatisticsFact> GetFacts(
            IEnumerable<EventTracker> eventTrackers) =>
            _calculators
                .Select(calculator => calculator.Calculate(eventTrackers))
                .Somes()
                .OrderBy(fact => fact.Priority)
                .ToList();
    }
}