using System;
using System.Linq;
using ItHappened.Application.Errors;
using ItHappened.Application.Services.EventService;
using ItHappened.Domain;
using ItHappened.Infrastructure.InMemoryRepositories;
using ItHappened.UnitTests.StatisticsCalculatorsTests;
using LanguageExt;
using NUnit.Framework;

namespace ItHappened.UnitTests.ServicesTests
{
    public class EventServiceTests
    {
        private EventTracker _tracker;
        private Event _event;
        private ITrackerRepository _trackerRepository;
        private IEventRepository _eventRepository;
        private IEventService _eventService;
        
        [SetUp]
        public void Init()
        {
            _trackerRepository = new TrackerRepository();
            _eventRepository = new EventRepository();
            _eventService = new EventService(_eventRepository, _trackerRepository);
            _tracker = TestingMethods.CreateTrackerWithDefaultCustomization(Guid.NewGuid());
            _event = TestingMethods.CreateEvent(_tracker.Id, _tracker.CreatorId);
        }

        [Test]
        public void CreateEventWhenNoRequiredTrackerInRepository_ThrowsException()
        {
            Assert.Throws<TrackerNotFoundException>(() =>
                _eventService.CreateEvent(Guid.NewGuid(), _tracker.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        
        [Test]
        public void CreateEventWhenUserCreateEventForNotHisOwnTracker_ThrowsException()
        {
            _trackerRepository.SaveTracker(_tracker);
            
            Assert.Throws<NoPermissionsForTrackerException>(() =>
                _eventService.CreateEvent(Guid.NewGuid(), _tracker.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void CreateEventWhenEventAndTrackerCustomizationDiffersAndCustomizationIsRequired_ThrowsException()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    true, 
                    true,
                    Option<string>.Some("meter"), 
                    true, 
                    false, 
                    true, 
                    true));
            _trackerRepository.SaveTracker(tracker);
            
            Assert.Throws<InvalidEventForTrackerException>(() =>
                _eventService.CreateEvent(tracker.CreatorId, tracker.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void CreateEventWhenEventAndTrackerCustomizationEqualsAndCustomizationIsRequired_CreatesEvent()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    true, 
                    true,
                    Option<string>.Some("meter"), 
                    false, 
                    true, 
                    false, 
                    true));
            _trackerRepository.SaveTracker(tracker);
            var newEventCustomization = new EventCustomParameters(
                Option<Photo>.Some(new Photo(photoBytes: new byte[5])),
                Option<double>.Some(1),
                Option<double>.None,
                Option<GeoTag>.Some(new GeoTag(10, 20)),
                Option<Comment>.None);
            
            var createdEventId = _eventService.CreateEvent(
                tracker.CreatorId, 
                tracker.Id, 
                DateTimeOffset.Now,
                newEventCustomization);
            
            Assert.True(_eventRepository.IsContainEvent(createdEventId));
            Assert.AreEqual(newEventCustomization.GetHashCode(), 
                _eventRepository.LoadEvent(createdEventId).CustomizationsParameters.GetHashCode());
            Assert.True(tracker.IsUpdated);
        }
        
        [Test]
        public void CreateEventWhenEventAndTrackerCustomizationDiffersAndCustomizationIsNotRequired_CreatesEvent()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    true, 
                    true,
                    Option<string>.Some("meter"), 
                    true, 
                    false, 
                    true, 
                    false));
            _trackerRepository.SaveTracker(tracker);
            
            var createdEventId = _eventService.CreateEvent(tracker.CreatorId, tracker.Id, DateTimeOffset.Now,
                new EventCustomParameters());
            
            Assert.True(_eventRepository.IsContainEvent(createdEventId));
            Assert.True(tracker.IsUpdated);
        }
        
        [Test]
        public void GetEventWhenNoRequiredEventInRepository_ThrowsException()
        {
            Assert.Throws<EventNotFoundException>(() =>
                _eventService.GetEvent(Guid.NewGuid(), Guid.NewGuid()));
        }
        
        [Test]
        public void GetEventWhenUserAskNotHisOwnEvent_ThrowsException()
        {
            var eventOfAnotherUser = TestingMethods.CreateEvent(Guid.NewGuid(), Guid.NewGuid());
            _eventRepository.SaveEvent(eventOfAnotherUser);
            
            Assert.Throws<NoPermissionsForEventException>(() =>
                _eventService.GetEvent(Guid.NewGuid(), eventOfAnotherUser.Id));
        }
        
        [Test]
        public void GetEventGoodCase_ReturnEvent()
        {
            _eventRepository.SaveEvent(_event);

            var eventFromService = _eventService.GetEvent(_event.CreatorId, _event.Id);
            
            Assert.AreEqual(_event.GetHashCode(),eventFromService.GetHashCode());
            Assert.True(_tracker.IsUpdated);
        }

        [Test]
        public void GetAllTrackerEventsWhenNoRequiredTrackerInRepository_ThrowsException()
        {
            Assert.Throws<TrackerNotFoundException>(() =>
                _eventService.GetAllTrackerEvents(Guid.NewGuid(), Guid.NewGuid()));
        }
        
        [Test]
        public void GetAllTrackerEventsWhenUserAskNotHisTracker_ThrowsRestException()
        {
            _trackerRepository.SaveTracker(_tracker);
            
            Assert.Throws<NoPermissionsForTrackerException>(() =>
                _eventService.GetAllTrackerEvents(Guid.NewGuid(), _tracker.Id));
        }
        
