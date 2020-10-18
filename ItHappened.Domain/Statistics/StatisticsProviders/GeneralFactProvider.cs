using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class GeneralFactProvider : IGeneralFactProvider
    {
        private readonly List<IGeneralCalculator> _calculators = new List<IGeneralCalculator>();

        public void Add(IGeneralCalculator calculator)
        {
            _calculators.Add(calculator);
        }

        public IReadOnlyCollection<IGeneralFact> GetFacts(IEnumerable<EventTracker> eventTrackers)
        {
            return _calculators
                .Select(calculator => calculator.Calculate(eventTrackers))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .ToList();
        }
    }
}