using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class SpecificFactProvider : ISpecificFactProvider
    {
        private readonly List<ISpecificCalculator> _calculators =
            new List<ISpecificCalculator>();

        public void Add(ISpecificCalculator calculator)
        {
            _calculators.Add(calculator);
        }

        public IReadOnlyCollection<ISpecificFact> GetFacts(EventTracker eventTracker)
        {
            return _calculators
                .Select(calculator => calculator.Calculate(eventTracker))
                .Somes()
                .OrderByDescending(fact => fact.Priority)
                .OrderBy(fact => fact.Priority)
                .ToList();
        }
    }
}