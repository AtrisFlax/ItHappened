using System;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Domain;
using ItHappened.Infrastructure.Repositories;
using NUnit.Framework;

namespace ItHappened.UnitTests.ServicesTests
{
    public class EventTrackerServiceTests
    {
        private EventTracker _eventTracker;
        private IEventTrackerService _eventTrackerService;
        private IEventTrackerRepository _eventTrackerRepository;
        [SetUp]
        public void Init()
        {
            _eventTrackerRepository = new EventTrackerRepository();
            var eventRepository = new EventRepository();
            _eventTrackerService = new EventTrackerService(_eventTrackerRepository, eventRepository);
            _eventTracker = CreateEventTracker();
            
        }

        [Test]
        public void DeleteTrackerNotByCreator_WrongCreatorIdStatus()
        {
            var wrongCreatorId = Guid.NewGuid();
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongCreatorId;
            _eventTrackerRepository.SaveTracker(_eventTracker);
            
            var actual = _eventTrackerService.DeleteTracker(_eventTracker.Id, wrongCreatorId);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void DeleteNonExistingTracker_TrackerDontExistStatus()
        {
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
            
            var actual = _eventTrackerService.DeleteTracker(_eventTracker.Id, _eventTracker.CreatorId);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void DeleteExistingTracker_OkStatus()
        {
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            _eventTrackerRepository.SaveTracker(_eventTracker);
            
            var actual = _eventTrackerService.DeleteTracker(_eventTracker.Id, _eventTracker.CreatorId);
            
            Assert.AreEqual(expected, actual);
        }
        
        private EventTracker CreateEventTracker()
        {
            var tracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "tracker")
                .Build();

            return tracker;
        }
    }
}