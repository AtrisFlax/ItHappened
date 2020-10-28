using System;
using System.Linq;
using ItHappened.Application.Errors;
using ItHappened.Application.Services.EventService;
using ItHappened.Application.Services.TrackerService;
using ItHappened.Domain;
using ItHappened.Infrastructure.Repositories;
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
        public void CreateEventWhenNoRequiredTrackerInRepository_ThrowsRestException()
        {
            Assert.Throws<RestException>(() =>
                _eventService.CreateEvent(Guid.NewGuid(), _tracker.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        
        [Test]
        public void CreateEventWhenUserCreateEventForNotHisOwnTracker_ThrowsRestException()
        {
            _trackerRepository.SaveTracker(_tracker);
            
            Assert.Throws<RestException>(() =>
                _eventService.CreateEvent(Guid.NewGuid(), _tracker.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void CreateEventWhenEventAndTrackerCustomizationDiffers_ThrowsRestException()
        {
            var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
                new TrackerCustomizationSettings(
                    Option<string>.Some("meter"), 
                    true, 
                    false, 
                    true, 
                    false, 
                    true, 
                    false));
            
            Assert.Throws<RestException>(() =>
                _eventService.CreateEvent(tracker.CreatorId, tracker.Id, DateTimeOffset.Now,
                    new EventCustomParameters()));
        }

        [Test]
        public void GetEventWhenNoRequiredEventInRepository_ThrowsRestException()
        {
            Assert.Throws<RestException>(() =>
                _eventService.GetEvent(Guid.NewGuid(), Guid.NewGuid()));
        }
        
        [Test]
        public void GetEventWhenUserAskNotHisOwnEvent_ThrowsRestException()
        {
            var eventOfAnotherUser = TestingMethods.CreateEvent(Guid.NewGuid(), Guid.NewGuid());
            _eventRepository.SaveEvent(eventOfAnotherUser);
            
            Assert.Throws<RestException>(() =>
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
        public void GetAllTrackerEventsWhenNoRequiredTrackerInRepository_ThrowsRestException()
        {
            Assert.Throws<RestException>(() =>
                _eventService.GetAllTrackerEvents(Guid.NewGuid(), Guid.NewGuid()));
        }
        
        [Test]
        public void GetAllTrackerEventsWhenUserAskNotHisTracker_ThrowsRestException()
        {
            _trackerRepository.SaveTracker(_tracker);
            
            Assert.Throws<RestException>(() =>
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
        public void EditEventWhenNoRequiredEventInRepository_ThrowsRestException()
        {
            Assert.Throws<RestException>(() =>
                _eventService.EditEvent(Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void EditEventWhenUserAskNotHisEvent_ThrowsRestException()
        {
            _trackerRepository.SaveTracker(_tracker);
            _eventRepository.SaveEvent(_event);
            
            Assert.Throws<RestException>(() =>
                _eventService.EditEvent(Guid.NewGuid(),
                    _event.Id,
                    DateTimeOffset.Now,
                    new EventCustomParameters()));
        }
        
        [Test]
        public void EditEventWhenNewCustomizationDontMatchTrackerCustomization_ThrowsRestException()
        {
            //TODO сделать после merge. Возможно нужно два вида. Где Customization is required and not
        }
        
        [Test]
        public void EditEventGoodCase_RequiredEventUpdatedInRepository()
        {
            //TODO сделать после merge
        }

        [Test]
        public void DeleteEventWhenNoRequiredEventInRepository_ThrowsRestException()
        {
            Assert.Throws<RestException>(() =>
                _eventService.DeleteEvent(Guid.NewGuid(),
                    Guid.NewGuid()));
        }
        
        [Test]
        public void DeleteEventWhenUserAskNotHisEvent_ThrowsRestException()
        {
            _eventRepository.SaveEvent(_event);
            
            Assert.Throws<RestException>(() =>
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
        
        //TODO проверить после merge, т.к. функция IsSettingsAndEventCustomizationsMatch написана в этой ветке с логическими ошибками
        // [Test]
        // public void CreateEventGoodCase_CreatedEventSavedInRepository()
        // {
        //     var tracker = TestingMethods.CreateTrackerWithRequiredCustomization(Guid.NewGuid(), "tracker",
        //         new TrackerCustomizationSettings(
        //             Option<string>.Some("meter"), 
        //             true, 
        //             false, 
        //             true, 
        //             false, 
        //             true, 
        //             false));
        //     _trackerRepository.SaveTracker(tracker);
        //     var happensDateOfNewEvent = DateTimeOffset.Now;
        //     var newEventCustomization = new EventCustomParameters(
        //         Option<Photo>.Some(new Photo(photoBytes: new byte[5])),
        //         Option<double>.Some(1),
        //         Option<double>.None,
        //         Option<GeoTag>.Some(new GeoTag(10, 20)),
        //         Option<Comment>.None);
        //     var createdEventId = _eventService.CreateEvent(
        //         tracker.CreatorId,
        //         tracker.Id, happensDateOfNewEvent,
        //         newEventCustomization);
        //
        //     var eventFromRepository = _eventRepository.LoadEvent(createdEventId);
        //
        //     Assert.AreEqual(newEventCustomization.GetHashCode(), eventFromRepository.CustomizationsParameters.GetHashCode());
        //     Assert.AreEqual(happensDateOfNewEvent, eventFromRepository.HappensDate);
        // }
    }
}