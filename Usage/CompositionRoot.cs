using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Application.Services.StatisticService;
using ItHappened.Application.Services.UserService;
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
            return new CompositionRoot
            {
                UserService = new UserService(userRepository),
                EventTrackerService = new EventTrackerService(eventTrackerRepository, eventRepository),
                StatisticsService = new StatisticsService(userRepository, eventTrackerRepository);
            };
        }
    }
}