        [Test]
        public void GetAllTrackerEventsGoodCase_ReturnsCollectionOfEvents()
        {
            _trackerRepository.SaveTracker(_tracker);
            var event2 = TestingMethods.CreateEvent(_tracker.Id, _tracker.CreatorId);
            _eventRepository.AddRangeOfEvents(new []{_event, event2});
            const int expected = 2;
            
            var events = _eventService.GetAllTrackerEvents(_tracker.CreatorId, _tracker.Id);
            
            Assert.AreEqual(expected, events.Count);
            Assert.AreEqual(_event.GetHashCode(), events.First().GetHashCode());
            Assert.AreEqual(event2.GetHashCode(), events.Last().GetHashCode());
        }
        
        [Test]
        public void GetAllTrackerEventsWhenTrackerHasNoEvents_ReturnsEmptyCollection()
        {
            var tracker = TestingMethods.CreateTrackerWithDefaultCustomization(Guid.NewGuid(), "Tracker");
            _trackerRepository.SaveTracker(tracker);
            const int expected = 0;
            
            var events = _eventService.GetAllTrackerEvents(tracker.CreatorId, tracker.Id);
            
            Assert.AreEqual(expected, events.Count);
        }

        [Test]
        public void EditEventWhenNoRequiredEventInRepository_ThrowsException()
        {
            Assert.Throws<EventNotFoundException>(() =>
                _eventService.EditEvent(Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void EditEventWhenUserAskNotHisEvent_ThrowsException()
        {
            _trackerRepository.SaveTracker(_tracker);
            _eventRepository.SaveEvent(_event);
            
            Assert.Throws<NoPermissionsForEventException>(() =>
                _eventService.EditEvent(Guid.NewGuid(),
                    _event.Id,
                    DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void EditEventWhenNewCustomizationDontMatchTrackerCustomizationAndCustomizationIsRequired_ThrowsException()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    true, 
                    true,
                    Option<string>.Some("meter"), 
                    true, 
                    false, 
                    true, 
                    true));
            _trackerRepository.SaveTracker(tracker);
            var oldEvent = TestingMethods.CreateEvent(tracker.Id, tracker.CreatorId);
            _eventRepository.SaveEvent(oldEvent);
            
            Assert.Throws<InvalidEventForTrackerException>(() =>
                _eventService.EditEvent(tracker.CreatorId, oldEvent.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        
        [Test]
        public void EditEventWhenNewCustomizationMatchTrackerCustomizationAndCustomizationIsRequired_EventInRepositoryUpdated()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    true, 
                    true,
                    Option<string>.Some("meter"), 
                    false, 
                    true, 
                    false, 
                    true));
            _trackerRepository.SaveTracker(tracker);
            var oldEvent = new Event(Guid.NewGuid(), tracker.CreatorId, tracker.Id, DateTimeOffset.Now, 
                new EventCustomParameters(
                    Option<Photo>.Some(new Photo(photoBytes: new byte[5])),
                    Option<double>.Some(1),
                    Option<double>.None,
                    Option<GeoTag>.Some(new GeoTag(10, 20)),
                    Option<Comment>.None));
            _eventRepository.SaveEvent(oldEvent);
            
            var updatedCustomizationParameters = new EventCustomParameters(
                Option<Photo>.Some(new Photo(photoBytes: new byte[7])),
                Option<double>.Some(15),
                Option<double>.None,
                Option<GeoTag>.Some(new GeoTag(30, 40)),
                Option<Comment>.None);
            
            _eventService.EditEvent(tracker.CreatorId, oldEvent.Id, DateTimeOffset.Now,
                updatedCustomizationParameters);
            
            Assert.AreEqual(updatedCustomizationParameters.GetHashCode(), 
                _eventRepository.LoadEvent(oldEvent.Id).CustomizationsParameters.GetHashCode());
            Assert.True(tracker.IsUpdated);
        }
        
        [Test]
        public void EditEventWhenNewCustomizationDontMatchTrackerCustomizationAndCustomizationNotRequired_EventInRepositoryUpdated()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    true, 
                    true,
                    Option<string>.Some("meter"), 
                    true, 
                    false, 
                    true, 
                    false));
            _trackerRepository.SaveTracker(tracker);
            var oldEvent = TestingMethods.CreateEvent(tracker.Id, tracker.CreatorId);
            _eventRepository.SaveEvent(oldEvent);

            _eventService.EditEvent(tracker.CreatorId, oldEvent.Id, DateTimeOffset.Now,
                new EventCustomParameters());
            
            Assert.AreNotEqual(oldEvent.GetHashCode(), 
                _eventRepository.LoadEvent(oldEvent.Id).GetHashCode());
            Assert.True(tracker.IsUpdated);
        }

        [Test]
        public void DeleteEventWhenNoRequiredEventInRepository_ThrowsRestException()
        {
            Assert.Throws<EventNotFoundException>(() =>
                _eventService.DeleteEvent(Guid.NewGuid(),
                    Guid.NewGuid()));
        }
        
        [Test]
        public void DeleteEventWhenUserAskNotHisEvent_ThrowsRestException()
        {
            _eventRepository.SaveEvent(_event);
            
            Assert.Throws<NoPermissionsForEventException>(() =>
                _eventService.DeleteEvent(Guid.NewGuid(),
                    _event.Id));
        }
        
        [Test]
        public void DeleteEventGoodCase_EventRemovedFromRepository()
        {
            _trackerRepository.SaveTracker(_tracker);
            _eventRepository.SaveEvent(_event);
            
            _eventService.DeleteEvent(_event.CreatorId, _event.Id);
            
            Assert.False(_eventRepository.IsContainEvent(_event.Id));
            Assert.True(_tracker.IsUpdated);
        }
    }
}