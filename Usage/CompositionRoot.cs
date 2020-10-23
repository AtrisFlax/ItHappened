using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure;
using ItHappened.Infrastructure.Repositories;

namespace Usage
{
    public class CompositionRoot
    {
        public ITrackerService TrackerService { get; private set; }
        public IUserService UserService { get; private set; }

        public IStatisticsService StatisticsService { get; private set; }

        public static CompositionRoot Create()
        {
            var userRepository = new UserRepository();
            var eventRepository = new EventRepository();
            var eventTrackerRepository = new TrackerRepository();
            var generalFactProvider = new MultipleTrackersFactProvider();
            generalFactProvider.Add(new MultipleTrackersStatisticsEventsCountCalculator(eventRepository));
            generalFactProvider.Add(new MostFrequentEventStatisticsCalculator(eventRepository));
            var specificFactProvider = new SingleTrackerFactProvider();
            specificFactProvider.Add( new BestEventCalculator(eventRepository));
            specificFactProvider.Add( new AverageRatingCalculator(eventRepository));
            specificFactProvider.Add( new LongestBreakCalculator(eventRepository));
            specificFactProvider.Add( new OccursOnCertainDaysOfTheWeekCalculator(eventRepository));
            specificFactProvider.Add( new SingleTrackerEventsCountCalculator(eventRepository));
            //specificFactProvider.Add( new SingleTrackerStatisticsDayTimeEventCalculator(eventRepository));
            specificFactProvider.Add( new WorstEventCalculator(eventRepository));
            return new CompositionRoot
            {
                UserService = new UserService(userRepository, new PasswordHasher()),
                TrackerService = new TrackerService(eventTrackerRepository, eventRepository),
                StatisticsService = new StatisticsService(eventTrackerRepository, generalFactProvider, specificFactProvider)
            };
        }
    }
}