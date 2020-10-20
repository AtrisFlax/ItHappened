//TODO 

// using System;
// using System.Collections.Generic;
// using ItHappened.Domain;
// using ItHappened.Domain.Statistics;
// using ItHappened.Infrastructure.Repositories;
// using LanguageExt;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests.StatisticsCalculatorsTests
// {
//     public class WorstEventCalculatorTests
//     {
//         private const int InitialEventsNumber = 9;
//         private Guid _creatorId;
//         private List<Event> _events;
//         private EventTracker _eventTracker;
//         private WorstEventCalculator _worstEventCalculator;
//         private IEventRepository _eventRepository;
//         
//         [SetUp]
//         public void Init()
//         {
//             _eventRepository = new EventRepository();
//             _creatorId = Guid.NewGuid();
//             _events = CreateEvents(_creatorId, InitialEventsNumber);
//             _eventTracker = CreateEventTracker();
//             _worstEventCalculator = new WorstEventCalculator(_eventRepository);
//         }
//
//         [Test]
//         public void CalculateLessThanRequiredNumberEvents_ReturnNone()
//         {
//             //arrange
//             _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
//             _events[1].Rating = 1;
//             _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
//             var expected = Option<IStatisticsFact>.None;
//
//             //act
//             var actual = _worstEventCalculator.Calculate(_eventTracker);
//
//             //assert
//             Assert.AreEqual(expected, actual);
//         }
//
//         [Test]
//         public void CalculateWithoutOldEnoughEvent_ReturnNone()
//         {
//             //arrange
//             _events.Add(CreateEventWithoutComment(_creatorId));
//             _events[1].Rating = 1;
//             _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
//             var expected = Option<IStatisticsFact>.None;
//
//             //act
//             var actual = _worstEventCalculator.Calculate(_eventTracker);
//
//             //assert
//             Assert.AreEqual(expected, actual);
//         }
//
//         [Test]
//         public void CalculateWhenWorstEventHappenedLessThanWeekAgo_ReturnNone()
//         {
//             //arrange
//             _events.Add(CreateEventWithoutComment(_creatorId));
//             _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
//             _events[1].Rating = 1;
//             _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
//             var expected = Option<IStatisticsFact>.None;
//
//             //act
//             var actual = _worstEventCalculator.Calculate(_eventTracker);
//
//             //assert
//             Assert.AreEqual(expected, actual);
//         }
//
//         [Test]
//         public void CalculateGoodCase_ReturnsFact()
//         {
//             //arrange
//             _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
//             _events.Add(CreateEventWithoutComment(_creatorId));
//             _events[9].Rating = 1;
//             _events[9].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
//
//             //act
//             var worstEvent = _worstEventCalculator.Calculate(_eventTracker);
//
//             //assert
//             Assert.True(worstEvent.IsSome);
//         }
//
//         private Event CreateEventWithoutComment(Guid creatorId)
//         {
//             return EventBuilder
//                 .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
//                 .WithRating(5)
//                 .Build();
//         }
//
//         private List<Event> CreateEvents(Guid creatorId, int quantity)
//         {
//             var events = new List<Event>();
//             for (var i = 0; i < quantity; i++)
//                 events.Add(EventBuilder
//                     .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
//                     .WithComment("comment")
//                     .WithRating(5)
//                     .Build());
//             return events;
//         }
//
//         private EventTracker CreateEventTracker()
//         {
//             var tracker = EventTrackerBuilder
//                 .Tracker(Guid.NewGuid(), _eventTracker.Id, "tracker")
//                 .Build();
//             return tracker;
//         }
//     }
// }