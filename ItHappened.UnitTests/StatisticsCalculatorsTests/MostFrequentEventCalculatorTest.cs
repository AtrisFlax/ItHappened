// using System;
// using System.Collections.Generic;
// using ItHappened.Domain;
// using ItHappened.Domain.Statistics;
// using ItHappened.Infrastructure.Repositories;
// using LanguageExt.UnsafeValueAccess;
// using NUnit.Framework;
// using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConsts;
//
// namespace ItHappened.UnitTests.StatisticsCalculatorsTests
// {
//     public class MostFrequentEventCalculatorTest
//     {
//         private IEventRepository _eventRepository;
//
//         [SetUp]
//         public void Init()
//         {
//             _eventRepository = new EventRepository();
//         }
//
//         [Test]
//         public void CreateTwoEventTrackersWithEnoughEvents_GetMostFrequentEventFact_CheckAllProperties()
//         {
//             //arrange
//             var userId = Guid.NewGuid();
//             var eventTracker1 = CreateTracker(userId, "Pains after drinking sugar water");
//             var eventTracker2 = CreateTracker(userId, "Pains after eating sugar");
//             var events = CreateEventsEnoughEventForCalculateForTwoTrackers(eventTracker1.Id, eventTracker2.Id, Guid.NewGuid());
//             _eventRepository.AddRangeOfEvents(events);
//
//             //act
//             var fact = new MostFrequentEventStatisticsCalculator(_eventRepository)
//                 .Calculate(new[] {eventTracker1, eventTracker2})
//                 .ConvertTo<MostFrequentEventTrackersFact>().ValueUnsafe();
//
//             //assert 
//             Assert.AreEqual("Самое частое событие", fact.FactName);
//             Assert.AreEqual($"Чаще всего у вас происходит событие Pains after drinking sugar water - раз в {fact.EventsPeriod:0.#} дней", fact.Description);
//             Assert.AreEqual(4.0, fact.Priority, PriorityAccuracy);
//             Assert.AreEqual("Pains after drinking sugar water", fact.TrackingName);
//             Assert.AreEqual(2.5, fact.EventsPeriod, EventsPeriodAccuracy);
//         }
//         
//         //TODO test with only one tracker (==1)
//         
//         //TODO test enough tracker but not enough events in one tracker (<=3)
//         
//         //TODO test enough tracker but not enough events in all trackers (<=3)
//         
//         
//         
//         private static EventTracker CreateTracker(Guid userId, string trackerName)
//         {
//             var eventTracker1 = EventTrackerBuilder
//                 .Tracker(userId, Guid.NewGuid(), trackerName)
//                 .Build();
//             return eventTracker1;
//         }
//
//         private static IEnumerable<Event> CreateEventsEnoughEventForCalculateForTwoTrackers(Guid eventTracker1Id, Guid eventTracker2Id,
//             Guid userId)
//         {
//             var now = DateTimeOffset.Now;
//             var headacheEvent1DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker1Id, "headache", now.AddDays(-1));
//             var headacheEvent2DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker1Id, "headache", now.AddDays(-2));
//             var headacheEvent3DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker1Id, "headache", now.AddDays(-3));
//             var headacheEvent4DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker1Id, "headache", now.AddDays(-10)); //first event
//
//             var toothacheEvent1DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker2Id, "toothache", now.AddDays(-1));
//             var toothacheEvent2DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker2Id, "toothache", now.AddDays(-2));
//             var toothacheEvent3DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker2Id, "toothache", now.AddDays(-3));
//             var toothacheEvent4DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker2Id, "toothache", now.AddDays(-4));
//             var toothacheEvent5DayAgo =
//                 CreateEventWithNameAndDateTime(userId, eventTracker2Id, "toothache", now.AddDays(-15)); //first event
//             return new[]
//             {
//                 headacheEvent1DayAgo,
//                 headacheEvent2DayAgo,
//                 headacheEvent3DayAgo,
//                 headacheEvent4DayAgo,
//                 toothacheEvent1DayAgo,
//                 toothacheEvent2DayAgo,
//                 toothacheEvent3DayAgo,
//                 toothacheEvent4DayAgo,
//                 toothacheEvent5DayAgo
//             };
//         }
//
//         private static Event CreateEventWithNameAndDateTime(Guid userId, Guid trackerId, string title,
//             DateTimeOffset dateTime)
//         {
//             return EventBuilder.Event(Guid.NewGuid(), userId, trackerId, dateTime, title).Build();
//         }
//     }
// }