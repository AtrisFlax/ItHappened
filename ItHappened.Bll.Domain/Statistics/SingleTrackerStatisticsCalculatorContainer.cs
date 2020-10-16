using System.Collections.Generic;
using System.Linq;
using ItHappened.Bll.Domain.Statistics.SingleTrackerCalculator;
using ItHappened.Bll.Domain.Statistics.SingleTrackerStatisticsFacts;
using LanguageExt;

namespace ItHappened.Bll.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculatorContainer
    {
        void Add(ISingleTrackerStatisticsCalculator calculator);
        IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetFacts(EventTracker eventTracker);
    }

    public class SingleTrackerStatisticsCalculatorContainer : ISingleTrackerStatisticsCalculatorContainer
    {
        public void Add(ISingleTrackerStatisticsCalculator calculator)
        {
            _calculators.Add(calculator);
        }
        
        private List<ISingleTrackerStatisticsCalculator> _calculators =
            new List<ISingleTrackerStatisticsCalculator>();

        public IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetFacts(EventTracker eventTracker) =>
            _calculators.Select(calculator => calculator.Calculate(eventTracker)).ToList();
    }
}