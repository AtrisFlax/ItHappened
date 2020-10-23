using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Application.Services.EventTrackerService;
using ItHappened.Domain;
using ItHappened.Infrastructure.Repositories;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.ServicesTests
{
    public class EventTrackerServiceTests
    {
        private EventTracker _eventTracker;
        private IEventTrackerService _eventTrackerService;
        private IEventTrackerRepository _eventTrackerRepository;
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventTrackerRepository = new EventTrackerRepository();
            _eventRepository = new EventRepository();
            _eventTrackerService = new EventTrackerService(_eventTrackerRepository, _eventRepository);
            _eventTracker = CreateEventTracker();
            
        }

        [Test]
        public void DeleteTrackerNotByCreator_WrongInitiatorIdStatus()
        {
            var wrongCreatorId = Guid.NewGuid();
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
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
        public void AddEventWhenEventAlreadyExistInRepository_EventAlreadyExistStatus()
        {
            var eventToAdd = CreateEvent(Guid.NewGuid());
            _eventRepository.AddEvent(eventToAdd);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.EventAlreadyExist;

            var actual = _eventTrackerService
                .AddEventToTracker(Guid.NewGuid(), Guid.NewGuid(), eventToAdd);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void AddEventToTrackerNotByTrackerCreator_WrongInitiatorIdStatus()
        {
            var wrongCreatorId = Guid.NewGuid();
            var eventToAdd = CreateEvent(Guid.NewGuid());
            _eventTrackerRepository.SaveTracker(_eventTracker);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;

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
        public void AddEventToTrackerGoodCase_OkStatusAndTrackerEventsNumberIncreases()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var eventToAdd = CreateEvent(userId, tracker.Id);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            const int expectedTrackerEventsNumber = 1;
            
            var actual = _eventTrackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
            var trackerEventsNumber =_eventRepository.LoadAllTrackerEvents(tracker.Id).Count;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedTrackerEventsNumber, trackerEventsNumber);
        }

        [Test]
        public void AddEventToTrackerWhenEventCustomisationDontMatchTrackerCustomisation_WrongEventCustomisationStatus()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var eventToAdd = CreateEvent(userId, tracker.Id);
            eventToAdd.Comment = new Comment("comment");
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongEventCustomisation;
            
            var actual = _eventTrackerService.AddEventToTracker(userId, tracker.Id, eventToAdd);
            
            Assert.AreEqual(expected, actual);
            Assert.AreNotEqual(tracker.HasComment, eventToAdd.Comment.IsSome);
            Assert.AreEqual(tracker.HasPhoto, eventToAdd.Photo.IsSome);
            Assert.AreEqual(tracker.HasGeoTag, eventToAdd.GeoTag.IsSome);
            Assert.AreEqual(tracker.HasRating, eventToAdd.Rating.IsSome);
            Assert.AreEqual(tracker.HasScale, eventToAdd.Scale.IsSome);
        }

        [Test]
        public void RemoveEventFromNonExistentTracker_TrackerDontExistStatus()
        {
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;

            var actual = _eventTrackerService
                .RemoveEventFromTracker(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void RemoveEventWhenEventDontExist_EventDontExistStatus()
        {
            _eventTrackerRepository.SaveTracker(_eventTracker);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.EventDontExist;

            var actual = _eventTrackerService
                .RemoveEventFromTracker(Guid.NewGuid(), _eventTracker.Id, Guid.NewGuid());
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void RemoveEventFromTrackerNotByTrackerCreator_WrongInitiatorIdStatus()
        {
            _eventTrackerRepository.SaveTracker(_eventTracker);
            var eventToRemove = CreateEvent(Guid.NewGuid());
            _eventRepository.AddEvent(eventToRemove);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;

            var actual = 
                _eventTrackerService.RemoveEventFromTracker(Guid.NewGuid(), _eventTracker.Id, eventToRemove.Id);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveEventFromTrackerWithDifferentEventCreator_WrongEventCreatorIdStatus()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var eventToRemove = CreateEvent(Guid.NewGuid());
            _eventRepository.AddEvent(eventToRemove);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongEventCreatorId;

            var actual = _eventTrackerService.RemoveEventFromTracker(userId, tracker.Id, eventToRemove.Id);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemoveEventFromTrackerGoodCase_OkStatusAndTrackerEventsNumberDecreases()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var event1 = CreateEvent(userId, tracker.Id);
            var eventToRemove = CreateEvent(userId, tracker.Id);
            _eventRepository.AddRangeOfEvents(new []{event1, eventToRemove});
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            const int expectedTrackerEventsNumber = 1;
            
            var actual = _eventTrackerService.RemoveEventFromTracker(userId, tracker.Id, eventToRemove.Id);
            var trackerEventsNumber =_eventRepository.LoadAllTrackerEvents(tracker.Id).Count;
            
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedTrackerEventsNumber, trackerEventsNumber);
        }

        [Test]
        public void EditEventFromNonExistentTracker_TrackerDontExistStatus()
        {
            var eventToEdit = CreateEvent(Guid.NewGuid());
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;

            var actual = _eventTrackerService
                .EditEventInTracker(Guid.NewGuid(), Guid.NewGuid(), eventToEdit);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EditEventFromTrackerNotByTrackerCreator_WrongInitiatorIdStatus()
        {
            var eventToEdit = CreateEvent(Guid.NewGuid());
            _eventTrackerRepository.SaveTracker(_eventTracker);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;

            var actual = 
                _eventTrackerService.EditEventInTracker(Guid.NewGuid(), _eventTracker.Id, eventToEdit);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EditEventWhenEventDontExistInRepository_EventDontExistStatus()
        {
            var eventToEdit = CreateEvent(_eventTracker.CreatorId);
            _eventTrackerRepository.SaveTracker(_eventTracker);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.EventDontExist;
            
            var actual = 
                _eventTrackerService.EditEventInTracker(_eventTracker.CreatorId, _eventTracker.Id, eventToEdit);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EditEventGoodCase_OkStatusAndEventDataChanges()
        {
            _eventTrackerRepository.SaveTracker(_eventTracker);
            var eventInitial = CreateEvent(_eventTracker.CreatorId, _eventTracker.Id);
            var eventEdited = EventBuilder
                .Event(eventInitial.Id,
                    eventInitial.CreatorId,
                    _eventTracker.Id,
                    DateTimeOffset.Now - TimeSpan.FromDays(1),
                    "Edited event")
                .WithRating(7)
                .WithComment("Hello!")
                .Build();
            _eventRepository.AddEvent(eventInitial);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            
            var actual = 
                _eventTrackerService.EditEventInTracker(_eventTracker.CreatorId, _eventTracker.Id, eventEdited);
            
            Assert.AreEqual(expected, actual);
            Assert.True(eventInitial.Rating.IsNone);
            Assert.True(eventInitial.Comment.IsNone);
            Assert.AreEqual("Hello!", eventEdited.Comment.ValueUnsafe().Text);
            Assert.AreEqual(7, eventEdited.Rating.ValueUnsafe());
        }

        [Test]
        public void GetAllEventsFromTrackerWhenTrackerDontExist_TrackerDontExistStatusAndEmptyCollection()
        {
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
            
            var (collection, statusOfResult) = _eventTrackerService
                .GetAllEventsFromTracker(Guid.NewGuid(), Guid.NewGuid());
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.False(collection.Any());
        }

        [Test]
        public void GetAllEventsFromTrackerNotByTrackerCreator_WrongInitiatorIdStatusAndEmptyCollection()
        {
            _eventTrackerRepository.SaveTracker(_eventTracker);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
            
            var (collection, statusOfResult) = _eventTrackerService
                .GetAllEventsFromTracker(_eventTracker.Id, Guid.NewGuid());
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.False(collection.Any());
        }
        
        [Test]
        public void GetAllEventsFromTrackerGoodCase_OkStatusAndCollectionWithEvents()
        {
            _eventTrackerRepository.SaveTracker(_eventTracker);
            var event1 = CreateEvent(_eventTracker.CreatorId, _eventTracker.Id);
            var event2 = CreateEvent(_eventTracker.CreatorId, _eventTracker.Id);
            _eventRepository.AddRangeOfEvents(new []{event1, event2});
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            
            var (collection, statusOfResult) = _eventTrackerService
                .GetAllEventsFromTracker(_eventTracker.Id, _eventTracker.CreatorId);
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.AreEqual(2, collection.Count);
        }

        [Test]
        public void FilterEventsWhenTrackerDontExist_TrackerDontExistStatusAndEmptyCollection()
        {
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.TrackerDontExist;
            var filter = new RatingFilter("rating", 5, 6);
            
            var (collection, statusOfResult) = _eventTrackerService
                .FilterEvents(Guid.NewGuid(), Guid.NewGuid(), new IEventsFilter[] {filter});
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.False(collection.Any());
        }

        [Test]
        public void FilterEventsFromTrackerNotByTrackerCreator_WrongInitiatorIdStatusAndEmptyCollection()
        {
            _eventTrackerRepository.SaveTracker(_eventTracker);
            var filter = new RatingFilter("rating", 5, 6);
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.WrongInitiatorId;
            
            var (collection, statusOfResult) = _eventTrackerService
                .FilterEvents(_eventTracker.Id, Guid.NewGuid(), new IEventsFilter[] {filter});
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.False(collection.Any());
        }

        [Test]
        public void FilterEventsWhenGivenEmptyFilterCollection_NoFiltersReceivedStatusAndAllTrackerEventsCollection()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var event1 = CreateEvent(userId, tracker.Id);
            var event2 = CreateEvent(userId, tracker.Id);
            _eventRepository.AddRangeOfEvents(new []{event1, event2});
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.NoFiltersReceived;
            const int expectedNumberOfReturnedEvents = 2;
            var (collection, statusOfResult) = _eventTrackerService
                .FilterEvents(tracker.Id, userId, new IEventsFilter[] {});
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.AreEqual(expectedNumberOfReturnedEvents,collection.Count);
        }

        [Test]
        public void FilterEventsGoodCase_OkStatusAndFilteredCollectionOfEvents()
        {
            var userId = Guid.NewGuid();
            var tracker = CreateEventTracker(userId);
            _eventTrackerRepository.SaveTracker(tracker);
            var events = CreateEvents(tracker.Id, userId, 10);
            //Each one of these events won't pass its one parameter filter
            events[0].Comment = Option<Comment>.Some(new Comment("привет!"));
            events[1].Scale = 1;
            events[2].HappensDate = DateTimeOffset.Now;
            events[3].Rating = 1;
            events[4].GeoTag = new GeoTag(0, 0);
            _eventRepository.AddRangeOfEvents(events);
            var filters = CreateFilters();
            const EventTrackerServiceStatusCodes expected = EventTrackerServiceStatusCodes.Ok;
            const int expectedNumberOfReturnedEvents = 5;
            
            var (collection, statusOfResult) = _eventTrackerService
                .FilterEvents(tracker.Id, userId, filters);
            
            Assert.AreEqual(expected, statusOfResult);
            Assert.AreEqual(expectedNumberOfReturnedEvents,collection.Count);
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
        
        private List<Event> CreateEvents(Guid trackerId, Guid userId,int quantity)
        {
            var events = new List<Event>();
            for (var i = 0; i < quantity; i++)
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), userId, trackerId,
                        new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime(),
                        "tittle")
                    .WithComment("здравствуй!")
                    .WithRating(5.6)
                    .WithScale(8)
                    .WithGeoTag(new GeoTag(11, 16))
                    .Build());
            return events;
        }

        private IReadOnlyList<IEventsFilter> CreateFilters()
        {
            var from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            return new IEventsFilter[]
            {
                new CommentFilter("regex", @"\A[з]"),
                new GeoTagFilter("geotag", new GeoTag(10, 15), new GeoTag(20, 25)),
                new RatingFilter("rating", 5.5, 8.6),
                new DateTimeFilter("DateTime", from, to),
                new ScaleFilter("scale", 5.5, 8.6)
            };

        }
    }
}