using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;
namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class SpecificDayTimeEventCalculatorTest
    {
        private IEventRepository _eventRepository;
        private DateTimeOffset _now;

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _now = DateTimeOffset.UtcNow;
        }

        [Test]
        public void CreateEventTrackerWithHeadacheAndSmokingEvents_CalculateSpecificDayTimeEventFact_CheckAProperties()
        {
            //assert
            var userId = Guid.NewGuid();
            const string trackerName = "Tracker name";
            var tracker = CreateTracker(userId, trackerName);
            var events = new List<Event>
            {
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.10.8 01:05:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.11.9 02:05:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.12.9 03:07:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.10.3 4:05:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2021.10.9 05:05:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.10.9 10:05:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.10.9 04:05:00"),
                CreateEventWithNameAndDateTime(userId, tracker.Id, "2020.10.9 09:05:00")
            };
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            
            //act
            var fact = new SpecificDayTimeCalculator().Calculate(allEvents, tracker, _now)
                .ConvertTo<SpecificDayTimeFact>().ValueUnsafe();
            
            //arrange
            Assert.AreEqual("Происходит в определённое время суток", fact.FactName);
            Assert.AreEqual($"В 75% случаев событие {tracker.Name} происходит ночью", fact.Description);
            Assert.AreEqual(10.5, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(75.0, fact.Percentage);
            Assert.AreEqual("ночью", fact.TimeOfTheDay);
        }

         
        //TODO add test применимо, если событий в отслеживании больше семи (<=7)
        
        //TODO add test и если на одно из времён суток доля событий превышает 70% (70%)
        
        private static Event CreateEventWithNameAndDateTime(Guid userId, Guid trackerId, string dateTime)
        {
            return new Event(Guid.NewGuid(), userId, trackerId, DateTime.Parse(dateTime), new EventCustomParameters());
        }
    }
}