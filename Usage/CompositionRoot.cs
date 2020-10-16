using ItHappend.Application;
using ItHappend.EventService;
using ItHappend.EventTrackerService;
using ItHappend.Infrastructure;
using ItHappend.UserService;

namespace Usage
{
    public class CompositionRoot
    {
        public IEventService EventService { get; private set; }
        public IEventTrackerService EventTrackerService { get; private set; }
        public IUserService UserService { get; private set; }

        public static CompositionRoot Create()
        {
            var userRepository = new UserRepository();
            var eventRepository = new EventRepository();
            var eventTrackerRepository = new EventTrackerRepository();
            var passwordHasher = new PasswordHasher();
            return new CompositionRoot()
            {
                EventService = new EventService(eventRepository),
                UserService = new UserService(userRepository, passwordHasher, eventTrackerRepository),
                EventTrackerService = new EventTrackerService(eventTrackerRepository, userRepository, eventRepository)
            };
        }
    }
}