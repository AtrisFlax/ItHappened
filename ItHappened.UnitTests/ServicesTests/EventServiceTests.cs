using System;
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
        private ITrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;
        private IEventService _eventService;
        
        [SetUp]
        public void Init()
        {
            _trackerRepository = new TrackerRepository();
            _eventService = new EventService(_eventRepository, _trackerRepository);
            _tracker = TestingMethods.CreateTrackerWithDefaultCustomization(Guid.NewGuid());
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
        public void CreateEventGoodCase_CreatedEventSavedInRepository()
        {
            
        }
        
        // [Test]
        // public void AddEventToNonExistentTracker_TrackerDontExistStatus()
        // {
        //     var eventToAdd = CreateEvent(Guid.NewGuid());
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
        //
        //     var actual = _trackerService
        //         .AddEventToTracker(Guid.NewGuid(), Guid.NewGuid(), eventToAdd);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void AddEventWhenEventAlreadyExistInRepository_EventAlreadyExistStatus()
        // {
        //     var eventToAdd = CreateEvent(Guid.NewGuid());
        //     _eventRepository.CreateEvent(eventToAdd);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.EventAlreadyExist;
        //
        //     var actual = _trackerService
        //         .AddEventToTracker(Guid.NewGuid(), Guid.NewGuid(), eventToAdd);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void AddEventToTrackerNotByTrackerCreator_WrongInitiatorIdStatus()
        // {
        //     var wrongCreatorId = Guid.NewGuid();
        //     var eventToAdd = CreateEvent(Guid.NewGuid());
        //     _trackerRepository.SaveTracker(_tracker);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
        //
        //     var actual = 
        //         _trackerService.AddEventToTracker(wrongCreatorId, _tracker.Id, eventToAdd);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void AddEventToTrackerWithDifferentEventCreator_WrongEventCreatorIdStatus()
        // {
        //     var userId = Guid.NewGuid();
        //     var tracker = CreateEventTracker(userId);
        //     _trackerRepository.SaveTracker(tracker);
        //     var eventToAdd = CreateEvent(Guid.NewGuid());
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongEventCreatorId;
        //
        //     var actual = _trackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void AddEventToTrackerGoodCase_OkStatusAndTrackerEventsNumberIncreases()
        // {
        //     var userId = Guid.NewGuid();
        //     var tracker = CreateEventTracker(userId);
        //     _trackerRepository.SaveTracker(tracker);
        //     var eventToAdd = CreateEvent(userId, tracker.Id);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
        //     const int expectedTrackerEventsNumber = 1;
        //     
        //     var actual = _trackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
        //     var trackerEventsNumber =_eventRepository.LoadAllTrackerEvents(tracker.Id).Count;
        //     Assert.AreEqual(expected, actual);
        //     Assert.AreEqual(expectedTrackerEventsNumber, trackerEventsNumber);
        // }
        //
        // [Test]
        // public void AddEventToTrackerWhenEventCustomisationDontMatchTrackerCustomisation_WrongEventCustomisationStatus()
        // {
        //     var userId = Guid.NewGuid();
        //     var tracker = CreateEventTracker(userId);
        //     _trackerRepository.SaveTracker(tracker);
        //     var eventToAdd = CreateEvent(userId, tracker.Id);
        //     eventToAdd.Comment = new Comment("comment");
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongEventCustomisation;
        //     
        //     var actual = _trackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
        //     
        //     Assert.AreEqual(expected, actual);
        //     Assert.AreNotEqual(tracker.HasComment, eventToAdd.Comment.IsSome);
        //     Assert.AreEqual(tracker.HasPhoto, eventToAdd.Photo.IsSome);
        //     Assert.AreEqual(tracker.HasGeoTag, eventToAdd.GeoTag.IsSome);
        //     Assert.AreEqual(tracker.HasRating, eventToAdd.Rating.IsSome);
        //     Assert.AreEqual(tracker.HasScale, eventToAdd.Scale.IsSome);
        // }
        //
        // [Test]
        // public void RemoveEventFromNonExistentTracker_TrackerDontExistStatus()
        // {
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
        //
        //     var actual = _trackerService
        //         .RemoveEventFromTracker(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void RemoveEventWhenEventDontExist_EventDontExistStatus()
        // {
        //     _trackerRepository.SaveTracker(_tracker);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.EventDontExist;
        //
        //     var actual = _trackerService
        //         .RemoveEventFromTracker(Guid.NewGuid(), _tracker.Id, Guid.NewGuid());
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void RemoveEventFromTrackerNotByTrackerCreator_WrongInitiatorIdStatus()
        // {
        //     _trackerRepository.SaveTracker(_tracker);
        //     var eventToRemove = CreateEvent(Guid.NewGuid());
        //     _eventRepository.CreateEvent(eventToRemove);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
        //
        //     var actual = 
        //         _trackerService.RemoveEventFromTracker(Guid.NewGuid(), _tracker.Id, eventToRemove.Id);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void RemoveEventFromTrackerWithDifferentEventCreator_WrongEventCreatorIdStatus()
        // {
        //     var userId = Guid.NewGuid();
        //     var tracker = CreateEventTracker(userId);
        //     _trackerRepository.SaveTracker(tracker);
        //     var eventToRemove = CreateEvent(Guid.NewGuid());
        //     _eventRepository.CreateEvent(eventToRemove);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongEventCreatorId;
        //
        //     var actual = _trackerService.RemoveEventFromTracker(userId, tracker.Id, eventToRemove.Id);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void RemoveEventFromTrackerGoodCase_OkStatusAndTrackerEventsNumberDecreases()
        // {
        //     var userId = Guid.NewGuid();
        //     var tracker = CreateEventTracker(userId);
        //     _trackerRepository.SaveTracker(tracker);
        //     var event1 = CreateEvent(userId, tracker.Id);
        //     var eventToRemove = CreateEvent(userId, tracker.Id);
        //     _eventRepository.AddRangeOfEvents(new []{event1, eventToRemove});
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
        //     const int expectedTrackerEventsNumber = 1;
        //     
        //     var actual = _trackerService.RemoveEventFromTracker(userId, tracker.Id, eventToRemove.Id);
        //     var trackerEventsNumber =_eventRepository.LoadAllTrackerEvents(tracker.Id).Count;
        //     
        //     Assert.AreEqual(expected, actual);
        //     Assert.AreEqual(expectedTrackerEventsNumber, trackerEventsNumber);
        // }
        //
        // [Test]
        // public void EditEventFromNonExistentTracker_TrackerDontExistStatus()
        // {
        //     var eventToEdit = CreateEvent(Guid.NewGuid());
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
        //
        //     var actual = _trackerService
        //         .EditEventInTracker(Guid.NewGuid(), Guid.NewGuid(), eventToEdit);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void EditEventFromTrackerNotByTrackerCreator_WrongInitiatorIdStatus()
        // {
        //     var eventToEdit = CreateEvent(Guid.NewGuid());
        //     _trackerRepository.SaveTracker(_tracker);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
        //
        //     var actual = 
        //         _trackerService.EditEventInTracker(Guid.NewGuid(), _tracker.Id, eventToEdit);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void EditEventWhenEventDontExistInRepository_EventDontExistStatus()
        // {
        //     var eventToEdit = CreateEvent(_tracker.CreatorId);
        //     _trackerRepository.SaveTracker(_tracker);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.EventDontExist;
        //     
        //     var actual = 
        //         _trackerService.EditEventInTracker(_tracker.CreatorId, _tracker.Id, eventToEdit);
        //     
        //     Assert.AreEqual(expected, actual);
        // }
        //
        // [Test]
        // public void EditEventGoodCase_OkStatusAndEventDataChanges()
        // {
        //     _trackerRepository.SaveTracker(_tracker);
        //     var eventInitial = CreateEvent(_tracker.CreatorId, _tracker.Id);
        //     var eventEdited = EventBuilder
        //         .Event(eventInitial.Id,
        //             eventInitial.CreatorId,
        //             _tracker.Id,
        //             DateTimeOffset.Now - TimeSpan.FromDays(1),
        //             "Edited event")
        //         .WithRating(7)
        //         .WithComment("Hello!")
        //         .Build();
        //     _eventRepository.CreateEvent(eventInitial);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
        //     
        //     var actual = 
        //         _trackerService.EditEventInTracker(_tracker.CreatorId, _tracker.Id, eventEdited);
        //     
        //     Assert.AreEqual(expected, actual);
        //     Assert.True(eventInitial.Rating.IsNone);
        //     Assert.True(eventInitial.Comment.IsNone);
        //     Assert.AreEqual("Hello!", eventEdited.Comment.ValueUnsafe().Text);
        //     Assert.AreEqual(7, eventEdited.Rating.ValueUnsafe());
        // }
        //
        // [Test]
        // public void GetAllEventsFromTrackerWhenTrackerDontExist_TrackerDontExistStatusAndEmptyCollection()
        // {
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
        //     
        //     var (collection, statusOfResult) = _trackerService
        //         .GetAllEventsFromTracker(Guid.NewGuid(), Guid.NewGuid());
        //     
        //     Assert.AreEqual(expected, statusOfResult);
        //     Assert.False(collection.Any());
        // }
        //
        // [Test]
        // public void GetAllEventsFromTrackerNotByTrackerCreator_WrongInitiatorIdStatusAndEmptyCollection()
        // {
        //     _trackerRepository.SaveTracker(_tracker);
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
        //     
        //     var (collection, statusOfResult) = _trackerService
        //         .GetAllEventsFromTracker(_tracker.Id, Guid.NewGuid());
        //     
        //     Assert.AreEqual(expected, statusOfResult);
        //     Assert.False(collection.Any());
        // }
        //
        // [Test]
        // public void GetAllEventsFromTrackerGoodCase_OkStatusAndCollectionWithEvents()
        // {
        //     _trackerRepository.SaveTracker(_tracker);
        //     var event1 = CreateEvent(_tracker.CreatorId, _tracker.Id);
        //     var event2 = CreateEvent(_tracker.CreatorId, _tracker.Id);
        //     _eventRepository.AddRangeOfEvents(new []{event1, event2});
        //     const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
        //     
        //     var (collection, statusOfResult) = _trackerService
        //         .GetAllEventsFromTracker(_tracker.Id, _tracker.CreatorId);
        //     
        //     Assert.AreEqual(expected, statusOfResult);
        //     Assert.AreEqual(2, collection.Count);
        // }
        
        // private EventTracker CreateEventTracker()
        // {
        //     var tracker = EventTrackerBuilder
        //         .Tracker(Guid.NewGuid(), Guid.NewGuid(), "tracker")
        //         .Build();
        //
        //     return tracker;
        // }
        //
        // private EventTracker CreateEventTracker(Guid creatorId)
        // {
        //     var tracker = EventTrackerBuilder
        //         .Tracker(creatorId, Guid.NewGuid(), "tracker: " + $"{creatorId}")
        //         .Build();
        //
        //     return tracker;
        // }
        //
        // private Event CreateEvent(Guid creatorId)
        // {
        //     return EventBuilder
        //         .Event(Guid.NewGuid(), creatorId, _tracker.Id, DateTimeOffset.Now, "event: " + $"{creatorId}")
        //         .Build();
        // }
        //
        // private Event CreateEvent(Guid creatorId, Guid trackerId)
        // {
        //     return EventBuilder
        //         .Event(Guid.NewGuid(), creatorId, trackerId, DateTimeOffset.Now, "event: " + $"{creatorId}")
        //         .Build();
        // }
    }
}