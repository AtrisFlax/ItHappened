using ItHappend.Domain.Statistics;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : StatisticsServiceCreator
    {
        public StatisticsService(IEventTrackerRepository eventTrackerRepository) : base(eventTrackerRepository)
        {
        }

        protected override SingleTrackerStatisticsProvider SingleTrackersStatisticsProvider()
        {
            var singleTrackersStatisticsProvider = new SingleTrackerStatisticsProvider();
            singleTrackersStatisticsProvider.Add(new BestEventCalculator());
            singleTrackersStatisticsProvider.Add(new AverageRatingCalculator());
            singleTrackersStatisticsProvider.Add(new LongestBreakCalculator());
            singleTrackersStatisticsProvider.Add(new OccursOnCertainDaysOfTheWeekCalculator());
            singleTrackersStatisticsProvider.Add(new SingleTrackerEventsCountCalculator());
            singleTrackersStatisticsProvider.Add(new SpecificDayTimeEventCalculator());
            singleTrackersStatisticsProvider.Add(new WorstEventCalculator());
            return singleTrackersStatisticsProvider;
        }

        protected override MultipleTrackersStatisticsProvider MultipleTrackersStatisticsProvider()
        {
            var multipleTrackersStatisticsProvider = new MultipleTrackersStatisticsProvider();
            multipleTrackersStatisticsProvider.Add(new EventsCountCalculator());
            multipleTrackersStatisticsProvider.Add(new MostFrequentEventCalculator());
            return multipleTrackersStatisticsProvider;
        }
    }
}