using System;
using System.Linq;
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
        public void DeleteTrackerNotByCreator_WrongTrackerCreatorIdStatus()
        {
            var wrongCreatorId = Guid.NewGuid();
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongTrackerCreatorId;
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

        [Test]
        public void CreateTracker_CreatesTrackerWithRequiredParameters()
        {
            var creatorId = Guid.NewGuid();
            
            var trackerId = _eventTrackerService.CreateTracker(creatorId, 
                "Created Tracker", 
                false,
                true,
                "meter",
                false,
                true,
                false);
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            var actualMeasurementUnit = tracker.ScaleMeasurementUnit.Match(
                Some: value => value,
                None: string.Empty);
            
            Assert.AreEqual(creatorId, tracker.CreatorId);
            Assert.False(tracker.HasPhoto);
            Assert.True(tracker.HasScale);
            Assert.AreEqual("meter", actualMeasurementUnit);
            Assert.False(tracker.HasRating);
            Assert.True(tracker.HasGeoTag);
            Assert.False(tracker.HasComment);
        }

        [Test]
        public void GetAllUserTrackersWhenUserHasNoTrackers_ReturnsEmptyCollection()
        {
            const int expected = 0;

            var actual = _eventTrackerRepository.LoadAllUserTrackers(Guid.NewGuid()).Count();
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetAllUserTrackers_ReturnsUserTrackers()
        {
            var userId = Guid.NewGuid();
            var tracker1 = CreateEventTracker(userId);
            var tracker2 = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker1);
            _eventTrackerRepository.SaveTracker(tracker2);
            const int expected = 2;
            
            var actual = _eventTrackerRepository.LoadAllUserTrackers(userId).Count();
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddEventToNonExistentTracker_TrackerDontExistStatus()
        {
            var eventToAdd = CreateEvent(Guid.NewGuid());
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;

            var actual = _eventTrackerService
                .AddEventToTracker(Guid.NewGuid(), Guid.NewGuid(), eventToAdd);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void AddEventToTrackerNotByTrackerCreator_WrongTrackerCreatorIdStatus()
        {
            var wrongCreatorId = Guid.NewGuid();
            var eventToAdd = CreateEvent(Guid.NewGuid());
            _eventTrackerRepository.SaveTracker(_eventTracker);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongTrackerCreatorId;

            var actual = 
                _eventTrackerService.AddEventToTracker(wrongCreatorId, _eventTracker.Id, eventToAdd);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void AddEventToTrackerWithDifferentEventCreator_WrongEventCreatorIdStatus()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var eventToAdd = CreateEvent(Guid.NewGuid());
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongEventCreatorId;

            var actual = _eventTrackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void AddEventToTrackerGoodCase_OkStatus()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var eventToAdd = CreateEvent(userId, tracker.Id);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            
            var actual = _eventTrackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
            
            Assert.AreEqual(expected, actual);
        }
        
        private EventTracker CreateEventTracker()
        {
            var tracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "tracker")
                .Build();

            return tracker;
        }
        
        private EventTracker CreateEventTracker(Guid creatorId)
        {
            var tracker = EventTrackerBuilder
                .Tracker(creatorId, Guid.NewGuid(), "tracker: " + $"{creatorId}")
                .Build();

            return tracker;
        }
        
        private Event CreateEvent(Guid creatorId)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "event: " + $"{creatorId}")
                .Build();
        }
        
        private Event CreateEvent(Guid creatorId, Guid trackerId)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), creatorId, trackerId, DateTimeOffset.Now, "event: " + $"{creatorId}")
                .Build();
        }
        
    }
}