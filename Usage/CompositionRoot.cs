using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.UserService;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure;
using ItHappened.Infrastructure.Repositories;

namespace Usage
{
    public class CompositionRoot
    {
        public IEventTrackerService EventTrackerService { get; private set; }
        public IUserService UserService { get; private set; }

        public IStatisticsService StatisticsService { get; private set; }

        public static CompositionRoot Create()
        {
            var userRepository = new UserRepository();
            var eventRepository = new EventRepository();
            var eventTrackerRepository = new EventTrackerRepository();
            var generalFactProvider = new GeneralFactProvider();
            generalFactProvider.Add(new MultipleTrackersEventsCountCalculator(eventRepository));
            generalFactProvider.Add(new MostFrequentEventCalculator(eventRepository));
            var specificFactProvider = new SpecificFactProvider();
            specificFactProvider.Add( new BestEventCalculator(eventRepository));
            specificFactProvider.Add( new AverageRatingCalculator(eventRepository));
            specificFactProvider.Add( new LongestBreakCalculator(eventRepository));
            specificFactProvider.Add( new OccursOnCertainDaysOfTheWeekCalculator(eventRepository));
            specificFactProvider.Add( new SingleTrackerEventsCountCalculator(eventRepository));
            specificFactProvider.Add( new SpecificDayTimeEventCalculator(eventRepository));
            specificFactProvider.Add( new WorstEventCalculator(eventRepository));
            return new CompositionRoot
            {
                UserService = new UserService(userRepository, new PasswordHasher()),
                EventTrackerService = new EventTrackerService(eventTrackerRepository, eventRepository),
                StatisticsService = new StatisticsService(eventTrackerRepository, generalFactProvider, specificFactProvider)
            };
        }
    }
}