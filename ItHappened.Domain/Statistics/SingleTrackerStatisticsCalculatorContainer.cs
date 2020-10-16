using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Statistics.Calculators.ForSingleTracker;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculatorContainer
    {
        void Add(ISingleTrackerStatisticsCalculator<ISingleTrackerStatisticsFact> calculator);
        IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetFacts(EventTracker eventTracker);
    }

    public class SingleTrackerStatisticsCalculatorContainer : ISingleTrackerStatisticsCalculatorContainer
    {
        public void Add(ISingleTrackerStatisticsCalculator<ISingleTrackerStatisticsFact> calculator)
        {
            _calculators.Add(calculator);
        }
        
        private List<ISingleTrackerStatisticsCalculator<ISingleTrackerStatisticsFact>> _calculators =
            new List<ISingleTrackerStatisticsCalculator<ISingleTrackerStatisticsFact>>();

        public IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetFacts(EventTracker eventTracker) =>
            _calculators.Select(calculator => calculator.Calculate(eventTracker)).ToList();
    }
}